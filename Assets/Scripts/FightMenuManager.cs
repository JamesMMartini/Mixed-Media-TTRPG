using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FightMenuManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject enemyParent;
    [SerializeField] GameObject partyParent;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject turnIcon;
    [SerializeField] GameObject actionMenu;
    [SerializeField] Camera mainCamera;

    List<GameObject> enemies;
    List<GameObject> players;
    List<List<GameObject>> turnOrder;
    int turnList = 0;
    int turnIndex = 0;
    bool reading;
    GameObject actedObject;
    int readingCount;

    // Start is called before the first frame update
    void Start()
    {
        reading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (turnList == 0) // The players are currently acting
        {
            if (!reading && Input.GetMouseButtonUp(0)) // Someone is choosing who to act on
            {
                RaycastHit2D ray = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));

                if (ray.collider != null && (ray.collider.tag == "Enemy" || ray.collider.tag == "Player"))
                {
                    actedObject = ray.collider.gameObject;
                    reading = true;
                    readingCount = 1;
                }
            }
            else if (reading && Input.GetMouseButtonUp(0)) // Someone is acting
            {
                RaycastHit2D ray = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));

                if (ray.collider.gameObject == actedObject)
                {
                    readingCount++;
                }
            }
            else if (reading && Input.GetKeyUp(KeyCode.Return)) // Someone is finishing an action
            {
                if (actedObject.tag == "Enemy") // Deal damage to an enemy
                {
                    actedObject.GetComponent<EnemyObject>().health -= readingCount;
                    if (actedObject.GetComponent<EnemyObject>().health <= 0)
                    {
                        // Kill the enemy
                        enemies.Remove(actedObject);
                        Destroy(actedObject);

                        Debug.Log(enemyParent.transform.childCount);

                        // Check to see if all of the enemies are dead
                        if (enemies.Count == 0) // If they're all dead
                        {
                            gameManager.EndFight();
                        }
                    }
                }
                else if (actedObject.tag == "Player") // Heal a player
                {
                    // Find the correct player object in the party and add the necessary health
                    for (int i = 0; i < gameManager.players.Count; i++)
                    {
                        if (players[i] == actedObject)
                        {
                            gameManager.players[i].health += readingCount;

                            actedObject.transform.Find("Health").GetComponent<TMP_Text>().text = "Health: " + gameManager.players[i].health;
                        }
                    }
                }

                // Reset the acted object
                reading = false;
                actedObject = null;
                readingCount = 0;

                // Go to the next player in the list
                if (turnIndex < turnOrder[turnList].Count - 1)
                {
                    turnIndex++;
                }
                else
                {
                    if (turnList == 0)
                        turnList = 1;
                    else
                        turnList = 0;

                    turnIndex = 0;
                }

                // Move the turn icon
                turnIcon.transform.position = turnOrder[turnList][turnIndex].transform.position;
                turnIcon.transform.position = new Vector3(turnIcon.transform.position.x, turnIcon.transform.position.y + 1.2f, turnIcon.transform.position.z);
            }
        }
        else if (turnList == 1) // The enemies are acting
        {
            StartCoroutine(EnemiesActing());
            turnList = 2;
        }
    }

    IEnumerator EnemiesActing()
    {
        foreach (GameObject enemy in enemies) // Go through the enemies that need to act
        {
            EnemyObject enemyObject = enemy.GetComponent<EnemyObject>();
            int turn = 1;

            while (turn <= enemyObject.turnCount)
            {
                // Choose a player to attack
                int playerIndex = (int)(Random.value * turnOrder[0].Count);

                // Deal the damage
                gameManager.players[playerIndex].health -= enemyObject.attack;
                players[playerIndex].transform.Find("Health").GetComponent<TMP_Text>().text = "Health: " + gameManager.players[playerIndex].health;

                turn++;

                yield return new WaitForSeconds(1f);
            }
            turnIndex++;

            if (turnIndex < turnOrder[1].Count)
            {
                // Move the turn icon
                turnIcon.transform.position = turnOrder[1][turnIndex].transform.position;
                turnIcon.transform.position = new Vector3(turnIcon.transform.position.x, turnIcon.transform.position.y + 1.2f, turnIcon.transform.position.z);
            }
        }

        // Swap the list
        turnList = 0;
        turnIndex = 0;

        // Move the turn icon
        turnIcon.transform.position = turnOrder[turnList][turnIndex].transform.position;
        turnIcon.transform.position = new Vector3(turnIcon.transform.position.x, turnIcon.transform.position.y + 1.2f, turnIcon.transform.position.z);
    }

    public void NewRoom()
    {
        // Create the forest room object
        int difficulty = 1;
        if (gameManager.row < 3 && gameManager.column < 3)
        {
            difficulty = ((int)(Random.value * 3)) + 1;
        }
        else if (gameManager.row < 5 && gameManager.column < 5)
        {
            difficulty = ((int)(Random.value * 3)) + 4;
        }
        else
        {
            difficulty = ((int)(Random.value * 3)) + 7;
        }

        if (gameManager.row == 5 && gameManager.column == 5)
            difficulty = 10;

        ForestRoom room = new ForestRoom(difficulty);

        // Create the enemy objects
        int numEnemies = room.enemies.Count;
        enemies = new List<GameObject>();

        for (int i = 0; i < numEnemies; i++)
        {
            GameObject newEnemy = GameObject.Instantiate(enemyPrefab);
            newEnemy.transform.parent = enemyParent.transform;
            newEnemy.GetComponent<EnemyObject>().enemyBase = room.enemies[i];
            enemies.Add(newEnemy);
        }

        // Now we need to lay out the enemies on the screen
        int numRows = 1;
        if (numEnemies > 4)
            numRows = 2;

        if (numRows == 1)
        {
            float half = (float)numEnemies / 3;
            float distance = 4f;

            for (int i = 0; i < numEnemies; i++)
            {
                enemies[i].transform.position = new Vector3(distance * (i - half), 0f, 0f);
            }

        }
        else
        {
            int half = numEnemies / 2;
            float third = (((float)numEnemies) / 2) / 2.5f;
            float distance = 4f;

            for (int i = 0; i < half; i++)
            {
                enemies[i].transform.position = new Vector3(distance * (i - third), 2.7f, 0f);
            }

            for (int i = half; i < enemies.Count; i++)
            {
                enemies[i].transform.position = new Vector3(distance * (i - half - third), 0f, 0f);
            }

        }

        // Add the player tokens
        players = new List<GameObject>();
        for (int i = 0; i < gameManager.players.Count; i++)
        {
            GameObject playerToken = GameObject.Instantiate(playerPrefab);
            playerToken.transform.parent = partyParent.transform;
            playerToken.transform.localScale = new Vector3(1f, 1f, 1f);
            playerToken.transform.Find("Name").GetComponent<TMP_Text>().text = gameManager.players[i].name;
            playerToken.transform.Find("Health").GetComponent<TMP_Text>().text = "Health: " + gameManager.players[i].health;
            players.Add(playerToken);

            playerToken.transform.localPosition = new Vector3(i * 150f, 0f, 0f);
        }

        // Create the turn order
        turnOrder = new List<List<GameObject>>();
        turnOrder.Add(players);
        turnOrder.Add(enemies);

        // Reset the turn orders
        turnList = 0;
        turnIndex = 0;

        // Create the turn order icon over the first player
        turnIcon.transform.position = turnOrder[0][0].transform.position;
        turnIcon.transform.position = new Vector3(turnIcon.transform.position.x, turnIcon.transform.position.y + 1.2f, turnIcon.transform.position.z);
    }
}
