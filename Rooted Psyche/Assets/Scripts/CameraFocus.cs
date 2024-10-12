using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para interactuar con UI

public class CameraFocus : MonoBehaviour
{
    public Camera mainCamera; // La cámara principal
    public Camera focusCamera; // Referencia a la cámara principal
    private bool isFocusing = false; // Estado de si está enfocando
  
    public Button focusButton; // El botón de UI para enfocar
    public Button exitButton; // El botón de UI para salir del enfoque
    public Button additionalButton; // El botón adicional que se debe ocultar

    void Start()
    {
        // Vincular el evento del botón al método que activa el enfoque
        focusButton.onClick.AddListener(OnFocusButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);

        // Desactivar la cámara de enfoque al inicio
        focusCamera.enabled = false;
        mainCamera.enabled = true;

        // Desactivar el botón de salida al inicio
        exitButton.gameObject.SetActive(false);
    }

    void OnFocusButtonClick()
    {
        if (!isFocusing)
        {
            // Activar la cámara de enfoque y desactivar la principal
            focusCamera.enabled = true;
            mainCamera.enabled = false;
            isFocusing = true;

            // Ocultar el botón de enfoque y mostrar el botón de salida
            focusButton.gameObject.SetActive(false);
            exitButton.gameObject.SetActive(true);

            // Ocultar el botón adicional
            additionalButton.gameObject.SetActive(false);
        }
    }

    void OnExitButtonClick()
    {
        if (isFocusing)
        {
            // Activar la cámara principal y desactivar la de enfoque
            focusCamera.enabled = false;
            mainCamera.enabled = true;
            isFocusing = false;

            // Ocultar el botón de salida y mostrar el botón de enfoque
            exitButton.gameObject.SetActive(false);
            focusButton.gameObject.SetActive(true);

            // Mostrar el botón adicional
            additionalButton.gameObject.SetActive(true);
        }
    }
}

