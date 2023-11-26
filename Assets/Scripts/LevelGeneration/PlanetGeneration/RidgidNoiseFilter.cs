using ProjectSRG.Utils.Noises;
using UnityEngine;


namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    public class RidgidNoiseFilter : INoiseFilter
    {
        private NoiseSettings.RidgidNoiseSettings settings;
        private SimplexNoise _noise = new SimplexNoise();

        public RidgidNoiseFilter(NoiseSettings settings)
        {
            this.settings = settings.ridgidNoiseSettings;
        }

        public float Evaluate(Vector3 point)
        {
            //(_noise.Evaluate(point * settings.roughness + settings.centre) + 1) * 0.5f
            float noiseValue = 0;
            float frequency = settings.baseRoughness;
            float amplitude = 1;
            float weight = 1;

            for (int i = 0; i < settings.numberOfLayers; i++)
            {
                float v = 1 - Mathf.Abs(_noise.Evaluate(point * frequency + settings.centre));
                v *= v;
                v *= weight;
                weight = Mathf.Clamp01(v * settings.weightMultilier);
                noiseValue += v * amplitude;
                frequency *= settings.roughness;
                amplitude *= settings.persistence;
            }

            noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
            return noiseValue * settings.strength;
        }
    }
}