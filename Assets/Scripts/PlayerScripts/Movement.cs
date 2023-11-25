using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectSRG.PlayerScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class Movement : MonoBehaviour
    {
        private Vector3 _speed = new Vector3(7.5f, 5f, 25f);
        private Vector3 _activeSpeed;
        private Vector3 _directionAcceleration = new Vector3(2.5f, 2f, 2f);
        [SerializeField] private float _lookRateSpeed = 90f;

        private Rigidbody _rigitBody;

        private float _thrust1D, _strafe1D, _upDown1D, _roll1D;
        private Vector2 _mouseYaw, _mouseDistance;
        private Vector2 _screenCenter = new Vector2(Screen.width, Screen.height)/2;

        private void Awake()
        {
            _rigitBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            transform.Rotate(-_mouseDistance.y * _lookRateSpeed * Time.fixedDeltaTime, _mouseDistance.x * _lookRateSpeed * Time.deltaTime, _roll1D * _lookRateSpeed * Time.deltaTime, Space.Self);
            _activeSpeed.x = Mathf.Lerp(_activeSpeed.x, _strafe1D * _speed.x, _directionAcceleration.x * Time.deltaTime);
            _activeSpeed.z = Mathf.Lerp(_activeSpeed.z, _thrust1D * _speed.z, _directionAcceleration.z * Time.deltaTime);
            _activeSpeed.y = Mathf.Lerp(_activeSpeed.y, _upDown1D * _speed.y, _directionAcceleration.y * Time.deltaTime);
            _rigitBody.velocity = transform.TransformVector(_activeSpeed);            
        }

        #region Input Methods
        public void OnThurst(InputAction.CallbackContext context)       
            => _thrust1D = context.ReadValue<float>();

        public void OnStrafe(InputAction.CallbackContext context)
            => _strafe1D = context.ReadValue<float>();

        public void OnUpDown(InputAction.CallbackContext context)
            => _upDown1D = context.ReadValue<float>();

        public void OnRoll(InputAction.CallbackContext context)
            => _roll1D = -context.ReadValue<float>();

        public void OnMouseYaw(InputAction.CallbackContext context)
        {
            _mouseYaw = context.ReadValue<Vector2>();
            _mouseDistance = (_mouseYaw - _screenCenter) / _screenCenter;
        }
        #endregion
    }
}
