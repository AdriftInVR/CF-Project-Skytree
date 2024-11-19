using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trigger_Boss_M : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player has entered the combat zone");
            SceneManager.LoadScene("M_CombateJefe");
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
