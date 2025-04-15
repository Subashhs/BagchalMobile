using UnityEngine;
using System.Collections.Generic;

public class TigerMovementB4 : NewPieceMovementB4
{
    public Dictionary<string, List<string>> validCaptures = new Dictionary<string, List<string>>();

    private void Start()
    {
        InitializeValidMoves();
        InitializeValidCaptures();
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

    private void InitializeValidCaptures()
    {
        validCaptures.Add("Tile_0_0", new List<string> { "Tile_2_0", "Tile_2_2" });
        validCaptures.Add("Tile_1_0", new List<string> { "Tile_3_0", "Tile_3_2" });
        validCaptures.Add("Tile_1_1", new List<string> { "Tile_3_1" });
        validCaptures.Add("Tile_1_2", new List<string> { "Tile_3_0", "Tile_3_2" });
        validCaptures.Add("Tile_2_0", new List<string> { "Tile_0_0", "Tile_4_0", "Tile_4_2" });
        validCaptures.Add("Tile_2_1", new List<string> { "Tile_4_1" });
        validCaptures.Add("Tile_2_2", new List<string> { "Tile_0_0", "Tile_4_0", "Tile_4_2" });
        validCaptures.Add("Tile_3_0", new List<string> { "Tile_1_0", "Tile_1_2" });
        validCaptures.Add("Tile_3_1", new List<string> { "Tile_1_1" });
        validCaptures.Add("Tile_3_2", new List<string> { "Tile_1_0", "Tile_1_2" });
        validCaptures.Add("Tile_4_0", new List<string> { "Tile_2_0", "Tile_2_2" });
        validCaptures.Add("Tile_4_1", new List<string> { "Tile_2_1" });
        validCaptures.Add("Tile_4_2", new List<string> { "Tile_2_0", "Tile_2_2" });
    }



    public override bool TryMove(GameObject selectedTiger, GameObject targetTile, Dictionary<string, GameObject> tiles)
    {
        string currentTileName = GetTileName(selectedTiger.transform.position, tiles);
        if (currentTileName == null)
        {
            return false;
        }

        List<string> adjacentTiles = GetAdjacentTiles(currentTileName);
        List<string> jumpTiles = GetJumpTiles(currentTileName); // Get capture moves


        // 1. Check for regular move
        if (adjacentTiles.Contains(targetTile.name))
        {
            if (!IsTileOccupied(targetTile))
            {
                MoveTigerTo(selectedTiger, targetTile, tiles);
                return true;
            }
        }

        // 2. Check for capture move
        else if (jumpTiles.Contains(targetTile.name))
        {
            string jumpedTileName = adjacentTiles[jumpTiles.IndexOf(targetTile.name)];
            GameObject jumpedTile = tiles[jumpedTileName];

            if (IsTileOccupied(jumpedTile) && jumpedTile.transform.childCount > 0)
            {
                GameObject capturedGoat = jumpedTile.transform.GetChild(0).gameObject;
                MoveTigerTo(selectedTiger, targetTile, tiles);
                Destroy(capturedGoat);
                return true;
            }
        }
        return false;
    }

    public void MoveTigerTo(GameObject tiger, GameObject targetTile, Dictionary<string, GameObject> tiles)
    {
        tiger.transform.position = targetTile.transform.position;
    }

    private List<string> GetJumpTiles(string tileName)
    {
        if (validCaptures.ContainsKey(tileName))
        {
            return validCaptures[tileName];
        }
        return new List<string>();
    }



    public List<string> GetValidMoves(string currentTileName, Dictionary<string, GameObject> tiles)
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

    public List<string> GetValidCaptureMoves(string currentTileName, Dictionary<string, GameObject> tiles)
    {
        List<string> validCaptureMovesList = new List<string>();
        if (!validCaptures.ContainsKey(currentTileName))
        {
            return validCaptureMovesList;
        }

        List<string> adjacentTiles = GetAdjacentTiles(currentTileName);
        List<string> jumpTiles = GetJumpTiles(currentTileName);


        for (int i = 0; i < jumpTiles.Count; i++)
        {
            string targetTileName = jumpTiles[i];
            string jumpedTileName = adjacentTiles[i];
            GameObject jumpedTile = tiles[jumpedTileName];

            if (IsTileOccupied(jumpedTile) && jumpedTile.transform.childCount > 0)
            {
                validCaptureMovesList.Add(targetTileName);
            }
        }
        return validCaptureMovesList;
    }
}
