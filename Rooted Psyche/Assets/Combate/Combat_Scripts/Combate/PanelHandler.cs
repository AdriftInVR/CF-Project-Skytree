using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PanelHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Panels;

    private static GameObject Solo;
    private static GameObject Duo;
    private static GameObject Item;
    private static GameObject Fusion;
    private static GameObject Run;
    private static GameObject Current;
    
    void Awake()
    {
        Solo = Panels[0];
        Duo = Panels[1];
        Item = Panels[2];
        Fusion = Panels[3];
        Run = Panels[4];
    }

    void Update()
    {
        if(CombatManager.cancelButton.WasPressedThisFrame() && CombatManager.menuOpen)
        {
            ClosePanel();
            WheelSelection.lockedRotation = false;
        };
    }

    public static void SoloActive()
    {
        Solo.SetActive(true);
        CombatManager.menuOpen = true;
        Current = Solo;
    }

    public static void DuoActive()
    {
        Duo.SetActive(true);
        CombatManager.menuOpen = true;
        Current = Duo;
    }

    public static void ItemActive()
    {
        Item.SetActive(true);
        CombatManager.menuOpen = true;
        Current = Item;
    }

    public static void FusionActive()
    {
        Fusion.SetActive(true);
        CombatManager.menuOpen = true;
        Current = Fusion;
    }

    public static void RunActive()
    {
        Run.SetActive(true);
        CombatManager.menuOpen = true;
        Current = Run;
    }

    public static void ReopenActive()
    {
        Current.SetActive(true);
        CombatManager.menuOpen = true;
    }

    public static void ClosePanel()
    {
        Solo.SetActive(false);
        Duo.SetActive(false);
        Item.SetActive(false);
        Fusion.SetActive(false);
        Run.SetActive(false);
        PlayerController.locked = false;
        CombatManager.menuOpen = false;
    }
}
