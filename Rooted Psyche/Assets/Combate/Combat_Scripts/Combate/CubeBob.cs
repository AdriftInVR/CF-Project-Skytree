using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBob : MonoBehaviour
{

    // Amplitude of bob
    public float amplitude = 0.5f;

    // Frequency of bob
    public float frequency = 4f;

    // Update is called once per frame

    private Vector3 pos = new Vector3(0,0,0);

    void Start()
    {
        pos = transform.position;
    }
    void Update()
    {
        pos.y = 16.31f + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = pos;
    }
}
