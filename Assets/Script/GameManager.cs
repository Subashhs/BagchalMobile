using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Player { Tiger, Goat }
    public Player currentPlayer;

    public GameObject selectedPiece;  // The currently selected piece (tiger or goat)
    public GameBoard gameBoard;       // Reference to the GameBoard script to manage pieces and the board
    private bool isPieceSelected = false;

    void Start()
    {
        currentPlayer = Player.Tiger; // Start with Tiger's turn
        selectedPiece = null;  // Initially no piece is selected
    }

    void Update()
    {
        // Listen for user input to select a piece and move it
        if (Input.GetMouseButtonDown(0))  // Left-click (or touch) to select or move a piece
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Check if a piece is selected and move it
            if (isPieceSelected && selectedPiece != null)
            {
                // Check if the click is on a valid grid cell position
                if (IsValidGridPosition(mousePosition))
                {
                    // Move the piece to the selected position
                    selectedPiece.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
                    isPieceSelected = false;  // Deselect the piece after moving it
                    SwitchTurn();  // Switch the turn after moving the piece
                }
            }
            else
            {
                // If no piece is selected, select a piece based on user click
                SelectPieceAtPosition(mousePosition);
            }
        }
    }

    // Select a piece based on where the user clicks
    void SelectPieceAtPosition(Vector2 position)
    {
        // Find all the pieces (tiger or goat) and check if the user clicked on one
        Collider2D hit = Physics2D.OverlapPoint(position);  // Check for a collision at the mouse click position
        if (hit != null && hit.gameObject.CompareTag("Tiger") && currentPlayer == Player.Tiger)
        {
            // Select the tiger piece
            selectedPiece = hit.gameObject;
            isPieceSelected = true;
        }
        else if (hit != null && hit.gameObject.CompareTag("Goat") && currentPlayer == Player.Goat)
        {
            // Select the goat piece
            selectedPiece = hit.gameObject;
            isPieceSelected = true;
        }
    }

    // Check if the position clicked is valid for the piece to move to
    bool IsValidGridPosition(Vector2 position)
    {
        // Here we could add logic for valid movement (e.g., tigers can move 1 space at a time, goats can move 1 space horizontally or vertically)
        return true;  // For simplicity, assume every click is valid. You can implement movement rules here.
    }

    // Switch the turn between Tiger and Goat
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
