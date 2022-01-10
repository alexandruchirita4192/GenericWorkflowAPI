using System;

namespace GenericWorkflowAPI.Domain
{
    public class InterfaceImplementationMapper
    {
        public Type Interface { get; private set; }
        public Type Implementation { get; private set; }

        public InterfaceImplementationMapper(Type _interface, Type _implementation)
        {
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

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var other = obj as InterfaceImplementationMapper;
            return other == this;
        }

        #endregion Comparison operators

        public override int GetHashCode()
        {
            return Interface.GetHashCode() + Implementation.GetHashCode();
        }
    }
}