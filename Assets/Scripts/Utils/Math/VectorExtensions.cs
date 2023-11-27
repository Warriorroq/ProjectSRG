using UnityEngine;

namespace ProjectSRG.Utils.Maths{
    public static class VectorExtensions {
        public static Vector2 ClampNeg1To1 (this Vector2 v) {
            return new Vector2( v.x.ClampNeg1To1(), v.y.ClampNeg1To1() );
        }

        public static Vector3 GetRandomValueFromCurrentVector(this Vector3 v)
        {
            Vector3 v1 = new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
            return new Vector3(Random.Range(-v1.x, v1.x), Random.Range(-v1.y, v1.y), Random.Range(-v1.z, v1.z));
        }
    }
}
