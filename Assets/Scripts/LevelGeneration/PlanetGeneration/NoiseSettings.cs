using ProjectSRG.Utils.Common;
using UnityEngine;
namespace ProjectSRG.LevelGeneration.PlanetGeneration
{

    [System.Serializable]
    public class NoiseSettings
    {
        public FilterType filterType;

        [ConditionalHide("filterType", 0)]
        public SimpleNoiseSettings simpleNoiseSettings;

        [ConditionalHide("filterType", 1)]
        public RidgidNoiseSettings ridgidNoiseSettings;

        [System.Serializable]
        public class SimpleNoiseSettings
        {
            [Range(1, 8)] public int numberOfLayers = 1;
            public float strength = 1;
            public float baseRoughness = 1;
            public float roughness = 2;
            public float persistence = .5f;
            public float minValue;
            public Vector3 centre;
        }

        [System.Serializable]
        public class RidgidNoiseSettings : SimpleNoiseSettings
        {
            public float weightMultilier = 0.8f;
        }

        public enum FilterType
        {
            simple,
            ridgid
        }
    }
}