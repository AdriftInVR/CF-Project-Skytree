using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para interactuar con UI

public class CameraFocus : MonoBehaviour
{
    // Player and proximity settings
    public GameObject player;         // El jugador
    public GameObject targetObject;   // El objeto de destino
    public float triggerDistance = 5f; // Distancia en la que el botón aparece

    // Camera focus settings
    public Camera mainCamera;         // La cámara principal
    public Camera focusCamera;        // La cámara de enfoque
    private bool isFocusing = false;  // Estado de si está enfocando

    // UI Buttons
    public Button focusButton;        // El botón de UI para enfocar
    public Button exitButton;         // El botón de UI para salir del enfoque
    public List<Button> additionalButtons; // Lista de botones adicionales que se deben ocultar

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

        // Asegurarse de que el botón de enfoque esté oculto al inicio
        focusButton.gameObject.SetActive(false);
    }

    void Update()
    {
        // Calcular la distancia entre el jugador y el objeto objetivo
        float distance = Vector3.Distance(player.transform.position, targetObject.transform.position);

        // Mostrar el botón si el jugador está dentro de la distancia especificada
        if (distance <= triggerDistance && !isFocusing)
        {
            focusButton.gameObject.SetActive(true);
        }
        else if (!isFocusing) // Ocultar el botón si el jugador está fuera de la distancia
        {
            focusButton.gameObject.SetActive(false);
        }
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

            // Ocultar los botones adicionales
            foreach (var button in additionalButtons)
            {
                button.gameObject.SetActive(false);
            }
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

            // Mostrar los botones adicionales
            foreach (var button in additionalButtons)
            {
                button.gameObject.SetActive(true);
            }
        }
    }

    // Opcional: Dibujar el área de enfoque en la vista de escena
    void OnDrawGizmosSelected()
    {
        // Dibujar una esfera para visualizar el área de activación
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetObject.transform.position, triggerDistance);
    }
}

