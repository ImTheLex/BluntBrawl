using Mirror;
using PrimeTween;
using UnityEngine;

namespace Weapon.Runtime
{
    public class WeaponColorFeedback : NetworkBehaviour
    {

        #region Unity API
        

        private void Awake()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
        }
        
        #endregion


        #region Main Method

        public void SetRedWeapon(float duration)
        {
            Tween.Custom(Color.red, Color.white, duration,
                color => ColorFeedbackWeapon(color), Ease.Linear);
        }

        #endregion

        #region Utils

        private void ColorFeedbackWeapon(Vector4 color)
        {
            _materialPropertyBlock.SetColor("_BaseColor",color);
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        #endregion

        #region Private and protected

        
        private Renderer _meshRenderer => GetComponent<MeshRenderer>();
        private MaterialPropertyBlock _materialPropertyBlock;

        #endregion
    }
}
