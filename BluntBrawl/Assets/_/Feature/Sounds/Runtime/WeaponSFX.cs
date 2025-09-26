using AK.Wwise;
using Mirror;
using UnityEngine;
using Event = AK.Wwise.Event;

namespace Sounds.Runtime
{

    public class WeaponSFX : NetworkBehaviour
    {

        #region Unity API


        private void Awake()
        {
            AkUnitySoundEngine.SetSwitch(_SwitchWeaponType.GroupId, _SwitchWeaponType.Id,gameObject);
        }

        
        private void Update()
        {
            if (!_sfxSend) return;
            _chrono -= Time.deltaTime;
            if (_chrono < 0)
            {
                _sfxSend = false;
                _chrono = _timer;
            }

        }


        #endregion

        #region Main Method

        [TargetRpc]
        public void WeaponSlashSFX(NetworkConnectionToClient target,float velocity)
        {
            AkUnitySoundEngine.SetRTPCValue(_RTCPVelocity.ToString(), velocity);
            if (_sfxSend) return;
            SendSlashSFX();
        }

        [TargetRpc]
        public void WeaponHitSFX(NetworkConnectionToClient target)=> AkUnitySoundEngine.PostEvent(_SFXWeaponHit.ToString(), gameObject);

        [TargetRpc]
        public void WeaponDropSFX(NetworkConnectionToClient target)
        {
            AkUnitySoundEngine.SetSwitch(_switchWeaponDrop.GroupId,_switchWeaponDrop.Id, gameObject);
            AkUnitySoundEngine.PostEvent(_SFXWeaponDrop.ToString(), gameObject);
        }


        #endregion

        #region Utils

        private void SendSlashSFX()
        {
            AkUnitySoundEngine.PostEvent(_SFXWeaponSlash.ToString(), gameObject);            
            _sfxSend = true;
        }

        #endregion


        #region private and Protected

        [Header("Weapon slash")]
        [SerializeField] private Switch _SwitchWeaponType;
        [SerializeField] private Event _SFXWeaponSlash;
        [SerializeField] private RTPC _RTCPVelocity;
        [Header("Weapon hit")]
        [SerializeField] private Event _SFXWeaponHit;

        private Switch _switchWeaponDrop => _SwitchWeaponType;
        [Header("Weapon Drop")] 
        [SerializeField] private Event _SFXWeaponDrop;


        private float _timer = 0.7f;
        private float _chrono;
        private bool _sfxSend;

        #endregion
    }
}
