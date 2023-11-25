namespace ProjectSRG.Utils.Common {
    public class ReferenceableVariable<T> {
        public T Value { get => _value; set => this._value = value; }
        public T startValue { get; }
        private T _value;

        public ReferenceableVariable(T startValue) {
            _value = startValue;
            this.startValue = startValue;
        }
    }
}
