using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowHandler : MonoBehaviour
{
    public GameObject shadow;
    Ray shadowRay;
    RaycastHit hit;
    public LayerMask layerMask;
    private Vector3 offset;
    public float offX, offZ;

    private void FixedUpdate()
    {
        shadowRay = new Ray(transform.position, -Vector3.up);
        if(Physics.Raycast(shadowRay, out hit, 50f, layerMask))
        {
            Vector3 targetPos = new Vector3(this.transform.position.x + offX, hit.point.y, this.transform.position.z + offZ);
            shadow.transform.position = Vector3.Lerp(shadow.transform.position, targetPos, 1.1f);   
        }
    }
}
