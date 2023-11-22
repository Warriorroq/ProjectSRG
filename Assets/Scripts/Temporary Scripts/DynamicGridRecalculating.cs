using System;
using UnityEngine;

public class DynamicGridRecalculating : MonoBehaviour
{
    private ProjectSRG.AStarNavigation.Grid _grid;
    [SerializeField] private float _updatingRate;
    void Awake()
    {
        _grid = FindObjectOfType<ProjectSRG.AStarNavigation.Grid>();
    }
    private void Start()
    {
        InvokeRepeating(nameof(UpdateWalkableState), _updatingRate, _updatingRate);
    }

    private void UpdateWalkableState()
        => _grid.RecalculateGridWalkableNodes();
}
