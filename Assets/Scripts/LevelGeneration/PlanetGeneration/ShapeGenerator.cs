using UnityEngine;

namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    public class ShapeGenerator
    {
        private ShapeSettings _shapeSettings;

        public ShapeGenerator(ShapeSettings shapeSettings)
        {
            _shapeSettings = shapeSettings;
        }

        public Vector3 CalculatePointOnThePlanet(Vector3 pointOnUnitySphere)
            => pointOnUnitySphere * _shapeSettings.radius;
    }
}
