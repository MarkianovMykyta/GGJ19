using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RespawnZone")) return;

        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        other.transform.position = new Vector3(0, 10, 0);
    }
}
