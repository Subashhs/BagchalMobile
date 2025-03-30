using UnityEngine;

public class GameManagerAI : MonoBehaviour
{
    // Game options for creating a room
    private string roomName = "BagchalRoom";

    void Start()
    {
        // Logic to start the game
        StartGame();
    }

    public void StartGame()
    {
        // Logic to start the game
        Debug.Log("Game is starting!");
        // Set up the game board for Bagchal here
    }
}