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
        photonView.RPC("ReceiveFromServer_Ready", PhotonTargets.All);
    }

    [PunRPC]
    private void ReceiveFromServer_Ready()
    {
        StaticObjects.MainMenuManager.OnReceiveReadyFromServer();
    }
}
