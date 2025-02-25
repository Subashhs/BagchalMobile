using UnityEngine;
using UnityEngine.SceneManagement;  // Needed for scene management
using TMPro;
using System.Collections.Generic;

public class GameManagerBoard2 : MonoBehaviour
{
    public static GameManagerBoard2 Instance { get; private set; }

    public enum Turn { Tiger, Goat }
    public Turn currentTurn = Turn.Goat;

    public TMP_Text turnText;
    public TMP_Text winText;  // Text to show the winner

    public GameObject tigerPrefab;
    public GameObject goatPrefab;

    public GameObject selectedTiger;
    public GameObject selectedGoat;
    public Dictionary<string, GameObject> tiles = new Dictionary<string, GameObject>();
    public GameObject tiger;
    public List<GameObject> goats = new List<GameObject>();

    private GoatMovementB2 goatMovement;
    private TigerMovementB2 tigerMovement;

    private bool tigerCapturedGoat = false;  // Track if the Tiger has captured a Goat

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

        goatMovement = goatPrefab.GetComponent<GoatMovementB2>();
        tigerMovement = tigerPrefab.GetComponent<TigerMovementB2>();
    }

    private void Start()
    {
        InitializeBoard();
        UpdateTurnText();
    }

    private void InitializeBoard()
    {
        string[] tileNames = { "Tile_0_0", "Tile_1_0", "Tile_1_1", "Tile_1_2", "Tile_2_0", "Tile_2_1", "Tile_2_2", "Tile_3_0", "Tile_3_1", "Tile_3_2" };

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

        if (tiles.ContainsKey("Tile_0_0") && tigerPrefab != null)
        {
            tiger = Instantiate(tigerPrefab, tiles["Tile_0_0"].transform.position, Quaternion.identity);
            Debug.Log("Tiger instantiated.");
        }

        string[] goatPositions = { "Tile_3_0", "Tile_3_1", "Tile_3_2" };
        foreach (string position in goatPositions)
        {
            if (tiles.ContainsKey(position) && goatPrefab != null)
            {
                GameObject goat = Instantiate(goatPrefab, tiles[position].transform.position, Quaternion.identity);
                goats.Add(goat);
                Debug.Log($"Goat instantiated at {position}.");
            }
        }
    }

    public void SelectPiece(GameObject piece)
    {
        if (currentTurn == Turn.Tiger && piece == tiger)
        {
            selectedTiger = piece;
            Debug.Log("Tiger selected.");
        }
        else if (currentTurn == Turn.Goat && goats.Contains(piece))
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
        if (selectedTiger != null && currentTurn == Turn.Tiger && tiles.ContainsValue(tile))
        {
            if (tigerMovement.TryMove(selectedTiger, tile, tiles))
            {
                selectedTiger = null;
                currentTurn = Turn.Goat;
                CheckForWinCondition();
                UpdateTurnText();
            }
            else
            {
                Debug.LogWarning("Invalid move for the Tiger.");
            }
        }
        else if (selectedGoat != null && currentTurn == Turn.Goat && tiles.ContainsValue(tile))
        {
            if (goatMovement.TryMove(selectedGoat, tile, tiles))
            {
                selectedGoat = null;
                currentTurn = Turn.Tiger;
                UpdateTurnText();
            }
            else
            {
                Debug.LogWarning("Invalid move for the Goat.");
            }
        }
        else
        {
            Debug.LogWarning("Invalid move attempt.");
        }
    }

    private void CheckForWinCondition()
    {
        // Check if Tiger captured a Goat
        if (!tigerCapturedGoat && goats.Count < 3)
        {
            tigerCapturedGoat = true;
            winText.text = "TIGER WINS! CAPTURED A GOAT!";
            Invoke("ReturnToOptionBoard", 3f);  // Wait 3 seconds to show the win message before transitioning
            return;
        }

        // Check if Tiger is blocked by 3 Goats (Goats win)
        int blockedMoves = 0;
        foreach (var tile in tiles.Values)
        {
            if (!tigerMovement.IsTileOccupied(tile))
            {
                blockedMoves++;
            }
        }

        if (blockedMoves == 0)
        {
            winText.text = "GOATS WIN! TIGER IS BLOCKED!";
            Invoke("ReturnToOptionBoard", 3f);
        }
    }

    public void UpdateTurnText()
    {
        if (turnText != null)
        {
            turnText.text = (currentTurn == Turn.Tiger) ? "TIGER'S MOVE" : "GOAT'S MOVE";
        }
    }

    private void ReturnToOptionBoard()
    {
        SceneManager.LoadScene("OptionBoard");
    }
}
