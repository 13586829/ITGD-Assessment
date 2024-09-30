using UnityEngine;

public class ChefCatMovement : MonoBehaviour
{
    public float speed = 5f; // Speed of the character movement
    private Animator animator; // Reference to the Animator
    private Vector2 direction; // Store movement direction
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer for flipping

    public AudioSource audioSource; // The AudioSource component to play sounds
    public AudioClip startAudio; // The audio clip to play at the start
    public AudioClip moveAudio;  // The audio clip to play when moving

    private bool hasStartedMoving = false; // To track if the player has started moving

    void Start()
    {
        // Get the Animator and SpriteRenderer components attached to the cat sprite
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Get the AudioSource component attached to the cat
        audioSource = GetComponent<AudioSource>();

        // Play the start audio when the game begins
        PlayStartAudio();
    }

    void Update()
    {
        // Handle player input for movement
        GetInput();
        // Move the cat based on the input
        Move();
        // Update the animation parameters
        UpdateAnimation();
        // Flip the sprite based on movement direction
        FlipSprite();

        // Check for movement input
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            // If the player hasn't started moving yet, play the movement audio
            if (!hasStartedMoving)
            {
                PlayMoveAudio();
                hasStartedMoving = true; // Mark that the player has started moving
            }
        }
    }

    void GetInput()
    {
        // Reset direction
        direction = Vector2.zero;

        // Check for input in each direction
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direction = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = Vector2.right;
        }
    }

    void Move()
    {
        // Move the cat sprite based on direction and speed
        Vector3 moveDirection = new Vector3(direction.x, direction.y, 0);
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    void UpdateAnimation()
    {
        // Update the moveX and moveY parameters in the Animator
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }

    void FlipSprite()
    {
        // Flip the sprite only when moving left or right
        if (direction.x > 0) // Moving Right
        {
            spriteRenderer.flipX = false; // Face right (no flip)
        }
        else if (direction.x < 0) // Moving Left
        {
            spriteRenderer.flipX = true; // Face left (flip horizontally)
        }
    }

    // Method to play the start audio
    private void PlayStartAudio()
    {
        audioSource.clip = startAudio; // Set the start audio clip
        audioSource.Play(); // Play the audio
    }

    // Method to play the movement audio
    private void PlayMoveAudio()
    {
        audioSource.Stop(); // Stop the current audio (start audio)
        audioSource.clip = moveAudio; // Set the movement audio clip
        audioSource.Play(); // Play the movement audio
    }
}