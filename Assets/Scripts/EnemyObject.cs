using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyObject : MonoBehaviour
{
    public Enemy enemyBase;

    public int health;
    public int attack;
    public int turnCount;

    [SerializeField] TMP_Text title;

    // Start is called before the first frame update
    void Start()
    {
        health = enemyBase.Health;
        attack = enemyBase.Attack;
        turnCount = enemyBase.TurnCount;

        title.text = enemyBase.Name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
