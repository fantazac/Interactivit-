using UnityEngine;

public class CharacterNetworkManager : MonoBehaviour
{
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void SendToServer_Ready()
    {
        photonView.RPC("ReceiveFromServer_Ready", PhotonTargets.AllViaServer);
    }

    [PunRPC]
    private void ReceiveFromServer_Ready()
    {
        StaticObjects.MainMenuManager.OnReceiveReadyFromServer();
    }

    public void SendToServer_Start()
    {
        photonView.RPC("ReceiveFromServer_Start", PhotonTargets.AllViaServer);
    }

    [PunRPC]
    private void ReceiveFromServer_Start()
    {
        StaticObjects.MainMenuManager.OnReceiveStartFromServer();
    }
}
