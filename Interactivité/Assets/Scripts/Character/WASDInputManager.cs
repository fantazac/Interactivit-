using UnityEngine;

public class WASDInputManager : InputManager
{
    protected override bool MoveDownKeyIsDown()
    {
        return Input.GetKeyDown(KeyCode.S);
    }

    protected override bool MoveDownKeyIsUp()
    {
        return Input.GetKeyUp(KeyCode.S);
    }

    protected override bool MoveLeftKeyIsDown()
    {
        return Input.GetKeyDown(KeyCode.A);
    }

    protected override bool MoveLeftKeyIsUp()
    {
        return Input.GetKeyUp(KeyCode.A);
    }

    protected override bool MoveRightKeyIsDown()
    {
        return Input.GetKeyDown(KeyCode.D);
    }

    protected override bool MoveRightKeyIsUp()
    {
        return Input.GetKeyUp(KeyCode.D);
    }

    protected override bool MoveUpKeyIsDown()
    {
        return Input.GetKeyDown(KeyCode.W);
    }

    protected override bool MoveUpKeyIsUp()
    {
        return Input.GetKeyUp(KeyCode.W);
    }
}
