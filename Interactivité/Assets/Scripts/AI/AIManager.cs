using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
	public static AIManager Instance { get; protected set; }
	public Transform[] targets { get; protected set; }

	[SerializeField] Transform[] spawnpoint;
	[SerializeField] GameObject pawnPrefab;
	[SerializeField] Behaviour behaviourHard;
	List<AI> ais;

	private void Start()
	{
		if (!Instance)
			Instance = this;
	}

	public static void SpawnAIs(bool difficultMode)
	{
		if (!Instance)
			return;

		Instance.ais = new List<AI>();
		for (int index = 0; index < Instance.spawnpoint.Length; index++)
		{
			Instance.CreateAIInstance(Instance.spawnpoint[index].position, difficultMode);
		}
	}

	private void CreateAIInstance(Vector3 position, bool difficultMode)
	{
		Debug.Log("Spawning AI Pawn at " + position);
		
		GameObject pawn = Instantiate(pawnPrefab, position, Quaternion.identity);
		AI ai = pawn.GetComponent<AI>();
		ai.ShouldLookForTargets = false;
		ais.Add(ai);
		
		if (difficultMode)
		{
			ai.SetBehaviour(behaviourHard);
			ai.ReactionTime = .3f;
		}
	}

	public void GetTargets()
	{
		CharacterNetworkManager[] players = FindObjectsOfType<CharacterNetworkManager>();
		targets = new Transform[players.Length];

		for (int i = 0; i < players.Length; i++)
		{
			targets[i] = players[i].transform;
		}

		for (int i = 0; i < ais.Count; i++)
		{
			ais[i].ShouldLookForTargets = true;
		}
	}
}
