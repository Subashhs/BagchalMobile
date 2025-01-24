using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Player { Tiger, Goat }
    public Player currentPlayer;

    public GameObject selectedPiece;  // The currently selected piece (tiger or goat)
    public GameBoard gameBoard;       // Reference to the GameBoard script
    private bool isPieceSelected = false;

    public float gridSize = 1.0f; // Size of the grid cells (used for grid snapping)

    void Start()
    {
        currentPlayer = Player.Tiger; // Start with Tiger's turn
    }

    void Update()
    {
        // Listen for user input to select or move a piece
        if (Input.GetMouseButtonDown(0)) // Left-click or touch to select or move a piece
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // If a piece is selected, check if it's a valid move and move it
            if (isPieceSelected && selectedPiece != null)
            {
                // Snap the mouse position to the nearest grid cell
                Vector2 gridPosition = SnapToGrid(mousePosition);

                // Move the piece to the new position
                selectedPiece.transform.position = new Vector3(gridPosition.x, gridPosition.y, 0);

                // Deselect the piece and switch turn
                isPieceSelected = false;
                selectedPiece = null;
                SwitchTurn();
            }
            else
            {
                // If no piece is selected, select a piece based on where the user clicks
                SelectPiece(mousePosition);
            }
        }
    }

    // Select a piece based on where the user clicks
    void SelectPiece(Vector2 position)
    {
        // Use a collider check to see if the user clicked on a piece (Tiger or Goat)
        Collider2D hit = Physics2D.OverlapPoint(position);

        if (hit != null)
        {
            // Only allow selecting a piece that belongs to the current player
            if (hit.gameObject.CompareTag("Tiger") && currentPlayer == Player.Tiger)
            {
                selectedPiece = hit.gameObject;
                isPieceSelected = true;
                Debug.Log("Tiger Selected");
            }
            else if (hit.gameObject.CompareTag("Goat") && currentPlayer == Player.Goat)
            {
                selectedPiece = hit.gameObject;
                isPieceSelected = true;
                Debug.Log("Goat Selected");
            }
        }
    }

    // Snap the mouse position to the nearest grid cell for proper piece alignment
    Vector2 SnapToGrid(Vector2 position)
    {
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float y = Mathf.Round(position.y / gridSize) * gridSize;
        return new Vector2(x, y);
    }

    // Switch turns between Tiger and Goat players
    void SwitchTurn()
    {
        if (currentPlayer == Player.Tiger)
        {
            currentPlayer = Player.Goat;
        }
        else
        {
            currentPlayer = Player.Tiger;
        }

        Debug.Log("Current Turn: " + currentPlayer);
    }
}
