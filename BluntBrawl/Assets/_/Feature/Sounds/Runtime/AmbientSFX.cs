using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Event = AK.Wwise.Event;

namespace Sounds.Runtime
{
    public class AmbientSFX : NetworkBehaviour
    {
        #region Public

        public static AmbientSFX instance;

        #endregion

        #region Unity API

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            AkUnitySoundEngine.PostEvent(_menuMusic.Id, gameObject);
        }


        #endregion

        #region Main Methods

        
        
        public void SetCombatMusic()
        {
            AkUnitySoundEngine.PostEvent(_ingameMusic.Id, gameObject);
        }


        #endregion


        #region private and Protected




        [SerializeField] private Event _menuMusic;
        [SerializeField] private Event _ingameMusic;
        

        #endregion
    }
    
    
}
