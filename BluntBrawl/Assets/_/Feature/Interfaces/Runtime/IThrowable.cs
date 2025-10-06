using UnityEngine;

namespace Interfaces.Runtime
{
    public interface IThrowable
    {

        public enum ThrowableState
        {
            None,
            Launched,
            HasHit,
            HasDespawn,
        }

        public void SetState(ThrowableState state);

    }
}
