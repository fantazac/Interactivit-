using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject firstScreenCamera;
    [SerializeField]
    private GameObject mainMenuCamera;

    private MainMenuState state;

    private GameController gameController;
    private NetworkConnectionController networkConnectionController;

    private void Start()
    {
        gameController = GetComponent<GameController>();
        networkConnectionController = GetComponent<NetworkConnectionController>();

        state = MainMenuState.START_SCREEN;
    }

    private void OnGUI()
    {
        switch (state)
        {
            case MainMenuState.START_SCREEN:
                if (GUILayout.Button("Enter", GUILayout.Height(40)))
                {
                    LoadMainMenu();
                }
                break;
            case MainMenuState.MAIN_MENU:
                if (GUILayout.Button("Single player", GUILayout.Height(40)))
                {
                    LoadSinglePlayer();
                }
                if (GUILayout.Button("Multiplayer", GUILayout.Height(40)))
                {
                    LoadMultiplayer();
                }
                GUILayout.Label("------------------------------");
                if (GUILayout.Button("Settings", GUILayout.Height(40)))
                {
                    LoadSettings();
                }
                GUILayout.Label("------------------------------");
                if (GUILayout.Button("Quit", GUILayout.Height(40)))
                {
                    ExitGame();
                }
                break;
            case MainMenuState.SETTINGS:
                if (gameController.SelectedControls == Controls.WASD)
                {
                    GUILayout.Label("Selected controls: WASD");
                }
                else if (gameController.SelectedControls == Controls.ARROWS)
                {
                    GUILayout.Label("Selected controls: Arrows");
                }
                if (GUILayout.Button("Arrows", GUILayout.Height(40)))
                {
                    gameController.SelectedControls = Controls.ARROWS;
                }
                if (GUILayout.Button("WASD", GUILayout.Height(40)))
                {
                    gameController.SelectedControls = Controls.WASD;
                }
                GUILayout.Label("-----------------------------------------");
                if (!gameController.HardMode)
                {
                    GUILayout.Label("Selected difficulty: Easy");
                }
                else
                {
                    GUILayout.Label("Selected difficulty: Hard");
                }
                if (GUILayout.Button("Easy", GUILayout.Height(40)))
                {
                    gameController.HardMode = false;
                }
                if (GUILayout.Button("Hard", GUILayout.Height(40)))
                {
                    gameController.HardMode = true;
                }
                GUILayout.Label("-----------------------------------------");
                if (GUILayout.Button("Return to Main Menu", GUILayout.Height(40)))
                {
                    LoadMainMenuFromSettings();
                }
                break;
            case MainMenuState.CONNECTING:
                GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
                break;
            case MainMenuState.CONFIGURATION:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + "\n");
                if (GUILayout.Button("Map 1", GUILayout.Height(40)))
                {
                    LoadPreGame(true, "Map1");
                }
                if (GUILayout.Button("Map 2", GUILayout.Height(40)))
                {
                    LoadPreGame(true, "Map2");
                }
                if (GUILayout.Button("Map 3", GUILayout.Height(40)))
                {
                    LoadPreGame(true, "Map3");
                }
                break;
            case MainMenuState.PRE_GAME:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + "\n");
                if (PhotonNetwork.playerList.Length == 2 || PhotonNetwork.offlineMode)
                {
                    if (GUILayout.Button("Ready to play!", GUILayout.Height(40)))
                    {
                        LoadReadyForGame();
                    }
                }
                else
                {
                    GUILayout.Label("Waiting for another player to connect...");
                }
                break;
            case MainMenuState.READY_TO_PLAY:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + "\n");
                GUILayout.Label("Waiting for the other player to be ready...");
                break;
            case MainMenuState.GAME_STARTING:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + "\n");
                GUILayout.Label("GO!!!");
                break;
            case MainMenuState.IN_GAME:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + "\n");
                GUILayout.Label("Deaths: " + gameController.GetPlayerDeaths());
                GUILayout.Label("Distance until goal: " + gameController.GetDistanceFromEnd());
                GUILayout.Label("Game time: " + gameController.GetCurrentGameTime());
                GUILayout.Label("");
                if (PhotonNetwork.offlineMode)
                {
                    if (GUILayout.Button("Pause", GUILayout.Height(40)))
                    {
                        LoadPauseMenu();
                    }
                }
                break;
            case MainMenuState.PAUSE:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + "\n");
                if (GUILayout.Button("Resume game", GUILayout.Height(40)))
                {
                    ExitPauseMenu();
                }
                if (GUILayout.Button("Return to Main Menu", GUILayout.Height(40)))
                {
                    ExitToMainMenu();
                }
                break;
            case MainMenuState.WINNER:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + "\n");
                GUILayout.Label("You win!!!\n");
                if (GUILayout.Button("Return to Main Menu", GUILayout.Height(40)))
                {
                    ExitToMainMenu();
                }
                break;
            case MainMenuState.LOSER:
                GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + "\n");
                GUILayout.Label("You lose...\n");
                if (GUILayout.Button("Return to Main Menu", GUILayout.Height(40)))
                {
                    ExitToMainMenu();
                }
                break;
        }
    }

    public void ConnectedToServer(bool isGameHost)
    {
        gameController.SpawnPlayer(isGameHost);
        mainMenuCamera.SetActive(false);
        if (isGameHost)
        {
            LoadConfiguration();
        }
        else
        {
            LoadPreGame(false);
        }
    }

    public void PrepareStartOfGame()
    {
        state = MainMenuState.GAME_STARTING;
    }

    public void StartGame()
    {
        state = MainMenuState.IN_GAME;
    }

    public void SetupWinner()
    {
        state = MainMenuState.WINNER;
    }

    public void SetupLoser()
    {
        state = MainMenuState.LOSER;
    }

    private void LoadMainMenu()
    {
        firstScreenCamera.SetActive(false);
        mainMenuCamera.SetActive(true);
        state = MainMenuState.MAIN_MENU;
    }

    private void LoadSinglePlayer()
    {
        state = MainMenuState.CONNECTING;
        networkConnectionController.ConnectToServer(true);
    }

    private void LoadMultiplayer()
    {
        state = MainMenuState.CONNECTING;
        networkConnectionController.ConnectToServer(false);
    }

    private void LoadSettings()
    {
        state = MainMenuState.SETTINGS;
    }

    private void LoadConfiguration()
    {
        state = MainMenuState.CONFIGURATION;
    }

    private void LoadPreGame(bool isGameHost, string mapName = "")
    {
        if (isGameHost)
        {
            gameController.SpawnMapAndAIs(mapName);
        }
        state = MainMenuState.PRE_GAME;
    }

    private void LoadMainMenuFromSettings()
    {
        state = MainMenuState.MAIN_MENU;
    }

    private void LoadReadyForGame()
    {
        state = MainMenuState.READY_TO_PLAY;
        gameController.SetReadyToPlay();
    }

    private void LoadPauseMenu()
    {
        gameController.PauseGame();
        state = MainMenuState.PAUSE;
    }

    private void ExitPauseMenu()
    {
        state = MainMenuState.IN_GAME;
        gameController.UnpauseGame();
    }

    private void ExitToMainMenu()
    {
        gameController.UnpauseGame();
        mainMenuCamera.SetActive(true);
        gameController.ResetGame();
        networkConnectionController.Disconnect();
        state = MainMenuState.MAIN_MENU;
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
    }
}
