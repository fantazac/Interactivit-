using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private bool moveLeft;
    private bool moveRight;
    private bool moveUp;
    private bool moveDown;

    private float movementSpeed;

    private CharacterMovement()
    {
        movementSpeed = 5;
    }

    private void Start()
    {
        InputManager inputManager = GetComponent<InputManager>();
        if (inputManager.enabled)
        {
            inputManager.OnMoveLeft += OnMoveLeft;
            inputManager.OnMoveRight += OnMoveRight;
            inputManager.OnMoveUp += OnMoveUp;
            inputManager.OnMoveDown += OnMoveDown;
        }
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
        moveLeft = move;
    }

    private void OnMoveRight(bool move)
    {
        moveRight = move;
    }

    private void OnMoveUp(bool move)
    {
        moveUp = move;
    }

    private void OnMoveDown(bool move)
    {
        moveDown = move;
    }
}
