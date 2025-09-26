using PrimeTween;
using UnityEngine;

namespace UINavigation.Runtime
{
    public class SpriteSliderShader : MonoBehaviour
    {
        #region Unity API

        private void OnEnable()
        {
            MoveSlider(_duration);
        }

        #endregion
        
        #region Main Method


        public void MoveSlider(float duration)
        {
            Tween.Custom(0f, 1f, duration, onValueChange: value => IncrementSliderValue(value), Ease.Linear);
        }

        #endregion

        #region Utils


        private void IncrementSliderValue(float value)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.SetFloat("_Reveal", value);
            _spriteRenderer.SetPropertyBlock(block);
        }

        #endregion
        
        
        #region Private and protected
        
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _duration;


        #endregion
    }
}
