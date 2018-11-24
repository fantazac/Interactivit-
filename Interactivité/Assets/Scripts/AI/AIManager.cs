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
        ais = new List<AI>();
        for (int i = 0; i < spawnPoint.Length; i++)
        {
            CreateAIInstance(spawnPoint[i].position, hardMode);
        }
    }

    private void CreateAIInstance(Vector3 position, bool hardMode)
    {
        Debug.Log("Spawning AI Pawn at " + position);
        ais.Add(PhotonNetwork.Instantiate("BadGuy_" + (hardMode ? "Hard" : "Easy"), position, Quaternion.identity, 0).GetComponent<AI>());
    }

    public void GetTargets()
    {
        CharacterNetworkManager[] players = FindObjectsOfType<CharacterNetworkManager>();
        Targets = new Transform[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            Targets[i] = players[i].transform;
        }

        for (int i = 0; i < ais.Count; i++)
        {
            ais[i].ShouldLookForTargets = true;
        }
    }
}
