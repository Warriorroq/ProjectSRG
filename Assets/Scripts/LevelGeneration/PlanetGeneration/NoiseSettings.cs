using UnityEngine;

namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    [System.Serializable]
    public class NoiseSettings
    {
        public FilterType filterType;
        [Range(1, 8)]public int numberOfLayers = 1;
        public float strength = 1;
        public float baseRoughness = 1;
        public float roughness = 2;
        public float persistence = .5f;
        public float minValue;
        public Vector3 centre;

        public enum FilterType
        {
            simple,
            ridgid
        }
    }
}