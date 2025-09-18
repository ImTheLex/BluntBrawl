using Mirror;
using UnityEngine;

namespace Sounds.Runtime
{

    public class WeaponSFX : NetworkBehaviour
    {

        #region Unity API


        private void Awake()
        {
            AkUnitySoundEngine.SetSwitch(_SwitchWeaponType, _WeaponType, this.gameObject);
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
            AkUnitySoundEngine.SetRTPCValue(_RTCPVelocity, velocity);
            AkUnitySoundEngine.PostEvent(_SFXWeaponSlash, this.gameObject);            
            _sfxSend = true;
        }

        [ClientRpc]
        public void WeaponHitSFX()=> AkUnitySoundEngine.PostEvent(_SFXWeaponHit, this.gameObject);

        [ClientRpc]
        public void WeaponDropSFX()
        {
            AkUnitySoundEngine.SetSwitch(_switchWeaponDrop,_weaponDropType, this.gameObject);
            AkUnitySoundEngine.PostEvent(_SFXWeaponDrop, this.gameObject);
        }


        #endregion


        #region private and Protected

        [Header("Weapon slash")]
        [SerializeField] private string _SFXWeaponSlash;
        [SerializeField] private string _SwitchWeaponType;
        [SerializeField] private string _WeaponType;
        [SerializeField] private string _RTCPVelocity;
        [Header("Weapon hit")]
        [SerializeField] private string _SFXWeaponHit;

        [Header("Weapon Drop")]
        [SerializeField] private string _switchWeaponDrop;
        [SerializeField] private string _weaponDropType;
        [SerializeField] private string _SFXWeaponDrop;

        private float _timer = 1f;
        private float _chrono;
        private bool _sfxSend;

        #endregion
    }
}
