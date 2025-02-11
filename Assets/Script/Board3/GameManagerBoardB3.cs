using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; // Import TextMeshPro

public class GameManagerBoard3 : MonoBehaviour
{
    public static GameManagerBoard3 Instance { get; private set; }

    public enum Turn { Goat, Tiger }
    public Turn currentTurn = Turn.Goat; // First move starts with Goat

    private GameObject selectedTiger;
    private GameObject selectedGoat;

    private Dictionary<string, GameObject> tiles = new Dictionary<string, GameObject>();

    private GameObject tiger;
    private List<GameObject> goats = new List<GameObject>();

    public TMP_Text turnText;      // UI Text for turn notification
    public TMP_Text goatDeathText; // UI Text for goat death counter
    private int deadGoats = 0; // Counter for dead goats

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
        // Find UI elements correctly
        turnText = GameObject.Find("TurnText")?.GetComponent<TMP_Text>();
        goatDeathText = GameObject.Find("GoatDeathText")?.GetComponent<TMP_Text>();

        if (turnText == null)
        {
            Debug.LogError("❌ TurnText (TMP_Text) not found! Make sure it's in the scene.");
        }
        if (goatDeathText == null)
        {
            Debug.LogError("❌ GoatDeathText (TMP_Text) not found! Make sure it's in the scene.");
        }

        InitializeBoard();
        UpdateTurnUI(); // Update UI at game start
    }

    private void InitializeBoard()
    {
        // Define allowed tiles
        string[] tileNames = { "Tile_0_0", "Tile_1_0", "Tile_1_1", "Tile_1_2", "Tile_2_0", "Tile_2_1", "Tile_2_2",
                               "Tile_3_0", "Tile_3_1", "Tile_3_2", "Tile_4_0", "Tile_4_1", "Tile_4_2" };

        // Store tile references in a dictionary
        foreach (string tileName in tileNames)
        {
            GameObject tile = GameObject.Find(tileName);
            if (tile != null)
            {
                tiles[tileName] = tile;
            }
            else
            {
                Debug.LogError($"❌ Tile {tileName} not found! Make sure it exists in the scene.");
            }
        }

        // 🐅 Spawn Tiger at Tile_0_0
        if (tiles.ContainsKey("Tile_0_0"))
        {
            GameObject tigerPrefab = Resources.Load<GameObject>("TigerPrefab");
            if (tigerPrefab != null)
            {
                tiger = Instantiate(tigerPrefab, tiles["Tile_0_0"].transform.position, Quaternion.identity);
                Debug.Log("✅ Tiger spawned at Tile_0_0");
            }
            else
            {
                Debug.LogError("❌ TigerPrefab not found in Resources folder!");
            }
        }

        // 🐐 Spawn Goats at Tile_4_0, Tile_4_1, Tile_4_2
        string[] goatPositions = { "Tile_4_0", "Tile_4_1", "Tile_4_2" };
        foreach (string position in goatPositions)
        {
            if (tiles.ContainsKey(position))
            {
                GameObject goatPrefab = Resources.Load<GameObject>("GoatPrefab");
                if (goatPrefab != null)
                {
                    GameObject goat = Instantiate(goatPrefab, tiles[position].transform.position, Quaternion.identity);
                    goats.Add(goat);
                    Debug.Log($"✅ Goat spawned at {position}");
                }
                else
                {
                    Debug.LogError("❌ GoatPrefab not found in Resources folder!");
                }
            }
        }
    }

    private void UpdateTurnUI()
    {
        if (turnText != null)
        {
            turnText.text = currentTurn == Turn.Goat ? "Goat's Turn" : "Tiger's Turn";
        }
    }

    public void SelectTiger(GameObject tigerObj)
    {
        if (tiger == tigerObj && currentTurn == Turn.Tiger)
        {
            selectedTiger = tigerObj;
        }
    }

    public bool IsTigerSelected()
    {
        return selectedTiger != null;
    }

    public void MoveTiger(GameObject tile)
    {
        if (selectedTiger != null && tiles.ContainsValue(tile))
        {
            selectedTiger.transform.position = tile.transform.position;
            selectedTiger = null;

            // Check if a goat was killed
            CheckForGoatKill(tile);

            // Switch turn to Goat
            currentTurn = Turn.Goat;
            UpdateTurnUI();
        }
    }

    public void SelectGoat(GameObject goatObj)
    {
        if (goats.Contains(goatObj) && currentTurn == Turn.Goat)
        {
            selectedGoat = goatObj;
        }
    }

    public GameObject GetSelectedGoat()
    {
        return selectedGoat;
    }

    public List<GameObject> GetPlacedGoats()
    {
        return goats;
    }

    public void MoveGoat(GameObject tile)
    {
        if (selectedGoat != null && tiles.ContainsValue(tile))
        {
            selectedGoat.transform.position = tile.transform.position;
            selectedGoat = null;

            // Switch turn to Tiger
            currentTurn = Turn.Tiger;
            UpdateTurnUI();
        }
    }

    private void CheckForGoatKill(GameObject tile)
    {
        // Check if there is a goat at this tile and remove it
        GameObject goatToRemove = null;
        foreach (GameObject goat in goats)
        {
            if (goat.transform.position == tile.transform.position)
            {
                goatToRemove = goat;
                break;
            }
        }

        if (goatToRemove != null)
        {
            goats.Remove(goatToRemove);
            Destroy(goatToRemove);
            deadGoats++;

            // Update UI for dead goats count
            if (goatDeathText != null)
            {
                goatDeathText.text = "Dead Goats: " + deadGoats;
            }

            Debug.Log($"🐐 Goat at {tile.name} was killed!");
        }
    }
}
