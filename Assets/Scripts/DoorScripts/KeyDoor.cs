
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{

    private DoorAnims doorAnims;

    private void Awake()
    {
        doorAnims = GetComponent<DoorAnims>();
    }

    public void OpenDoor()
    {
        doorAnims.OpenDoor();
    }

    public void PlayOpenFailAnim()
    {
        doorAnims.PlayOpenFailAnim();
    }

}
