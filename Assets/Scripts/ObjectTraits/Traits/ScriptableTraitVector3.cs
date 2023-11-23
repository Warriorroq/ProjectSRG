using UnityEngine;

namespace ProjectSRG.ObjectTraits.Traits
{
    [CreateAssetMenu(fileName = "STraitVector3", menuName = "ScriptableObjects/ScriptableTraits/Vector3")]
    public class ScriptableTraitVector3 : ScriptableTrait
    {
        public Vector3 value;
        public override ITrait GetTrait()
            => new Trait<Vector3>(traitName, value);
    }
}
