using System;
using UnityEngine;
using Event = AK.Wwise.Event;

namespace Sounds.Runtime
{
    public class WaitingRoomSFX : MonoBehaviour
    {
        #region Unity API

        private void Awake()=> DontDestroyOnLoad(gameObject);

        private void Start()
        {
            StartWaitingRoomSound();
        }

        #endregion

        #region Main Methods

        public void DestroyWaitingRoomSFX()
        {
            _waitingRoomSounds.Stop(gameObject);
            Destroy(gameObject);
        } 

        #endregion
        
        #region Utils

        private void StartWaitingRoomSound()
        {
            AkUnitySoundEngine.PostEvent(_waitingRoomSounds.Id, gameObject);
        }

        #endregion
        
        
        #region Private and Protected

        [SerializeField] private Event _waitingRoomSounds;

        #endregion
    }
}
