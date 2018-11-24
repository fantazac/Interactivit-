using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
	public static AIManager Instance { get; protected set; }

	[SerializeField] Transform[] spawnpoint;
	[SerializeField] GameObject pawnPrefab;
	[SerializeField] Behaviour behaviourHard;

	private void Start()
	{
		if (!Instance)
			Instance = this;
	}

	public static void SpawnAIs(bool difficultMode)
	{
		if (!Instance)
			return;

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
		
		if (difficultMode)
		{
			ai.SetBehaviour(behaviourHard);
			ai.ReactionTime = .3f;
		}
	}
}
