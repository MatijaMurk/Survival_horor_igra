using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    private bool isPlayerInteracting = false;
    public bool isOpen = false;
    public bool isLocked=false;
    [SerializeField] GameObject door;

    public void OpenDoor()
    {
        isPlayerInteracting=true;
    }

    private void RotateDoor()
    {
        door.gameObject.transform.Rotate(door.transform.localRotation.x, door.transform.localRotation.y, door.transform.localRotation.z + 90);
    }

    private void Update()
    {
        if (isPlayerInteracting)
        {
            RotateDoor();
        }

    }
}
