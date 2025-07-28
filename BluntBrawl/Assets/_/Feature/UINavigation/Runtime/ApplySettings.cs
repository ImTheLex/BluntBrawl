using Foundation.Runtime;
using TMPro;
using UnityEngine;

namespace UINavigation.Runtime
{
    public class ApplySettings : FBehaviour
    {
        #region Publics


        public void Apply()=> SetLanguage(_dropdown.captionText.text);
        

        #endregion
        
        #region Private And Protected



        [SerializeField] private TMP_Dropdown _dropdown;


        #endregion
    }
}
