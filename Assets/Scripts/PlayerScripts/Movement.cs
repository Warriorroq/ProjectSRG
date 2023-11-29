using ProjectSRG.ObjectTraits;
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

        private Rigidbody _rigidBody;
        private Trait<float> _speedMultiplier;

        private float _thrust1D, _strafe1D, _upDown1D, _roll1D;
        private Vector2 _mouseYaw, _inputDistance;
        private Vector2 _screenCenter = new Vector2(Screen.width, Screen.height)/2;
        [SerializeField] private RectTransform _crossHair;
        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _speedMultiplier = GetComponent<Unit>().traits.GetTrait<Trait<float>>("Speed");
        }

        private void FixedUpdate()
        {
            transform.Rotate(-_inputDistance.y * _lookRateSpeed * Time.fixedDeltaTime, _inputDistance.x * _lookRateSpeed * Time.fixedDeltaTime, _roll1D * _lookRateSpeed * Time.fixedDeltaTime, Space.Self);
            Ray ray = new Ray(transform.position, transform.forward);
            _crossHair.position = Camera.main.WorldToScreenPoint(ray.GetPoint(20f));

            _activeSpeed.x = Mathf.Lerp(_activeSpeed.x, _strafe1D * _speed.x * _speedMultiplier.Value, _directionAcceleration.x * Time.fixedDeltaTime);
            _activeSpeed.z = Mathf.Lerp(_activeSpeed.z, _thrust1D * _speed.z * _speedMultiplier.Value, _directionAcceleration.z * Time.fixedDeltaTime);
            _activeSpeed.y = Mathf.Lerp(_activeSpeed.y, _upDown1D * _speed.y * _speedMultiplier.Value, _directionAcceleration.y * Time.fixedDeltaTime);
            _rigidBody.velocity = transform.TransformVector(_activeSpeed);
            _rigidBody.angularVelocity = Vector3.zero;
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
            _inputDistance = (_mouseYaw - _screenCenter) / _screenCenter;
        }

        public void OnControllerYaw(InputAction.CallbackContext context)
            =>_inputDistance = context.ReadValue<Vector2>();
        #endregion
    }
}
