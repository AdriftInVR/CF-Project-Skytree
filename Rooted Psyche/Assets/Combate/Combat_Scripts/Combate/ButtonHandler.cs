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
        button.onClick.AddListener(delegate { SetPlayerAction(player, action); });

        // Desactivar la interacción si el jugador no tiene suficiente especial
        button.interactable = player.stats.special >= action.cost;

        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }
    void SetPlayerAction(Player player, Action action)
    {
        player.currentAction = action;
    }
}
