using UnityEngine;
namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    [CreateAssetMenu(fileName = "PlanetColorSettings", menuName = "ScriptableObjects/PlanetGeneration/PlanetColorSettings")]
    public class ColorSettings : ScriptableObject
    {
        public Material planetMaterial;
        public BiomeColorSettings biomeColorSettings;

        [System.Serializable]
        public class BiomeColorSettings
        {
            public Biome[] biomes;
            public NoiseSettings noiseSettings;
            public float noiseOffset;
            public float noiseStrength;
            [Range(0, 1)]public float blendAmount;

            [System.Serializable]
            public class Biome
            {
                public Gradient gradient;
                public Color tint;
                [Range(0, 1)] public float startHeight;
                [Range(0, 1)] public float tintPercent;

            }
        }
    }
}