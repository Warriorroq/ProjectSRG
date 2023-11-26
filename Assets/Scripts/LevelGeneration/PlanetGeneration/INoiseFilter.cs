using UnityEngine;

namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    public interface INoiseFilter
    {
        public float Evaluate(Vector3 pointOnUnitSphere);
    }
}