using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction_M : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            Destroy(other.gameObject);
        }
    }
}
