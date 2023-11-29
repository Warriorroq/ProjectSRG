using ProjectSRG.ObjectTraits;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectSRG
{
    public class HealthBehaviour : MonoBehaviour
    {
        public UnityEvent onDamageTaken;
        public UnityEvent onHeal;
        public UnityEvent onDestroy;
        public Trait<float> health;
        public Trait<float> maxHealth;
        [SerializeField] private Unit _owner;

        private void Awake()
        {
            if(!_owner)
                _owner = GetComponent<Unit>();
        }

        private void Start()
        {
            health = _owner.traits.GetTrait<Trait<float>>("Health");
            maxHealth = _owner.traits.GetTrait<Trait<float>>("MaxHealth");
        }

        public void TakeDamage(float amount)
        {
            onDamageTaken.Invoke();
            health.Value -= amount;
            Debug.Log(health.Value);
            if (health.Value < 0)
            {
                onDestroy.Invoke();
                Destroy(_owner.gameObject);
            }
        }

        public void Heal(float amount)
        {
            onHeal.Invoke();
            health.Value += amount;
            health.Value = Mathf.Clamp(health.Value, 0, maxHealth.Value);
        }
    }
}
