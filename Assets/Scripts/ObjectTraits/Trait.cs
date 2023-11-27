using UnityEngine;
using UnityEngine.Events;

namespace ProjectSRG.ObjectTraits
{
    [System.Serializable]
    public class Trait<T> : ITrait
    {
        public readonly string name;
        public UnityEvent<T> onValueChanged;
        [SerializeField] private T _value;

        public Trait(string name, T value)
        {
            this.name = name;
            _value = value;
            onValueChanged = new UnityEvent<T>();
        }

        public T Value
        {
            get => _value;
            set
            {
                ModifyValue(ref value);
                _value = value;
                onValueChanged.Invoke(_value);
            }
        }

        public override string ToString()
            => $"[{this.GetType()}] {name}: {_value}";

        protected void ModifyValue(ref T value){}
    }
}
