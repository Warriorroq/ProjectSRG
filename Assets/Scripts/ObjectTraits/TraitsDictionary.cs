using System.Collections.Generic;
using UnityEngine;

namespace ProjectSRG.ObjectTraits
{
    [System.Serializable]
    public class TraitsDictionary
    {
        private Dictionary<string, ITrait> _traits;
        [SerializeField] private List<ScriptableTrait> _traitsToCreate;

        public void Awake()
        {
            if (_traits is null)
                _traits = new Dictionary<string, ITrait>();

            foreach (var traitData in _traitsToCreate)
                _traits.Add(traitData.traitName, traitData.GetTrait());
        }

        public T GetTrait<T>(string name) where T : ITrait
        {
            if(_traits is null)
                throw new System.Exception($"Awake wasn't played");

            if (!_traits.ContainsKey(name))
                return default(T);

            return (T)_traits[name];
        }

        public void AddTrait(string name, ITrait trait)
            => _traits?.Add(name, trait);

        public void DebugTraits()
        {
            foreach(var trait in _traits.Values)
                Debug.Log(trait.ToString());
        }
    }
}