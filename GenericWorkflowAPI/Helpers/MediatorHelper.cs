using System;
using System.Collections.Generic;
using System.Linq;
using GenericWorkflowAPI.CommandHandlers;
using GenericWorkflowAPI.Controllers.v1;
using GenericWorkflowAPI.Domain;
using GenericWorkflowAPI.Domain.Requests;
using GenericWorkflowAPI.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace GenericWorkflowAPI.Helpers
{
    /// <summary>
    /// Gets MediatR handlers based on controllers with their generic arguments
    /// </summary>
    public static class MediatorHelper
    {
        public static List<InterfaceImplementationMapper> GetMappings(ILogger logger)
        {
            var mappings = new List<InterfaceImplementationMapper>();

            var types = GetControllerTypes();
            if (types == null || types.Count == 0)
                return mappings;

            foreach (var type in types)
            {
                if (type.BaseType?.GetGenericArguments()?.Length != 2)
                    continue;

                var entityType = type.BaseType.GetGenericArguments()[0];
                var dtoType = type.BaseType.GetGenericArguments()[1];

                Type requestType;
                Type responseType;
                Type interfaceType;
                Type implementedType;

                var genericCrudControllerType = typeof(GenericCRUDController<,>).MakeGenericType(entityType, dtoType);
                if (genericCrudControllerType.IsAssignableFrom(type))
                {
                    // Example handling input GenericApiResponse<string>:
                    var genericApiResponseHandlerStringMapper = new InterfaceImplementationMapper(
                            typeof(IRequestHandler<GenericApiResponse<string>, ActionResult>),
                            typeof(GenericApiResponseHandler<string>)
                        );
                    if (!mappings.Contains(genericApiResponseHandlerStringMapper))
                        mappings.Add(genericApiResponseHandlerStringMapper);

                    // Example handling input GenericCreateListRequest:
                    //services.AddScoped(typeof(IRequestHandler<GenericCreateListRequest<WorkflowDto>, GenericApiResponse<string>>),
                    //    typeof(GenericCreateListCommandHandler<Workflow, WorkflowDto>));

                    // GenericCreateListRequest<TDto>:
                    WrapInTryCatch(() =>
                    {
                        requestType = typeof(GenericCreateListRequest<>).MakeGenericType(dtoType);
                        responseType = typeof(GenericApiResponse<string>);
                        interfaceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
                        implementedType = typeof(GenericCreateListCommandHandler<,>).MakeGenericType(entityType, dtoType);

                        var createListCommandHandlerMapper = new InterfaceImplementationMapper(interfaceType, implementedType);
                        if (!interfaceType.IsAssignableFrom(implementedType))
                            throw new InvalidOperationException($"Implementation {implementedType} doesn't implement the interface {interfaceType}");
                        if (!mappings.Contains(createListCommandHandlerMapper))
                            mappings.Add(createListCommandHandlerMapper);
                    }, logger);

                    // Example handling input GenericCreateRequest:
                    //services.AddScoped(typeof(IRequestHandler<GenericCreateRequest<WorkflowDto>, GenericApiResponse<string>>),
                    //    typeof(GenericCreateCommandHandler<Workflow, WorkflowDto>));

                    // GenericCreateRequest<TDto>:
                    WrapInTryCatch(() =>
                    {
                        requestType = typeof(GenericCreateRequest<>).MakeGenericType(dtoType);
                        responseType = typeof(GenericApiResponse<string>);
                        interfaceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
                        implementedType = typeof(GenericCreateCommandHandler<,>).MakeGenericType(entityType, dtoType);

                        var createCommandHandlerMapper = new InterfaceImplementationMapper(interfaceType, implementedType);
                        if (!interfaceType.IsAssignableFrom(implementedType))
                            throw new InvalidOperationException($"Implementation {implementedType} doesn't implement the interface {interfaceType}");
                        if (!mappings.Contains(createCommandHandlerMapper))
                            mappings.Add(createCommandHandlerMapper);
                    }, logger);

                    // Example handling input GenericDeleteListRequest:
                    //services.AddScoped(typeof(IRequestHandler<GenericDeleteListRequest<WorkflowDto>, GenericApiResponse<string>>),
                    //    typeof(GenericDeleteListCommandHandler<Workflow, WorkflowDto>));

                    // GenericDeleteListRequest<TDto>:
                    WrapInTryCatch(() =>
                    {
                        requestType = typeof(GenericDeleteListRequest<>).MakeGenericType(dtoType);
                        responseType = typeof(GenericApiResponse<string>);
                        interfaceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
                        implementedType = typeof(GenericDeleteListCommandHandler<,>).MakeGenericType(entityType, dtoType);

                        var deleteListCommandHandlerMapper = new InterfaceImplementationMapper(interfaceType, implementedType);
                        if (!interfaceType.IsAssignableFrom(implementedType))
                            throw new InvalidOperationException($"Implementation {implementedType} doesn't implement the interface {interfaceType}");
                        if (!mappings.Contains(deleteListCommandHandlerMapper))
                            mappings.Add(deleteListCommandHandlerMapper);
                    }, logger);

                    // Example handling input GenericDeleteRequest:
                    //services.AddScoped(typeof(IRequestHandler<GenericDeleteRequest<WorkflowDto>, GenericApiResponse<string>>),
                    //    typeof(GenericDeleteCommandHandler<Workflow, WorkflowDto>));

                    // GenericDeleteRequest<TDto>:
                    WrapInTryCatch(() =>
                    {
                        requestType = typeof(GenericDeleteRequest<>).MakeGenericType(dtoType);
                        responseType = typeof(GenericApiResponse<string>);
                        interfaceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
                        implementedType = typeof(GenericDeleteCommandHandler<,>).MakeGenericType(entityType, dtoType);

                        var deleteCommandHandlerMapper = new InterfaceImplementationMapper(interfaceType, implementedType);
                        if (!interfaceType.IsAssignableFrom(implementedType))
                            throw new InvalidOperationException($"Implementation {implementedType} doesn't implement the interface {interfaceType}");
                        if (!mappings.Contains(deleteCommandHandlerMapper))
                            mappings.Add(deleteCommandHandlerMapper);
                    }, logger);

                    // Example handling input GenericUpdateListRequest:
                    //services.AddScoped(typeof(IRequestHandler<GenericUpdateListRequest<WorkflowDto>, GenericApiResponse<string>>),
                    //    typeof(GenericUpdateListCommandHandler<Workflow, WorkflowDto>));

                    // GenericUpdateListRequest<TDto>:
                    WrapInTryCatch(() =>
                    {
                        requestType = typeof(GenericUpdateListRequest<>).MakeGenericType(dtoType);
                        responseType = typeof(GenericApiResponse<string>);
                        interfaceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
                        implementedType = typeof(GenericUpdateListCommandHandler<,>).MakeGenericType(entityType, dtoType);

                        var updateListCommandHandlerMapper = new InterfaceImplementationMapper(interfaceType, implementedType);
                        if (!interfaceType.IsAssignableFrom(implementedType))
                            throw new InvalidOperationException($"Implementation {implementedType} doesn't implement the interface {interfaceType}");
                        if (!mappings.Contains(updateListCommandHandlerMapper))
                            mappings.Add(updateListCommandHandlerMapper);
                    }, logger);

                    // Example handling input GenericUpdateRequest:
                    //services.AddScoped(typeof(IRequestHandler<GenericUpdateRequest<WorkflowDto>, GenericApiResponse<string>>),
                    //    typeof(GenericUpdateCommandHandler<Workflow, WorkflowDto>));

                    // GenericUpdateRequest<TDto>:
                    WrapInTryCatch(() =>
                    {
                        requestType = typeof(GenericUpdateRequest<>).MakeGenericType(dtoType);
                        responseType = typeof(GenericApiResponse<string>);
                        interfaceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
                        implementedType = typeof(GenericUpdateCommandHandler<,>).MakeGenericType(entityType, dtoType);

                        var updateCommandHandlerMapper = new InterfaceImplementationMapper(interfaceType, implementedType);
                        if (!interfaceType.IsAssignableFrom(implementedType))
                            throw new InvalidOperationException($"Implementation {implementedType} doesn't implement the interface {interfaceType}");
                        if (!mappings.Contains(updateCommandHandlerMapper))
                            mappings.Add(updateCommandHandlerMapper);
                    }, logger);
                }

                var genericGetControllerType = typeof(GenericGetController<,>).MakeGenericType(entityType, dtoType);
                if (genericGetControllerType.IsAssignableFrom(type))
                {
                    // Example handling input GenericApiResponse<TDto>:
                    //services.AddScoped(typeof(IRequestHandler<GenericApiResponse<WorkflowDto>, ActionResult>), typeof(GenericApiResponseHandler<WorkflowDto>));

                    // GenericApiResponse<TDto>:
                    WrapInTryCatch(() =>
                    {
                        requestType = typeof(GenericApiResponse<>).MakeGenericType(dtoType);
                        responseType = typeof(ActionResult);
                        interfaceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
                        implementedType = typeof(GenericApiResponseHandler<>).MakeGenericType(dtoType);

                        var genericApiResponseHandlerMapper = new InterfaceImplementationMapper(interfaceType, implementedType);
                        if (!interfaceType.IsAssignableFrom(implementedType))
                            throw new InvalidOperationException($"Implementation {implementedType} doesn't implement the interface {interfaceType}");
                        if (!mappings.Contains(genericApiResponseHandlerMapper))
                            mappings.Add(genericApiResponseHandlerMapper);
                    }, logger);

                    // Example handling input GenericGetRequest<TDto>:
                    //services.AddScoped(typeof(IRequestHandler<GenericGetRequest<WorkflowDto>, GenericApiResponse<WorkflowDto>>),
                    //    typeof(GenericGetCommandHandler<Workflow, WorkflowDto>));

                    // GenericGetRequest<TDto>:
                    WrapInTryCatch(() =>
                    {
                        requestType = typeof(GenericGetRequest<>).MakeGenericType(dtoType);
                        responseType = typeof(GenericApiResponse<>).MakeGenericType(dtoType);
                        interfaceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
                        implementedType = typeof(GenericGetCommandHandler<,>).MakeGenericType(entityType, dtoType);

                        var getCommandHandlerMapper = new InterfaceImplementationMapper(interfaceType, implementedType);
                        if (!interfaceType.IsAssignableFrom(implementedType))
                            throw new InvalidOperationException($"Implementation {implementedType} doesn't implement the interface {interfaceType}");
                        if (!mappings.Contains(getCommandHandlerMapper))
                            mappings.Add(getCommandHandlerMapper);
                    }, logger);
                }

                var genericOnlyGetAllControllerType = typeof(GenericOnlyGetAllController<,>).MakeGenericType(entityType, dtoType);
                if (genericOnlyGetAllControllerType.IsAssignableFrom(type))
                {
                    var listDtoType = typeof(List<>).MakeGenericType(dtoType);

                    // Example handling input GenericApiResponse<List<TDto>>:
                    //services.AddScoped(typeof(IRequestHandler<GenericApiResponse<List<WorkflowDto>>, ActionResult>), typeof(GenericApiResponseHandler<List<WorkflowDto>>));

                    // GenericApiResponse<List<TDto>>:
                    WrapInTryCatch(() =>
                    {
                        requestType = typeof(GenericApiResponse<>).MakeGenericType(listDtoType);
                        responseType = typeof(ActionResult);
                        interfaceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
                        implementedType = typeof(GenericApiResponseHandler<>).MakeGenericType(listDtoType);

                        var genericApiResponseHandlerListMapper = new InterfaceImplementationMapper(interfaceType, implementedType);
                        if (!interfaceType.IsAssignableFrom(implementedType))
                            throw new InvalidOperationException($"Implementation {implementedType} doesn't implement the interface {interfaceType}");
                        if (!mappings.Contains(genericApiResponseHandlerListMapper))
                            mappings.Add(genericApiResponseHandlerListMapper);
                    }, logger);

                    // Example handling input GenericGetListRequest<TDto>:
                    //services.AddScoped(typeof(IRequestHandler<GenericGetListRequest<WorkflowDto>, GenericApiResponse<List<WorkflowDto>>>),
                    //    typeof(GenericGetListCommandHandler<Workflow, WorkflowDto>));

                    // GenericGetListRequest<TDto>:
                    WrapInTryCatch(() =>
                    {
                        requestType = typeof(GenericGetListRequest<>).MakeGenericType(dtoType);
                        responseType = typeof(GenericApiResponse<>).MakeGenericType(listDtoType);
                        interfaceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
                        implementedType = typeof(GenericGetListCommandHandler<,>).MakeGenericType(entityType, dtoType);

                        var getListCommandHandlerMapper = new InterfaceImplementationMapper(interfaceType, implementedType);
                        if (!interfaceType.IsAssignableFrom(implementedType))
                            throw new InvalidOperationException($"Implementation {implementedType} doesn't implement the interface {interfaceType}");
                        if (!mappings.Contains(getListCommandHandlerMapper))
                            mappings.Add(getListCommandHandlerMapper);
                    }, logger);
                }
            }
            return mappings;
        }

        private static List<Type> GetControllerTypes()
        {
            return typeof(Program)
                .Assembly
                .GetTypes()
                .Where(t => t.IsClass
                    && t.IsPublic
                    && !t.IsAbstract
                    && t.IsAssignableTo(typeof(ControllerBase))
                    && t.BaseType?.GetGenericArguments()?.Length == 2).ToList();
        }

        /// <summary>
        /// It might not be the most performant (creating closures with <paramref name="action"/>) but this way I won't repeat code.
        /// </summary>
        private static void WrapInTryCatch(Action action, ILogger logger)
        {
            try
            {
                if (action != null)
                    action();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{nameof(WrapInTryCatch)} exception");
            }
        }
    }
}