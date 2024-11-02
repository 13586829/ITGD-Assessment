using UnityEngine;
using UnityEngine.Tilemaps;

public class BeeMovement : MonoBehaviour
{
    public float speed = 5f;
    public AudioSource moveEffectSource; // Source for movement sounds
    public AudioClip moveSoundEffect; // Sound for moving
    public AudioClip pelletEatAudio; // Sound for eating pellets
    public AudioClip wallHitAudio; // Sound for hitting a wall

    public Tilemap pelletTilemap; // The tilemap containing pellets
    public TileBase emptyTile; // Tile to replace pellet after it's eaten
    public TileBase pelletTile; // Tile representing pellets

    private Animator animator;
    private Vector2 direction = Vector2.right;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Check if pelletTilemap is assigned and log a message
        if (pelletTilemap == null)
        {
            Debug.LogError("Pellet Tilemap is not assigned in the Inspector.");
        }

        // Setup move effect source if assigned
        if (moveEffectSource != null && moveSoundEffect != null)
        {
            moveEffectSource.clip = moveSoundEffect;
            moveEffectSource.loop = true; // Loop the moving sound effect while moving
        }
    }

    void Update()
    {
        GetInput();
        Move();
        UpdateAnimation();
        CheckForPellet(); // Call CheckForPellet every frame to detect pellets
    }

    // Handles movement input
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

        // Play or stop the move effect sound depending on input
        if (direction != Vector2.zero && moveEffectSource != null && !moveEffectSource.isPlaying)
        {
            moveEffectSource.Play();
        }
        else if (direction == Vector2.zero && moveEffectSource != null && moveEffectSource.isPlaying)
        {
            moveEffectSource.Stop();
        }
    }

    // Handles the movement of the bee
    void Move()
    {
        Vector3 moveDirection = new Vector3(direction.x, direction.y, 0);
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    // Updates the animation based on movement direction
    void UpdateAnimation()
    {
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }

    // Sets the direction and rotation of the bee
    void SetDirection(int directionValue, float rotationZ)
    {
        animator.SetInteger("Direction", directionValue);
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    // Checks if the bee is on a pellet tile, replaces it, and plays the eat sound
    void CheckForPellet()
    {
        if (pelletTilemap == null)
        {
            Debug.LogError("Pellet Tilemap is not assigned.");
            return;
        }

        Vector3Int gridPosition = pelletTilemap.WorldToCell(transform.position);
        TileBase currentTile = pelletTilemap.GetTile(gridPosition);

        // Check if the current tile is a pellet tile
        if (currentTile == pelletTile)
        {
            // Replace the pellet tile with an empty tile
            pelletTilemap.SetTile(gridPosition, emptyTile);

            // Play the pellet-eating sound
            PlayPelletEatAudio();
        }
    }

    // Method to play the pellet-eating sound
    private void PlayPelletEatAudio()
    {
        if (moveEffectSource != null && pelletEatAudio != null)
        {
            moveEffectSource.PlayOneShot(pelletEatAudio);
        }
        else
        {
            Debug.LogWarning("Pellet eat audio or moveEffectSource is not assigned.");
        }
    }

    // Plays the wall hit sound, if needed
    private void PlayWallHitAudio()
    {
        if (moveEffectSource != null && wallHitAudio != null)
        {
            moveEffectSource.PlayOneShot(wallHitAudio);
        }
    }
}
