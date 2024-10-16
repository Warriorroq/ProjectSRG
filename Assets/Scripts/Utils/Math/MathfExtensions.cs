using UnityEngine;

namespace ProjectSRG.Utils.Maths
{
    public static class MathfExtensions
    {
        public static float InverseRelationship(float constant, float mutable) {
            return constant / mutable;
        }

        public static float Frac(this float f) {
            return f - Mathf.FloorToInt(f);
        }

        public static float ClampNeg1To1(this float f) {
            return Mathf.Clamp(f, -1f, 1f);
        }

        public static float ClampPos1ToMaxValue(this float f) {
            return Mathf.Clamp(f, 1f, float.MaxValue);
        }
    }
}
