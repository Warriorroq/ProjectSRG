using UnityEngine;
namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    [CreateAssetMenu(fileName = "PlanetShapeSettings", menuName = "ScriptableObjects/PlanetGeneration/PlanetShapeSettings")]
    public class ShapeSettings : ScriptableObject
    {
        public float radius = 1;
    }
}