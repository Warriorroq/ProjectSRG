using ProjectSRG.ObjectTraits;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace ProjectSRG.PlayerScripts
{
    [RequireComponent(typeof(PlayerInput), typeof(Unit))]
    public class Movement : MonoBehaviour
    {
        // everything is bad and it's just a rough sketch
        [SerializeField] private Trait<float> _speed;

        private Quaternion _targetRotation;
        [SerializeField] private Vector2 _sensitivity;
        [SerializeField] private float _rotationSpeed;

        private Vector2 _lastInput;
        private Vector3 _currentMovementInput;
        private Vector3 _smoothInputVelocity;
        [SerializeField] private float _smoothInputSpeed;
        private void Start()
        {
            Cursor.visible = false;
            var traits = GetComponent<Unit>().traits;

            _speed = traits.GetTrait<Trait<float>>("Speed");
        }

        public void MovementPerformed(InputAction.CallbackContext obj)
            => _lastInput = obj.ReadValue<Vector2>();
        public void RotationPerformed(InputAction.CallbackContext obj)
        {
            Vector2 input = obj.ReadValue<Vector2>();
            Vector2 deltaRotation = input * _sensitivity;

            Quaternion currentRotation = transform.rotation;
            transform.eulerAngles = transform.eulerAngles + new Vector3(-deltaRotation.y, deltaRotation.x, 0);
            _targetRotation = transform.rotation;
            transform.rotation = currentRotation;

        }
        private void Update() {
            Vector3 moveDirectionInput = transform.forward * _lastInput.y + transform.right * _lastInput.x;
            _currentMovementInput = Vector3.SmoothDamp(_currentMovementInput, moveDirectionInput, ref _smoothInputVelocity, _smoothInputSpeed);
            transform.position += _currentMovementInput * Time.deltaTime * _speed.Value;
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, Time.deltaTime * _rotationSpeed);
        }
    }
}
