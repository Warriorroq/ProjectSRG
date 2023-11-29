using UnityEngine;
using ProjectSRG.AStarNavigation;
using System.Linq;
using System.Collections;
using ProjectSRG.Game;
using System.Collections.Generic;
using UnityEngine.InputSystem;
namespace ProjectSRG
{
    public class Enemy : MonoBehaviour
    {

        public float minPathUpdateTime = .3f;
        public float pathUpdateMoveThreshold = 10f;

        public Transform target;
        public float speed = 20;
        private Path _path;
        [SerializeField] private List<Gun> _guns;
        [SerializeField] private float _shootAngle;
        private Node _currentPoint;
        [SerializeField] private Rigidbody _rigidbody;
        private void Start()
        {
            target = Beacon.Instance.playerTransform;
            StartCoroutine(UpdatePath());
        }

        private void Update()
        {
            if (_currentPoint is not null)
            {
                var rotation = transform.rotation;
                transform.LookAt(_currentPoint.worldPosition);
                transform.rotation = Quaternion.Slerp(rotation, transform.rotation, Time.deltaTime);
                _rigidbody.velocity = transform.forward * speed;
                if((transform.position - _currentPoint.worldPosition).sqrMagnitude < 11*11)
                    _currentPoint = _path.Peek();
            }
            else
            {
                var rotation = transform.rotation;
                transform.LookAt(target.transform.position);
                transform.rotation = Quaternion.Slerp(rotation, transform.rotation, Time.deltaTime);
            }
            if(Vector3.Angle((target.transform.position - transform.position).normalized, transform.forward) < _shootAngle)
            {
                foreach (Gun gun in _guns)
                    gun.Shoot(new InputAction.CallbackContext());
            }
        }

        public void OnPathFound(Node[] waypoints, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                _path = new Path(waypoints, transform.position, 0, 0);
                _currentPoint = _path.Peek();
                _currentPoint = _path.Peek();
            }
        }

        private IEnumerator UpdatePath()
        {

            if (Time.timeSinceLevelLoad < .3f)
                yield return new WaitForSeconds(.3f);

            PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));

            while (true)
            {
                yield return new WaitForSeconds(minPathUpdateTime);
                float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
                if ((target.position - _path.waypoints.Last().worldPosition).sqrMagnitude > sqrMoveThreshold)
                    PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
            }
        }

        public void OnDrawGizmos()
        {
            if (_path != null)
            {
                _path.DrawWithGizmos();
            }
        }
    }
}
