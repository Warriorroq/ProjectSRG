using ProjectSRG.ObjectTraits;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public TraitsDictionary traits;

    private void Awake()
    {
        traits.Awake();
        //traits.GetTrait<Trait<float>>("ShipTemperature").onValueChanged.AddListener(x => Debug.Log(x));
        //traits.DebugTraits();
    }
}
