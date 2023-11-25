using UnityEngine;
namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    public class TerrainFace
    {
        private int _resolution;
        private Mesh _mesh;

        private Vector3 _localUp;
        private Vector3 _axisA;
        private Vector3 _axisB;

        private ShapeGenerator _shapeGenerator;
        public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
        {
            _mesh = mesh;
            _resolution = resolution;
            _localUp = localUp;
            _shapeGenerator = shapeGenerator;

            _axisA = new Vector3(_localUp.y, _localUp.z, _localUp.x);
            _axisB = Vector3.Cross(localUp, _axisA);
        }

        public void ConstructMesh()
        {
            Vector3[] vertices = new Vector3[_resolution * _resolution];
            var resolutionMinus1 = _resolution - 1;
            int[] triangles = new int[resolutionMinus1 * resolutionMinus1 * 6];
            int verticeIndex = 0;
            int triangleIndex = 0;
            for (int y = 0; y < _resolution; y++) 
            { 
                for(int x = 0; x < _resolution; x++)
                {
                    Vector2 percent = new Vector2(x, y) / resolutionMinus1;
                    Vector3 pointOnUnitCube = _localUp + (percent.x-0.5f)*2*_axisA + (percent.y - 0.5f) * 2 * _axisB;
                    Vector3 pointOnUnitSphere = _shapeGenerator.CalculatePointOnThePlanet(pointOnUnitCube.normalized);
                    if (x != resolutionMinus1 && y != resolutionMinus1)
                    {
                        triangles[triangleIndex++] = verticeIndex;
                        triangles[triangleIndex++] = verticeIndex + _resolution + 1;
                        triangles[triangleIndex++] = verticeIndex + _resolution;
                        triangles[triangleIndex++] = verticeIndex;
                        triangles[triangleIndex++] = verticeIndex + 1;
                        triangles[triangleIndex++] = verticeIndex + _resolution + 1;
                    }
                    vertices[verticeIndex++] = pointOnUnitSphere;
                }
            }
            _mesh.Clear();
            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();
        }
    }
}