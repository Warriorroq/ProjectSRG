using UnityEngine;
namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    [CreateAssetMenu(fileName = "PlanetColorSettings", menuName = "ScriptableObjects/PlanetGeneration/PlanetColorSettings")]
    public class ColorSettings : ScriptableObject
    {
        public Material planetMaterial;
        public BiomeColorSettings biomeColorSettings;

        public ColorSettings(ColorSettings colorSettings)
        {
            planetMaterial = new Material(colorSettings.planetMaterial);
            biomeColorSettings = new BiomeColorSettings(biomeColorSettings);
        }

        [System.Serializable]
        public class BiomeColorSettings
        {
            public Biome[] biomes;
            public NoiseSettings noiseSettings;
            public float noiseOffset;
            public float noiseStrength;
            [Range(0, 1)]public float blendAmount;

            public BiomeColorSettings(BiomeColorSettings biomeColorSettings)
            {
                noiseSettings = new NoiseSettings(biomeColorSettings.noiseSettings);
                noiseOffset = biomeColorSettings.noiseOffset;
                noiseStrength = biomeColorSettings.noiseStrength;
                blendAmount = biomeColorSettings.blendAmount;

                biomes = new Biome[biomeColorSettings.biomes.Length];
                for (int i = 0; i < biomeColorSettings.biomes.Length; i++)
                    biomes[i] = new Biome(biomeColorSettings.biomes[i]);
            }

            [System.Serializable]
            public class Biome
            {
                public Gradient gradient;
                public Color tint;
                [Range(0, 1)] public float startHeight;
                [Range(0, 1)] public float tintPercent;

                public Biome(Biome biome)
                {
                    gradient = biome.gradient;
                    tint = biome.tint;
                    startHeight = biome.startHeight;
                    tintPercent = biome.tintPercent;
                }

            }
        }
    }
}