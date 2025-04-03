using UnityEngine;

public class GameManagerAI : MonoBehaviour
{
    private string playerCharacter;

    void Start()
    {
        playerCharacter = PlayerPrefs.GetString("PlayerCharacter", "Goat"); // Default to Goat if not set
        StartGame();
    }

    public void StartGame()
    {
        Debug.Log("Game is starting!");
        SetupGameBoard();
    }

    void SetupGameBoard()
    {
        BagchalGameAI gameAI = FindObjectOfType<BagchalGameAI>();
        if (gameAI != null)
        {
            gameAI.InitializeBoard(playerCharacter);
        }
        else
        {
            Debug.LogError("BagchalGameAI component not found!");
        }
    }
}