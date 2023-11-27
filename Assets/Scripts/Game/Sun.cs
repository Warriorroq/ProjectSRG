using ProjectSRG.ObjectTraits;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    [SerializeField] private float _heatPerSecond;
    [SerializeField] private AnimationCurve _heatCurve;
    [SerializeField] private float _activeRadiusPerScaleUnit;
    [SerializeField] private SphereCollider _triggerCollider;
    private List<(Unit, Trait<float>)> _affectedUnits;
    private void Awake()
    {
        _affectedUnits = new List<(Unit, Trait<float>)>();
    }

    private void Start()
    {
        _triggerCollider.radius = _activeRadiusPerScaleUnit;
        _activeRadiusPerScaleUnit *= transform.parent.localScale.y;
        _activeRadiusPerScaleUnit *= _activeRadiusPerScaleUnit;
    }

    private void Update()
    {
        foreach(var tuple in _affectedUnits)
        {
            float sqrtDistance = Vector3.SqrMagnitude(tuple.Item1.transform.position - transform.position);
            float heatToAdd = _heatCurve.Evaluate(Mathf.Clamp01(1 - sqrtDistance/_activeRadiusPerScaleUnit)) * Time.deltaTime * _heatPerSecond;
            tuple.Item2.Value += heatToAdd;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Unit>(out var unit))
            return;
        var trait = unit.traits.GetTrait<Trait<float>>("ShipTemperature");
        if (trait is not default(Trait<float>))
            _affectedUnits.Add((unit, trait));
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Unit>(out var unit))
            return;
        var trait = unit.traits.GetTrait<Trait<float>>("ShipTemperature");
        if (trait is not default(Trait<float>))
            _affectedUnits.RemoveAll(item => item.Item1 == unit);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.body.TryGetComponent<Unit>(out var unit))
            return;
        var trait = unit.traits.GetTrait<Trait<float>>("ShipTemperature");
        if (trait is not default(Trait<float>))
            _affectedUnits.RemoveAll(item => item.Item1 == unit);

        //TODO: kill unit
    }
}
