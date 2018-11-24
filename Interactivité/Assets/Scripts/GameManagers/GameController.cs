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

    private string patrolPrefabPath;
    private GameObject patrolPrefab;

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

    public delegate void OnConnectingToServerHandler();
    public event OnConnectingToServerHandler OnConnectingToServer;

    private GameController()
    {
        spawn1 = new Vector3(-21, 1.5f, 2);
        spawn2 = new Vector3(21, 1.5f, 2);
        end1 = new Vector3(18.5f, 1.5f, 2);
        end2 = new Vector3(-18.5f, 1.5f, 2);

        timeBeforeGameStart = 1;
        delayGameStart = new WaitForSeconds(timeBeforeGameStart);

        characterParentPrefabPath = "CharacterTemplatePrefab/CharacterTemplate";
        endPrefabPath = "EndPrefab/End";
        patrolPrefabPath = "BadGuy Patrol";
    }

    private void Start()
    {
        StaticObjects.GameController = this;

        GetComponent<NetworkConnectionManager>().OnConnectedToServer += OnNetworkConnectedToServer;

        state = MainMenuState.MAIN;

        characterParentPrefab = Resources.Load<GameObject>(characterParentPrefabPath);
        endPrefab = Resources.Load<GameObject>(endPrefabPath);
        patrolPrefab = Resources.Load<GameObject>(patrolPrefabPath);
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
            case MainMenuState.IN_DELAY:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + "\n\n");
                GUILayout.Label("GO!!!");
                break;
            case MainMenuState.IN_GAME:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length);
                break;
            case MainMenuState.END:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + "\n\n");
                if (isWinner)
                {
                    GUILayout.Label("You won!!!");
                }
                else
                {
                    GUILayout.Label("You lost...");
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
        
        //Instantiate(patrolPrefab, Vector3.forward * 2, Quaternion.identity);
        AIManager.SpawnAIs(false);
        
        state = MainMenuState.IN_ROOM;
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
        if (createdMap && playersReady == 2)
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
        StaticObjects.CharacterNetworkManager.gameObject.AddComponent<InputManager>();
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
    IN_ROOM,
    READY_TO_PLAY,
    IN_DELAY,
    IN_GAME,
    END,
}
