using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Muerte : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
        StartCoroutine(cambio());
    }

    IEnumerator cambio()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("Pantalla_Carga");
    }
}
