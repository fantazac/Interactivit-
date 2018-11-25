using UnityEngine;

public class AINetworkManager : MonoBehaviour
{
    public AI ai;
    private PhotonView photonView;

    private void Init()
    {
        if (!photonView)
        {
            ai = GetComponent<AI>();
            photonView = GetComponent<PhotonView>();
        }
    }

    public void SendToServer_UpdateStateFromServer(float value)
    {
        if (!photonView)
        {
            Init();
        }
        photonView.RPC("ReceiveFromServer_UpdateStateFromServer", PhotonTargets.AllBufferedViaServer, value);
    }

    [PunRPC]
    private void ReceiveFromServer_UpdateStateFromServer(float value)
    {
        ai.StateMachine.CurrentState.UpdateStateFromServer(value);
    }
}
