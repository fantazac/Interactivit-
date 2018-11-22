using UnityEngine;

public class EndManager : MonoBehaviour
{
    private Collider characterCollider;

    public delegate void OnGameEndHandler();
    public event OnGameEndHandler OnGameEnd;

    public void SetCharacter(GameObject character)
    {
        characterCollider = character.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider == characterCollider && OnGameEnd != null)
        {
            OnGameEnd();
        }
    }
}
