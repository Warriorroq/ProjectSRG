using UnityEngine;
using System.Collections;
using System.Linq;

public class Unit : MonoBehaviour {

	public float minPathUpdateTime = .2f;
	public float pathUpdateMoveThreshold = .5f;

	public Transform target;
	public float speed = 20;
	public float turnSpeed = 3;
	public float turnDst = 5;
	public float stoppingDst = 10;

    private ProjectSRG.AStarNavigation.Path _path;

    private void Start() {
		StartCoroutine (UpdatePath ());
		//grid.gridWasDirty.AddListener(RecalcPath);
	}

	public void OnPathFound(ProjectSRG.AStarNavigation.Node[] waypoints, bool pathSuccessful) {
		if (pathSuccessful) {
			_path = new ProjectSRG.AStarNavigation.Path(waypoints, transform.position, turnDst, stoppingDst);
			//follow it
		}
	}

	private IEnumerator UpdatePath() {

		if (Time.timeSinceLevelLoad < .3f) {
			yield return new WaitForSeconds (.3f);
		}
        ProjectSRG.AStarNavigation.PathRequestManager.RequestPath (new ProjectSRG.AStarNavigation.PathRequest(transform.position, target.position, OnPathFound));

		while (true) {
			yield return new WaitForSeconds (minPathUpdateTime);
            float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
            if ((target.position - _path.waypoints.Last().worldPosition).sqrMagnitude > sqrMoveThreshold) {
                ProjectSRG.AStarNavigation.PathRequestManager.RequestPath (new ProjectSRG.AStarNavigation.PathRequest(transform.position, target.position, OnPathFound));
			}
		}
	}

	public void OnDrawGizmos() {
		if (_path != null) {
            _path.DrawWithGizmos();
        }
	}
}
