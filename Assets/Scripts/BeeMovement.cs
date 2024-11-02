using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class BeeMovement : MonoBehaviour
{
    public float speed = 5f;
    private Animator animator;
    private Vector2 direction;
    private SpriteRenderer spriteRenderer;

    // Audio components
    public AudioSource audioSource;
    public AudioSource moveEffectSource;
    public AudioClip startAudio;
    public AudioClip moveAudio;
    public AudioClip scaredAudio;
    public AudioClip ghostDeadAudio;
    public AudioClip moveSoundEffect;
    public AudioClip pelletEatAudio;
    public AudioClip deadSoundAudio;

    // Audio cycle variables
    private float audioTimer = 0f;
    private float cycleDuration = 15f;
    private bool hasStartedMoving = true; // Start moving immediately
    private bool isScared = false;
    private bool isGhostDead = false;

    // Tilemap and pellet interaction
    public Tilemap pelletTilemap;
    public TileBase emptyTile;
    public TileBase pelletTile;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (moveEffectSource != null)
        {
            moveEffectSource.clip = moveSoundEffect;
            moveEffectSource.loop = true;
            moveEffectSource.Play(); // Play the movement effect sound right away
        }

        PlayStartAudio(); // Play start audio immediately
        PlayMoveAudio();  // Start looping move audio
    }

    void Update()
    {
        if (hasStartedMoving)
        {
            GetInput();
            Move();
            CheckForPellet();
            CycleThroughAudio();
        }
    }

    void GetInput()
    {
        direction = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction = Vector2.up;
            SetDirection(0, 0); // Up
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direction = Vector2.down;
            SetDirection(1, 180); // Down
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = Vector2.left;
            SetDirection(2, 90); // Left
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = Vector2.right;
            SetDirection(3, 270); // Right
        }
    }

    void SetDirection(int directionValue, float rotationZ)
    {
        animator.SetInteger("Direction", directionValue);
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    void Move()
    {
        if (direction != Vector2.zero)
        {
            Vector3 moveDirection = new Vector3(direction.x, direction.y, 0);
            transform.Translate(moveDirection * (speed * Time.deltaTime), Space.World);
        }
    }

    void CheckForPellet()
    {
        Vector3Int gridPosition = pelletTilemap.WorldToCell(transform.position);
        TileBase currentTile = pelletTilemap.GetTile(gridPosition);

        if (currentTile == pelletTile)
        {
            pelletTilemap.SetTile(gridPosition, emptyTile);
            PlayPelletEatAudio();
        }
    }

    private void PlayStartAudio()
    {
        if (startAudio != null)
        {
            audioSource.clip = startAudio;
            audioSource.Play();
        }
    }

    private void PlayMoveAudio()
    {
        if (moveAudio != null)
        {
            audioSource.clip = moveAudio;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void PlayPelletEatAudio()
    {
        if (pelletEatAudio != null)
        {
            audioSource.PlayOneShot(pelletEatAudio);
        }
    }

    private void PlayScaredAudio()
    {
        if (scaredAudio != null)
        {
            audioSource.clip = scaredAudio;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    private void PlayGhostDeadAudio()
    {
        if (ghostDeadAudio != null)
        {
            audioSource.clip = ghostDeadAudio;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    private void PlayDeadSoundEffect()
    {
        if (deadSoundAudio != null)
        {
            audioSource.PlayOneShot(deadSoundAudio);
        }
    }

    void CycleThroughAudio()
    {
        audioTimer += Time.deltaTime;

        if (!isScared && !isGhostDead && audioTimer >= cycleDuration)
        {
            PlayScaredAudio();
            isScared = true;
            isGhostDead = false;
            audioTimer = 0f;
        }
        else if (isScared && !isGhostDead && audioTimer >= cycleDuration)
        {
            PlayGhostDeadAudio();
            isScared = false;
            isGhostDead = true;
            audioTimer = 0f;
        }
        else if (isGhostDead && audioTimer >= cycleDuration)
        {
            PlayMoveAudio();
            isScared = false;
            isGhostDead = false;
            audioTimer = 0f;
        }
    }

    void TriggerDeadAnimation()
    {
        PlayDeadSoundEffect();
        animator.SetTrigger("isDead");
        if (moveEffectSource != null)
        {
            moveEffectSource.Stop();
        }
        audioSource.Stop();
    }
}
