using UnityEngine;
using System.Collections.Generic;
using System;
namespace ProjectSRG.AStarNavigation
{
	[RequireComponent(typeof(Grid))]
	public class Pathfinding : Singleton<Pathfinding>
	{

		[SerializeField] private bool _simplify;

		public void FindPath(PathRequest request, Action<PathResult> callback)
		{

			Node[] waypoints = new Node[0];
			bool pathSuccess = false;

			Node startNode = Grid.Instance.NodeFromWorldPoint(request.pathStart);
			Node targetNode = Grid.Instance.NodeFromWorldPoint(request.pathEnd);
			startNode.parent = startNode;


			if (startNode.walkable && targetNode.walkable)
			{
				Heap<Node> openSet = new Heap<Node>(Grid.Instance.HeapSize);
				HashSet<Node> closedSet = new HashSet<Node>();
				openSet.Add(startNode);

				while (openSet.Count > 0)
				{
					Node currentNode = openSet.RemoveFirst();
					closedSet.Add(currentNode);

					if (currentNode == targetNode)
					{
						//print ("Path found: " + sw.ElapsedMilliseconds + " ms");
						pathSuccess = true;
						break;
					}

					foreach (Node neighbour in Grid.Instance.GetNeighbours(currentNode))
					{
						if (!neighbour.walkable || closedSet.Contains(neighbour))
						{
							continue;
						}

						int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
						if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
						{
							neighbour.gCost = newMovementCostToNeighbour;
							neighbour.hCost = GetDistance(neighbour, targetNode);
							neighbour.parent = currentNode;

							if (!openSet.Contains(neighbour))
								openSet.Add(neighbour);
							else
								openSet.UpdateItem(neighbour);
						}
					}
				}
			}

			if (pathSuccess)
			{
				waypoints = RetracePath(startNode, targetNode);
				pathSuccess = waypoints.Length > 0;
			}

			callback(new PathResult(waypoints, pathSuccess, request.callback));

		}

		private Node[] RetracePath(Node startNode, Node endNode)
		{
			List<Node> path = new List<Node>();
			Node currentNode = endNode;

			while (currentNode != startNode)
			{
				path.Add(currentNode);
				currentNode = currentNode.parent;
			}
			Node[] waypoints = _simplify ? SimplifyPath(path) : path.ToArray();
			Array.Reverse(waypoints);
			return waypoints;

		}

		private Node[] SimplifyPath(List<Node> path)
		{
			List<Node> waypoints = new List<Node>();
			Vector2 directionOld = Vector2.zero;

			for (int i = 1; i < path.Count; i++)
			{
				Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
				if (directionNew != directionOld)
				{
					waypoints.Add(path[i]);
				}
				directionOld = directionNew;
			}
			return waypoints.ToArray();
		}

		private int GetDistance(Node nodeA, Node nodeB)
		{
			int[] dst = new int[] {
			Mathf.Abs(nodeA.gridX - nodeB.gridX),
			Mathf.Abs(nodeA.gridY - nodeB.gridY),
			Mathf.Abs(nodeA.gridZ - nodeB.gridZ)};

			Array.Sort(dst);
			int cost = dst[0] * 173;
			dst[1] -= dst[0];
			dst[2] -= dst[0];
			return 141 * dst[1] + 100 * (dst[2] - dst[1]) + cost;
		}
	}
}