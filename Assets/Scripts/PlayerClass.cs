using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerClass", menuName = "ScriptableObjects/PlayerClass")]
public class PlayerClass : ScriptableObject
{
    [SerializeField] string name;

    [SerializeField] int attack;

    [SerializeField] int health;

    public string Name { get { return name; } }

    public int Attack { get { return attack; } }

    public int Health { get { return health; } }
}
