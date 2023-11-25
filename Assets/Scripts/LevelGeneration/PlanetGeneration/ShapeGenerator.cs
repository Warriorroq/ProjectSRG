using UnityEngine;

namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    public class ShapeGenerator
    {
        private ShapeSettings _shapeSettings;
        private NoiseFilter _noiseFilter;
        public ShapeGenerator(ShapeSettings shapeSettings)
        {
            _shapeSettings = shapeSettings;
            _noiseFilter = new NoiseFilter(shapeSettings.noiseSettings);
        }

        public Vector3 CalculatePointOnThePlanet(Vector3 pointOnUnitySphere)
        {
            float elevation = _noiseFilter.Evalutate(pointOnUnitySphere);
            return pointOnUnitySphere * _shapeSettings.radius * (1 + elevation);
        }
    }
}
