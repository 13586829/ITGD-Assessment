using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public float gridMoveSpeed = 5f; 
    private Vector2 targetPosition; 
    private Vector2 currentGridPosition;
    private bool isMoving = false; 
    private string lastInput = ""; 
    private string currentInput = ""; 

    private BeeVisuals beeVisuals;

    void Start()
    {
        currentGridPosition = transform.position;
        targetPosition = currentGridPosition;
        beeVisuals = GetComponent<BeeVisuals>();
    }

    void Update()
    {
        GetInput();
        
        if (!isMoving)
        {
            HandleMovement();
        }
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) lastInput = "up";
        else if (Input.GetKeyDown(KeyCode.S)) lastInput = "down";
        else if (Input.GetKeyDown(KeyCode.A)) lastInput = "left";
        else if (Input.GetKeyDown(KeyCode.D)) lastInput = "right";
    }

    void HandleMovement()
    {
        Vector2 direction = Vector2.zero;

        switch (lastInput)
        {
            case "up": direction = Vector2.up; break;
            case "down": direction = Vector2.down; break;
            case "left": direction = Vector2.left; break;
            case "right": direction = Vector2.right; break;
        }

        Vector2 nextPosition = currentGridPosition + direction;

        if (IsWalkable(nextPosition))
        {
            currentInput = lastInput;
            StartLerping(nextPosition);
        }
        else if (IsWalkable(currentGridPosition + GetDirectionVector(currentInput)))
        {
            StartLerping(currentGridPosition + GetDirectionVector(currentInput));
        }
    }

    Vector2 GetDirectionVector(string input)
    {
        switch (input)
        {
            case "up": return Vector2.up;
            case "down": return Vector2.down;
            case "left": return Vector2.left;
            case "right": return Vector2.right;
        }
        return Vector2.zero;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, gridMoveSpeed * Time.fixedDeltaTime);

            if ((Vector2)transform.position == targetPosition)
            {
                isMoving = false;
                currentGridPosition = targetPosition;
                beeVisuals.StopMoving(); 
            }
        }
    }

    void StartLerping(Vector2 destination)
    {
        isMoving = true;
        targetPosition = destination;
        beeVisuals.StartMoving();
    }

    bool IsWalkable(Vector2 position)
    {
        return true;
    }
}
