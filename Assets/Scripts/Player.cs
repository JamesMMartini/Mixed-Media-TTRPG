using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public PlayerClass classBase;

    public int health;

    public int attack;

    public string name;

    public Player(PlayerClass classBase)
    {
        this.classBase = classBase;

        health = classBase.Health;

        attack = classBase.Attack;

        name = classBase.Name;
    }
}
