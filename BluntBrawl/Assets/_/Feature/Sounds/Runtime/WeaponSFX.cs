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

        [ClientRpc]
        public void WeaponSlashSFX(float velocity)
        {
            if (_sfxSend) return;
            AkUnitySoundEngine.SetRTPCValue(_RTCPVelocity.ToString(), velocity);
            AkUnitySoundEngine.PostEvent(_SFXWeaponSlash.ToString(), gameObject);            
            _sfxSend = true;
        }

        [ClientRpc]
        public void WeaponHitSFX()=> AkUnitySoundEngine.PostEvent(_SFXWeaponHit.ToString(), gameObject);

        [ClientRpc]
        public void WeaponDropSFX()
        {
            AkUnitySoundEngine.SetSwitch(_switchWeaponDrop.GroupId,_switchWeaponDrop.Id, gameObject);
            AkUnitySoundEngine.PostEvent(_SFXWeaponDrop.ToString(), gameObject);
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


        private float _timer = 1f;
        private float _chrono;
        private bool _sfxSend;

        #endregion
    }
}
