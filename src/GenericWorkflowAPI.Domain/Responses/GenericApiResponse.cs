using System.Collections.Generic;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GenericWorkflowAPI.Domain.Responses
{
    public class GenericApiResponse<TPayload> : IRequest<ActionResult>
        where TPayload : class
    {
        #region Constructors

        private GenericApiResponse(TPayload payload, HttpStatusCode status)
        {
            Payload = payload;
            Status = status;
        }

        private GenericApiResponse(TPayload payload)
        {
            Status = HttpStatusCode.OK;
            Payload = payload;
        }

        private GenericApiResponse(HttpStatusCode status)
        {
            Status = status;
        }

        private GenericApiResponse(string failureMessage, HttpStatusCode status)
        {
            Message = failureMessage;
            Status = status;
        }

        public GenericApiResponse(string message, HttpStatusCode status, Dictionary<string, object> extensions)
        {
            Message = message;
            Status = status;
            Extensions = extensions;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The payload sent in the body (in ActionResult)
        /// </summary>
        public TPayload Payload { get; private set; }

        /// <summary>
        /// The status code set to the ActionResult
        /// </summary>
        public HttpStatusCode? Status { get; private set; }

        /// <summary>
        /// Used later in the <see cref="ProblemDetails"/> DTO to specify the Detail of the error occured
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Used later in the <see cref="ProblemDetails"/> DTO to point to the invalid properties with their error
        /// </summary>
        public Dictionary<string, object> Extensions { get; set; }

        #endregion Properties

        #region Methods

        public static GenericApiResponse<TPayload> Ok() => new(HttpStatusCode.OK);

        public static GenericApiResponse<TPayload> Created(TPayload payload) => new(payload, HttpStatusCode.Created);

        public static GenericApiResponse<TPayload> Ok(TPayload payload) => new(payload);

        public static GenericApiResponse<TPayload> Problem(HttpStatusCode status) => new(status);

        public static GenericApiResponse<TPayload> Problem(string message, HttpStatusCode status) => new(message, status);

        public static GenericApiResponse<TPayload> Problem(string message, HttpStatusCode status, Dictionary<string, object> extensions) => new(message, status, extensions);

        public static GenericApiResponse<TPayload> NoContent() => new(HttpStatusCode.NoContent);

        #endregion Methods
    }
}