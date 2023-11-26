using UnityEngine;
namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    public class ColorGenerator 
    {
        private ColorSettings _settings;
        private Texture2D _texture;
        private const int _textureResolution = 50;
        private INoiseFilter _biomeNoiseFilter;
        public void UpdateSettings(ColorSettings settings)
        {
            _settings = settings;
            int numberOfBiomes = settings.biomeColorSettings.biomes.Length;
            if (_texture == null || _texture.height != numberOfBiomes)
                _texture = new Texture2D(_textureResolution, numberOfBiomes);
            _biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColorSettings.noiseSettings);
        }

        public void UpdateElevation(MinMax elevationMinMax)
            =>_settings.planetMaterial.SetVector("_ElevationMinMax", new Vector4(elevationMinMax.min, elevationMinMax.max));

        public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
        {
            float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
            var biomeColorSettings = _settings.biomeColorSettings;
            heightPercent += (_biomeNoiseFilter.Evaluate(pointOnUnitSphere) - biomeColorSettings.noiseOffset) * biomeColorSettings.noiseStrength;
            float biomeIndex = 0;
            int numBiomes = biomeColorSettings.biomes.Length;
            float blendRange = biomeColorSettings.blendAmount / 2f + 0.001f;

            for (int i = 0; i < numBiomes; i++)
            {
                float dst = heightPercent - biomeColorSettings.biomes[i].startHeight;
                float weight = Mathf.InverseLerp(-blendRange, blendRange, dst);
                biomeIndex *= 1 - weight;
                biomeIndex += i * weight;
            }
            return biomeIndex / Mathf.Max(1, numBiomes - 1);
        }

        public void UpdateColors()
        {
            Color[] colors = new Color[_texture.width * _texture.height];
            int colorIndex = 0;
            foreach (var biome in _settings.biomeColorSettings.biomes)
            {
                for (int i = 0; i < _textureResolution; i++)
                {
                    Color gradientColor = biome.gradient.Evaluate(i / (_textureResolution - 1f));
                    Color tintColor = biome.tint;
                    colors[colorIndex++] = gradientColor * (1 - biome.tintPercent) + tintColor * biome.tintPercent;
                }
            }
            _texture.SetPixels(colors);
            _texture.Apply();
            _settings.planetMaterial.SetTexture("_PlanetTexture", _texture);
        }
    }
}
