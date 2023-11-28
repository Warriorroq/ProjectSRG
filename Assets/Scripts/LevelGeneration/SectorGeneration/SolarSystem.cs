using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    public GameObject sun;
    public List<SolarSystemBody> bodiesAroundSun = new List<SolarSystemBody>();

    public void AddBody(GameObject go, float rotationSpeed)
    {
        var sbody = new SolarSystemBody();
        sbody.spaceObject = go;
        sbody.speed = rotationSpeed;
        bodiesAroundSun.Add(sbody);
    }

    private void Update()
    {
        foreach(var bodyData in bodiesAroundSun)
        {
            var direction = bodyData.spaceObject.transform.position - sun.transform.position;
            direction = Quaternion.AngleAxis(bodyData.speed * Time.deltaTime, Vector3.up) * direction;
            bodyData.spaceObject.transform.position = sun.transform.position + direction;
        }
    }

    [System.Serializable]
    public struct SolarSystemBody
    {
        public float speed;
        public GameObject spaceObject;
    }
}
