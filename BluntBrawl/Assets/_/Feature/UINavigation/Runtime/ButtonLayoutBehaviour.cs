using System;
using UnityEngine;
using UnityEngine.UI;

namespace UINavigation.Runtime
{
    public class ButtonLayoutBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            _image = GetComponent<Image>();
            _oldSprite = _image.sprite;
        }

        public void Swipe()
        {
            _image.sprite = _newSprite;
        }

        public void UnSwipe()
        {
            _image.sprite = _oldSprite;
        }
        
        private Image _image;
        private Sprite _oldSprite;
        [SerializeField] private Sprite _newSprite;
    }
}
