using System;

namespace FactSystem.Runtime
{
    public interface IFact
    {

        public Type ValueType { get; }
        
        public object GetObjectValue();
        
        public void SetObjectValue(object value);

        public bool IsPersistent { get; }

    }

    
}
