using ProjectSRG.Utils.Maths;
using System.Collections.Generic;
using UnityEngine;
using ProjectSRG.LevelGeneration.SectorGeneration;

namespace ProjectSRG.Game
{
    public class Beacon : Singleton<Beacon>
    {
        public float sectorSpawnDistance;
        public float speedOfSpaceThread;
        public Vector3 spaceVectorMovementDirection = Vector3.forward;
        public float updateRate;
        [HideInInspector] public Transform playerTransform { get; private set; }

        [SerializeField] private Vector3 _possibleAbsoluteChangeOfVector;
        [SerializeField] private float _strengthOfChangingVector;
        [SerializeField] private float _maxPlayerSqrtDistance;
        private float _lastSectorSqrtDistanceForSpawnNewOne;
        [SerializeField] private float _sectorDespawnSqrtDistance;
        [SerializeField] private string _playerTag;

        private List<Sector> _currentSectors;
        private void Awake()
        {
            playerTransform = GameObject.FindGameObjectWithTag(_playerTag).transform;
            _currentSectors = new List<Sector>();
            _lastSectorSqrtDistanceForSpawnNewOne = sectorSpawnDistance * sectorSpawnDistance * 1.01f;
            InvokeRepeating(nameof(UpdateMap), 0, updateRate);
        }

        private void Start()
        {
            if(_currentSectors.Count == 0)
                _currentSectors.Add(SectorSpawner.Instance.GetNewSector());
        }

        private void UpdateMap()
        {
            ChangeDirectionOfMovement();
            UpdatePlayerState();
            UpdateSector();
        }

        private void Update()
        {
            Debug.DrawRay(transform.position, spaceVectorMovementDirection * 10f, Color.red);
        }

        private void ChangeDirectionOfMovement()
        {
            Vector3 randomizedDirection = _possibleAbsoluteChangeOfVector.GetRandomValueFromCurrentVector();
            randomizedDirection *= _strengthOfChangingVector;
            spaceVectorMovementDirection += randomizedDirection;
            spaceVectorMovementDirection = spaceVectorMovementDirection.normalized;
        }
        private void UpdateSector()
        {

            bool needANewSector = false;
            foreach (var sector in _currentSectors)
            {
                float currentSqrtDistance = Vector3.SqrMagnitude(transform.position - sector.transform.position);

                if (currentSqrtDistance > _lastSectorSqrtDistanceForSpawnNewOne && !sector.wasUsedForCreationOfNextSector)
                {
                    sector.wasUsedForCreationOfNextSector = true;
                    needANewSector = true;
                }

                if (currentSqrtDistance > _sectorDespawnSqrtDistance)
                {
                    _currentSectors.Remove(sector);
                    Destroy(sector.gameObject);
                    break;
                }
            }

            if(needANewSector)
                _currentSectors.Add(SectorSpawner.Instance.GetNewSector());
        }

        private void UpdatePlayerState()
        {
            if (Vector3.SqrMagnitude(transform.position - playerTransform.position) > _maxPlayerSqrtDistance)
            {
                //TODO:
                //Apply something to player or kill it;
                Debug.Log("Too far away");
            }
        }
    }
}
