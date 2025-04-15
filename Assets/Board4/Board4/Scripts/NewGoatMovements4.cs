using UnityEngine;
using System.Collections.Generic;

public class GoatMovementB4 : NewPieceMovementB4
{
    private void Start()
    {
        InitializeValidMoves();
    }

    private void InitializeValidMoves()
    {
        validMoves.Add("Tile_0_0", new List<string> { "Tile_1_0", "Tile_1_1" });
        validMoves.Add("Tile_1_0", new List<string> { "Tile_0_0", "Tile_2_0", "Tile_2_1" });
        validMoves.Add("Tile_1_1", new List<string> { "Tile_0_0", "Tile_2_1", "Tile_2_2" });
        validMoves.Add("Tile_1_2", new List<string> { "Tile_2_2", "Tile_2_1" });
        validMoves.Add("Tile_2_0", new List<string> { "Tile_1_0", "Tile_3_0", "Tile_3_1" });
        validMoves.Add("Tile_2_1", new List<string> { "Tile_1_0", "Tile_1_1", "Tile_3_1" });
        validMoves.Add("Tile_2_2", new List<string> { "Tile_1_1", "Tile_1_2", "Tile_3_1", "Tile_3_2" });
        validMoves.Add("Tile_3_0", new List<string> { "Tile_2_0", "Tile_4_0", "Tile_4_1" });
        validMoves.Add("Tile_3_1", new List<string> { "Tile_2_0", "Tile_2_1", "Tile_2_2", "Tile_4_1" });
        validMoves.Add("Tile_3_2", new List<string> { "Tile_2_2", "Tile_4_1", "Tile_4_2" });
        validMoves.Add("Tile_4_0", new List<string> { "Tile_3_0", "Tile_3_1" });
        validMoves.Add("Tile_4_1", new List<string> { "Tile_3_0", "Tile_3_1", "Tile_3_2" });
        validMoves.Add("Tile_4_2", new List<string> { "Tile_3_1", "Tile_3_2" });
    }

    public override List<string> GetValidMoves(string currentTileName, Dictionary<string, GameObject> tiles)
    {
        List<string> validMovesList = new List<string>();
        if (!validMoves.ContainsKey(currentTileName))
        {
            return validMovesList;
        }

        foreach (string possibleMove in validMoves[currentTileName])
        {
            if (!IsTileOccupied(tiles[possibleMove]))
            {
                validMovesList.Add(possibleMove);
            }
        }
        return validMovesList;
    }

    public override bool TryMove(GameObject selectedGoat, GameObject targetTile, Dictionary<string, GameObject> tiles)
    {
        string currentTileName = GetTileName(selectedGoat.transform.position, tiles);
        if (currentTileName == null)
        {
            return false;
        }

        List<string> adjacentTiles = GetAdjacentTiles(currentTileName);
        if (adjacentTiles.Contains(targetTile.name))
        {
            if (!IsTileOccupied(targetTile))
            {
                MoveGoatTo(selectedGoat, targetTile, tiles);
                return true;
            }
        }
        return false;
    }

    public void MoveGoatTo(GameObject goat, GameObject targetTile, Dictionary<string, GameObject> tiles)
    {
        goat.transform.position = targetTile.transform.position;
    }
}

