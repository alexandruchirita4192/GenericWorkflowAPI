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
            Interface = _interface ?? throw new ArgumentNullException(nameof(_interface));
            Implementation = _implementation ?? throw new ArgumentNullException(nameof(_implementation));
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
            if (obj is not ServiceInterfaceImplementationPair other)
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