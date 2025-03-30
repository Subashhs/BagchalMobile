using UnityEngine;

public class GameBoardAI : MonoBehaviour
{
    public GameObject tiger;
    public GameObject[] goats = new GameObject[5];
    private Vector3[] goatPositions = new Vector3[5];  // You can keep an array of positions for each goat

    void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        // Set up the initial positions of the tiger and goats
        SetInitialPositions();
    }

    void SetInitialPositions()
    {
        // Set the initial positions of the tiger and goats on the board
        tiger.transform.position = new Vector3(2, 0, 2);  // Example position
        for (int i = 0; i < 5; i++)
        {
            goats[i].transform.position = new Vector3(i, 0, 0);  // Example positions for goats
        }
    }

    public void MoveGoat(int goatIndex, Vector3 newPosition)
    {
        goats[goatIndex].transform.position = newPosition;
    }
}