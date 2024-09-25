using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timber : Fighter
{
    void Awake()
    {
        this.stats = new Stats(100, 100, 10, 10, 50, 1, 10, 1.0f, 1.0f);
    }
    public override void InitTurn()
    {
        
    }
}
