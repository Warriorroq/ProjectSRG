using System.Linq;
using UnityEngine;
namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    public class Planet : MonoBehaviour
    {
        [SerializeField] private ShapeSettings _shapeSettings;
        [SerializeField] private ColorSettings _colorSettings;
        
        private ShapeGenerator _shapeGenerator;

        [SerializeField][Range(2, 256)] private int _resolution;
        private MeshFilter[] _meshFilters;
        private TerrainFace[] _terrainFaces;

        private void OnValidate()
        {
            Initialize();
            GenerateMesh();
        }

        public void OnColorSettingsUpdated()
        {
            Initialize();
            GenerateColors();
        }

        public void OnShapeSettignsUpdated()
        {
            Initialize();
            GenerateMesh();
        }

        private void Initialize()
        {
            _shapeGenerator = new ShapeGenerator(_shapeSettings);
            if (_meshFilters == null || _meshFilters.Count() == 0)
                _meshFilters = new MeshFilter[6];
            _terrainFaces = new TerrainFace[6];

            Vector3[] directions = new Vector3[] { Vector3.up, Vector3.down, Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

            for (int i = 0; i < 6; i++)
            {
                if (_meshFilters[i] == null)
                {
                    GameObject mesh = new GameObject($"Planet Mesh {i}");
                    mesh.transform.parent = transform;
                    mesh.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                    _meshFilters[i] = mesh.AddComponent<MeshFilter>();
                    _meshFilters[i].sharedMesh = new Mesh();
                }
                _terrainFaces[i] = new TerrainFace(_shapeGenerator, _meshFilters[i].sharedMesh, _resolution, directions[i]);
            }
        }
        
        private void GenerateMesh()
        {
            foreach (TerrainFace terrainFace in _terrainFaces)
                terrainFace.ConstructMesh();
        }

        private void GenerateColors()
        {
            foreach(var meshFilther in _meshFilters)
            {
                meshFilther.GetComponent<MeshRenderer>().sharedMaterial.color = _colorSettings.colorOfPlanet;
            }
        }
    }
}