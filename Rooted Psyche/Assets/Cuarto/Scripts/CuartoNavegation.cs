using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CuartoNavegation : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject MainMenuFirstButton;

    private GameObject currentMenu = null; // Menú actualmente activo
    private GameObject currentFirstButton = null; // Botón predeterminado del menú activo


    void Update()
    {
        // Actualizar la lógica solo si el menú activo cambia
        CheckMenuState(MainMenu, MainMenuFirstButton);
    }

    private void CheckMenuState(GameObject menu, GameObject firstButton)
    {
        if (menu.activeInHierarchy)
        {
            // Si este menú es diferente al actual, actualiza el estado
            if (currentMenu != menu)
            {
                ActivateMenu(menu, firstButton);
            }
        }
        else
        {
            // Si el menú actual ya no está activo, limpiarlo
            if (currentMenu == menu)
            {
                currentMenu = null;
                currentFirstButton = null;
            }
        }
    }

    private void ActivateMenu(GameObject menu, GameObject firstButton)
    {
        Debug.Log($"Activando menú: {menu.name}");

        // Actualizar el menú actual y su botón predeterminado
        currentMenu = menu;
        currentFirstButton = firstButton;

        // Establecer la selección en el botón correspondiente
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
}

