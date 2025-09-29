using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DeathCam.Runtime
{
    public class DeathCamVignette : MonoBehaviour
    {

        public float m_FadeInDuration => _vignetteFadeIn;
        public bool m_isPaused => _pause;
        public UnityEvent m_onPausedEvent;
        private void Awake()
        {
            SetAlpha(0f);
        }

        private void Update()
        {
            if (_pause)
            {
                _pauseTimer += Time.deltaTime;
                if (m_onPausedEvent is null)
                {
                    //m_onPausedEvent.Invoke();
                }
                if (_pauseTimer < _vignettePause) return;
                m_onPausedEvent = null;
                _pauseTimer = 0; 
                _pause = false;

            }
            if (_display)
            {
                _timer += Time.deltaTime;
                float alpha = Mathf.Clamp01(_timer / _vignetteFadeIn);
                SetAlpha(alpha);
                if (_timer >= _vignetteFadeIn)
                {
                    _display = false;
                    _pause = true;
                    _timer = 0f;
                }

            }
            else if (_vignette.color.a > 0f)
            {
                _timer += Time.deltaTime;
                float alpha = Mathf.Clamp01(1f - (_timer / _vignetteFadeIn));
                SetAlpha(alpha);

            }
        }

        public void DisplayBlackScreenAfterLoad()
        {
            Invoke(nameof(DisplayVignette),4f);
        }
        
        public void DisplayVignette()
        {
            _display = true;
            _timer = 0f;
        }

        public void RestoreVignette()
        {
            _display = false;
            SetAlpha(0f);
        }

        private void SetAlpha(float alpha)
        {
            Color c = _vignette.color;
            c.a = alpha;
            _vignette.color = c;
        }

        private bool _display;
        private bool _pause;
        private float _timer;
        private float _pauseTimer;
        [SerializeField] private float _vignetteFadeIn = 0.5f;
        [SerializeField] private float _vignettePause = 3f;
        [SerializeField] private Image _vignette;
        
        
    }
}
