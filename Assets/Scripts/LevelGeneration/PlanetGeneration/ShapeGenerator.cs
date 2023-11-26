using UnityEngine;

namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    public class ShapeGenerator
    {
        private ShapeSettings _shapeSettings;
        private INoiseFilter[] _noiseFilters;
        public ShapeGenerator(ShapeSettings shapeSettings)
        {
            _shapeSettings = shapeSettings;
            var shapeNoiseLayers = shapeSettings.noiseLayers;
            _noiseFilters = new INoiseFilter[shapeNoiseLayers.Length];

            for (int i = 0; i < shapeNoiseLayers.Length; i++)
                _noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(shapeSettings.noiseLayers[i].settings);
        }

        public Vector3 CalculatePointOnThePlanet(Vector3 pointOnUnitySphere)
        {
            float firstLayerValue = 0f;
            float elevation = 0;
            if (_noiseFilters.Length > 0)
            {
                firstLayerValue = _noiseFilters[0].Evaluate(pointOnUnitySphere);
                if (_shapeSettings.noiseLayers[0].enabled)
                    elevation = firstLayerValue;
            }

            for(int i = 1; i < _noiseFilters.Length; i++)
            {
                if (!_shapeSettings.noiseLayers[i].enabled)
                    continue;

                float mask = _shapeSettings.noiseLayers[i].useFirstLayerAsMask ? firstLayerValue : 1;
                elevation += _noiseFilters[i].Evaluate(pointOnUnitySphere) * mask;
            }
            return pointOnUnitySphere * _shapeSettings.radius * (1 + elevation);
        }
    }
}
