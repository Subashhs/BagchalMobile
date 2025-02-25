using UnityEngine;

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameManagerBoard2 : MonoBehaviour
{
    public static GameManagerBoard2 Instance { get; private set; }

    public enum Turn { Tiger, Goat }
    public Turn currentTurn = Turn.Goat;

    public TMP_Text turnText;

    public GameObject tigerPrefab;  // Assign your Tiger prefab manually
    public GameObject goatPrefab;   // Assign your Goat prefab manually

    public GameObject selectedTiger;
    public GameObject selectedGoat;
    public Dictionary<string, GameObject> tiles = new Dictionary<string, GameObject>();
    public GameObject tiger;
    public List<GameObject> goats = new List<GameObject>();


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
    }

    private void Start()
    {
        InitializeBoard();
        UpdateTurnText();
    }

    private void InitializeBoard()
    {
        string[] tileNames = { "Tile_0_0", "Tile_1_0", "Tile_1_1", "Tile_1_2", "Tile_2_0",
                               "Tile_2_1", "Tile_2_2", "Tile_3_0", "Tile_3_1", "Tile_3_2" };

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
            selectedTiger.transform.position = tile.transform.position;
            Debug.Log("Tiger moved.");
            selectedTiger = null;
            currentTurn = Turn.Goat;
            UpdateTurnText();
        }
        else if (selectedGoat != null && currentTurn == Turn.Goat && tiles.ContainsValue(tile))
        {
            selectedGoat.transform.position = tile.transform.position;
            Debug.Log("Goat moved.");
            selectedGoat = null;
            currentTurn = Turn.Tiger;
            UpdateTurnText();
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
            turnText.text = (currentTurn == Turn.Tiger) ? "TIGERS MOVE" : "GOATS MOVE";
        }
    }
}
