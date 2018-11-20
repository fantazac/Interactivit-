using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorManager : MonoBehaviour {
	
	static SensorManager Instance { get; set; }

	[Tooltip("The tag the sensors will react to")]
	public const string _TagToLookFor = "Player";

	int _targetCount;
	GameObject[] _targets;
	

	#region Monobehaviour Callbacks

	void Start ()
	{
		Instance = this;
		
		// Look for every potential targets in the scene
		_targets = GameObject.FindGameObjectsWithTag(_TagToLookFor);
		_targetCount = _targets.Length;
	}

	#endregion

	public static int TargetCount()
	{
		return Instance._targetCount;
	}

	public static GameObject GetTargetPosition(int targetIndex)
	{
		return Instance._targets[targetIndex];
	}
}
