using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 10, -12);
    public Transform player;

    // Update is called once per frame
    void Update () {
        transform.position = player.transform.position + offset;
    }
}
