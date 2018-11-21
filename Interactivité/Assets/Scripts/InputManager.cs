using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void OnPressedWHandler();
    public event OnPressedWHandler OnPressedW;

    public delegate void OnPressedAHandler();
    public event OnPressedAHandler OnPressedA;

    public delegate void OnPressedSHandler();
    public event OnPressedSHandler OnPressedS;

    public delegate void OnPressedDHandler();
    public event OnPressedDHandler OnPressedD;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            //move up
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //move left
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //move down
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //move right
        }
    }
}
