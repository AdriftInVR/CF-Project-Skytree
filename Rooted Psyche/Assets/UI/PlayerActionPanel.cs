using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerActionPanel : MonoBehaviour
{
    public GameObject[] actionButtons;
    public TMP_Text[] actionButtonLabels;

    void Awake(){
        this.Hide();
        foreach (var btn in this.actionButtons){
            btn.SetActive (false);
        }
    }

    public void ConfigureButtons(int index, string actionName){
        this.actionButtons[index].SetActive(true);
        this.actionButtonLabels[index].text = actionName;
    }

    public void Show (){
        this.gameObject.SetActive(true);
    }

    public void Hide(){
        this.gameObject.SetActive(false);
    }
    
}
