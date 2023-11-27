using AYellowpaper.SerializedCollections;
using ProjectSRG.Game;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectSRG.LevelGeneration.SectorGeneration {
    public class SectorSpawner : Singleton<SectorSpawner>
    {
        [SerializeField] private SerializedDictionary<float, SectorType> _chanceOfSpawnSectors;
        [SerializeField] private SerializedDictionary<SectorType, UnityEvent<Sector>> _generateSectorsMethods;

        public SolarSystemGenerator solarSystemGenerator;

        public void GenerateSolarSystem(Sector sector)
            => solarSystemGenerator.Generate(sector);

        public SectorType GetRandomSectorType()
        {
            float currentChance = UnityEngine.Random.value;
            foreach(var sectorChance in _chanceOfSpawnSectors)
            {
                if(sectorChance.Key > currentChance)
                    return sectorChance.Value;
            }
            return SectorType.None;
        }

        public Sector GetNewSector()
        {
            var beacon = Beacon.Instance;
            var sectorType = GetRandomSectorType();
            var go = new GameObject($"Sector {sectorType}");
            go.transform.position = beacon.spaceVectorMovementDirection * beacon.sectorSpawnDistance;
            var sector = go.AddComponent<Sector>();
            _generateSectorsMethods[sectorType].Invoke(sector);
            return sector;
        }

        public enum SectorType : int
        {
            None,
            SolarSystem
        }
    }
}
