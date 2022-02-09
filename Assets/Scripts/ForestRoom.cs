using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestRoom
{
    GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    List<Enemy> enemyTypes;

    public List<Enemy> enemies;

    public ForestRoom (int difficulty)
    {
        enemyTypes = gameManager.enemyTypes;

        enemies = new List<Enemy>();
        GenerateEnemies(difficulty);
    }

    public void GenerateEnemies(int difficulty)
    {

        if (difficulty < 4)
        {
            int numEnemies = (int)((Random.value * difficulty) + 2);
            for (int i = 0; i < numEnemies; i++)
            {
                enemies.Add(enemyTypes[0]);
            }
        }
        else if (difficulty < 7)
        {
            int numEnemies = (int)((Random.value * (difficulty - 1)) + 3);
            for (int i = 0; i < numEnemies; i++)
            {
                int enemyType = (int)(Random.value * 2);
                enemies.Add(enemyTypes[enemyType]);
            }
        }
        else if (difficulty < 10)
        {
            int numEnemies = (int)((Random.value * (difficulty - 1)) + 3);
            for (int i = 0; i < numEnemies; i++)
            {
                int enemyType = (int)(Random.value * 3);
                enemies.Add(enemyTypes[enemyType]);
            }
        }
        else // Miniboss
        {
            enemies.Add(enemyTypes[3]);
        }
    }
}
