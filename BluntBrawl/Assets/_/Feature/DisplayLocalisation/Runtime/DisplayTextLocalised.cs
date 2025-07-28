using System.Collections.Generic;
using Foundation.Runtime;
using TMPro;
using UnityEngine;

namespace DisplayLocalisation.Runtime
{
    [RequireComponent(typeof(TMP_Text))]
    public class DisplayTextLocalised : FBehaviour
    {
        #region Unity API
        

        private void OnEnable()
        {
            DisplayLocalisedText();
            GameManager.Runtime.GameManager.m_OnLanguageChanged.AddListener(DisplayLocalisedText);
        } 
        
        private void OnDisable() => GameManager.Runtime.GameManager.m_OnLanguageChanged.RemoveListener(DisplayLocalisedText);
        
        
        #endregion
        
        #region Utils


        private void DisplayLocalisedText()
        {
            string text = GetText(_keyText);
            if (text == null)
            {
                text = ("ERROR_TEXT_NOT_FOUND");
                _text.text = text;
                throw new KeyNotFoundException($"The key {_keyText} was not found in the localisation file.");
            }
            _text.text = text;
            
        }
        
        #endregion
        
        

        #region Private and Protected

        private TMP_Text _text=> GetComponent<TMP_Text>();
        [SerializeField] private string _keyText;

        #endregion
    }
}
