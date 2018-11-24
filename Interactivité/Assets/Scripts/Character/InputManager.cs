using UnityEngine;

public abstract class InputManager : MonoBehaviour
{
    public delegate void OnMoveUpHandler(bool move);
    public event OnMoveUpHandler OnMoveUp;

    public delegate void OnMoveDownHandler(bool move);
    public event OnMoveDownHandler OnMoveDown;

    public delegate void OnMoveLeftHandler(bool move);
    public event OnMoveLeftHandler OnMoveLeft;

    public delegate void OnMoveRightHandler(bool move);
    public event OnMoveRightHandler OnMoveRight;

    private void Start()
    {
        GetComponent<CharacterMovement>().SetupMovementInputs(this);
    }

    private void Update()
    {
        if (MoveUpKeyIsDown())
        {
            OnMoveUp(true);
        }
        else if (MoveUpKeyIsUp())
        {
            OnMoveUp(false);
        }

        if (MoveLeftKeyIsDown())
        {
            OnMoveLeft(true);
        }
        else if (MoveLeftKeyIsUp())
        {
            OnMoveLeft(false);
        }

        if (MoveDownKeyIsDown())
        {
            OnMoveDown(true);
        }
        else if (MoveDownKeyIsUp())
        {
            OnMoveDown(false);
        }

        if (MoveRightKeyIsDown())
        {
            OnMoveRight(true);
        }
        else if (MoveRightKeyIsUp())
        {
            OnMoveRight(false);
        }
    }

    protected abstract bool MoveUpKeyIsDown();
    protected abstract bool MoveDownKeyIsDown();
    protected abstract bool MoveLeftKeyIsDown();
    protected abstract bool MoveRightKeyIsDown();
    protected abstract bool MoveUpKeyIsUp();
    protected abstract bool MoveDownKeyIsUp();
    protected abstract bool MoveLeftKeyIsUp();
    protected abstract bool MoveRightKeyIsUp();
}
