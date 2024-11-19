using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trigger_Combat : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.name != "BrierTarget")
        {
            Debug.Log("Player has entered the combat zone");
            SceneManager.LoadScene("Combate");
            Destroy(this.gameObject);
        }
    }
}