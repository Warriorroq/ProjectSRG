using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

namespace ProjectSRG.AStarNavigation
{
    public class Grid : Singleton<Grid>
    {

        public bool displayGridGizmos, displayOnlyUnwalkable;
        public LayerMask unwalkableMask;
        public Vector3 gridWorldSize;
        public float nodeRadius;
        public TerrainType[] walkableRegions;
        public int obstacleProximityPenalty = 10;
        public UnityEvent gridWasDirty;

        private Dictionary<int, int> _walkableRegionsDictionary = new Dictionary<int, int>();
        private LayerMask _walkableMask;

        private Node[,,] _grid;

        private float _nodeDiameter;
        private int _gridSizeX, _gridSizeY, _gridSizeZ;

        private int _penaltyMin = int.MaxValue;
        private int _penaltyMax = int.MinValue;

        private Vector3 _gridBottomLeft, _gridTopRight;
        private bool _isDirty;
        private const float _updateRate = 0.05f;
        [Range(0.001f, 1f)][SerializeField] private float _heapSizePercentFromMaxSize;
        public int MaxSize => _gridSizeX * _gridSizeY * _gridSizeZ;

        public int HeapSize => (int)(MaxSize * _heapSizePercentFromMaxSize);

        public void RecalculateGridInBoxVolume(Vector3 position, Vector3 size, int movementPenalty = 0)
        {
            _isDirty = true;
            ForEachNodeInCube(position, size, node =>
            {
                node.walkable = !(Physics.CheckSphere(node.worldPosition, nodeRadius, unwalkableMask));
                node.movementPenalty = movementPenalty;
            });
        }

        public void ClearGridInBoxVolume(Vector3 position, Vector3 size)
        {
            _isDirty = true;
            ForEachNodeInCube(position, size, ClearNode);
        }

        public void RecalculateGridWalkableNodes()
        {
            _isDirty = true;
            foreach (var n in _grid)
                n.walkable = !(Physics.CheckSphere(n.worldPosition, nodeRadius, unwalkableMask));
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0 && z == 0)
                            continue;

                        int checkX = node.gridX + x;
                        int checkY = node.gridY + y;
                        int checkZ = node.gridZ + z;

                        if (AreGridCoordsInGrid(checkX, checkY, checkZ))
                            neighbours.Add(_grid[checkX, checkY, checkZ]);
                    }
                }
            }

            return neighbours;
        }

        public Node NodeFromWorldPoint(Vector3 nodeWorldPosition)
        {
            float percentX = Mathf.Clamp01((nodeWorldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
            float percentY = Mathf.Clamp01((nodeWorldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y);
            float percentZ = Mathf.Clamp01((nodeWorldPosition.z + gridWorldSize.z / 2) / gridWorldSize.z);

            int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);
            int z = Mathf.RoundToInt((_gridSizeZ - 1) * percentZ);
            return _grid[x, y, z];
        }

        private void Awake()
        {
            _nodeDiameter = nodeRadius * 2;
            _gridSizeX = Mathf.RoundToInt(gridWorldSize.x / _nodeDiameter);
            _gridSizeY = Mathf.RoundToInt(gridWorldSize.y / _nodeDiameter);
            _gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / _nodeDiameter);

            foreach (TerrainType region in walkableRegions)
            {
                _walkableMask.value |= region.terrainMask.value;
                _walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
            }

            CreateGrid();
            InvokeRepeating(nameof(AlertIfGridDirty), _updateRate, _updateRate);
        }

        private void AlertIfGridDirty()
        {
            if (!_isDirty)
                return;
            gridWasDirty.Invoke();
            _isDirty = false;

        }

        private void CreateGrid()
        {
            _grid = new Node[_gridSizeX, _gridSizeY, _gridSizeZ];
            var gridSize = new Vector3(gridWorldSize.x / 2, gridWorldSize.y / 2, gridWorldSize.z / 2);
            _gridBottomLeft = transform.position - gridSize;
            _gridTopRight = transform.position + gridSize;

            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int z = 0; z < _gridSizeZ; z++)
                {
                    for (int y = 0; y < _gridSizeY; y++)
                    {
                        Vector3 worldPoint = _gridBottomLeft + Vector3.right * (x * _nodeDiameter + nodeRadius) + Vector3.forward * (z * _nodeDiameter + nodeRadius) + Vector3.up * (y * _nodeDiameter + nodeRadius);
                        bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                        int movementPenalty = 0; //could be added penalty

                        if (!walkable)
                            movementPenalty += obstacleProximityPenalty;


                        _grid[x, y, z] = new Node(walkable, worldPoint, x, y, z, movementPenalty);
                    }
                }
            }

        }

        private void ClearNode(Node node)
        {
            node.walkable = true;
            node.movementPenalty = 0;
        }

        private void ForEachNodeInCube(Vector3 position, Vector3 size, Action<Node> action)
        {
            Vector3 startCoords = position - size / 2;
            Vector3 endCoords = position + size / 2;
            if (!AreCoordsInGrid(startCoords) && !AreCoordsInGrid(endCoords)) //won't work in case of rotations be aware of that
                return;

            Vector3 currentPoint;
            for (currentPoint.x = startCoords.x; Mathf.Abs(currentPoint.x - endCoords.x) > nodeRadius; currentPoint.x += nodeRadius)
            {
                for (currentPoint.z = startCoords.z; Mathf.Abs(currentPoint.z - endCoords.z) > nodeRadius; currentPoint.z += nodeRadius)
                {
                    for (currentPoint.y = startCoords.y; Mathf.Abs(currentPoint.y - endCoords.y) > nodeRadius; currentPoint.y += nodeRadius)
                    {
                        if (!AreCoordsInGrid(currentPoint))
                            continue;
                        action(NodeFromWorldPoint(currentPoint));
                    }
                }
            }
        }

        private bool AreCoordsInGrid(Vector3 coords)
            => (_gridBottomLeft.x <= coords.x && coords.x <= _gridTopRight.x) &&
            (_gridBottomLeft.y <= coords.y && coords.y <= _gridTopRight.y) &&
            (_gridBottomLeft.z <= coords.z && coords.z <= _gridTopRight.z);

        private bool AreGridCoordsInGrid(int x, int y, int z)
            => (x >= 0 && y >= 0 && z >= 0) && (x < _gridSizeX && y < _gridSizeY && z < _gridSizeZ);

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, gridWorldSize.z));
            Color gridColor = Color.black;
            gridColor.a = 0;
            if (_grid != null && displayGridGizmos)
            {
                foreach (Node n in _grid)
                {
                    if (displayOnlyUnwalkable && n.walkable)
                        continue;

                    gridColor.a = Mathf.Clamp01(n.movementPenalty / 2000);
                    Gizmos.color = (n.walkable) ? gridColor : Color.red;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (_nodeDiameter));
                }
            }
        }

        [Serializable]
        public class TerrainType
        {
            public LayerMask terrainMask;
            public int terrainPenalty;
        }


    }
}