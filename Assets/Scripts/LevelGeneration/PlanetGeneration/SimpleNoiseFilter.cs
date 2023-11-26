using ProjectSRG.Utils.Noises;
using UnityEngine;

namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    public class SimpleNoiseFilter : INoiseFilter
    {
        private NoiseSettings settings;
        private SimplexNoise _noise = new SimplexNoise();

        public SimpleNoiseFilter(NoiseSettings settings)
        {
            this.settings = settings;
        }

        public float Evaluate(Vector3 point)
        {
            //(_noise.Evaluate(point * settings.roughness + settings.centre) + 1) * 0.5f
            float noiseValue = 0;
            float frequency = settings.baseRoughness;
            float amplitude = 1;

            for (int i = 0; i < settings.numberOfLayers; i++)
            {
                float v = _noise.Evaluate(point * frequency + settings.centre);
                noiseValue += (v + 1)*.5f * amplitude;
                frequency *= settings.roughness;
                amplitude *= settings.persistence;
            }

            noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
            return noiseValue * settings.strength;
        }
    }
}
