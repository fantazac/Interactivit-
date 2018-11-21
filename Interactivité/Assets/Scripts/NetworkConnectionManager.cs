using UnityEngine;

public class NetworkConnectionManager : MonoBehaviour
{
    public delegate void OnConnectedToServerHandler(bool createdMap);
    public event OnConnectedToServerHandler OnConnectedToServer;

    private void Start()
    {
        MainMenuManager mainMenuManager = GetComponent<MainMenuManager>();
        mainMenuManager.OnConnectingToServer += OnConnectingToServer;
    }

    private void OnConnectingToServer()
    {
        Connect();
    }

    private void Connect()
    {
        PhotonNetwork.ConnectUsingSettings("Interactivite");
    }

    private void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    private void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    private void OnJoinedRoom()
    {
        if (PhotonNetwork.playerList.Length > 2)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
            Debug.Log("There are already 2 players in the game. Please wait until at least 1 player disconnects.");
        }
        else
        {
            OnConnectedToServer(PhotonNetwork.playerList.Length == 1);
        }
    }
}
