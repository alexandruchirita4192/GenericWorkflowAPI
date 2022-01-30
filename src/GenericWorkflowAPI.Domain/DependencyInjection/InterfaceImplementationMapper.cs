using System;

namespace GenericWorkflowAPI.Domain
{
    public class InterfaceImplementationMapper
    {
        public Type Interface { get; private set; }
        public Type Implementation { get; private set; }

        public InterfaceImplementationMapper(Type _interface, Type _implementation)
        {
            if (_interface == null)
                throw new ArgumentNullException(nameof(_interface));
            if (_implementation == null)
                throw new ArgumentNullException(nameof(_implementation));

            Interface = _interface;
            Implementation = _implementation;
        }

        #region Comparison operators

        public static bool operator ==(InterfaceImplementationMapper a, InterfaceImplementationMapper b)
        {
            return a.Interface == b.Interface && a.Implementation == b.Implementation;
        }

        public static bool operator !=(InterfaceImplementationMapper a, InterfaceImplementationMapper b)
        {
            return a.Interface != b.Interface || a.Implementation != b.Implementation;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as InterfaceImplementationMapper;
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