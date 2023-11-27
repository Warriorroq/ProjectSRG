using ProjectSRG.Game;
using ProjectSRG.Utils.Maths;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSRG.LevelGeneration.SectorGeneration
{
    [System.Serializable]
    public class SolarSystemGenerator
    {
        public Vector3 maxOffSet;
        public List<Material> sunMaterials;
        public Vector2 maxScaleOfSun;
        public GameObject sunPrefab;
        public void Generate(Sector sector)
        {
            GenerateAndApplySun(sector);
        }

        private GameObject GenerateAndApplySun(Sector sector)
        {
            var sun = Sector.Instantiate(sunPrefab);
            sun.transform.parent = sector.transform;
            sun.transform.localScale *= Random.Range(maxScaleOfSun.x, maxScaleOfSun.y);
            int i = 100;
            while (i > 0)
            {
                var position = maxOffSet.GetRandomValueFromCurrentVector();
                if(Physics.CheckSphere(position, sun.transform.localScale.x))
                {
                    sun.transform.localPosition = position;
                    break;
                }
                i--;
            }
            sunPrefab.GetComponent<MeshRenderer>().material = sunMaterials.TakeRandom();
            return sun;
        }
    }
}
