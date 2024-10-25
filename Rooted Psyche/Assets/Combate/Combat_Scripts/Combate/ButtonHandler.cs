using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHandler : MonoBehaviour
{
    public GameObject textHolder;
    public Button button;

    public void UpdateButton(Player player, Action action)
    {
        textHolder.GetComponent<TMP_Text>().text = action.actionName;
        button.onClick.AddListener(delegate {SetPlayerAction(player, action);});
    }

    void SetPlayerAction(Player player, Action action)
    {
        player.currentAction = action;
    }
}
