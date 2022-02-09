using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRoomMenu : MonoBehaviour
{
    [SerializeField] Vector3 activePos;
    [SerializeField] Vector3 inactivePos;

    public Camera mainCamera;
    public GameManager gameManager;

    [SerializeField] GameObject upButton;
    [SerializeField] GameObject downButton;
    [SerializeField] GameObject leftButton;
    [SerializeField] GameObject rightButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D ray = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));

            if (ray.collider != null)
            {
                if (ray.collider.name == "Up")
                {
                    gameManager.ChangeRoom("up");
                }
                else if (ray.collider.name == "Down")
                {
                    gameManager.ChangeRoom("down");
                }
                else if (ray.collider.name == "Left")
                {
                    gameManager.ChangeRoom("left");
                }
                else if (ray.collider.name == "Right")
                {
                    gameManager.ChangeRoom("right");
                }
            }
        }
    }

    public void LoadMenu()
    {
        // Set all the buttons active or inactive
        if (gameManager.row == 0)
            upButton.SetActive(false);
        else
            upButton.SetActive(true);

        if (gameManager.row == gameManager.rooms.Length - 1)
            downButton.SetActive(false);
        else
            downButton.SetActive(true);

        if (gameManager.column == 0)
            leftButton.SetActive(false);
        else
            leftButton.SetActive(true);

        if (gameManager.column == gameManager.rooms[0].Length - 1)
            rightButton.SetActive(false);
        else
            rightButton.SetActive(true);

        transform.position = activePos;
    }

    public void UnloadMenu()
    {
        transform.position = inactivePos;
        //gameObject.SetActive(false);
    }
}
