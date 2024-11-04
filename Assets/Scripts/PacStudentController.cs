using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    public float gridMoveSpeed = 5f;
    public Tilemap pelletTilemap;
    public Tilemap powerPelletTilemap;
    public int score = 0;
    private Vector2 targetPosition;
    private Vector2 currentGridPosition;
    private bool isMoving = false;
    private string lastInput = "";
    private string currentInput = "";
    [SerializeField] private HUDManager hudManager;
    private BeeVisuals beeVisuals;

    void Start()
    {
        currentGridPosition = transform.position;
        targetPosition = currentGridPosition;
        beeVisuals = GetComponent<BeeVisuals>();
        hudManager = Object.FindFirstObjectByType<HUDManager>();

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

        currentInput = lastInput;
        StartLerping(nextPosition);
        RotateBee(direction);
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

    void RotateBee(Vector2 direction)
    {
        if (direction == Vector2.up)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (direction == Vector2.down)
            transform.rotation = Quaternion.Euler(0, 0, 180);
        else if (direction == Vector2.left)
            transform.rotation = Quaternion.Euler(0, 0, 90);
        else if (direction == Vector2.right)
            transform.rotation = Quaternion.Euler(0, 0, -90);
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
                CheckForPelletOrPowerPellet();
            }
        }
    }

    void StartLerping(Vector2 destination)
    {
        isMoving = true;
        targetPosition = destination;
        beeVisuals.StartMoving();
    }

    void CheckForPelletOrPowerPellet()
    {
        Vector3Int gridPosition = pelletTilemap.WorldToCell(transform.position);

        if (pelletTilemap.HasTile(gridPosition))
        {
            pelletTilemap.SetTile(gridPosition, null);
            hudManager.UpdateScore(10);
            beeVisuals.PlayPelletEatAudio();
        }
        else if (powerPelletTilemap.HasTile(gridPosition))
        {
            powerPelletTilemap.SetTile(gridPosition, null);
            hudManager.UpdateScore(50);
            beeVisuals.PlayPelletEatAudio();
        }
    }

}
