using System;

namespace FactSystem.Runtime
{
    public class Fact<T> : IFact
    {
        #region Publics
        
        
        public T Value;
        public Type ValueType { get; }
        public bool IsPersistent { get; set; }

        public Fact(T value, bool isPersistent = false)
        {
            Value = value;
            ValueType = value.GetType();
            IsPersistent = isPersistent;
        }
        
        
        #endregion
        
        #region Main Method


        public object GetObjectValue() => Value;

        public void SetObjectValue(object value)
        {
            if (value is T cast) Value = cast;
            else throw new ArgumentException($"Cannot cast {value.GetType()} to {typeof(T)}", nameof(value));
        }
        
        
        #endregion

    }
}
