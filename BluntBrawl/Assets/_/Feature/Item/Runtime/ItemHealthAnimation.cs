using PrimeTween;
using UnityEngine;

namespace Item.Runtime
{
    public class ItemHealthAnimation : MonoBehaviour
    {
        #region Unity API

        private void OnEnable()
        {
            AnimateTheParent();
            AnimateTheChild(_cubeTransform);
            AnimateTheChild(_healthTransform, true);
        }

        #endregion


        #region Utils

        
        private void AnimateTheParent()
        {
            Sequence.Create(cycles: -1,cycleMode: CycleMode.Rewind)
                .Chain(Tween.PositionY(_transform, _oscillationRangeForParent/2, duration: _oscillationSpeed, Ease.Linear))
                .Chain(Tween.PositionY(_transform, 0f, duration: _oscillationSpeed, Ease.Linear));
        }
        
        
        private void AnimateTheChild(Transform target, bool reverse = false)
        {
            if (!reverse)
            {
                Sequence.Create(cycles: 1)
                    .Chain(Tween.Custom(target.eulerAngles.y, target.eulerAngles.y + 10f, _rotationSpeed,
                        onValueChange: value=> AddRotationY(value, target), Ease.Linear).OnComplete(()=>AddTheRotationZ(target)));
            }
            else
            {
                Sequence.Create(cycles: 1)
                    .Chain(Tween.Custom(target.eulerAngles.y, target.eulerAngles.y - 10f, _rotationSpeed,
                        onValueChange: value=> AddRotationY(value, target), Ease.Linear).OnComplete(()=>AddTheRotationY(target)));
            }
        }
        
        private void AddRotationY(float value, Transform target ) => target.rotation = Quaternion.Euler(0f,value,0f);

        private void AddTheRotationZ(Transform target)
        {
            AnimateTheChild(_cubeTransform);
        }
        
        private void AddTheRotationY(Transform target)
        {
            AnimateTheChild(_healthTransform, true);
        }
        

        #endregion
        
        #region Private and protected

        private Transform _transform => transform;
        [SerializeField] private Transform _cubeTransform;
        [SerializeField] private Transform _healthTransform;

        [Header("Animation Settings")] 
        [SerializeField] private float _oscillationRangeForParent;
        [SerializeField] private float _oscillationSpeed;

        [Header("Rotation Settings")] 
        [SerializeField] private float _rotationSpeed;

        #endregion
    }
}
