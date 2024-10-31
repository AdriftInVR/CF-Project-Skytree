using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float DestroyTime = 1f;
    public Vector3 RamdomizeIntensity = new Vector3(0.5f, 0, 0);
    void Start(){
        ShowAndDestroy();
    }
    public void ShowAndDestroy()
    {
       Destroy(gameObject, DestroyTime);
       //transform.localPosition += new Vector3(Random.Range(-RamdomizeIntensity.x, RamdomizeIntensity.x), Random.Range(-RamdomizeIntensity.y, RamdomizeIntensity.y), Random.Range(-RamdomizeIntensity.z, RamdomizeIntensity.z));

    }
}
