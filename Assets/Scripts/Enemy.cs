using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]
public class Enemy : ScriptableObject
{
    [SerializeField] string enemyName;

    [SerializeField] int attack;

    [SerializeField] int health;

    [SerializeField] int turnCount;

    public string Name { get { return enemyName; } }

    public int Attack { get { return attack; } }

    public int Health { get { return health; } }

    public int TurnCount { get { return turnCount; } }
}
