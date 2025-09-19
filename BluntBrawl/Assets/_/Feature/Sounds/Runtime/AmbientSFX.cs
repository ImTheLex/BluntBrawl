using UnityEngine;

namespace Sounds.Runtime
{
    public class AmbientSFX : MonoBehaviour
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
            AkUnitySoundEngine.PostEvent(_menuMusic, gameObject);
        }


        #endregion

        #region Main Methods


        [ContextMenu("Set Combat Music")]
        public void SetCombatMusic()
        {
            AkUnitySoundEngine.PostEvent(_ingameMusic, gameObject);
        }


        #endregion


        #region private and Protected




        [SerializeField] private string _menuMusic;
        [SerializeField] private string _ingameMusic;


        #endregion
    }
}
