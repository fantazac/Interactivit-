using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager Instance { get; protected set; }

    [SerializeField]
    Transform[] spawnpoint;

    [SerializeField]
    Behaviour behaviourHard;

    private void Start()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    public static void SpawnAIs(bool hardMode)
    {
        if (!Instance)
        {
            return;
        }

        for (int i = 0; i < Instance.spawnpoint.Length; i++)
        {
            Instance.CreateAIInstance(Instance.spawnpoint[i].position, hardMode);
        }
    }

    private void CreateAIInstance(Vector3 position, bool hardMode)
    {
        Debug.Log("Spawning AI Pawn at " + position);
        if (hardMode)
        {
            PhotonNetwork.Instantiate("BadGuy_Hard", position, Quaternion.identity, 0);
        }
        else
        {
            PhotonNetwork.Instantiate("BadGuy_Easy", position, Quaternion.identity, 0);
        }
    }
}
