using UnityEngine;
namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    [CreateAssetMenu(fileName = "PlanetColorSettings", menuName = "ScriptableObjects/PlanetGeneration/PlanetColorSettings")]
    public class ColorSettings : ScriptableObject
    {
        public Gradient gradient;
        public Material planetMaterial;
    }
}