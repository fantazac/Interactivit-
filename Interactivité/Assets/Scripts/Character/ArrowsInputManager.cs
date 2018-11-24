using UnityEngine;

public class ArrowsInputManager : InputManager
{
    protected override bool MoveDownKeyIsDown()
    {
        return Input.GetKeyDown(KeyCode.DownArrow);
    }

    protected override bool MoveDownKeyIsUp()
    {
        return Input.GetKeyUp(KeyCode.DownArrow);
    }

    protected override bool MoveLeftKeyIsDown()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow);
    }

    protected override bool MoveLeftKeyIsUp()
    {
        return Input.GetKeyUp(KeyCode.LeftArrow);
    }

    protected override bool MoveRightKeyIsDown()
    {
        return Input.GetKeyDown(KeyCode.RightArrow);
    }

    protected override bool MoveRightKeyIsUp()
    {
        return Input.GetKeyUp(KeyCode.RightArrow);
    }

    protected override bool MoveUpKeyIsDown()
    {
        return Input.GetKeyDown(KeyCode.UpArrow);
    }

    protected override bool MoveUpKeyIsUp()
    {
        return Input.GetKeyUp(KeyCode.UpArrow);
    }
}
