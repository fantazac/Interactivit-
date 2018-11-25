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
        if (!photonView)
        {
            Init();
        }
        ai.StateMachine.CurrentState.UpdateStateFromServer(value, Vector3.zero);
    }

    public void SendToServer_UpdateStateFromServerWithVector(float value1, Vector3 value2)
    {
        if (!photonView)
        {
            Init();
        }
        photonView.RPC("ReceiveFromServer_UpdateStateFromServerWithVector", PhotonTargets.AllBufferedViaServer, value1, value2);
    }

    [PunRPC]
    private void ReceiveFromServer_UpdateStateFromServerWithVector(float value1, Vector3 value2)
    {
        if (!photonView)
        {
            Init();
        }
        ai.StateMachine.CurrentState.UpdateStateFromServer(value1, value2);
    }
}
