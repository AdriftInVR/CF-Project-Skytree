using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats 
{
    public int health;
    public int MaxHealth;
    public int attack;
    public int defense;
    public int speed;
    public int level;
    //Los puntos de special sera lo que se necesite para usar habilidades especiales - Puede ser cambiado a Puntos de sincronizacion (PZ)
    public int special;

    public Stats(int health,int MaxHealth, int attack, int defense, int speed, int level, int special)
    {
        this.health = health;
        this.MaxHealth = MaxHealth;
        this.attack = attack;
        this.defense = defense;
        this.speed = speed;
        this.level = level;
        this.special = special;
    }

    public Stats Clone (){
        return new Stats(this.health, this.MaxHealth, this.attack, this.defense, this.speed, this.level, this.special);
    }
}
