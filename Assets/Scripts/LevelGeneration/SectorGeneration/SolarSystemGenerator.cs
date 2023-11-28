using ProjectSRG.AStarNavigation;
using ProjectSRG.Game;
using ProjectSRG.LevelGeneration.PlanetGeneration;
using ProjectSRG.Utils.Maths;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectSRG.LevelGeneration.SectorGeneration
{
    [System.Serializable]
    public class SolarSystemGenerator
    {
        public Vector3 maxOffSet;

        [Header("=== Sun settings ===")]
        public GameObject sunPrefab;
        public List<Material> sunMaterials;
        public Vector2 minMaxLocalScaleOfSun;

        [Header("=== Planet settings ===")]
        public Planet planetPrefab;
        public Vector2 minMaxLocalScaleOfPlanets;
        public Vector2 minMaxRotationSpeedOfPlanets;
        public Vector3 randomizedNoiseCenter;
        public Vector2Int amountOfPlanets;

        public List<ShapeSettings> planetShapeSettings;
        public List<ColorSettings> planetColorSettings;

        public void Generate(Sector sector)
        {
            var solarSystem = sector.AddComponent<SolarSystem>();
            GenerateAndApplySun(solarSystem);
            GeneratePlanetsInSolarSystem(solarSystem);
        }

        private void GeneratePlanetsInSolarSystem(SolarSystem system)
        {
            int amount = Random.Range(amountOfPlanets.x, amountOfPlanets.y);
            while (amount > 0)
            {
                var go = SolarSystem.Instantiate(planetPrefab);
                go.transform.parent = system.transform;
                go.shapeSettings = planetShapeSettings.TakeRandom();
                go.colorSettings = planetColorSettings.TakeRandom();
                go.transform.localScale *= Random.Range(minMaxLocalScaleOfPlanets.x, minMaxLocalScaleOfPlanets.y);
                foreach (var item in go.shapeSettings.noiseLayers)
                {
                    item.settings.simpleNoiseSettings.centre = randomizedNoiseCenter.GetRandomValueFromCurrentVector();
                    item.settings.ridgidNoiseSettings.centre = randomizedNoiseCenter.GetRandomValueFromCurrentVector();
                }
                go.shapeSettings.radius = go.transform.localScale.y / 2;
                go.Initialize();

                var minDistance = system.sun.transform.localScale.y + go.transform.localScale.y;
                minDistance *= minDistance;
                int i = 30;
                while (i > 0)
                {
                    var position = maxOffSet.GetRandomValueFromCurrentVector();
                    if (!Physics.CheckSphere(position, go.transform.localScale.y * 2f))
                    {
                        if (Vector3.SqrMagnitude(go.transform.localPosition - system.sun.transform.position) < minDistance)
                        {
                            amount--;
                            continue;
                        }

                        go.transform.localPosition = position;
                        system.AddBody(go.gameObject, Random.Range(minMaxRotationSpeedOfPlanets.x, minMaxRotationSpeedOfPlanets.y));
                        go.GeneratePlanet();
                        go.GeneratePlanet();
                        go.GeneratePlanet();
                        amount--;
                        break;
                    }
                    i--;
                }

                if(i == 0)
                    GameObject.Destroy(go);
            }
        }

        private GameObject GenerateAndApplySun(SolarSystem system)
        {
            var sun = Sector.Instantiate(sunPrefab);
            system.sun = sun;
            sun.transform.parent = system.transform;
            sun.transform.localScale *= Random.Range(minMaxLocalScaleOfSun.x, minMaxLocalScaleOfSun.y);
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
