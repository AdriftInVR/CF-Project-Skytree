using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 8, -10);
    public Transform player;

    // Update is called once per frame
    void Update () {
        Vector3 playerPos = new Vector3(player.transform.position.x,0,player.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, playerPos + offset,0.5f);
    }
}
