using ProjectSRG.Utils.Common;
using UnityEngine;
namespace ProjectSRG.LevelGeneration.PlanetGeneration
{

    [System.Serializable]
    public class NoiseSettings
    {
        public NoiseSettings(NoiseSettings settings) 
        {
            simpleNoiseSettings = new SimpleNoiseSettings(settings.simpleNoiseSettings);
            ridgidNoiseSettings = new RidgidNoiseSettings(settings.ridgidNoiseSettings);
            filterType = settings.filterType;
        }

        public FilterType filterType;

        [ConditionalHide("filterType", 0)]
        public SimpleNoiseSettings simpleNoiseSettings;

        [ConditionalHide("filterType", 1)]
        public RidgidNoiseSettings ridgidNoiseSettings;

        [System.Serializable]
        public class SimpleNoiseSettings
        {
            public SimpleNoiseSettings(SimpleNoiseSettings simpleNoiseSettings)
            {
                numberOfLayers = simpleNoiseSettings.numberOfLayers;
                strength = simpleNoiseSettings.strength;
                baseRoughness = simpleNoiseSettings.baseRoughness;
                roughness = simpleNoiseSettings.roughness;
                persistence = simpleNoiseSettings.persistence;
                minValue = simpleNoiseSettings.minValue;
                centre = simpleNoiseSettings.centre;
            }

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

            public RidgidNoiseSettings(RidgidNoiseSettings ridgidNoiseSettings) : base(ridgidNoiseSettings)
            {
                weightMultilier = ridgidNoiseSettings.weightMultilier;
            }
        }

        public enum FilterType
        {
            simple,
            ridgid
        }
    }
}