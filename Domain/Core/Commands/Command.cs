using FluentValidation.Results;
using MediatR;
using Newtonsoft.Json;
using System;

namespace QAP4.Domain.Core.Commands
{
    public abstract class Command : IRequest
    {

        [JsonIgnore]
        public string MessageType { get; set; }
        public ValidationResult ValidationResult { get; set; }
        public abstract bool IsValid();
        protected Command()
        {
            MessageType = GetType().Name;
        }
    }
}
