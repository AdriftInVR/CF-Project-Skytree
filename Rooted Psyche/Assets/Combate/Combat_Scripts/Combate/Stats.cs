using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int special;
    public int maxSpecial;
    public int attack;
    public int defense;
    public int speed;
    public int level;

    public Stats(int health, int maxHealth, int special, int maxSpecial, int attack, int defense, int speed, int level)
    {
        this.health = health;
        this.maxHealth = maxHealth;
        this.special = special;
        this.maxSpecial = maxSpecial;
        this.attack = attack;
        this.defense = defense;
        this.speed = speed;
        this.level = level;
    }

    public Stats Clone()
    {
        return new Stats(this.health, this.maxHealth, this.special, this.maxSpecial, this.attack, this.defense, this.speed, this.level);
    }
}

