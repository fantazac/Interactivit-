using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuCamera;

    private string characterParentPrefabPath;
    private GameObject characterParentPrefab;

    private string endPrefabPath;
    private GameObject endPrefab;

    private MainMenuState state;

    private float timeBeforeGameStart;
    private WaitForSeconds delayGameStart;

    private Vector3 spawn1;
    private Vector3 spawn2;
    private Vector3 end1;
    private Vector3 end2;

    private int playersReady;
    private bool createdMap;
    private bool isWinner;

    private Controls selectedControls;

    public delegate void OnConnectingToServerHandler(bool offline);
    public event OnConnectingToServerHandler OnConnectingToServer;

    private GameController()
    {
        spawn1 = new Vector3(-21, 1.5f, 2);
        spawn2 = new Vector3(21, 1.5f, 2);
        end1 = new Vector3(18.5f, 1.5f, 2);
        end2 = new Vector3(-18.5f, 1.5f, 2);

        timeBeforeGameStart = 1;
        delayGameStart = new WaitForSeconds(timeBeforeGameStart);

        selectedControls = Controls.WASD;

        characterParentPrefabPath = "CharacterTemplatePrefab/CharacterTemplate";
        endPrefabPath = "EndPrefab/End";
    }

    private void Start()
    {
        StaticObjects.GameController = this;

        GetComponent<NetworkConnectionManager>().OnConnectedToServer += OnNetworkConnectedToServer;

        state = MainMenuState.MAIN;

        characterParentPrefab = Resources.Load<GameObject>(characterParentPrefabPath);
        endPrefab = Resources.Load<GameObject>(endPrefabPath);
    }

    private void OnGUI()
    {
        switch (state)
        {
            case MainMenuState.MAIN:
                if (GUILayout.Button("Single player", GUILayout.Height(40)))
                {
                    state = MainMenuState.CONNECTING;
                    OnConnectingToServer(true);
                }
                if (GUILayout.Button("Multiplayer", GUILayout.Height(40)))
                {
                    state = MainMenuState.CONNECTING;
                    OnConnectingToServer(false);
                }
                if (GUILayout.Button("Settings", GUILayout.Height(40)))
                {
                    state = MainMenuState.SETTINGS;
                }
                break;
            case MainMenuState.SETTINGS:
                if (selectedControls == Controls.WASD)
                {
                    GUILayout.Label("Selected controls: WASD");
                }
                else if (selectedControls == Controls.ARROWS)
                {
                    GUILayout.Label("Selected controls: Arrows");
                }
                if (GUILayout.Button("Arrows", GUILayout.Height(40)))
                {
                    selectedControls = Controls.ARROWS;
                }
                if (GUILayout.Button("WASD", GUILayout.Height(40)))
                {
                    selectedControls = Controls.WASD;
                }
                GUILayout.Label("\n");
                if (GUILayout.Button("Return to Main Menu", GUILayout.Height(40)))
                {
                    state = MainMenuState.MAIN;
                }
                break;
            case MainMenuState.CONNECTING:
                GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
                break;
            case MainMenuState.AI_SPAWN:
                if (GUILayout.Button("Easy", GUILayout.Height(40)))
                {
                    StaticObjects.AIManager.SpawnAIs(false);
                    state = MainMenuState.IN_ROOM;
                }
                if (GUILayout.Button("Hard", GUILayout.Height(40)))
                {
                    StaticObjects.AIManager.SpawnAIs(true);
                    state = MainMenuState.IN_ROOM;
                }
                break;
            case MainMenuState.IN_ROOM:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + "\n\n");
                if (PhotonNetwork.playerList.Length == 2 || PhotonNetwork.offlineMode)
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
            case MainMenuState.IN_DELAY:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + "\n\n");
                GUILayout.Label("GO!!!");
                break;
            case MainMenuState.IN_GAME:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length);
                if (PhotonNetwork.offlineMode)
                {
                    if (GUILayout.Button("Pause", GUILayout.Height(40)))
                    {
                        state = MainMenuState.PAUSE;
                        Time.timeScale = 0;
                    }
                }
                break;
            case MainMenuState.PAUSE:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length);
                if (GUILayout.Button("Resume game", GUILayout.Height(40)))
                {
                    state = MainMenuState.IN_GAME;
                    Time.timeScale = 1;
                }
                if (GUILayout.Button("Return to Main Menu", GUILayout.Height(40)))
                {
                    Time.timeScale = 1;
                    mainMenuCamera.SetActive(true);
                    PhotonNetwork.Disconnect();
                    PhotonNetwork.Destroy(StaticObjects.CharacterNetworkManager.transform.parent.gameObject);
                    state = MainMenuState.MAIN;
                }
                break;
            case MainMenuState.END:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + "\n\n");
                if (isWinner)
                {
                    GUILayout.Label("You win!!!");
                }
                else
                {
                    GUILayout.Label("You lose...");
                }
                if (GUILayout.Button("Return to Main Menu", GUILayout.Height(40)))
                {
                    mainMenuCamera.SetActive(true);
                    PhotonNetwork.Disconnect();
                    PhotonNetwork.Destroy(StaticObjects.CharacterNetworkManager.transform.parent.gameObject);
                    state = MainMenuState.MAIN;
                }
                break;
        }
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

        state = createdMap ? MainMenuState.AI_SPAWN : MainMenuState.IN_ROOM;
    }

    private void SpawnPlayer()
    {
        GameObject character = PhotonNetwork.Instantiate("Character" + (createdMap ? 1 : 2), createdMap ? spawn1 : spawn2, Quaternion.identity, 0);
        StaticObjects.CharacterNetworkManager = character.GetComponent<CharacterNetworkManager>();
        character.transform.parent = Instantiate(characterParentPrefab).transform;

        EndManager endManager = Instantiate(endPrefab, createdMap ? end1 : end2, Quaternion.identity).GetComponent<EndManager>();
        endManager.SetCharacter(character);
        endManager.OnGameEnd += OnGameEnd;

        mainMenuCamera.SetActive(false);
    }

    public void OnReceiveReadyFromServer()
    {
        playersReady++;
        if (playersReady == 2 || PhotonNetwork.offlineMode)
        {
            StaticObjects.AIManager.GetTargets();
        }
        if (createdMap && playersReady == 2 || PhotonNetwork.offlineMode)
        {
            StaticObjects.CharacterNetworkManager.SendToServer_Start();
        }
    }

    public void OnReceiveStartFromServer()
    {
        StartCoroutine(GameStartDelay());
    }

    private IEnumerator GameStartDelay()
    {
        state = MainMenuState.IN_DELAY;

        yield return delayGameStart;

        state = MainMenuState.IN_GAME;
        if (selectedControls == Controls.WASD)
        {
            StaticObjects.CharacterNetworkManager.gameObject.AddComponent<WASDInputManager>();
        }
        else if (selectedControls == Controls.ARROWS)
        {
            StaticObjects.CharacterNetworkManager.gameObject.AddComponent<ArrowsInputManager>();
        }
    }

    private void OnGameEnd()
    {
        StaticObjects.CharacterNetworkManager.SendToServer_End(createdMap);
    }

    public void OnReceiveEndFromServer(bool createdMap)
    {
        Destroy(StaticObjects.CharacterNetworkManager.GetComponent<InputManager>());
        StaticObjects.CharacterNetworkManager.GetComponent<CharacterMovement>().StopAllMovement();
        isWinner = this.createdMap == createdMap;
        state = MainMenuState.END;
    }
}

enum MainMenuState
{
    MAIN,
    CONNECTING,
    AI_SPAWN,
    IN_ROOM,
    READY_TO_PLAY,
    IN_DELAY,
    IN_GAME,
    PAUSE,
    END,
    SETTINGS,
}

enum Controls
{
    WASD,
    ARROWS,
}
