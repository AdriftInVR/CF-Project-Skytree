using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSave_M : MonoBehaviour
{
    public static RoomSave_M instance;
    private void Awake(){
         if (RoomSave_M.instance == null)
    {
        RoomSave_M.instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    else
    {
        Destroy(this.gameObject);
    }
    }

private void Update()
{
    GameObject combatObject = GameObject.FindWithTag("Combat");

    // Verifica si hay un objeto con el tag "Combat"
    if (combatObject != null)
    {
        // Desactiva todos los hijos
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    else
    {
        // Activa todos los hijos
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    
}

}
