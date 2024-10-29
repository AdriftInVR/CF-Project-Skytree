using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject botonPausa;
    private bool paused = false;
    public PlayerInput myInput;

    void Awake()
    {
        myInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (myInput.actions["Pause"].WasPressedThisFrame())
        {
            if (paused)
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
