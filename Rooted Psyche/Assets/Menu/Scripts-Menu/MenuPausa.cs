using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject botonPausa;
    public static bool paused;
    private PlayerInput myInput;

    void Awake()
    {
        paused = false;
        myInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (myInput.actions["Pause"].WasPressedThisFrame())
        {
            if (!paused)
            {
                Pausa();
            }
            else
            {
                Reanudar();
            }
        }
    }
    public void Pausa()
    {
        paused = true;
        Time.timeScale = 0f;
        menuPausa.SetActive(true);
        botonPausa.SetActive(false);
    }

    public void Reanudar()
    {
        paused = false;
        Time.timeScale = 1f;
        menuPausa.SetActive(false);
        botonPausa.SetActive(true);
    }
}
