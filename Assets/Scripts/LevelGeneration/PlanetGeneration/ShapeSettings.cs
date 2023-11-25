using System.Collections.Generic;
using UnityEngine;
namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    [CreateAssetMenu(fileName = "PlanetShapeSettings", menuName = "ScriptableObjects/PlanetGeneration/PlanetShapeSettings")]
    public class ShapeSettings : ScriptableObject
    {
        public float radius = 1;
        public NoiseLayer[] noiseLayers;

        [System.Serializable]
        public class NoiseLayer
        {
            public bool enabled = true;
            public bool useFirstLayerAsMask;
            public NoiseSettings settings;
        }
    }
}