using UnityEngine;

namespace ProjectSRG.ObjectTraits.Traits
{
    [CreateAssetMenu(fileName = "STraitFloat", menuName = "ScriptableObjects/ScriptableTraits/Float")]
    public class ScriptableTraitFloat : ScriptableTrait
    {
        public float value;
        public override ITrait GetTrait()
            => new Trait<float>(traitName, value);
    }
}