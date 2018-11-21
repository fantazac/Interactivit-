using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuCamera;

    private string characterParentPrefabPath;
    private GameObject characterParentPrefab;

    private MainMenuState state;

    private Vector3 spawn1;
    private Vector3 spawn2;

    private int playersReady;
    private bool createdMap;

    public delegate void OnConnectingToServerHandler();
    public event OnConnectingToServerHandler OnConnectingToServer;

    private MainMenuManager()
    {
        spawn1 = Vector3.left * 20 + Vector3.up * 0.5f;//TODO
        spawn2 = Vector3.right * 20 + Vector3.up * 0.5f;//TODO

        characterParentPrefabPath = "CharacterTemplatePrefab/CharacterTemplate";
    }

    private void Start()
    {
        GetComponent<NetworkConnectionManager>().OnConnectedToServer += OnNetworkConnectedToServer;

        state = MainMenuState.MAIN;

        characterParentPrefab = Resources.Load<GameObject>(characterParentPrefabPath);
    }

    private void OnGUI()
    {
        switch (state)
        {
            case MainMenuState.MAIN:
                if (GUILayout.Button("Connect", GUILayout.Height(40)))
                {
                    state = MainMenuState.CONNECTING;
                    OnConnectingToServer();
                }
                break;
            case MainMenuState.CONNECTING:
                GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
                break;
            case MainMenuState.IN_ROOM:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + "\n\n");
                if (PhotonNetwork.playerList.Length == 2)
                {
                    if (GUILayout.Button("Ready to play!", GUILayout.Height(40)))
                    {
                        //send info to the 2 players that this one is ready
                        state = MainMenuState.READY_TO_PLAY;
                    }
                }
                else
                {
                    GUILayout.Label("Waiting for another player to connect...");
                }
                break;
            case MainMenuState.READY_TO_PLAY:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + "\n\n");
                GUILayout.Label("Waiting for the other player to be ready...");
                break;
            case MainMenuState.IN_GAME:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length);
                break;
        }
    }

    private void OnNetworkConnectedToServer(bool createdMap)
    {
        this.createdMap = createdMap;
        SpawnPlayer();
        state = MainMenuState.IN_ROOM;
    }

    private void SpawnPlayer()
    {
        GameObject characterTemplate = Instantiate(characterParentPrefab);
        GameObject character;
        character = PhotonNetwork.Instantiate("Character", createdMap ? spawn1 : spawn2, Quaternion.identity, 0);
        character.transform.parent = characterTemplate.transform;
        StaticObjects.Character = character;
        character.GetComponent<InputManager>().enabled = true;

        mainMenuCamera.SetActive(false);
    }
}

enum MainMenuState
{
    MAIN,
    CONNECTING,
    IN_ROOM,
    READY_TO_PLAY,
    IN_GAME,
}
