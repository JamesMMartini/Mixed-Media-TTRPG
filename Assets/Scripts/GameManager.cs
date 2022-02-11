using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Enemy> enemyTypes;
    public List<PlayerClass> playerClasses;

    public bool[][] rooms = new bool[6][];
    public int row;
    public int column;

    public NextRoomMenu nextRoomMenu;

    public List<Player> players;

    public GameObject fightMenu;
    public GameObject lootMenu;
    public GameObject winMenu;

    // Start is called before the first frame update
    void Start()
    {
        players = new List<Player>();

        // Create the player objects (TEMPORARY)
        for (int i = 0; i < playerClasses.Count; i++)
        {
            Player newPlayer = new Player(playerClasses[i]);
            players.Add(newPlayer);
        }

        // Create the room grid and the default position
        row = 0;
        column = 0;

        for (int i = 0; i < rooms.Length; i++)
            rooms[i] = new bool[6];

        nextRoomMenu.LoadMenu();
    }

    public void EndFight()
    {
        if (row == 5 && column == 5)
        {
            fightMenu.SetActive(false);
            winMenu.SetActive(true);
        }
        else
        {
            fightMenu.SetActive(false);
            lootMenu.SetActive(true);
        }
    }

    public void ShowRoomMenu()
    {
        lootMenu.SetActive(false);
        nextRoomMenu.gameObject.SetActive(true);
    }

    public void ChangeRoom(string dir)
    {
        // Go to the next room index and whatnot
        switch (dir)
        {
            case "up":
                row--;
                break;
            case "down":
                row++;
                break;
            case "left":
                column--;
                break;
            case "right":
                column++;
                break;
        }

        // See if we have entered the boss room
        if (row == rooms.Length - 1 && column == rooms[0].Length - 1)
        {
            // Mark the room as visited
            rooms[row][column] = true;

            // Simulate the encounter
            fightMenu.SetActive(true);
            fightMenu.GetComponent<FightMenuManager>().NewRoom();
            nextRoomMenu.LoadMenu();
            nextRoomMenu.gameObject.SetActive(false);
        }
        else if (!rooms[row][column]) // If we haven't been to the room
        {
            Debug.Log(row + ", " + column);

            // Mark the room as visited
            rooms[row][column] = true;

            // Simulate the encounter
            fightMenu.SetActive(true);
            fightMenu.GetComponent<FightMenuManager>().NewRoom();
            nextRoomMenu.LoadMenu();
            nextRoomMenu.gameObject.SetActive(false);

            // TEMPORARY
            // Load the next room menu (Should normally unload the menu)
            //nextRoomMenu.LoadMenu();
        }
        else // We have been to the room before
        {
            Debug.Log(row + ", " + column);

            // Load the next room menu
            nextRoomMenu.LoadMenu();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
