using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Domain.Requests;
using GenericWorkflowAPI.Domain.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace GenericWorkflowAPI.Services
{
    public class WorkflowService : IWorkflowService, IDisposable
    {
        protected readonly ILogger logger;
        protected readonly IGenericCodeRepository<Workflow> workflowRepository;
        protected readonly IGenericCodeRepository<WorkflowInputCodeType> workflowInputCodeTypeRepository;
        protected readonly IGenericCodeRepository<WorkflowState> workflowStateRepository;
        protected readonly IGenericCodeRepository<WorkflowStateInputCodeType> workflowStateInputCodeTypeRepository;
        protected readonly IGenericCodeRepository<WorkflowTransition> workflowTransitionRepository;
        protected readonly IGenericCodeRepository<WorkflowInstance> workflowInstanceRepository;
        protected readonly IGenericRepository<WorkflowInstanceInputCode> workflowInstanceInputCodeRepository;
        protected readonly IGenericRepository<WorkflowInstanceHistory> workflowInstanceHistoryRepository;
        protected readonly IGenericRepository<WorkflowInstanceHistoryInputCode> workflowInstanceHistoryInputCodeRepository;

        public WorkflowService(ILogger _logger,
            IGenericCodeRepository<Workflow> _workflowRepository,
            IGenericCodeRepository<WorkflowInputCodeType> _workflowInputCodeTypeRepository,
            IGenericCodeRepository<WorkflowState> _workflowStateRepository,
            IGenericCodeRepository<WorkflowStateInputCodeType> _workflowStateInputCodeTypeRepository,
            IGenericCodeRepository<WorkflowTransition> _workflowTransitionRepository,
            IGenericCodeRepository<WorkflowInstance> _workflowInstanceRepository,
            IGenericRepository<WorkflowInstanceInputCode> _workflowInstanceInputCodeRepository,
            IGenericRepository<WorkflowInstanceHistory> _workflowInstanceHistoryRepository,
            IGenericRepository<WorkflowInstanceHistoryInputCode> _workflowInstanceHistoryInputCodeRepository)
        {
            if (_logger == null)
                throw new ArgumentNullException(nameof(_logger));
            if (_workflowRepository == null)
                throw new ArgumentNullException(nameof(_workflowRepository));
            if (_workflowInputCodeTypeRepository == null)
                throw new ArgumentNullException(nameof(_workflowInputCodeTypeRepository));
            if (_workflowStateRepository == null)
                throw new ArgumentNullException(nameof(_workflowStateRepository));
            if (_workflowStateInputCodeTypeRepository == null)
                throw new ArgumentNullException(nameof(_workflowStateInputCodeTypeRepository));
            if (_workflowTransitionRepository == null)
                throw new ArgumentNullException(nameof(_workflowTransitionRepository));
            if (_workflowInstanceRepository == null)
                throw new ArgumentNullException(nameof(_workflowInstanceRepository));
            if (_workflowInstanceInputCodeRepository == null)
                throw new ArgumentNullException(nameof(_workflowInstanceInputCodeRepository));
            if (_workflowInstanceHistoryRepository == null)
                throw new ArgumentNullException(nameof(_workflowInstanceHistoryRepository));
            if (_workflowInstanceHistoryInputCodeRepository == null)
                throw new ArgumentNullException(nameof(_workflowInstanceHistoryInputCodeRepository));

            logger = _logger;
            workflowRepository = _workflowRepository;
            workflowInputCodeTypeRepository = _workflowInputCodeTypeRepository;
            workflowStateRepository = _workflowStateRepository;
            workflowStateInputCodeTypeRepository = _workflowStateInputCodeTypeRepository;
            workflowTransitionRepository = _workflowTransitionRepository;
            workflowInstanceRepository = _workflowInstanceRepository;
            workflowInstanceInputCodeRepository = _workflowInstanceInputCodeRepository;
            workflowInstanceHistoryRepository = _workflowInstanceHistoryRepository;
            workflowInstanceHistoryInputCodeRepository = _workflowInstanceHistoryInputCodeRepository;
        }

        public async Task Run(ExecuteWorkflowRequest executeWorkflowRequest, CancellationToken cancellationToken)
        {
            if (executeWorkflowRequest == null)
                throw new ArgumentNullException(nameof(executeWorkflowRequest));

            var workflowCode = executeWorkflowRequest.WorkflowCode;
            if (string.IsNullOrWhiteSpace(workflowCode))
                throw new ArgumentNullException(nameof(workflowCode));

            var workflowInstanceCode = executeWorkflowRequest.WorkflowInstanceCode;
            var workflowInputCodeTypeXvalue = executeWorkflowRequest.WorkflowInputCodeTypeXvalue;

            var workflow = await workflowRepository.GetByCodeAsync(workflowCode, new List<string>(), cancellationToken);
            if (workflow == null)
                throw new InvalidOperationException($"Invalid {nameof(workflowCode)} {workflowCode} received. No workflow exists with that code.");

            // Try to get workflow instance by code
            var workflowInstance = await workflowInstanceRepository.GetByCodeAsync(workflowInstanceCode, new List<string>(), cancellationToken);
            if (workflowInstance == null) // If workflow instance doesn't exist then a new one must be created
            {
                // Try to create a new instance if all input codes for the first state are provided
                var firstWorkflowState = await workflowStateRepository.DbSet.FirstOrDefaultAsync(ws => ws.WorkflowId == workflow.Id && (ws.IsFirstState ?? false) && !ws.IsDeleted);
                if (firstWorkflowState == null)
                    throw new InvalidOperationException($"No first state defined in the workflow with code {workflowCode}. Cannot create a new workflow instance.");

                // Check if workflow input code types received are valid and get the valid workflow input code types
                (var validWorkflowInputCodeTypesForNewInstance, var moreWorkflowInputTypeCodesThanNecessaryReceivedForNewInstance) =
                    await GetValidWorkflowInputCodeTypesAndValidateReceivedValues(workflow, firstWorkflowState, workflowInputCodeTypeXvalue,
                    createWorkflowInstance: true, cancellationToken);

                workflowInstance = new WorkflowInstance
                {
                    WorkflowId = workflow.Id,
                    CurrentStateId = firstWorkflowState.Id,
                };
                await workflowInstanceRepository.AddAsync(workflowInstance, cancellationToken);

                foreach (var pair in workflowInputCodeTypeXvalue)
                {
                    // Ignore missing code or value
                    if (string.IsNullOrWhiteSpace(pair.Key) || string.IsNullOrWhiteSpace(pair.Value))
                        continue;

                    var workflowInputCodeType = validWorkflowInputCodeTypesForNewInstance.FirstOrDefault(wict => wict.Code == pair.Key);
                    if (workflowInputCodeType == null) // This should be already checked before but re-check here anyway
                        throw new InvalidOperationException($"Invalid workflow input code type {pair.Key} " +
                            $"for workflow with code {workflow.Code}. Cannot create a new workflow instance.");

                    var workflowInstanceInputCode = new WorkflowInstanceInputCode
                    {
                        InstanceId = workflowInstance.Id,
                        TypeId = workflowInputCodeType.Id,
                        Value = pair.Value,
                    };
                    await workflowInstanceInputCodeRepository.AddAsync(workflowInstanceInputCode, cancellationToken);
                }

                // There is no workflow transition executed when the workflow instance is first created
                // Also, because of that, there's no workflow instance history with it's associated input codes
                // (workflow instance history defines a transition with a current state and a next state with both states required)
            }
            else // If the workflow instance exists it is already loaded and a transition must be executed
            {
                // Execute workflow transition if all input codes for the current state of the instance are provided

                // Get all possible transitions starting from the current workflow instance state
                var executableTransitions = await workflowTransitionRepository.DbSet
                    .Where(wt => wt.WorkflowId == workflow.Id && wt.CurrentStateId == workflowInstance.CurrentStateId && !wt.IsDeleted)
                    .Include(wt => wt.NextState)
                    .ToListAsync(cancellationToken);

                var currentWorkflowState = await workflowStateRepository.GetByIdAsync(workflowInstance.CurrentStateId, new List<string>(), cancellationToken);
                if (currentWorkflowState == null)
                    throw new InvalidOperationException($"There is no start state with id {workflowInstance.Id} for the workflow with code {workflow.Code}. " +
                        $"Cannot execute transition for the workflow instance with code {workflowInstance.Code}.");

                if (executableTransitions == null || !executableTransitions.Any())
                    throw new InvalidOperationException($"There are no transitions with the start state with code {currentWorkflowState.Code}" +
                        $" for the workflow with code {workflow.Code}. Cannot execute the workflow transition.");

                // Validate workflow input type codes the same way as when the workflow instance is first created
                // (the only difference is that the workflow input type codes are checked for the next state of each transition
                // resulting in a list of valid transitions (with possible extra info: transitions with exact match of workflow input type codes
                // (in this case this is the preffered executed transition), or transitions with required workflow input type codes but others too
                // (if there's no exact match, then the first of this type is the executed transition)).
                WorkflowTransition exactMatchInputTypeCodesWorkflowTransition = null;
                WorkflowTransition moreThanRequiredInputTypeCodesWorkflowTransition = null;
                List<WorkflowInputCodeType> exactMatchInputTypeCodesWorkflowTransitionWorkflowInputCodeTypes = null;
                List<WorkflowInputCodeType> moreThanRequiredInputTypeCodesWorkflowTransitionWorkflowInputCodeTypes = null;

                foreach (var transition in executableTransitions)
                {
                    var nextWorkflowState = transition.NextState;
                    if (nextWorkflowState == null)
                        throw new InvalidOperationException("The transition next state should be already loaded. Cannot execute workflow transition.");

                    try
                    {
                        (var validWorkflowInputCodeTypesForTransition, var moreWorkflowInputTypeCodesThanNecessaryReceivedForTransition) =
                            await GetValidWorkflowInputCodeTypesAndValidateReceivedValues(workflow, nextWorkflowState, workflowInputCodeTypeXvalue,
                            createWorkflowInstance: false, cancellationToken);

                        if (exactMatchInputTypeCodesWorkflowTransition == null && !moreWorkflowInputTypeCodesThanNecessaryReceivedForTransition)
                        {
                            exactMatchInputTypeCodesWorkflowTransition = transition;
                            exactMatchInputTypeCodesWorkflowTransitionWorkflowInputCodeTypes = validWorkflowInputCodeTypesForTransition;
                            break; // First transition with exact match of the required input code types is all that is necessary, so exiting the loop here.
                        }

                        if (moreThanRequiredInputTypeCodesWorkflowTransition == null && moreWorkflowInputTypeCodesThanNecessaryReceivedForTransition)
                        {
                            moreThanRequiredInputTypeCodesWorkflowTransition = transition;
                            moreThanRequiredInputTypeCodesWorkflowTransitionWorkflowInputCodeTypes = validWorkflowInputCodeTypesForTransition;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Debug($"Validating a workflow transition with the current received workflow input code types failed with exception {ex}.");
                        continue;
                    }
                }

                var transitionToExecute = exactMatchInputTypeCodesWorkflowTransition ?? moreThanRequiredInputTypeCodesWorkflowTransition;
                if (transitionToExecute == null)
                    throw new InvalidOperationException($"Cannot execute transition for workflow with code {workflow.Code} " +
                        $"in state with code {currentWorkflowState.Code} because there's no valid transition regarding workflow input type codes.");

                var validWorkflowInputCodeTypes = exactMatchInputTypeCodesWorkflowTransitionWorkflowInputCodeTypes
                    ?? moreThanRequiredInputTypeCodesWorkflowTransitionWorkflowInputCodeTypes;
                if (validWorkflowInputCodeTypes == null)
                    throw new InvalidOperationException($"Cannot execute transition for workflow with code {workflow.Code} " +
                        $"in state with code {currentWorkflowState.Code} because there's no valid workflow input code types list.");

                var nextStateId = transitionToExecute.NextStateId;
                if (nextStateId == null)
                    throw new InvalidOperationException($"Cannot execute workflow transition for workflow with code {workflow.Code} because the next state is null.");

                // TODO: Validate if user has allocated the user role assigned to transition

                // Fill workflow instance history
                var workflowInstanceHistory = new WorkflowInstanceHistory()
                {
                    InstanceId = workflowInstance.Id,
                    CurrentStateId = workflowInstance.CurrentStateId,
                    NextStateId = nextStateId,
                };
                await workflowInstanceHistoryRepository.UpdateLoadedAsync(workflowInstanceHistory, cancellationToken);

                // Update workflow instance state based on the transition executed
                workflowInstance.CurrentStateId = nextStateId;
                await workflowInstanceRepository.UpdateAsync(workflowInstance, cancellationToken);

                // Update workflow instance input code
                foreach (var pair in workflowInputCodeTypeXvalue)
                {
                    // Ignore missing code or value
                    if (string.IsNullOrWhiteSpace(pair.Key) || string.IsNullOrWhiteSpace(pair.Value))
                        continue;

                    var workflowInputCodeType = validWorkflowInputCodeTypes.FirstOrDefault(wict => wict.Code == pair.Key);
                    if (workflowInputCodeType == null) // This should be already checked before but re-check here anyway
                        throw new InvalidOperationException($"Invalid workflow input code type {pair.Key} " +
                            $"for workflow with code {workflow.Code}. Cannot create a new workflow instance.");

                    var workflowInstanceInputCode = await workflowInstanceInputCodeRepository.DbSet
                        .FirstOrDefaultAsync(wiic => wiic.InstanceId == workflowInstance.Id && wiic.TypeId == workflowInputCodeType.Id && !wiic.IsDeleted);
                    if (workflowInstanceInputCode == null)
                    {
                        // add WorkflowInstanceInputCode if not exists
                        workflowInstanceInputCode = new WorkflowInstanceInputCode
                        {
                            InstanceId = workflowInstance.Id,
                            TypeId = workflowInputCodeType.Id,
                            Value = pair.Value,
                        };
                        await workflowInstanceInputCodeRepository.AddAsync(workflowInstanceInputCode, cancellationToken);
                    }
                    else
                    {
                        // or else update existing WorkflowInstanceInputCode
                        workflowInstanceInputCode.Value = pair.Value;
                        await workflowInstanceInputCodeRepository.UpdateLoadedAsync(workflowInstanceInputCode, cancellationToken);
                    }

                    // Fill workflow instance history input code
                    // (note that the first state doesn't keep a record in history so there's no WorkflowInstanceHistoryInputCode collection for it)
                    var workflowInstanceHistoryInputCode = new WorkflowInstanceHistoryInputCode()
                    {
                        InstanceId = workflowInstance.Id,
                        InputCodeTypeId = workflowInputCodeType.Id,
                        Value = pair.Value
                    };
                    await workflowInstanceHistoryInputCodeRepository.UpdateLoadedAsync(workflowInstanceHistoryInputCode, cancellationToken);
                }
            }
        }

        private async Task<(List<WorkflowInputCodeType>, bool)> GetValidWorkflowInputCodeTypesAndValidateReceivedValues(Workflow workflow, WorkflowState workflowState,
            Dictionary<string, string> workflowInputCodeTypeXvalue, bool createWorkflowInstance,
            CancellationToken cancellationToken)
        {
            var validWorkflowInputCodeTypes = await workflowInputCodeTypeRepository.DbSet
                .Where(wict => wict.WorkflowId == workflow.Id && !wict.IsDeleted)
                .ToListAsync();

            var noInputCodeTypes = validWorkflowInputCodeTypes == null || !validWorkflowInputCodeTypes.Any();
            var moreWorkflowInputTypeCodesThanNeecessaryReceived = false;

            if (!noInputCodeTypes)
            {
                var workflowInputCodeTypesForCurrentState = await workflowStateInputCodeTypeRepository.DbSet
                    .Where(wsict => wsict.StateId == workflowState.Id && !wsict.IsDeleted)
                    .ToListAsync(cancellationToken);

                if (workflowInputCodeTypesForCurrentState == null || !workflowInputCodeTypesForCurrentState.Any())
                    noInputCodeTypes = true; // No input code types for current state
                else
                {
                    var workflowInputTypeCodesReceived = new List<string>();

                    // Validate there are no duplicate workflow input type codes received
                    foreach (var pair in workflowInputCodeTypeXvalue)
                    {
                        // Ignore missing code or value
                        if (string.IsNullOrWhiteSpace(pair.Key) || string.IsNullOrWhiteSpace(pair.Value))
                            continue;

                        if (workflowInputTypeCodesReceived.Contains(pair.Key))
                            throw new InvalidOperationException(
                                $"{(createWorkflowInstance ? "Cannot create a workflow instance" : "Cannot execute a workflow transition")} " +
                                $"with duplicate workflow input type codes received.");

                        workflowInputTypeCodesReceived.Add(pair.Key);
                    }

                    var workflowInputCodeTypeXValueCount = 0;

                    // Validate workflow input code types received are for the current workflow
                    foreach (var pair in workflowInputCodeTypeXvalue)
                    {
                        // Ignore missing code or value
                        if (string.IsNullOrWhiteSpace(pair.Key) || string.IsNullOrWhiteSpace(pair.Value))
                            continue;

                        // Workflow input code type (pair.Key) must be from the current workflow (validWorkflowInputCodeTypes)

                        if (!validWorkflowInputCodeTypes.Any(wict => wict.Code == pair.Key))
                            throw new InvalidOperationException(
                                $"{(createWorkflowInstance ? "Cannot create a workflow instance" : "Cannot execute a workflow transition")} " +
                                $"for workflow with code {workflow.Code} " +
                                $"attaching an invalid workflow input code type {pair.Key}.");

                        workflowInputCodeTypeXValueCount++; // Increment the count for a valid workflow input code type
                    }

                    // Check if received workflow input code types cover all required codes for the current state
                    foreach (var inputCodeType in workflowInputCodeTypesForCurrentState)
                    {
                        var workflowInputCodeType = validWorkflowInputCodeTypes.FirstOrDefault(wict => wict.Id == inputCodeType.Id);
                        if (workflowInputCodeType == null)
                            throw new InvalidOperationException(
                                $"{(createWorkflowInstance ? "Cannot create a workflow instance" : "Cannot execute a workflow transition")}" +
                                $" of the workflow with code {workflow.Code} " +
                                $"because the first state of the workflow (state has code {workflowState.Code}) " +
                                $"has associated an invalid workflow input code type id {inputCodeType.Id}.");

                        if (!workflowInputCodeTypeXvalue.Any(pair => pair.Key == workflowInputCodeType.Code && !string.IsNullOrWhiteSpace(pair.Value)))
                            throw new InvalidOperationException(
                                $"{(createWorkflowInstance ? "Cannot create a workflow instance" : "Cannot execute a workflow transition")} " +
                                $"for workflow with code {workflow.Code} because the workflow input code type with code " +
                                $"{workflowInputCodeType.Code} doesn't have a value.");
                    }

                    moreWorkflowInputTypeCodesThanNeecessaryReceived = workflowInputCodeTypeXValueCount > workflowInputCodeTypesForCurrentState.Count;
                }
            }

            return (validWorkflowInputCodeTypes, moreWorkflowInputTypeCodesThanNeecessaryReceived);
        }

        public void Dispose()
        {
            workflowRepository.Dispose();
            workflowInputCodeTypeRepository.Dispose();
            workflowStateRepository.Dispose();
            workflowStateInputCodeTypeRepository.Dispose();
            workflowTransitionRepository.Dispose();
            workflowInstanceRepository.Dispose();
            workflowInstanceInputCodeRepository.Dispose();
            workflowInstanceHistoryRepository.Dispose();
            workflowInstanceHistoryInputCodeRepository.Dispose();
        }
    }
}