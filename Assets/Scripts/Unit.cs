using ProjectSRG.ObjectTraits;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public TraitsDictionary traits;

    private void Awake()
    {
        traits.Awake();
        //traits.DebugTraits();
    }
    private void Update()
    {
        
    }
}
