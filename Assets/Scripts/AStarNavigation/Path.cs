using System.Linq;
using UnityEngine;
namespace ProjectSRG.AStarNavigation
{
	[System.Serializable]
	public class Path
	{

		public readonly Node[] waypoints;
		public readonly Line[] turnBoundaries;
		public readonly int finishLineIndex;
		public readonly int slowDownIndex;
		private int index;
		public Path(Node[] waypoints, Vector3 startPos, float turnDst, float stoppingDst)
		{
			this.waypoints = waypoints;
			turnBoundaries = new Line[this.waypoints.Length];
			finishLineIndex = turnBoundaries.Length - 1;

			Vector2 previousPoint = V3ToV2(startPos);
			for (int i = 0; i < this.waypoints.Length; i++)
			{
				Vector2 currentPoint = V3ToV2(this.waypoints[i].worldPosition);
				Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;
				Vector2 turnBoundaryPoint = (i == finishLineIndex) ? currentPoint : currentPoint - dirToCurrentPoint * turnDst;
				turnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint - dirToCurrentPoint * turnDst);
				previousPoint = turnBoundaryPoint;
			}

			float dstFromEndPoint = 0;
			for (int i = this.waypoints.Length - 1; i > 0; i--)
			{
				dstFromEndPoint += Vector3.Distance(this.waypoints[i].worldPosition, this.waypoints[i - 1].worldPosition);
				if (dstFromEndPoint > stoppingDst)
				{
					slowDownIndex = i;
					break;
				}
			}
		}

		public Node Peek()
			=> index < waypoints.Count() ? waypoints[index++] : null;

        public void DrawWithGizmos()
		{
			if(waypoints is null) 
				return;

			Gizmos.color = Color.black;
			foreach (Node p in waypoints)
				Gizmos.DrawCube(p.worldPosition, Vector3.one * 4);
		}

		private Vector2 V3ToV2(Vector3 v3)
			=> new Vector2(v3.x, v3.z);
	}
}