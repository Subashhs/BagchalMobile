using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManagerBoard3 : MonoBehaviour
{
    public static GameManagerBoard3 Instance { get; private set; }

    public enum Turn { Tiger, Goat }
    public Turn currentTurn = Turn.Goat;

    public TMP_Text turnText;
    public TMP_Text winText;

    public GameObject tigerPrefab;
    public GameObject goatPrefab;

    public GameObject selectedTiger;
    public GameObject selectedGoat;
    public Dictionary<string, GameObject> tiles = new Dictionary<string, GameObject>();
    public GameObject tiger;
    public List<GameObject> goats = new List<GameObject>();

    private GoatMovementB3 goatMovement;
    private TigerMovementB3 tigerMovement;

    

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

        goatMovement = goatPrefab.GetComponent<GoatMovementB3>();
        tigerMovement = tigerPrefab.GetComponent<TigerMovementB3>();
    }

    private void Start()
    {
        InitializeBoard();
        UpdateTurnText();
    }

    private void InitializeBoard()
    {
        string[] tileNames = { "Tile_0_0", "Tile_1_0", "Tile_1_1", "Tile_1_2", "Tile_2_0", "Tile_2_1", "Tile_2_2", "Tile_3_0", "Tile_3_1", "Tile_3_2", "Tile_4_0", "Tile_4_1", "Tile_4_2" };

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

        string[] goatPositions = { "Tile_4_0", "Tile_4_1", "Tile_4_2" };
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
        Debug.Log($"Attempting to select piece: {piece.name}");

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
        Debug.Log($"Attempting to move piece to tile: {tile.name}");

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
                CheckForWinCondition();
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
        // Tiger wins if goats count is less than 3.
        if (goats.Count < 3)
        {
            winText.text = "TIGER WINS! CAPTURED A GOAT!";
            Invoke("ReturnToOptionBoard", 3f);
            return;
        }

        // Goats win if tiger is blocked.
        if (IsTigerBlocked())
        {
            winText.text = "GOATS WIN! TIGER IS BLOCKED!";
            Invoke("ReturnToOptionBoard", 3f);
        }
    }

    private bool IsTigerBlocked()
    {
        string currentTigerTile = tigerMovement.GetTileName(tiger.transform.position, tiles);
        if (!tigerMovement.validMoves.ContainsKey(currentTigerTile))
        {
            return false; //tiger is not on a valid tile, so can't be blocked.
        }

        foreach (string possibleMove in tigerMovement.validMoves[currentTigerTile])
        {
            GameObject targetTile = tiles[possibleMove];
            if (!tigerMovement.IsTileOccupied(targetTile))
            {
                return false; // Tiger has at least one valid move.
            }
        }
        return true; // Tiger is completely blocked.
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
