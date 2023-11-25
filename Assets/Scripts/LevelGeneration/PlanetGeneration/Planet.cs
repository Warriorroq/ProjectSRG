using System.Linq;
using UnityEngine;
namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    public class Planet : MonoBehaviour
    {
        [HideInInspector] public bool shapeSettingsFoldout;
        [HideInInspector] public bool colorSettingsFoldout;
        public bool autoUpdate;

        public ShapeSettings shapeSettings;
        public ColorSettings colorSettings;

        private ShapeGenerator _shapeGenerator;

        [SerializeField][Range(2, 256)] private int _resolution;
        [SerializeField] private MeshFilter[] _meshFilters;
        private TerrainFace[] _terrainFaces;

        public void GeneratePlanet()
        {
            Initialize();
            GenerateMesh();
            GenerateColors();
        }
        public void OnColorSettingsUpdated()
        {
            if (!autoUpdate)
                return;
            Initialize();
            GenerateColors();
        }

        public void OnShapeSettignsUpdated()
        {
            if (!autoUpdate)
                return;
            Initialize();
            GenerateMesh();
        }

        private void Initialize()
        {
            _shapeGenerator = new ShapeGenerator(shapeSettings);
            if (_meshFilters == null || _meshFilters.Count() == 0)
                _meshFilters = new MeshFilter[6];
            _terrainFaces = new TerrainFace[6];

            Vector3[] directions = new Vector3[] { Vector3.up, Vector3.down, Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

            for (int i = 0; i < 6; i++)
            {
                if (_meshFilters[i] == null || _meshFilters.Count() < 6)
                {
                    GameObject mesh = new GameObject($"Planet Mesh {i}");
                    mesh.transform.parent = transform;
                    mesh.transform.localPosition = Vector3.zero;
                    mesh.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                    _meshFilters[i] = mesh.AddComponent<MeshFilter>();
                    _meshFilters[i].sharedMesh = new Mesh();
                    //var meshCollider = mesh.AddComponent<MeshCollider>();
                    //meshCollider.convex = true;
                    //meshCollider.sharedMesh = _meshFilters[i].sharedMesh;

                }
                _terrainFaces[i] = new TerrainFace(_shapeGenerator, _meshFilters[i].sharedMesh, _resolution, directions[i]);
            }

            if (TryGetComponent<SphereCollider>(out var collider))
                collider.radius = shapeSettings.radius;
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
                meshFilther.GetComponent<MeshRenderer>().sharedMaterial.color = colorSettings.colorOfPlanet;
            }
        }
    }
}