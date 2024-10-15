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
    //Los puntos de mana sera lo que se necesite para usar habilidades especiales - Puede ser cambiado a Puntos de sincronizacion (PZ)
    public int Mana;
    public float AttackMultiplier; 
    public float specialAttackMultiplier; 

    public Stats(int health,int MaxHealth, int attack, int defense, int speed, int level, int Mana, float AttackMultiplier, float specialAttackMultiplier)
    {
        this.health = health;
        this.MaxHealth = MaxHealth;
        this.attack = attack;
        this.defense = defense;
        this.speed = speed;
        this.level = level;
        this.Mana = Mana;
        this.AttackMultiplier = AttackMultiplier;
        this.specialAttackMultiplier = specialAttackMultiplier;
    }

    public Stats Clone (){
        return new Stats(this.health, this.MaxHealth, this.attack, this.defense, this.speed, this.level, this.Mana, this.AttackMultiplier, this.specialAttackMultiplier);
    }
}
