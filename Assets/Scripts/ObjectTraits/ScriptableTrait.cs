using UnityEngine;

namespace ProjectSRG.ObjectTraits
{
    public abstract class ScriptableTrait : ScriptableObject
    {
        public string traitName;
        public abstract ITrait GetTrait();
    }
}
