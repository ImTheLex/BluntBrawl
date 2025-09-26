using UnityEngine;
using Event = AK.Wwise.Event;

namespace Sounds.Runtime
{
    public class ButtonSFX : MonoBehaviour
    {
        #region Main Methods
        

        public void ClickSFX()=> AkUnitySoundEngine.PostEvent(_clickEvent.Id, gameObject);
        public void BackSFX()=> AkUnitySoundEngine.PostEvent(_backEvent.Id, gameObject);
        public void LaunchSFX()=> AkUnitySoundEngine.PostEvent(_launchEvent.Id, gameObject);
        public void FancySFX()=> AkUnitySoundEngine.PostEvent(_fancyEvent.Id, gameObject);
        

        #endregion
        
        #region Private and Protected

        [SerializeField] private Event _clickEvent;
        [SerializeField] private Event _backEvent;
        [SerializeField] private Event _launchEvent;
        [SerializeField] private Event _fancyEvent;

        #endregion
    }
}
