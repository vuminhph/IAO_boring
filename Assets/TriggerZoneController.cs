using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneController : MonoBehaviour
{
    public bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other) {
        triggered = true;
    }
}
