using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectSRG.AStarNavigation
{
    public class NavGridCubeObstacle : MonoBehaviour
    {
        public Vector3 size;
        public int movementPenalty;

        private Grid _grid;
        [SerializeField] private float _updatingRate;
        [SerializeField] private Vector3 _lastPosition;
        private void Awake()
        {
            _grid = FindObjectOfType<Grid>();
            _lastPosition = transform.position;
        }

        private void Start()
        {
            InvokeRepeating(nameof(UpdateWalkableState), _updatingRate, _updatingRate);
        }

        private void UpdateWalkableState()
        {
            if (_lastPosition == transform.position)
                return;
            var scale = transform.localScale;
            var size = new Vector3(this.size.x * scale.x, this.size.y * scale.y, this.size.z * scale.z);
            _grid.ClearGridInBoxVolume(_lastPosition, size);
            _grid.RecalculateGridInBoxVolume(transform.position, size, movementPenalty);
            _lastPosition = transform.position;
        }

        private void OnDrawGizmos()
        {
            var scale = transform.localScale;
            var size = new Vector3(this.size.x * scale.x, this.size.y * scale.y, this.size.z * scale.z);
            Gizmos.DrawSphere(transform.position - size / 2, 1);
            Gizmos.DrawSphere(transform.position + size / 2, 1);
            Gizmos.color = Color.grey;
            Gizmos.DrawWireCube(transform.position, size);
            Gizmos.color = Color.white;
        }
    }

}