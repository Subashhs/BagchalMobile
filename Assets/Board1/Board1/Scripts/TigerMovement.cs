using UnityEngine;
using System.Collections.Generic;

public class TigerMovement : MonoBehaviour
{
    private Dictionary<string, List<string>> validMoves;
    private Dictionary<string, List<(string, string)>> captureMoves;

    private void Start()
    {
        InitializeMoveDictionaries();
    }

    private void InitializeMoveDictionaries()
    {
        validMoves = new Dictionary<string, List<string>>
        {
            { "Tile_0_0", new List<string> { "Tile_0_1", "Tile_1_0", "Tile_1_1" } },
            { "Tile_0_1", new List<string> { "Tile_0_0", "Tile_1_1", "Tile_0_2" } },
            { "Tile_0_2", new List<string> { "Tile_0_1", "Tile_1_1", "Tile_1_2", "Tile_1_3", "Tile_0_3" } },
            { "Tile_0_3", new List<string> { "Tile_0_2", "Tile_1_3", "Tile_0_4" } },
            { "Tile_0_4", new List<string> { "Tile_0_3", "Tile_1_3", "Tile_1_4" } },
            { "Tile_1_0", new List<string> { "Tile_0_0", "Tile_1_1", "Tile_2_0" } },
            { "Tile_1_1", new List<string> { "Tile_0_1", "Tile_0_0", "Tile_1_0", "Tile_2_0", "Tile_2_1", "Tile_2_2", "Tile_1_2", "Tile_0_2" } },
            { "Tile_1_2", new List<string> { "Tile_0_2", "Tile_1_1", "Tile_2_2", "Tile_1_3" } },
            { "Tile_1_3", new List<string> { "Tile_0_3", "Tile_1_2", "Tile_2_2", "Tile_2_3", "Tile_2_4", "Tile_1_4", "Tile_0_4" } },
            { "Tile_1_4", new List<string> { "Tile_0_4", "Tile_1_3", "Tile_2_4" } },
            { "Tile_2_0", new List<string> { "Tile_1_0", "Tile_1_1", "Tile_2_1", "Tile_3_1", "Tile_3_0" } },
            { "Tile_2_1", new List<string> { "Tile_1_1", "Tile_2_2", "Tile_3_1", "Tile_2_0" } },
            { "Tile_2_2", new List<string> { "Tile_1_1", "Tile_1_2", "Tile_1_3", "Tile_2_3", "Tile_3_3", "Tile_3_2", "Tile_3_1", "Tile_2_1" } },
            { "Tile_2_3", new List<string> { "Tile_1_3", "Tile_2_4", "Tile_3_3", "Tile_2_2" } },
            { "Tile_2_4", new List<string> { "Tile_1_4", "Tile_3_4", "Tile_3_3", "Tile_2_3", "Tile_1_3" } },
            { "Tile_3_0", new List<string> { "Tile_2_0", "Tile_3_1", "Tile_4_0" } },
            { "Tile_3_1", new List<string> { "Tile_2_0", "Tile_2_1", "Tile_2_2", "Tile_3_2", "Tile_4_2", "Tile_4_1", "Tile_4_0", "Tile_3_0" } },
            { "Tile_3_2", new List<string> { "Tile_3_1", "Tile_2_2", "Tile_3_3", "Tile_4_2" } },
            { "Tile_3_3", new List<string> { "Tile_3_2", "Tile_2_2", "Tile_2_3", "Tile_3_4", "Tile_4_4", "Tile_4_3", "Tile_4_2" } },
            { "Tile_3_4", new List<string> { "Tile_3_3", "Tile_2_4", "Tile_4_4" } },
            { "Tile_4_0", new List<string> { "Tile_3_0", "Tile_3_1", "Tile_4_1" } },
            { "Tile_4_1", new List<string> { "Tile_4_0", "Tile_3_1", "Tile_4_2" } },
            { "Tile_4_2", new List<string> { "Tile_4_1", "Tile_3_1", "Tile_3_2", "Tile_3_3", "Tile_4_3" } },
            { "Tile_4_3", new List<string> { "Tile_4_2", "Tile_3_3", "Tile_4_4" } },
            { "Tile_4_4", new List<string> { "Tile_4_3", "Tile_3_3", "Tile_3_4" } }
        };

        captureMoves = new Dictionary<string, List<(string, string)>>
        {
            { "Tile_0_0", new List<(string, string)> { ("Tile_0_1", "Tile_0_2"), ("Tile_1_1", "Tile_2_2"), ("Tile_1_0", "Tile_2_0") } },
            { "Tile_0_1", new List<(string, string)> { ("Tile_0_2", "Tile_0_3"), ("Tile_1_1", "Tile_2_1") } },
            { "Tile_0_2", new List<(string, string)> { ("Tile_0_1", "Tile_0_0"), ("Tile_1_1", "Tile_2_0"), ("Tile_1_2", "Tile_2_2"), ("Tile_1_3", "Tile_2_4"), ("Tile_0_3", "Tile_0_4") } },
            { "Tile_0_3", new List<(string, string)> { ("Tile_0_2", "Tile_0_1"), ("Tile_1_3", "Tile_2_3") } },
            { "Tile_0_4", new List<(string, string)> { ("Tile_0_3", "Tile_0_2"), ("Tile_1_3", "Tile_2_2"), ("Tile_1_4", "Tile_2_4") } },
            { "Tile_1_0", new List<(string, string)> { ("Tile_2_0", "Tile_3_0"), ("Tile_1_1", "Tile_1_2") } },
            { "Tile_1_1", new List<(string, string)> { ("Tile_2_1", "Tile_3_1"), ("Tile_2_2", "Tile_3_3"), ("Tile_1_2", "Tile_1_3") } },
            { "Tile_1_2", new List<(string, string)> { ("Tile_1_1", "Tile_1_0"), ("Tile_2_2", "Tile_3_2"), ("Tile_1_3", "Tile_1_4") } },
            { "Tile_1_3", new List<(string, string)> { ("Tile_1_2", "Tile_1_1"), ("Tile_2_2", "Tile_3_1"), ("Tile_2_3", "Tile_3_3") } },
            { "Tile_1_4", new List<(string, string)> { ("Tile_1_3", "Tile_1_2"), ("Tile_2_4", "Tile_3_4") } },
            { "Tile_2_0", new List<(string, string)> { ("Tile_1_1", "Tile_0_2"), ("Tile_2_1", "Tile_2_2"), ("Tile_3_1", "Tile_4_2"), ("Tile_3_0", "Tile_4_0") } },
            { "Tile_2_1", new List<(string, string)> { ("Tile_2_2", "Tile_2_3"), ("Tile_3_1", "Tile_4_1") } },
            { "Tile_2_2", new List<(string, string)> { ("Tile_1_1", "Tile_0_0"), ("Tile_1_2", "Tile_0_2"), ("Tile_1_3", "Tile_0_4"), ("Tile_2_3", "Tile_2_4"), ("Tile_3_3", "Tile_4_4"), ("Tile_3_2", "Tile_4_2"), ("Tile_3_1", "Tile_4_0"), ("Tile_2_1", "Tile_2_0") } },
            { "Tile_2_3", new List<(string, string)> { ("Tile_1_3", "Tile_0_3"), ("Tile_3_3", "Tile_4_3"), ("Tile_2_2", "Tile_2_1") } },
            { "Tile_2_4", new List<(string, string)> { ("Tile_1_3", "Tile_0_2"), ("Tile_1_4", "Tile_0_4"), ("Tile_3_4", "Tile_4_4"), ("Tile_3_3", "Tile_4_2"), ("Tile_2_3", "Tile_2_2") } },
            { "Tile_3_0", new List<(string, string)> { ("Tile_2_0", "Tile_1_0"), ("Tile_3_1", "Tile_3_2") } },
            { "Tile_3_1", new List<(string, string)> { ("Tile_2_1", "Tile_1_1"), ("Tile_2_2", "Tile_1_3"), ("Tile_3_2", "Tile_3_3") } },
            { "Tile_3_2", new List<(string, string)> { ("Tile_3_1", "Tile_3_0"), ("Tile_2_2", "Tile_1_2"), ("Tile_3_3", "Tile_3_4") } },
            { "Tile_3_3", new List<(string, string)> { ("Tile_3_2", "Tile_3_1"), ("Tile_2_2", "Tile_1_1"), ("Tile_2_3", "Tile_1_3") } },
            { "Tile_3_4", new List<(string, string)> { ("Tile_3_3", "Tile_3_2"), ("Tile_2_4", "Tile_1_4") } },
            { "Tile_4_0", new List<(string, string)> { ("Tile_3_0", "Tile_2_0"), ("Tile_3_1", "Tile_2_2"), ("Tile_4_1", "Tile_4_2") } },
            { "Tile_4_1", new List<(string, string)> { ("Tile_3_1", "Tile_2_1"), ("Tile_4_2", "Tile_4_3") } },
            { "Tile_4_2", new List<(string, string)> { ("Tile_4_1", "Tile_4_0"), ("Tile_3_1", "Tile_2_0"), ("Tile_3_2", "Tile_2_2"), ("Tile_3_3", "Tile_2_4"), ("Tile_4_3", "Tile_4_4") } },
            { "Tile_4_3", new List<(string, string)> { ("Tile_4_2", "Tile_4_1"), ("Tile_3_3", "Tile_2_3") } },
            { "Tile_4_4", new List<(string, string)> { ("Tile_4_3", "Tile_4_2"), ("Tile_3_3", "Tile_2_2"), ("Tile_3_4", "Tile_2_4") } }
        };
    }

    public bool IsValidMove(string currentTile, string destinationTile)
    {
        if (validMoves.ContainsKey(currentTile) && validMoves[currentTile].Contains(destinationTile))
        {
            return true;
        }
        return false;
    }

    public bool IsValidCapture(string currentTile, string destinationTile, out string goatTile)
    {
        goatTile = null;
        if (captureMoves.ContainsKey(currentTile))
        {
            foreach (var (goat, destination) in captureMoves[currentTile])
            {
                if (destination == destinationTile && GameManager.Instance.IsGoatAtPosition(GameManager.Instance.GetTilePosition(goat)))
                {
                    goatTile = goat;
                    return true;
                }
            }
        }
        return false;
    }
}