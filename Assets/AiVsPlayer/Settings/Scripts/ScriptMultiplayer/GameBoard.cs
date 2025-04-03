using Photon.Pun;
using UnityEngine;

public class GameBoardAI : MonoBehaviourPun
{
    public GameObject tiger;
    public GameObject[] goats = new GameObject[5];
    private Vector3[] goatPositions = new Vector3[5];  // You can keep an array of positions for each goat

    void Start()
    {
        if (photonView.IsMine)
        {
            // Only control the game state if this is the local player's view
            InitializeGame();
        }
    }

    public void InitializeGame()
    {
        // Set up the initial positions of the tiger and goats
        // Sync the positions with Photon
        photonView.RPC("SetInitialPositions", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void SetInitialPositions()
    {
        // Set the initial positions of the tiger and goats on the board
        tiger.transform.position = new Vector3(2, 0, 2);  // Example position
        for (int i = 0; i < 5; i++)
        {
            goats[i].transform.position = new Vector3(i, 0, 0);  // Example positions for goats
        }
    }

    public void MoveGoat(int goatIndex, Vector3 newPosition)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("SyncGoatMove", RpcTarget.All, goatIndex, newPosition);
        }
    }

    [PunRPC]
    void SyncGoatMove(int goatIndex, Vector3 newPosition)
    {
        goats[goatIndex].transform.position = newPosition;
    }
}
