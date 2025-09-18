using System;
using UnityEngine;
using UnityEngine.UI;

namespace UINavigation.Runtime
{
    public class RightControllerInteract : MonoBehaviour
    {
        
        
        private void Awake()
        {
            if (_lineRenderer == null)
            {
                _lineRenderer.startWidth = 0.01f;
                _lineRenderer.endWidth = 0.01f;
                //_lineRenderer.positionCount = 2;
                _lineRenderer.useWorldSpace = true;
                
                // Optionnel : couleur du laser
                //_lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
                //_lineRenderer.material.color = Color.red;
            }
        }

        
        private void FixedUpdate()
        {
            Vector3 start = _rightController.position; // POSITION MONDIALE
            Vector3 direction = _rightController.TransformDirection(Vector3.forward); // FORWARD en WORLD
            Vector3 end = start + direction * _rayLength;

            
            if (Physics.Raycast(start, direction, out RaycastHit hit, _rayLength))
            {
                _button = hit.collider.GetComponent<Button>(); // toujours overwrite
                if(_blb is null) _blb = _button.GetComponent<ButtonLayoutBehaviour>();
                _blb.Swipe();
                end = hit.point;
            }
            else
            {
                if (_blb is not null)
                {
                    _blb.UnSwipe();
                    _blb = null;    
                }
                _button = null;
            }


            // Couleur dynamique
            if (_button != null)
            {
                _lineRenderer.startColor = Color.green;
                _lineRenderer.endColor = Color.green;
            }
            else
            {
                _lineRenderer.startColor = Color.red;
                _lineRenderer.endColor = Color.red;
            }

            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, end);
        }

        
        public void Interact()
        {
            if (_button != null)
            {
                _button.onClick.Invoke(); // Simule un clic sur le bouton
            }
        }

        private Button _button;
        private ButtonLayoutBehaviour _blb;
        [SerializeField] private Transform _rightController;
        [SerializeField] private float _rayLength = 100f;
        [SerializeField] private LineRenderer _lineRenderer;

    }
}
