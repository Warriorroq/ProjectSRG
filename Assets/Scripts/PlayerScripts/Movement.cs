using ProjectSRG.ObjectTraits;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace ProjectSRG.PlayerScripts
{
    [RequireComponent(typeof(PlayerInput), typeof(Unit), typeof(Rigidbody))]
    public class Movement : MonoBehaviour
    {


        [Header("=== Ship Movement Settings ===")]
        [SerializeField] private float _yawTorque = 560f;
        [SerializeField] private float _pitchTorque = 1000f; 
        [SerializeField] private float _rollTorque = 1000f; 
        [SerializeField] private float _thrust = 180f; 
        [SerializeField] private float _upThrust = 50f; 
        [SerializeField] private float _strafeThrust = 50f; 
        [SerializeField, Range(0.001f, 0.999f)] 
        private float _thrustGlideReduction = 0.999f; 
        [SerializeField, Range(0.001f, 0.999f)] 
        private float _upDownGlideReduction = 0.111f; 
        [SerializeField, Range(0.001f, 0.999f)] 
        private float _leftRightGlideReduction = 0.111f;

        private float _glide;
        private float _verticalGlide;
        private float _horizontalGlide;
        private Vector3 _torque;
        private Rigidbody _rigitBody;

        private float _thrust1D;
        private float _strafe1D;
        private float _upDown1D;
        private float _roll1D;
        private Vector2 _pitchYaw;

        private void Awake()
        {
            _rigitBody = GetComponent<Rigidbody>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void FixedUpdate()
        {
            HandleMovement();
            Debug.DrawLine(transform.position + Vector3.up * 2, transform.position + _rigitBody.velocity + Vector3.up * 2);
            Debug.Log(_rigitBody.velocity.magnitude);
        }

        private void HandleMovement()
        {
            UpdateRotationTorque();

            UpdateRelativeForceInDirectionAndApplyGlide(_thrust1D, _thrust, ref _glide, _thrustGlideReduction, transform.forward);
            UpdateRelativeForceInDirectionAndApplyGlide(_upDown1D, _upThrust, ref _verticalGlide, _upDownGlideReduction, transform.up);
            UpdateRelativeForceInDirectionAndApplyGlide(_strafe1D, _strafeThrust, ref _horizontalGlide, _leftRightGlideReduction, transform.right);
        }

        private void UpdateRotationTorque()
        {
            _torque = Vector3.zero;
            _torque += Vector3.back * (_roll1D * _rollTorque * Time.fixedDeltaTime);
            _torque += Vector3.right * (Mathf.Clamp(-_pitchYaw.y, -1f, 1f) * _pitchTorque * Time.fixedDeltaTime);
            _torque += Vector3.up * (Mathf.Clamp(_pitchYaw.x, -1f, 1f) * _yawTorque * Time.fixedDeltaTime);
            _rigitBody.AddRelativeTorque(_torque);
        }

        private void UpdateRelativeForceInDirectionAndApplyGlide(float input1D, float thurst, ref float glide, float gliderReduction, Vector3 direction)
        {
            if (Mathf.Abs(input1D) > 0.1f)
            {
                float relativeThurst = input1D * thurst;
                _rigitBody.AddRelativeForce(direction * (relativeThurst * Time.fixedDeltaTime));
                glide = relativeThurst;
            }
            else
            {
                _rigitBody.AddRelativeForce(direction * (glide * Time.fixedDeltaTime));
                glide *= gliderReduction;
            }
        }
        #region Input Methods
        public void OnThurst(InputAction.CallbackContext context)       
            =>_thrust1D = context.ReadValue<float>();

        public void OnStrafe(InputAction.CallbackContext context)
            =>_strafe1D = context.ReadValue<float>();

        public void OnUpDown(InputAction.CallbackContext context)
            =>_upDown1D = context.ReadValue<float>();

        public void OnRoll(InputAction.CallbackContext context)
            =>_roll1D = context.ReadValue<float>();

        public void OnPitchYaw(InputAction.CallbackContext context)
            =>_pitchYaw = context.ReadValue<Vector2>();
        #endregion
    }
}
