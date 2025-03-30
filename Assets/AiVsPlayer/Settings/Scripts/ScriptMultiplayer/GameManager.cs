using Photon.Pun;
using Photon.Realtime;
using UnityEditor.XR;
using UnityEngine;

public class GameManagerAI : MonoBehaviourPunCallbacks
{
    // Game options for creating a room
    private string roomName = "BagchalRoom";
    private RoomOptions roomOptions;
    private TypedLobby lobby;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();  // Connect to Photon Cloud
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server!");
        roomOptions = new RoomOptions()
        {
            MaxPlayers = 2  // Maximum of 2 players in the game
        };
        lobby = new TypedLobby("BagchalLobby", LobbyType.Default);
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, lobby);  // Try to join or create a room
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined the Room!");
        // Now you can start the game logic
        if (PhotonNetwork.IsMasterClient)
        {
            // Start the game if you're the master client (the host)
            StartGame();
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room join failed: " + message);
    }

    public void StartGame()
    {
        // Logic to start the game after both players have joined
        Debug.Log("Game is starting!");
        // Set up the game board for Bagchal here
    }
}
