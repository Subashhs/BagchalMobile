using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    public GameObject gridCellPrefab; // Prefab for the grid cell (empty GameObject or UI Button)
    public GameObject tigerPrefab;    // Prefab for the tiger
    public GameObject goatPrefab;     // Prefab for the goat
    public Sprite boardImage;         // The image of the board to use as background
    public int boardSize = 5;         // Define board size (5x5)
    public Canvas canvas;             // Canvas for UI elements

    void Start()
    {
        SetUpBoardImage();
        GenerateBoard();
        PlacePieces();
    }

    // Set up the background image as the board
    void SetUpBoardImage()
    {
        GameObject board = new GameObject("Board");
        Image boardImageComponent = board.AddComponent<Image>();
        boardImageComponent.sprite = boardImage;
        boardImageComponent.rectTransform.sizeDelta = new Vector2(5 * 100, 5 * 100); // Adjust size as needed
        board.transform.SetParent(canvas.transform, false);
        board.transform.localPosition = Vector3.zero;
    }

    // Generates the grid
    void GenerateBoard()
    {
        float cellSize = gridCellPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        // Calculate the offset to center the grid
        float offset = (boardSize - 1) * cellSize / 2;

        // Generate the grid
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                // Position the grid cells with offset to center them
                Vector3 position = new Vector3(x * cellSize - offset, y * cellSize - offset, 0);

                // Instantiate the grid cell (empty GameObject or UI Button) at the position
                GameObject cell = Instantiate(gridCellPrefab, position, Quaternion.identity);
                cell.transform.SetParent(this.transform, false);
            }
        }
    }

    // Place the pieces (tigers and goats)
    void PlacePieces()
    {
        float cellSize = gridCellPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float offset = (boardSize - 1) * cellSize / 2f;

        // Place tigers at specific positions
        Vector3[] tigerPositions = {
            new Vector3(-offset, -offset, 0),
            new Vector3(offset, -offset, 0),
            new Vector3(-offset, offset, 0),
            new Vector3(offset, offset, 0)
        };

        foreach (Vector3 position in tigerPositions)
        {
            Instantiate(tigerPrefab, position, Quaternion.identity);
        }

        // Place goats along the middle row
        for (int i = 0; i < boardSize; i++)
        {
            Vector3 goatPosition = new Vector3(i * cellSize - offset, 2 * cellSize - offset, 0);
            Instantiate(goatPrefab, goatPosition, Quaternion.identity);
        }
    }
}
