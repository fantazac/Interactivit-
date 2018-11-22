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
        spawn1 = Vector3.left * 21 + Vector3.up * 0.5f + Vector3.forward * 2;
        spawn2 = Vector3.right * 21 + Vector3.up * 0.5f + Vector3.forward * 2;

        characterParentPrefabPath = "CharacterTemplatePrefab/CharacterTemplate";
    }

    private void Start()
    {
        StaticObjects.MainMenuManager = this;

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
                        state = MainMenuState.READY_TO_PLAY;
                        StaticObjects.CharacterNetworkManager.SendToServer_Ready();
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

    public void OnReceiveReadyFromServer()
    {
        playersReady++;
        if (createdMap && playersReady == 2)
        {
            StaticObjects.CharacterNetworkManager.SendToServer_Start();
        }
    }

    public void OnReceiveStartFromServer()
    {
        state = MainMenuState.IN_GAME;

        //do countdown

        StaticObjects.CharacterNetworkManager.gameObject.AddComponent<InputManager>();
    }

    private void OnNetworkConnectedToServer(bool createdMap)
    {
        this.createdMap = createdMap;
        if (createdMap)
        {
            int random = Random.Range(1, 4);
            PhotonNetwork.Instantiate("Map" + random, Vector3.zero, Quaternion.identity, 0);
        }
        SpawnPlayer();
        state = MainMenuState.IN_ROOM;
    }

    private void SpawnPlayer()
    {
        GameObject characterTemplate = Instantiate(characterParentPrefab);
        GameObject character;
        character = PhotonNetwork.Instantiate("Character", createdMap ? spawn1 : spawn2, Quaternion.identity, 0);
        character.transform.parent = characterTemplate.transform;
        StaticObjects.CharacterNetworkManager = character.GetComponent<CharacterNetworkManager>();

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
