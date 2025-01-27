using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    public GameObject tigerPrefab; // Prefab for tiger pieces
    public GameObject goatPrefab;  // Prefab for goat pieces

    private Dictionary<string, GameObject> boardPositions = new Dictionary<string, GameObject>();
    private int totalGoats = 20;      // Total number of goats allowed
    private int placedGoats = 0;     // Number of goats placed
    private bool isTigerTurn = false; // Flag to track the current turn
    private GameObject selectedTiger = null; // Keep track of the selected tiger

    private List<GameObject> tigers = new List<GameObject>(); // List of all tigers (4 tigers total)

    private void Awake()
    {
        // Ensure there is only one instance of GameManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        InitializeBoard();
        PlaceInitialTigers();
        isTigerTurn = false; // First turn is for the goat
    }

    // Initialize the board by linking buttons to their positions
    void InitializeBoard()
    {
        foreach (Button button in FindObjectsOfType<Button>())
        {
            string positionKey = button.name; // Use button name as a unique key
            boardPositions[positionKey] = null; // Set all positions to empty

            // Pass both the button and positionKey to the listener
            button.onClick.AddListener(() => OnPositionClicked(button, positionKey));
        }
    }

    // Place the four tigers at the corners of the board
    void PlaceInitialTigers()
    {
        // Specify the corner positions based on the button naming convention
        string[] tigerPositions = new string[] { "Tile_0_0", "Tile_0_4", "Tile_4_0", "Tile_4_4" }; // Top-left, top-right, bottom-left, bottom-right

        foreach (string position in tigerPositions)
        {
            // Find the button by its name in the scene
            Button button = GameObject.Find(position).GetComponent<Button>();

            if (button != null)
            {
                // Instantiate tiger prefab at the button's position
                GameObject tiger = Instantiate(tigerPrefab, button.transform.position, Quaternion.identity);
                tiger.transform.position = new Vector3(tiger.transform.position.x, tiger.transform.position.y, -1f); // Ensure it's on top

                // Set sorting layer and order to ensure the tiger is rendered above the board
                SpriteRenderer tigerRenderer = tiger.GetComponent<SpriteRenderer>();
                if (tigerRenderer != null)
                {
                    tigerRenderer.sortingLayerName = "Animals";
                    tigerRenderer.sortingOrder = 1; // Ensure it's rendered above the board
                }

                // Store the tiger in the tigers list
                tigers.Add(tiger);
                boardPositions[position] = tiger;
            }
            else
            {
                Debug.LogError("Button " + position + " not found in the scene!");
            }
        }
    }

    // Handles tile click logic
    public void OnPositionClicked(Button button, string positionKey)
    {
        // Check if the position is already occupied
        if (boardPositions[positionKey] != null)
        {
            Debug.Log("Tile already occupied!");
            return;
        }

        if (isTigerTurn)
        {
            // Tiger's turn: Select and move a tiger
            if (selectedTiger == null)
            {
                // No tiger selected, so select one (first time only)
                SelectTiger();
            }
            else
            {
                // Move the selected tiger to the new position
                MoveTiger(button, positionKey);
            }
        }
        else if (placedGoats < totalGoats)
        {
            PlaceGoat(button, positionKey);
        }
        else
        {
            Debug.Log("All goats placed! Tigers' turn only now.");
        }

        // Switch turn
        isTigerTurn = !isTigerTurn;
    }

    // Select a tiger (first time only during tiger's turn)
    void SelectTiger()
    {
        // For simplicity, we select the first tiger in the list
        if (tigers.Count > 0)
        {
            selectedTiger = tigers[0]; // Select the first tiger (you can add a UI or another logic to choose the tiger)

            Debug.Log("Tiger selected: " + selectedTiger.name);
        }
        else
        {
            Debug.LogError("No tigers available to select!");
        }
    }

    // Move the selected tiger to the new position
    void MoveTiger(Button button, string positionKey)
    {
        if (selectedTiger != null)
        {
            // Move the selected tiger to the button's position
            selectedTiger.transform.position = new Vector3(button.transform.position.x, button.transform.position.y, -1f);

            // Update the board positions dictionary to reflect the new position
            boardPositions[positionKey] = selectedTiger;

            // Reset selected tiger after moving
            selectedTiger = null;

            Debug.Log("Tiger moved to: " + positionKey);
        }
        else
        {
            Debug.LogError("No tiger selected to move!");
        }
    }

    // Place a goat on the board
    void PlaceGoat(Button button, string positionKey)
    {
        // Instantiate the goat prefab at the button's position
        GameObject goat = Instantiate(goatPrefab, button.transform.position, Quaternion.identity);
        goat.transform.position = new Vector3(goat.transform.position.x, goat.transform.position.y, -1f); // Ensure it's on top

        // Set sorting layer and order to ensure the goat is rendered above the board
        SpriteRenderer goatRenderer = goat.GetComponent<SpriteRenderer>();
        if (goatRenderer != null)
        {
            goatRenderer.sortingLayerName = "Animals";
            goatRenderer.sortingOrder = 1; // Ensure it's rendered above the board
        }

        // Mark the position as occupied
        boardPositions[positionKey] = goat;
        placedGoats++; // Increment the goat counter
        Debug.Log("Goat placed at: " + positionKey);
    }
}
