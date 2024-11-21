using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHandler : MonoBehaviour
{
    public TMP_Text nameHolder;
    public TMP_Text specialHolder;
    public Button button;

    public void UpdateButton(Player player, Action action)
    {
        nameHolder.text = action.actionName;
        specialHolder.text = action.cost > 0 ? action.cost.ToString() + " PS":"";
        button.onClick.AddListener(delegate {SetPlayerAction(player, action);});
        if(player.stats.special >= action.cost)
        {
            button.interactable = true;
        }
        else 
        {
            button.interactable = false;    
        }
    }

    void SetPlayerAction(Player player, Action action)
    {
        player.currentAction = action;
    }
}
