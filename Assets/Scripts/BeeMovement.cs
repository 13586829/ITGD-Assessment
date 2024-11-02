using UnityEngine;
using UnityEngine.Tilemaps;

public class BeeMovement : MonoBehaviour
{
    public float speed = 5f;
    private Animator animator;
    private Vector2 direction;
    private AudioSource audioSource;

    // Audio clips for specific actions
    public AudioClip startLevelAudio;
    public AudioClip moveSoundEffect;
    public AudioClip pelletEatAudio;
    public AudioClip wallHitAudio;

    // Tilemap and pellet interaction
    public Tilemap pelletTilemap;
    public TileBase emptyTile;
    public TileBase pelletTile;

    private bool hasStartedMoving = false;
    private bool isMoving = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Play the start level sound at the beginning
        PlayStartLevelAudio();
    }

    void Update()
    {
        if (!hasStartedMoving)
        {
            // Check if any arrow key is pressed to stop start music and begin movement
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                StopStartLevelAudio();
                EnableMovement();
            }
        }

        if (hasStartedMoving)
        {
            GetInput();
            Move();
            CheckForPellet();
        }
    }

    private void GetInput()
    {
        direction = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction = Vector2.up;
            RotateBee(0);
            SetDirectionAnimation("Up");
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direction = Vector2.down;
            RotateBee(180);
            SetDirectionAnimation("Down");
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = Vector2.left;
            RotateBee(90);
            SetDirectionAnimation("Left");
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = Vector2.right;
            RotateBee(270);
            SetDirectionAnimation("Right");
        }
    }

    private void Move()
    {
        if (direction != Vector2.zero)
        {
            Vector3 moveDirection = new Vector3(direction.x, direction.y, 0);
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

            // Start movement sound if the bee is moving and it's not already playing
            if (!isMoving)
            {
                PlayMoveAudio();
                isMoving = true;
            }
        }
        else
        {
            // Stop movement sound when the bee stops moving
            audioSource.Stop();
            isMoving = false;
        }
    }

    private void CheckForPellet()
    {
        Vector3Int gridPosition = pelletTilemap.WorldToCell(transform.position);
        TileBase currentTile = pelletTilemap.GetTile(gridPosition);

        if (currentTile == pelletTile)
        {
            pelletTilemap.SetTile(gridPosition, emptyTile);
            PlayPelletEatAudio();
        }
    }

    private void PlayStartLevelAudio()
    {
        if (audioSource != null && startLevelAudio != null)
        {
            audioSource.clip = startLevelAudio;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    private void StopStartLevelAudio()
    {
        if (audioSource.clip == startLevelAudio)
        {
            audioSource.Stop();
        }
    }

    private void PlayMoveAudio()
    {
        if (audioSource != null && moveSoundEffect != null)
        {
            audioSource.clip = moveSoundEffect;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void PlayPelletEatAudio()
    {
        if (audioSource != null && pelletEatAudio != null)
        {
            audioSource.PlayOneShot(pelletEatAudio); // Play one-shot to layer with background audio
        }
    }

    private void PlayWallHitAudio()
    {
        if (audioSource != null && wallHitAudio != null)
        {
            audioSource.PlayOneShot(wallHitAudio); // Play one-shot for wall hit sound
        }
    }

    private void RotateBee(float angle)
    {
        // Rotate the bee to face the direction of movement
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void SetDirectionAnimation(string direction)
    {
        // Use animator to set the direction
        animator.SetTrigger(direction);
    }

    private void EnableMovement()
    {
        hasStartedMoving = true;

        // Notify squid state controller to start playing background audio
        SquidStateController.EnableSquidAudio();
    }
}
