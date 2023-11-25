using UnityEngine;
namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    [CreateAssetMenu(fileName = "PlanetColorSettings", menuName = "ScriptableObjects/PlanetGeneration/PlanetColorSettings")]
    public class ColorSettings : ScriptableObject
    {
        [ColorUsage(true, true)] public Color colorOfPlanet;
    }
}