using UnityEngine;
using System.Collections.Generic;

public class GameManagerBoard2 : MonoBehaviour
{
    public static GameManagerBoard2 Instance { get; private set; }

    public enum Turn { Tiger, Goat }
    public Turn currentTurn = Turn.Goat;

    private GameObject selectedTiger;
    private GameObject selectedGoat;

    private Dictionary<string, GameObject> tiles = new Dictionary<string, GameObject>();

    private GameObject tiger;
    private List<GameObject> goats = new List<GameObject>();

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
    }

    private void InitializeBoard()
    {
        // Define allowed tiles
        string[] tileNames = { "Tile_0_0", "Tile_1_0", "Tile_1_1", "Tile_1_2", "Tile_2_0",
                               "Tile_2_1", "Tile_2_2", "Tile_3_0", "Tile_3_1", "Tile_3_2" };

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
                Debug.LogError($"Tile {tileName} not found! Make sure it exists in the scene.");
            }
        }

        // Spawn Tiger at Tile_0_0
        if (tiles.ContainsKey("Tile_0_0"))
        {
            tiger = Instantiate(Resources.Load<GameObject>("TigerPrefab"), tiles["Tile_0_0"].transform.position, Quaternion.identity);
        }

        // Spawn Goats at Tile_3_0, Tile_3_1, Tile_3_3
        string[] goatPositions = { "Tile_3_0", "Tile_3_1", "Tile_3_2" };
        foreach (string position in goatPositions)
        {
            if (tiles.ContainsKey(position))
            {
                GameObject goat = Instantiate(Resources.Load<GameObject>("GoatPrefab"), tiles[position].transform.position, Quaternion.identity);
                goats.Add(goat);
            }
        }
    }

    public void SelectTiger(GameObject tigerObj)
    {
        if (tiger == tigerObj)
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
            currentTurn = Turn.Goat;
        }
    }

    public void SelectGoat(GameObject goatObj)
    {
        if (goats.Contains(goatObj))
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
            currentTurn = Turn.Tiger;
        }
    }
}
