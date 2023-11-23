using UnityEngine;

namespace ProjectSRG.ObjectTraits.Traits
{
    [CreateAssetMenu(fileName = "STraitBool", menuName = "ScriptableObjects/ScriptableTraits/Bool")]
    public class ScriptableTraitBool : ScriptableTrait
    {
        public bool value;
        public override ITrait GetTrait()
            => new Trait<bool>(traitName, value);
    }
}
