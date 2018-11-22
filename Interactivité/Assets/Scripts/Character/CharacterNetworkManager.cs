using UnityEngine;

public class CharacterNetworkManager : MonoBehaviour
{
    private PhotonView photonView;
    private CharacterMovement characterMovement;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        characterMovement = GetComponent<CharacterMovement>();
    }

    public void SendToServer_Ready()
    {
        photonView.RPC("ReceiveFromServer_Ready", PhotonTargets.AllViaServer);
    }

    [PunRPC]
    private void ReceiveFromServer_Ready()
    {
        StaticObjects.GameController.OnReceiveReadyFromServer();
    }

    public void SendToServer_Start()
    {
        photonView.RPC("ReceiveFromServer_Start", PhotonTargets.AllViaServer);
    }

    [PunRPC]
    private void ReceiveFromServer_Start()
    {
        StaticObjects.GameController.OnReceiveStartFromServer();
    }

    public void SendToServer_MoveLeft(bool moveLeft)
    {
        photonView.RPC("ReceiveFromServer_MoveLeft", PhotonTargets.AllViaServer, moveLeft);
    }

    [PunRPC]
    private void ReceiveFromServer_MoveLeft(bool moveLeft)
    {
        characterMovement.OnReceiveMoveLeftFromServer(moveLeft);
    }

    public void SendToServer_MoveRight(bool moveRight)
    {
        photonView.RPC("ReceiveFromServer_MoveRight", PhotonTargets.AllViaServer, moveRight);
    }

    [PunRPC]
    private void ReceiveFromServer_MoveRight(bool moveRight)
    {
        characterMovement.OnReceiveMoveRightFromServer(moveRight);
    }

    public void SendToServer_MoveUp(bool moveUp)
    {
        photonView.RPC("ReceiveFromServer_MoveUp", PhotonTargets.AllViaServer, moveUp);
    }

    [PunRPC]
    private void ReceiveFromServer_MoveUp(bool moveUp)
    {
        characterMovement.OnReceiveMoveUpFromServer(moveUp);
    }

    public void SendToServer_MoveDown(bool moveDown)
    {
        photonView.RPC("ReceiveFromServer_MoveDown", PhotonTargets.AllViaServer, moveDown);
    }

    [PunRPC]
    private void ReceiveFromServer_MoveDown(bool moveDown)
    {
        characterMovement.OnReceiveMoveDownFromServer(moveDown);
    }

    public void SendToServer_End(bool createdMap)
    {
        photonView.RPC("ReceiveFromServer_End", PhotonTargets.AllViaServer, createdMap);
    }

    [PunRPC]
    private void ReceiveFromServer_End(bool createdMap)
    {
        StaticObjects.GameController.OnReceiveEndFromServer(createdMap);
    }
}
