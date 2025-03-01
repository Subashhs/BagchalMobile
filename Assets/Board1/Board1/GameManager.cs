using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum Turn { Tiger, Goat }
    public Turn currentTurn = Turn.Goat;

    public TMP_Text turnText;
    public TMP_Text winText;  // Text to show the winner

    public GameObject tigerPrefab;
    public GameObject goatPrefab;

    private List<GameObject> tigers = new List<GameObject>(); // Store tiger instances
    private List<GameObject> goats = new List<GameObject>();

    public GameObject selectedTiger;
    public GameObject selectedGoat;
    public Dictionary<string, GameObject> tiles = new Dictionary<string, GameObject>();

    private GoatMovement goatMovement;
    private bool allGoatsPlaced = false;  // Track if all goats have been placed

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        goatMovement = goatPrefab.GetComponent<GoatMovement>();
    }

    private void Start()
    {
        InitializeBoard();
        UpdateTurnText();
    }

    private void InitializeBoard()
    {
        // Define tile names and instantiate the board structure
        string[] tileNames = {
            "Tile_0_0", "Tile_0_1", "Tile_0_2", "Tile_0_3", "Tile_0_4",
            "Tile_1_0", "Tile_1_1", "Tile_1_2", "Tile_1_3", "Tile_1_4",
            "Tile_2_0", "Tile_2_1", "Tile_2_2", "Tile_2_3", "Tile_2_4",
            "Tile_3_1", "Tile_3_2", "Tile_3_3", "Tile_3_4", "Tile_3_0",
            "Tile_4_0", "Tile_4_1", "Tile_4_2", "Tile_4_3", "Tile_4_4"
        };

        foreach (string tileName in tileNames)
        {
            GameObject tile = GameObject.Find(tileName);
            if (tile != null)
            {
                tiles[tileName] = tile;
                Debug.Log($"Tile {tileName} found.");
            }
            else
            {
                Debug.LogError($"Tile {tileName} not found!");
            }
        }

        // Instantiate Tigers at specified locations
        for (int i = 0; i < 4; i++)
        {
            GameObject tiger = Instantiate(tigerPrefab, tiles[$"Tile_{i / 2}_0{(i % 2) * 4}"].transform.position, Quaternion.identity);
            tigers.Add(tiger);
        }

        Debug.Log("4 Tigers instantiated.");

        // Prepare for placement of Goats
        currentTurn = Turn.Goat; // Start with Goat's turn
    }

    public GameObject[] GetTigers() => tigers.ToArray(); // Return array of tigers

    public void PlaceGoat(GameObject tile)
    {
        if (!allGoatsPlaced && currentTurn == Turn.Goat && goats.Count < 20)
        {
            if (tiles.ContainsValue(tile) && tile.GetComponent<Tile>()?.IsOccupied() == false)
            {
                GameObject goat = Instantiate(goatPrefab, tile.transform.position, Quaternion.identity);
                goats.Add(goat);
                tile.GetComponent<Tile>().SetOccupied(true); // Assume Tile script manages occupied state

                Debug.Log($"Goat instantiated at {tile.name}.");

                if (goats.Count == 20)
                {
                    allGoatsPlaced = true; // All goats have been placed, can now move
                }

                currentTurn = Turn.Tiger; // Switch turn after placing
                UpdateTurnText();
            }
            else
            {
                Debug.LogWarning("Cannot place goat here, tile is occupied or invalid.");
            }
        }
    }

    public void SelectPiece(GameObject piece)
    {
        if (currentTurn == Turn.Tiger && tigers.Contains(piece)) // Check for Tigers
        {
            selectedTiger = piece;
            Debug.Log("Tiger selected.");
        }
        else if (currentTurn == Turn.Goat && goats.Contains(piece)) // Check for Goats
        {
            selectedGoat = piece;
            Debug.Log("Goat selected.");
        }
        else
        {
            Debug.LogWarning("Invalid selection for the current turn.");
        }
    }

    public bool IsPieceSelected()
    {
        return selectedTiger != null || selectedGoat != null;
    }

    public void MovePiece(GameObject tile)
    {
        if (selectedGoat != null && currentTurn == Turn.Goat && allGoatsPlaced && tiles.ContainsValue(tile))
        {
            if (goatMovement.TryMove(selectedGoat, tile, tiles))
            {
                selectedGoat = null; // Deselect the goat after moving
                currentTurn = Turn.Tiger; // Switch to Tiger's turn
                UpdateTurnText();
            }
            else
            {
                Debug.LogWarning("Invalid move for the Goat.");
            }
        }
        else if (selectedTiger != null && currentTurn == Turn.Tiger && tiles.ContainsValue(tile))
        {
            // Placeholder for Tiger movement logic
            Debug.LogWarning("Tiger movement logic not implemented in this version.");
        }
        else
        {
            Debug.LogWarning("Invalid move attempt.");
        }
    }

    public void UpdateTurnText()
    {
        if (turnText != null)
        {
            turnText.text = (currentTurn == Turn.Tiger) ? "TIGER'S MOVE" : "GOAT'S MOVE";
        }
    }
}