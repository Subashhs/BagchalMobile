using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    public GameManager gameManager;

    void OnMouseDown()
    {
        if (gameManager.isGameOver) return;

        if (gameManager.currentPlayer == Player.Tiger && gameObject.CompareTag("Tiger"))
        {
            // Move tiger piece logic here
        }
        else if (gameManager.currentPlayer == Player.Goat && gameObject.CompareTag("Goat"))
        {
            // Move goat piece logic here
        }
    }

    public void MovePiece(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    // Capture logic for tiger
    public void CaptureGoat(GameObject goat)
    {
        // Remove the goat from the game and update the game state
        Destroy(goat);
        gameManager.CheckWinCondition();
    }
}