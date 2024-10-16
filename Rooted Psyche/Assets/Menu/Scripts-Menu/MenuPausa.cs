using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject botonPausa;
    private bool pausado = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausado)
            {
                Reanudar();
            }
            else
            {
                Pausa();
            }
        }
    }
    public void Pausa()
    {
        pausado = true;
        Time.timeScale = 0f;
        menuPausa.SetActive(true);
        botonPausa.SetActive(false);
    }

    public void Reanudar()
    {
        pausado = false;
        Time.timeScale = 1f;
        menuPausa.SetActive(false);
        botonPausa.SetActive(true);
    }
}
