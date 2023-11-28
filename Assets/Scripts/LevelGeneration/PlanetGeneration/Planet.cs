using System.Linq;
using UnityEngine;
namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    public class Planet : MonoBehaviour
    {
        [HideInInspector] public bool shapeSettingsFoldout;
        [HideInInspector] public bool colorSettingsFoldout;
        public bool autoUpdate;
        public bool createMeshColliders;

        public FaceRenderMask faceRenderMask;
        public ShapeSettings shapeSettings;
        public ColorSettings colorSettings;

        private ShapeGenerator _shapeGenerator = new ShapeGenerator();
        private ColorGenerator _colorGenerator = new ColorGenerator();

        [Range(2, 256)] public int resolution;
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

        public void Initialize()
        {
            _shapeGenerator.UpdateSettings(shapeSettings);
            _colorGenerator.UpdateSettings(colorSettings);

            if (_meshFilters == null || _meshFilters.Count() == 0)
                _meshFilters = new MeshFilter[6];
            _terrainFaces = new TerrainFace[6];

            Vector3[] directions = new Vector3[] { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back};

            for (int i = 0; i < 6; i++)
            {
                if (_meshFilters[i] == null || _meshFilters.Count() < 6)
                {
                    GameObject obj = new GameObject($"Planet Mesh {i}");
                    obj.layer = gameObject.layer;
                    obj.transform.parent = transform;
                    obj.transform.localPosition = Vector3.zero;
                    obj.AddComponent<MeshRenderer>();
                    _meshFilters[i] = obj.AddComponent<MeshFilter>();
                    _meshFilters[i].sharedMesh = new Mesh();
                    if (createMeshColliders)
                    {
                        var meshCollider = obj.AddComponent<MeshCollider>();
                        meshCollider.sharedMesh = new Mesh();
                        meshCollider.convex = true;
                        meshCollider.sharedMesh = _meshFilters[i].sharedMesh;
                    }

                }
                _meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = new Material(colorSettings.planetMaterial);
                _terrainFaces[i] = new TerrainFace(_shapeGenerator, _meshFilters[i].sharedMesh, resolution, directions[i]);
                bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
                _meshFilters[i].gameObject.SetActive(renderFace);
            }

            if (TryGetComponent<SphereCollider>(out var collider))
                collider.radius = shapeSettings.radius;
        }
        
        public void GenerateMesh()
        {
            for (int i = 0; i < _terrainFaces.Length; i++)
            {
                if (!_meshFilters[i].gameObject.activeSelf)
                    continue;

                _terrainFaces[i].ConstructMesh();
                if (createMeshColliders && _meshFilters[i].TryGetComponent(out MeshCollider collider))
                    collider.sharedMesh = _meshFilters[i].sharedMesh;
            }

            _colorGenerator.UpdateElevation(_shapeGenerator.elevationMinMax);
        }

        public void GenerateColors()
        {
            _colorGenerator.UpdateColors();
            for(int i = 0 ; i < _terrainFaces.Length; i++)
            {
                if (_meshFilters[i].gameObject.activeSelf)
                    _terrainFaces[i].UpdateUVs(_colorGenerator);
            }
        }

        public enum FaceRenderMask
        {
            All,
            Top, 
            Bottom,
            Left,
            Right,
            Front,
            Back
        }
    }
}