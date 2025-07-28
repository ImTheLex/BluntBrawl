using System;
using System.Collections.Generic;

namespace FactSystem.Runtime
{
    public class FactDictionary
    {
        #region Publics
        
        
        public Dictionary<string, IFact> AllFacts => _facts;
        private Dictionary<string, IFact> _facts;
        
        
        #endregion
        
        #region Main API


        public bool FacExist<T>(string key, out T value)
        {
            if (_facts.TryGetValue(key, out var fact) && fact is Fact<T> typedFact)
            {
                value = typedFact.Value;
                return true;
            }

            value = default;
            return false;
        }

        public void RemoveFact(string key)=> _facts.Remove(key);

        public T GetFact<T>(string key)
        {
            if (!_facts.TryGetValue(key, out var fact))
                throw new KeyNotFoundException($"The key {key} was not found in the dictionary.");

            if (fact is not Fact<T> typedFact)
                throw new InvalidCastException("Cast failed");
            
            return typedFact.Value;
        }

        public void SetFact<T>(string key, T value, FactPersistence persistence)
        {
            if (_facts.TryGetValue(key, out var existingFact))
            {
                if (existingFact is Fact<T> typedFact)
                {
                    typedFact.Value = value;
                    typedFact.IsPersistent = persistence == FactPersistence.persistent;
                }
                else
                {
                    throw new InvalidCastException($"Cannot cast {existingFact.GetType().FullName} to Fact<{typeof(T).FullName}>.");
                }
            }
            else
            {
                bool IsPersistent = persistence == FactPersistence.persistent;
                _facts[key] = new Fact<T> (value, IsPersistent);
            }
        }

        public FactDictionary()
        {
            _facts = new Dictionary<string, IFact>();
        }
        
        #endregion

        public enum FactPersistence
        {
            temporary,
            persistent,
        }
    }
}
