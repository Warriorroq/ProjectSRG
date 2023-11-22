using UnityEngine;

namespace ProjectSRG.AStarNavigation
{
	public class Node : IHeapItem<Node>
	{

		public bool walkable;
		public Vector3 worldPosition;
		public int gridX;
		public int gridY;
		public int gridZ;
		public int movementPenalty;

		public int gCost;
		public int hCost;
		public Node parent;
		private int _heapIndex;

		public Node(bool walkable, Vector3 worldPos, int gridX, int gridY, int gridZ, int penalty)
		{
			this.walkable = walkable;
			worldPosition = worldPos;
			this.gridX = gridX;
			this.gridY = gridY;
			this.gridZ = gridZ;
			movementPenalty = penalty;
		}

		public int fCost
		{
			get => gCost + hCost;
		}

		public int HeapIndex
		{
			get => _heapIndex;
			set => _heapIndex = value;
		}

		public int CompareTo(Node nodeToCompare)
		{
			int compare = fCost.CompareTo(nodeToCompare.fCost);
			if (compare == 0)
				compare = hCost.CompareTo(nodeToCompare.hCost);
			return -compare;
		}
	}
}