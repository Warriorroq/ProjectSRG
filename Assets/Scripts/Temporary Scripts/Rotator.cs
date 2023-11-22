using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]private Vector3 _speed;
    void Update()
    {
        transform.eulerAngles += (_speed * Time.deltaTime);
    }
}
