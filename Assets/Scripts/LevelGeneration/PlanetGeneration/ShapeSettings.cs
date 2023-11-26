using UnityEngine;
namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    [CreateAssetMenu(fileName = "PlanetShapeSettings", menuName = "ScriptableObjects/PlanetGeneration/PlanetShapeSettings")]
    public class ShapeSettings : ScriptableObject
    {
        public float radius = 1;
        public NoiseLayer[] noiseLayers;

        public ShapeSettings(ShapeSettings shapeSettings) 
        { 
            radius = shapeSettings.radius;
            noiseLayers = new NoiseLayer[shapeSettings.noiseLayers.Length];
            for (int i = 0; i < noiseLayers.Length; i++)
                noiseLayers[i] = new NoiseLayer(shapeSettings.noiseLayers[i]);
        }

        [System.Serializable]
        public class NoiseLayer
        {
            public NoiseLayer(NoiseLayer noiseLayer)
            {
                enabled = noiseLayer.enabled;
                useFirstLayerAsMask = noiseLayer.useFirstLayerAsMask;
                settings = new NoiseSettings(noiseLayer.settings);
            }

            public bool enabled = true;
            public bool useFirstLayerAsMask;
            public NoiseSettings settings;
        }
    }
}