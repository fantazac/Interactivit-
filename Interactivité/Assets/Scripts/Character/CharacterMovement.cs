using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private bool moveLeft;
    private bool moveRight;
    private bool moveUp;
    private bool moveDown;

    private float movementSpeed;

    private CharacterNetworkManager cnm;

    private Vector3 spawn;

    private CharacterMovement()
    {
        movementSpeed = 5;
    }

    private void Start()
    {
        spawn = transform.position;

        cnm = GetComponent<CharacterNetworkManager>();
        cnm.OnBackToSpawnFromServer += OnBackToSpawnFromServer;
    }

    public void SetupMovementInputs(InputManager inputManager)
    {
        inputManager.OnMoveLeft += OnMoveLeft;
        inputManager.OnMoveRight += OnMoveRight;
        inputManager.OnMoveUp += OnMoveUp;
        inputManager.OnMoveDown += OnMoveDown;
    }

    private void Update()
    {
        if (moveLeft && !moveRight)
        {
            transform.position += Vector3.left * movementSpeed * Time.deltaTime;
        }
        if (moveRight && !moveLeft)
        {
            transform.position += Vector3.right * movementSpeed * Time.deltaTime;
        }
        if (moveUp && !moveDown)
        {
            transform.position += Vector3.forward * movementSpeed * Time.deltaTime;
        }
        if (moveDown && !moveUp)
        {
            transform.position += Vector3.back * movementSpeed * Time.deltaTime;
        }
    }

    private void OnMoveLeft(bool move)
    {
        cnm.SendToServer_MoveLeft(move);
    }

    private void OnMoveRight(bool move)
    {
        cnm.SendToServer_MoveRight(move);
    }

    private void OnMoveUp(bool move)
    {
        cnm.SendToServer_MoveUp(move);
    }

    private void OnMoveDown(bool move)
    {
        cnm.SendToServer_MoveDown(move);
    }

    public void OnReceiveMoveLeftFromServer(bool moveLeft)
    {
        this.moveLeft = moveLeft;
    }

    public void OnReceiveMoveRightFromServer(bool moveRight)
    {
        this.moveRight = moveRight;
    }

    public void OnReceiveMoveUpFromServer(bool moveUp)
    {
        this.moveUp = moveUp;
    }

    public void OnReceiveMoveDownFromServer(bool moveDown)
    {
        this.moveDown = moveDown;
    }

    public void StopAllMovement()
    {
        OnMoveLeft(false);
        OnMoveRight(false);
        OnMoveUp(false);
        OnMoveDown(false);
    }

    private void OnBackToSpawnFromServer()
    {
        transform.position = spawn;
    }
}
