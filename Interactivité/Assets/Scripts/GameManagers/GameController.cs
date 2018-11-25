using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private string characterParentPrefabPath;
    private GameObject characterParentPrefab;

    private string endPrefabPath;
    private GameObject endPrefab;

    private float timeBeforeGameStart;
    private WaitForSeconds delayGameStart;

    private Vector3 spawn1;
    private Vector3 spawn2;
    private Vector3 end1;
    private Vector3 end2;

    private EndManager endManager;

    private int playersReady;
    private bool isWinner;

    private float gameStartTime;

    private int deaths;

    private UIManager uiManager;

    public bool HardMode { get; set; }
    public bool IsGameHost { get; private set; }
    public Controls SelectedControls { get; set; }

    private GameController()
    {
        spawn1 = new Vector3(-21, 1.5f, 2);
        spawn2 = new Vector3(21, 1.5f, 2);
        end1 = new Vector3(18.5f, 1.5f, 2);
        end2 = new Vector3(-18.5f, 1.5f, 2);

        timeBeforeGameStart = 1;
        delayGameStart = new WaitForSeconds(timeBeforeGameStart);

        SelectedControls = Controls.WASD;

        characterParentPrefabPath = "CharacterTemplatePrefab/CharacterTemplate";
        endPrefabPath = "EndPrefab/End";
    }

    private void Start()
    {
        StaticObjects.GameController = this;

        uiManager = GetComponent<UIManager>();

        characterParentPrefab = Resources.Load<GameObject>(characterParentPrefabPath);
        endPrefab = Resources.Load<GameObject>(endPrefabPath);
    }

    public void SpawnPlayer(bool isGameHost)
    {
        IsGameHost = isGameHost;
        deaths = 0;

        GameObject character = PhotonNetwork.Instantiate("Character" + (IsGameHost ? 1 : 2), IsGameHost ? spawn1 : spawn2, Quaternion.identity, 0);
        StaticObjects.CharacterNetworkManager = character.GetComponent<CharacterNetworkManager>();
        character.transform.parent = Instantiate(characterParentPrefab).transform;

        endManager = Instantiate(endPrefab, IsGameHost ? end1 : end2, Quaternion.identity).GetComponent<EndManager>();
        endManager.SetCharacter(character);
        endManager.OnGameEnd += OnGameEnd;
    }

    public void SpawnMapAndAIs(string mapPath)
    {
        StaticObjects.AIManager.SpawnAIs(HardMode);
        PhotonNetwork.Instantiate(mapPath, Vector3.zero, Quaternion.identity, 0);
    }

    public void SetReadyToPlay()
    {
        StaticObjects.CharacterNetworkManager.SendToServer_Ready();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    public int GetCurrentGameTime()
    {
        return (int)(Time.time - gameStartTime);
    }

    public int GetDistanceFromEnd()
    {
        return (int)Vector3.Distance(endManager.transform.position, StaticObjects.CharacterNetworkManager.transform.position);
    }

    public int GetPlayerDeaths()
    {
        return deaths;
    }

    public void OnReceiveReadyFromServer()
    {
        playersReady++;
        if (playersReady == 2 || PhotonNetwork.offlineMode)
        {
            StaticObjects.AIManager.GetTargets();
        }
        if (IsGameHost && playersReady == 2 || PhotonNetwork.offlineMode)
        {
            StaticObjects.CharacterNetworkManager.SendToServer_Start();
        }
    }

    public void OnReceiveStartFromServer()
    {
        StartCoroutine(GameStartDelay());
    }

    public void OnPlayerBackToSpawn()
    {
        deaths++;
    }

    private IEnumerator GameStartDelay()
    {
        uiManager.PrepareStartOfGame();

        yield return delayGameStart;

        uiManager.StartGame();
        if (SelectedControls == Controls.WASD)
        {
            StaticObjects.CharacterNetworkManager.gameObject.AddComponent<WASDInputManager>();
        }
        else if (SelectedControls == Controls.ARROWS)
        {
            StaticObjects.CharacterNetworkManager.gameObject.AddComponent<ArrowsInputManager>();
        }
        gameStartTime = Time.time;
    }

    private void OnGameEnd()
    {
        StaticObjects.CharacterNetworkManager.SendToServer_End(IsGameHost);
    }

    public void OnReceiveEndFromServer(bool isGameHost)
    {
        Destroy(StaticObjects.CharacterNetworkManager.GetComponent<InputManager>());
        StaticObjects.CharacterNetworkManager.GetComponent<CharacterMovement>().StopAllMovement();
        if(IsGameHost == isGameHost)
        {
            uiManager.SetupWinner();
        }
        else
        {
            uiManager.SetupLoser();
        }
    }
}
