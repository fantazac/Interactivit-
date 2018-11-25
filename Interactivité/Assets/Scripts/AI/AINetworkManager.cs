using UnityEngine;

public class AINetworkManager : MonoBehaviour
{
    private PhotonView photonView;

    public delegate void OnStartFromServerHandler();
    public event OnStartFromServerHandler OnStartFromServer;

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
        //OnReadyFromServer();
    }
}
