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

    private float gameStartTime;
    private int playerDeaths;
    private int playersReady;

    private CharacterNetworkManager cnm;
    private EndManager endManager;
    private UIManager uiManager;

    public bool GameIsActive { get; private set; }
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
        playerDeaths = 0;
        playersReady = 0;
        GameIsActive = false;

        GameObject character = PhotonNetwork.Instantiate("Character" + (IsGameHost ? 1 : 2), IsGameHost ? spawn1 : spawn2, Quaternion.identity, 0);
        character.transform.parent = Instantiate(characterParentPrefab).transform;
        cnm = character.GetComponent<CharacterNetworkManager>();

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
        cnm.SendToServer_Ready();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    public void ResetGame()
    {
        PhotonNetwork.Destroy(cnm.transform.parent.gameObject);
    }

    public int GetCurrentGameTime()
    {
        return (int)(Time.time - gameStartTime);
    }

    public int GetDistanceFromEnd()
    {
        return (int)Vector3.Distance(endManager.transform.position, cnm.transform.position);
    }

    public int GetPlayerDeaths()
    {
        return playerDeaths;
    }

    public void OnReadyFromServer()
    {
        playersReady++;
        if (playersReady == 2 || PhotonNetwork.offlineMode)
        {
            StaticObjects.AIManager.SetTargetsAndAIs();
        }
        if (IsGameHost && playersReady == 2 || PhotonNetwork.offlineMode)
        {
            cnm.SendToServer_Start();
        }
    }

    public void OnStartFromServer()
    {
        StartCoroutine(GameStartDelay());
    }

    public void OnBackToSpawnFromServer()
    {
        playerDeaths++;
    }

    private IEnumerator GameStartDelay()
    {
        uiManager.PrepareStartOfGame();

        yield return delayGameStart;

        uiManager.StartGame();
        if (SelectedControls == Controls.WASD)
        {
            cnm.gameObject.AddComponent<WASDInputManager>();
        }
        else if (SelectedControls == Controls.ARROWS)
        {
            cnm.gameObject.AddComponent<ArrowsInputManager>();
        }
        gameStartTime = Time.time;
        GameIsActive = true;
    }

    private void OnGameEnd()
    {
        cnm.SendToServer_End(IsGameHost);
    }

    public void OnEndFromServer(bool isGameHost)
    {
        Destroy(cnm.GetComponent<InputManager>());
        cnm.GetComponent<CharacterMovement>().StopAllMovement();
        if (IsGameHost == isGameHost)
        {
            uiManager.SetupWinner();
        }
        else
        {
            uiManager.SetupLoser();
        }
    }
}
