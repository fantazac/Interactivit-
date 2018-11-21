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
        spawn1 = Vector3.left * 9 + Vector3.up * 0.5f + Vector3.back * 14;//TODO
        spawn2 = Vector3.right * 21 + Vector3.up * 0.5f + Vector3.forward * 9;//TODO

        characterParentPrefabPath = "CharacterTemplatePrefab/CharacterTemplate";
    }

    private void Start()
    {
        GetComponent<NetworkManager>().OnConnectedToServer += OnConnectedToServer;

        state = MainMenuState.MAIN;

        LoadPrefabs();
    }

    private void LoadPrefabs()
    {
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

    private void OnConnectedToServer(bool createdMap)
    {
        this.createdMap = createdMap;
        if (createdMap)
        {
            SpawnPlayer();
        }
        else
        {
            SpawnPlayer(Quaternion.Euler(0, -180, 0));
        }
        state = MainMenuState.IN_ROOM;
    }

    private void SpawnPlayer(Quaternion rotation = new Quaternion())
    {
        GameObject characterTemplate = Instantiate(characterParentPrefab);
        GameObject character;
        character = PhotonNetwork.Instantiate("Character", createdMap ? spawn1 : spawn2, rotation, 0);
        character.transform.parent = characterTemplate.transform;
        StaticObjects.Character = character;
        character.GetComponent<InputManager>().enabled = true;

        mainMenuCamera.SetActive(false);
    }

    private void OnDestroy()
    {
        PhotonNetwork.Destroy(StaticObjects.Character.transform.parent.gameObject);
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
