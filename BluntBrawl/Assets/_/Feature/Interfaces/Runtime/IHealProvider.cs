using UnityEngine;

namespace Interfaces.Runtime
{
    public interface IHealProvider
    {
        public int m_healAmount { get; }
        public void DestroyProvider();
    }
}
