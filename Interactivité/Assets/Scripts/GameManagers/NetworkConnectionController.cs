using UnityEngine;

public class NetworkConnectionController : MonoBehaviour
{
    private UIManager uiManager;

    private void Start()
    {
        uiManager = GetComponent<UIManager>();
    }

    public void ConnectToServer(bool offline)
    {
        if (offline)
        {
            PhotonNetwork.offlineMode = true;
        }
        else
        {
            Connect();
        }
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.Destroy(StaticObjects.CharacterNetworkManager.transform.parent.gameObject);
    }

    private void OnConnectedToMaster()
    {
        OnPhotonRandomJoinFailed();
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
            PhotonNetwork.Disconnect();
            Debug.Log("There are already 2 players in the game. Please wait until at least 1 player disconnects.");
        }
        else
        {
            uiManager.ConnectedToServer(PhotonNetwork.playerList.Length == 1);
        }
    }
}
