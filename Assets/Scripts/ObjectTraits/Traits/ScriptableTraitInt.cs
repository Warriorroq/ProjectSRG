using UnityEngine;

namespace ProjectSRG.ObjectTraits.Traits
{
    [CreateAssetMenu(fileName = "STraitInt", menuName = "ScriptableObjects/ScriptableTraits/Int")]
    public class ScriptableTraitInt : ScriptableTrait
    {
        public int value;
        public override ITrait GetTrait()
            => new Trait<int>(traitName, value);
    }
}