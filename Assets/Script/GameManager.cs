using UnityEngine;

public enum Player { Tiger, Goat }

public class GameManager : MonoBehaviour
{
    public Player currentPlayer = Player.Goat;
    public GameObject[] tigers;
    public GameObject[] goats;
    public bool isGameOver = false;

    void Start()
    {
        // Initialize the game
        tigers = new GameObject[4]; // 4 Tigers
        goats = new GameObject[20]; // 20 Goats
    }

    public void ChangeTurn()
    {
        currentPlayer = (currentPlayer == Player.Tiger) ? Player.Goat : Player.Tiger;
    }

    public void CheckWinCondition()
    {
        // Check for Tiger win (all goats captured)
        if (goats.Length == 0)
        {
            Debug.Log("Tigers Win!");
            isGameOver = true;
        }

        // Check for Goat win (Tigers blocked)
        // Implement logic for blocked tigers
    }
}