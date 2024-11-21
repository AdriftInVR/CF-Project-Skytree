using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillHandle : MonoBehaviour
{
    // Start is called before the first frame update
    void OnDestroy()
    {
        CombatManager.combatStatus = CombatStatus.CHECK_FOR_VICTORY;
    }
}
