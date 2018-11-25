using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public Transform[] Targets { get; protected set; }

    private List<AI> ais;

    [SerializeField]
    private Transform[] spawnPoint;

    [SerializeField]
    private Behaviour behaviourHard;

    private void Start()
    {
        StaticObjects.AIManager = this;
    }

    public void SpawnAIs(bool hardMode)
    {

        for (int i = 0; i < spawnPoint.Length; i++)
        {
            CreateAIInstance(spawnPoint[i].position, hardMode);
        }
    }

    private void CreateAIInstance(Vector3 position, bool hardMode)
    {
        Debug.Log("Spawning AI Pawn at " + position);
        PhotonNetwork.Instantiate("BadGuy_" + (hardMode ? "Hard" : "Easy"), position, Quaternion.identity, 0);
    }

    public void StartGame()
    {
        foreach (AI ai in ais)
        {
            ai.StateMachine.CurrentState.StartState();
        }
    }

    public void SetTargetsAndAIs()
    {
        CharacterNetworkManager[] players = FindObjectsOfType<CharacterNetworkManager>();
        Targets = new Transform[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            Targets[i] = players[i].transform;
        }

        ais = new List<AI>();
        foreach (AI ai in FindObjectsOfType<AI>())
        {
            ais.Add(ai);
            ai.ShouldLookForTargets = true;
        }
    }
}
