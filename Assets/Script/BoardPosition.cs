using UnityEngine;

public class BoardPosition : MonoBehaviour
{
    public int x; // X coordinate of this position on the board
    public int y; // Y coordinate of this position on the board

    private GameManager gameManager;

    void Start()
    {
        // Find the GameManager instance
        gameManager = GameManager.Instance;

        // Ensure the GameManager is found
        if (gameManager == null)
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    void OnMouseDown()
    {
        // Notify the GameManager when this position is clicked
        if (gameManager != null)
        {
            gameManager.OnPositionClicked(x, y);
        }
    }
}