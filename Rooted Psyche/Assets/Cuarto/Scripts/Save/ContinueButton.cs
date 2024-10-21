using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    public PlayerDataManager playerDataManager;

    public void OnContinueButtonPressed()
    {
        playerDataManager.LoadGame();
    }
}

