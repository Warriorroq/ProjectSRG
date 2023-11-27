using UnityEngine;

namespace ProjectSRG.Game
{
    public class Sector : MonoBehaviour
    {
        [HideInInspector] public bool wasUsedForCreationOfNextSector = false;
        private void Update()
        {
            var beacon = Beacon.Instance;
            transform.position -= beacon.spaceVectorMovementDirection * beacon.speedOfSpaceThread * Time.deltaTime;
        }
    }
}