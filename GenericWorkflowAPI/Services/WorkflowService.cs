using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Domain.Requests;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace GenericWorkflowAPI.Services
{
    public class WorkflowService : IWorkflowService, IDisposable
    {
        protected readonly ILogger _logger;
        protected readonly IGenericCodeRepository<Workflow> _workflowRepository;
        protected readonly IGenericCodeRepository<WorkflowInputCodeType> _workflowInputCodeTypeRepository;
        protected readonly IGenericCodeRepository<WorkflowState> _workflowStateRepository;
        protected readonly IGenericCodeRepository<WorkflowStateInputCodeType> _workflowStateInputCodeTypeRepository;
        protected readonly IGenericCodeRepository<WorkflowTransition> _workflowTransitionRepository;
        protected readonly IGenericCodeRepository<WorkflowInstance> _workflowInstanceRepository;
        protected readonly IGenericRepository<WorkflowInstanceInputCode> _workflowInstanceInputCodeRepository;
        protected readonly IGenericRepository<WorkflowInstanceHistory> _workflowInstanceHistoryRepository;
        protected readonly IGenericRepository<WorkflowInstanceHistoryInputCode> _workflowInstanceHistoryInputCodeRepository;
        protected readonly IGenericCodeRepository<IdentityRole> _identityRoleRepository;

        public WorkflowService(ILogger logger,
            IGenericCodeRepository<Workflow> workflowRepository,
            IGenericCodeRepository<WorkflowInputCodeType> workflowInputCodeTypeRepository,
            IGenericCodeRepository<WorkflowState> workflowStateRepository,
            IGenericCodeRepository<WorkflowStateInputCodeType> workflowStateInputCodeTypeRepository,
            IGenericCodeRepository<WorkflowTransition> workflowTransitionRepository,
            IGenericCodeRepository<WorkflowInstance> workflowInstanceRepository,
            IGenericRepository<WorkflowInstanceInputCode> workflowInstanceInputCodeRepository,
            IGenericRepository<WorkflowInstanceHistory> workflowInstanceHistoryRepository,
            IGenericRepository<WorkflowInstanceHistoryInputCode> workflowInstanceHistoryInputCodeRepository,
            IGenericCodeRepository<IdentityRole> identityRoleRepository)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (workflowRepository == null)
                throw new ArgumentNullException(nameof(workflowRepository));
            if (workflowInputCodeTypeRepository == null)
                throw new ArgumentNullException(nameof(workflowInputCodeTypeRepository));
            if (workflowStateRepository == null)
                throw new ArgumentNullException(nameof(workflowStateRepository));
            if (workflowStateInputCodeTypeRepository == null)
                throw new ArgumentNullException(nameof(workflowStateInputCodeTypeRepository));
            if (workflowTransitionRepository == null)
                throw new ArgumentNullException(nameof(workflowTransitionRepository));
            if (workflowInstanceRepository == null)
                throw new ArgumentNullException(nameof(workflowInstanceRepository));
            if (workflowInstanceInputCodeRepository == null)
                throw new ArgumentNullException(nameof(workflowInstanceInputCodeRepository));
            if (workflowInstanceHistoryRepository == null)
                throw new ArgumentNullException(nameof(workflowInstanceHistoryRepository));
            if (workflowInstanceHistoryInputCodeRepository == null)
                throw new ArgumentNullException(nameof(workflowInstanceHistoryInputCodeRepository));
            if (identityRoleRepository == null)
                throw new ArgumentNullException(nameof(identityRoleRepository));

            _logger = logger;
            _workflowRepository = workflowRepository;
            _workflowInputCodeTypeRepository = workflowInputCodeTypeRepository;
            _workflowStateRepository = workflowStateRepository;
            _workflowStateInputCodeTypeRepository = workflowStateInputCodeTypeRepository;
            _workflowTransitionRepository = workflowTransitionRepository;
            _workflowInstanceRepository = workflowInstanceRepository;
            _workflowInstanceInputCodeRepository = workflowInstanceInputCodeRepository;
            _workflowInstanceHistoryRepository = workflowInstanceHistoryRepository;
            _workflowInstanceHistoryInputCodeRepository = workflowInstanceHistoryInputCodeRepository;
            _identityRoleRepository = identityRoleRepository;
        }

        public async Task Run(ExecuteWorkflowRequest executeWorkflowRequest, CancellationToken cancellationToken)
        {
            if (executeWorkflowRequest == null)
                throw new ArgumentNullException(nameof(executeWorkflowRequest),
                    $"Cannot initialize a workflow instance or execute a workflow transition using a null request.");
            if (executeWorkflowRequest.User == null)
                throw new ArgumentNullException(nameof(executeWorkflowRequest.User),
                    $"Cannot initialize a workflow instance or execute a workflow transition using a null user.");

            var workflowCode = executeWorkflowRequest.WorkflowCode;
            if (string.IsNullOrWhiteSpace(workflowCode))
                throw new ArgumentNullException(nameof(workflowCode));

            var workflowInstanceCode = executeWorkflowRequest.WorkflowInstanceCode;
            var workflowInputCodeTypeXvalue = executeWorkflowRequest.WorkflowInputCodeTypeXvalue;

            var workflow = await _workflowRepository.GetByCodeAsync(workflowCode, new List<string>(), cancellationToken);
            if (workflow == null)
                throw new InvalidOperationException($"Invalid {nameof(workflowCode)} {workflowCode} received. No workflow exists with that code.");

            // Try to get workflow instance by code
            var workflowInstance = await _workflowInstanceRepository.GetByCodeAsync(workflowInstanceCode, new List<string>(), cancellationToken);
            if (workflowInstance == null) // If workflow instance doesn't exist then a new one must be created
            {
                // Try to create a new instance if all input codes for the first state are provided
                var firstWorkflowState = await _workflowStateRepository.DbSet.FirstOrDefaultAsync(ws => ws.WorkflowId == workflow.Id && (ws.IsFirstState ?? false) && !ws.IsDeleted);
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
                await _workflowInstanceRepository.AddAsync(workflowInstance, executeWorkflowRequest.User, cancellationToken);

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
                    await _workflowInstanceInputCodeRepository.AddAsync(workflowInstanceInputCode, executeWorkflowRequest.User, cancellationToken);
                }

                // There is no workflow transition executed when the workflow instance is first created
                // Also, because of that, there's no workflow instance history with it's associated input codes
                // (workflow instance history defines a transition with a current state and a next state with both states required)
            }
            else // If the workflow instance exists it is already loaded and a transition must be executed
            {
                // Execute workflow transition if all input codes for the current state of the instance are provided

                // Get all possible transitions starting from the current workflow instance state
                var executableTransitions = await _workflowTransitionRepository.DbSet
                    .Where(wt => wt.WorkflowId == workflow.Id && wt.CurrentStateId == workflowInstance.CurrentStateId && !wt.IsDeleted)
                    .Include(wt => wt.NextState)
                    .ToListAsync(cancellationToken);

                var currentWorkflowState = await _workflowStateRepository.GetByIdAsync(workflowInstance.CurrentStateId, new List<string>(), cancellationToken);
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
                        _logger.Debug($"Validating a workflow transition with the current received workflow input code types failed with exception {ex}.");
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
                await _workflowInstanceHistoryRepository.UpdateLoadedAsync(workflowInstanceHistory, executeWorkflowRequest.User, cancellationToken);

                // Update workflow instance state based on the transition executed
                workflowInstance.CurrentStateId = nextStateId;
                await _workflowInstanceRepository.UpdateAsync(workflowInstance, executeWorkflowRequest.User, cancellationToken);

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

                    var workflowInstanceInputCode = await _workflowInstanceInputCodeRepository.DbSet
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
                        await _workflowInstanceInputCodeRepository.AddAsync(workflowInstanceInputCode, executeWorkflowRequest.User, cancellationToken);
                    }
                    else
                    {
                        // or else update existing WorkflowInstanceInputCode
                        workflowInstanceInputCode.Value = pair.Value;
                        await _workflowInstanceInputCodeRepository.UpdateLoadedAsync(workflowInstanceInputCode, executeWorkflowRequest.User, cancellationToken);
                    }

                    // Fill workflow instance history input code
                    // (note that the first state doesn't keep a record in history so there's no WorkflowInstanceHistoryInputCode collection for it)
                    var workflowInstanceHistoryInputCode = new WorkflowInstanceHistoryInputCode()
                    {
                        InstanceId = workflowInstance.Id,
                        InputCodeTypeId = workflowInputCodeType.Id,
                        Value = pair.Value
                    };
                    await _workflowInstanceHistoryInputCodeRepository.UpdateLoadedAsync(workflowInstanceHistoryInputCode, executeWorkflowRequest.User, cancellationToken);
                }
            }
        }

        private async Task<(List<WorkflowInputCodeType>, bool)> GetValidWorkflowInputCodeTypesAndValidateReceivedValues(Workflow workflow, WorkflowState workflowState,
            Dictionary<string, string> workflowInputCodeTypeXvalue, bool createWorkflowInstance,
            CancellationToken cancellationToken)
        {
            var validWorkflowInputCodeTypes = await _workflowInputCodeTypeRepository.DbSet
                .Where(wict => wict.WorkflowId == workflow.Id && !wict.IsDeleted)
                .ToListAsync();

            var noInputCodeTypes = validWorkflowInputCodeTypes == null || !validWorkflowInputCodeTypes.Any();
            var moreWorkflowInputTypeCodesThanNeecessaryReceived = false;

            if (!noInputCodeTypes)
            {
                var workflowInputCodeTypesForCurrentState = await _workflowStateInputCodeTypeRepository.DbSet
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
            _workflowRepository.Dispose();
            _workflowInputCodeTypeRepository.Dispose();
            _workflowStateRepository.Dispose();
            _workflowStateInputCodeTypeRepository.Dispose();
            _workflowTransitionRepository.Dispose();
            _workflowInstanceRepository.Dispose();
            _workflowInstanceInputCodeRepository.Dispose();
            _workflowInstanceHistoryRepository.Dispose();
            _workflowInstanceHistoryInputCodeRepository.Dispose();
            _identityRoleRepository.Dispose();
        }
    }
}