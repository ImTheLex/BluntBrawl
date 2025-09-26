using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Event = AK.Wwise.Event;

namespace Sounds.Runtime
{
    public class CombatSFX : NetworkBehaviour
    {
        #region Main Methods
        
        //Combat Music behaviour
        public void StartCombatMusic(int stage)
        {
            if (stage > _combatSounds.Count - 1) stage = 0;
            AkUnitySoundEngine.PostEvent(_combatSounds[stage].Id, gameObject);
            _soundPlayingIndex = stage;
        }

        public void StopCombatMusic()
        {
            if (_soundPlayingIndex == -1) return;
            _combatSounds[_soundPlayingIndex].Stop(gameObject);
            _soundPlayingIndex = -1;
        }
        
        //UI SFX behaviour
        public void StartRoundSFX()=> AkUnitySoundEngine.PostEvent(_startRoundSFX.Id, gameObject);
        
        public void EndRoundSFX()=> AkUnitySoundEngine.PostEvent(_endRoundSFX.Id, gameObject);
        
        public void LoseRoundSFX()=> AkUnitySoundEngine.PostEvent(_loseRoundSFX.Id, gameObject);
        
        public void WinRoundSFX()=> AkUnitySoundEngine.PostEvent(_winRoundSFX.Id, gameObject);
        

        #endregion
        
        #region Private and Protected
        
        [Header("Combat Music List")]
        [SerializeField] private List<Event> _combatSounds;

        [Header("UI SFX")] 
        [SerializeField] private Event _startRoundSFX;
        [SerializeField] private Event _endRoundSFX;
        [SerializeField] private Event _loseRoundSFX;
        [SerializeField] private Event _winRoundSFX;

        private int _soundPlayingIndex = -1;

        #endregion
    }
}
