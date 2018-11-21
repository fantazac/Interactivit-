using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void OnMoveUpHandler(bool move);
    public event OnMoveUpHandler OnMoveUp;

    public delegate void OnMoveDownHandler(bool move);
    public event OnMoveDownHandler OnMoveDown;

    public delegate void OnMoveLeftHandler(bool move);
    public event OnMoveLeftHandler OnMoveLeft;

    public delegate void OnMoveRightHandler(bool move);
    public event OnMoveRightHandler OnMoveRight;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnMoveUp(true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            OnMoveUp(false);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            OnMoveLeft(true);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            OnMoveLeft(false);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            OnMoveDown(true);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            OnMoveDown(false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            OnMoveRight(true);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            OnMoveRight(false);
        }
    }
}
