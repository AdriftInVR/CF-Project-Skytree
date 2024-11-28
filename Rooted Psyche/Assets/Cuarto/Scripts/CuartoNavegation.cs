using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CuartoNavegation : MonoBehaviour
{
    public GameObject MainMenu,Opt1,Opt2,Cuarto;
    public GameObject MainMenuFirstButton, Opt1FirstButton,Opt2FirstButton,CuartoFirstButton;

    private GameObject currentMenu = null; // Menú actualmente activo
    private GameObject currentFirstButton = null; // Botón predeterminado del menú activo


    void Update()
    {
        // Actualizar la lógica solo si el menú activo cambia
        CheckMenuState(MainMenu, MainMenuFirstButton);
        ExtraOptions();

       
    }

    private void ExtraOptions()
    {
        if (SceneManager.GetActiveScene().name == "CuartoSkyler"){
            // Si la escena es la de cuarto de skyler entonces activa el menu de cuarto
            CheckMenuState(Opt1, Opt1FirstButton);
            CheckMenuState(Opt2, Opt2FirstButton);
            CheckMenuState(Cuarto, CuartoFirstButton);
        }
        if (SceneManager.GetActiveScene().name == "M_CuartoSkyler"){
            CheckMenuState(Opt1, Opt1FirstButton);
            CheckMenuState(Cuarto, CuartoFirstButton);
        }
                if (SceneManager.GetActiveScene().name == "M_Exploracion"){
            CheckMenuState(Opt1, Opt1FirstButton);
        }

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

