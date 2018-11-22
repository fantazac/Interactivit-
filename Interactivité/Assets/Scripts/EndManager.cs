using UnityEngine;

public class EndManager : MonoBehaviour
{
    private Collider characterCollider;

    public void SetCharacter(GameObject character)
    {
        characterCollider = character.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider == characterCollider)
        {
            StaticObjects.GameController.EndGame();
        }
    }
}
