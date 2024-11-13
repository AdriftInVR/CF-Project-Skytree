using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Focus_M : MonoBehaviourPunCallbacks
{
    public GameObject targetObject;          // El objeto de destino
    public float triggerDistance = 5f;       // Distancia en la que el botón aparece

    public Camera mainCamera;                // La cámara principal
    public Camera focusCamera;               // La cámara de enfoque
    private bool isFocusing = false;         // Estado de si está enfocando

    public Button focusButton;               // El botón de UI para enfocar
    public Button exitButton;                // El botón de UI para salir del enfoque
    public List<Button> additionalButtons;   // Lista de botones adicionales que se deben ocultar

    private GameObject player;

    void Start()
    {
        // Solo ejecuta el código si es el jugador local
        if (!photonView.IsMine)
        {
            enabled = false;
            return;
        }

        // Encuentra el jugador local si aún no se ha asignado
        player = PhotonNetwork.LocalPlayer.TagObject as GameObject;

        // Vincular los eventos de los botones a los métodos de enfoque
        focusButton.onClick.AddListener(OnFocusButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);

        focusCamera.enabled = false;
        mainCamera.enabled = true;
        exitButton.gameObject.SetActive(false);
        focusButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!player) return; // Asegura que el jugador esté asignado

        float distance = Vector3.Distance(player.transform.position, targetObject.transform.position);

        if (distance <= triggerDistance && !isFocusing)
        {
            focusButton.gameObject.SetActive(true);
        }
        else if (!isFocusing)
        {
            focusButton.gameObject.SetActive(false);
        }
    }

    void OnFocusButtonClick()
    {
        if (!isFocusing)
        {
            focusCamera.enabled = true;
            mainCamera.enabled = false;
            isFocusing = true;

            focusButton.gameObject.SetActive(false);
            exitButton.gameObject.SetActive(true);

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
            focusCamera.enabled = false;
            mainCamera.enabled = true;
            isFocusing = false;

            exitButton.gameObject.SetActive(false);
            focusButton.gameObject.SetActive(true);

            foreach (var button in additionalButtons)
            {
                button.gameObject.SetActive(true);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetObject.transform.position, triggerDistance);
    }
}
