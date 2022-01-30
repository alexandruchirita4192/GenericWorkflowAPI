using System;

namespace GenericWorkflowAPI.Domain
{
    /// <summary>
    /// Service <see cref="Interface"/>-<see cref="Implementation"/> pair.
    /// </summary>
    /// <remarks>Used for MediatR handler services.</remarks>
    public class ServiceInterfaceImplementationPair
    {
        public Type Interface { get; private set; }
        public Type Implementation { get; private set; }

        public ServiceInterfaceImplementationPair(Type _interface, Type _implementation)
        {
            if (_interface == null)
                throw new ArgumentNullException(nameof(_interface));
            if (_implementation == null)
                throw new ArgumentNullException(nameof(_implementation));

            Interface = _interface;
            Implementation = _implementation;
        }

        #region Comparison operators

        public static bool operator ==(ServiceInterfaceImplementationPair a, ServiceInterfaceImplementationPair b)
        {
            return a.Interface == b.Interface && a.Implementation == b.Implementation;
        }

        public static bool operator !=(ServiceInterfaceImplementationPair a, ServiceInterfaceImplementationPair b)
        {
            return a.Interface != b.Interface || a.Implementation != b.Implementation;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as ServiceInterfaceImplementationPair;
            if (other is null)
                return false;
            return other == this;
        }

        #endregion Comparison operators

        public override int GetHashCode()
        {
            return Interface.GetHashCode() + Implementation.GetHashCode();
        }
    }
}