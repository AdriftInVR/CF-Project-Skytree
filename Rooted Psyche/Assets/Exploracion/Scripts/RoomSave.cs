using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSave : MonoBehaviour
{
    public static RoomSave instance;
    private void Awake(){
         if (RoomSave.instance == null)
    {
        RoomSave.instance = this;
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

        // Activa o desactiva este objeto basado en si se encontró o no el objeto con tag "Combat"
        if (combatObject != null)
        {
            gameObject.SetActive(false); // Desactiva este objeto si hay un objeto con el tag "Combat"
        }
        else
        {
            gameObject.SetActive(true); // Activa este objeto si no se encuentra el objeto con el tag "Combat"
        }
    }
}
