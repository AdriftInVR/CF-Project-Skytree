using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // Necesario para la navegación de botones

public class ButtonHandler : MonoBehaviour
{
    public TMP_Text nameHolder;
    public TMP_Text specialHolder;
    public Button button;

    public void UpdateButton(Player player, Action action)
    {
        nameHolder.text = action.actionName;
        specialHolder.text = action.cost > 0 ? action.cost.ToString() + " PS" : "";

        button.onClick.RemoveAllListeners();  // Asegurarse de no duplicar listeners
        button.onClick.AddListener(delegate { SetPlayerAction(player, action); });

        // Desactivar la interacción si el jugador no tiene suficiente especial
        button.interactable = player.stats.special >= action.cost;

        // Establecer el primer botón como seleccionado por defecto
        if (button.interactable && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
    }

    void SetPlayerAction(Player player, Action action)
    {
        player.currentAction = action;
    }

    // Esta función será llamada desde un script que controla la navegación con el control
    public void NavigateButton(Vector2 input)
    {
        // Si tienes un joystick, este input lo puedes usar para mover entre los botones.
        // Este es solo un ejemplo de cómo puedes implementar la navegación manual si fuera necesario.
        if (input != Vector2.zero)
        {
            // Aquí deberías controlar el cambio de selección, con un simple ejemplo usando EventSystem
            if (input.y > 0) // Arriba
            {
                EventSystem.current.SetSelectedGameObject(button.gameObject);
            }
        }
    }
}
