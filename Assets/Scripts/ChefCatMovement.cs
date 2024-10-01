using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class ChefCatMovement : MonoBehaviour
{
    public float speed = 5f;
    private Animator animator;
    private Vector2 direction = Vector2.right;
    private SpriteRenderer spriteRenderer;

    public AudioSource audioSource;
    public AudioSource moveEffectSource;
    public AudioClip startAudio;
    public AudioClip moveAudio;
    public AudioClip scaredAudio;
    public AudioClip ghostDeadAudio;
    public AudioClip moveSoundEffect;
    public AudioClip pelletEatAudio;
    public AudioClip deadSoundAudio;

    private float audioTimer = 0f;
    private float cycleDuration = 15f;
    private bool hasStartedMoving = false;
    private const float tileSize = 30f;
    public Tilemap pelletTilemap;
    public TileBase emptyTile;
    public TileBase pelletTile;
    private bool isScared = false;
    private bool isGhostDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        moveEffectSource.clip = moveSoundEffect;
        moveEffectSource.loop = true;

        StartCoroutine(PlayStartAudioThenMove()); 
    }
    
    IEnumerator PlayStartAudioThenMove()
    {
        PlayStartAudio(); 
        yield return new WaitForSeconds(5f); 
        PlayMoveAudio();
        hasStartedMoving = true;
        StartCoroutine(MoveInPattern());
    }
    
    IEnumerator MoveInPattern()
    {
        yield return MoveInDirection(Vector2.right, 11);
        yield return MoveInDirection(Vector2.down, 4);
        yield return MoveInDirection(Vector2.right, 3);
        yield return MoveInDirection(Vector2.up, 4);
        yield return MoveInDirection(Vector2.right, 11);
        yield return MoveInDirection(Vector2.down, 7);
        yield return MoveInDirection(Vector2.left, 5);
        yield return MoveInDirection(Vector2.down, 13);
        yield return MoveInDirection(Vector2.right, 5);
        yield return MoveInDirection(Vector2.down, 7);
        yield return MoveInDirection(Vector2.left, 11);
        yield return MoveInDirection(Vector2.up, 4);
        yield return MoveInDirection(Vector2.left, 3);
        yield return MoveInDirection(Vector2.down, 4);
        yield return MoveInDirection(Vector2.left, 11);
        yield return MoveInDirection(Vector2.up, 7);
        yield return MoveInDirection(Vector2.right, 5);
        yield return MoveInDirection(Vector2.up, 13);
        yield return MoveInDirection(Vector2.left, 5);
        yield return MoveInDirection(Vector2.up, 7);
        
        TriggerDeadAnimation();
    }
    
    IEnumerator MoveInDirection(Vector2 moveDirection, int numTiles)
    {
        direction = moveDirection;
        FlipSprite();
        
        for (int i = 0; i < numTiles; i++)
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = startPosition + new Vector3(direction.x * tileSize, direction.y * tileSize, 0f); 
            float elapsedTime = 0f;
            float moveDuration = tileSize / speed; 

            while (elapsedTime < moveDuration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / moveDuration));
                elapsedTime += Time.deltaTime;
                CheckForPellet(); 
                yield return null;
            }
            
            transform.position = targetPosition;
            yield return null;
        }
    }
    
    void FlipSprite()
    {
        Vector3 localScale = transform.localScale;
        
        if (direction.x != 0)
        {
            localScale.x = Mathf.Abs(localScale.x) * Mathf.Sign(direction.x);
            transform.localScale = localScale;
        }
    }

    void Update()
    {
        if (hasStartedMoving)
        {
            UpdateAnimation();
            CycleThroughAudio(); 
        }
    }

    void UpdateAnimation()
    {
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }

    private void PlayStartAudio()
    {
        audioSource.clip = startAudio;
        audioSource.Play();
    }

    private void PlayMoveAudio()
    {
        audioSource.clip = moveAudio;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void PlayScaredAudio()
    {
        audioSource.clip = scaredAudio;
        audioSource.loop = false;
        audioSource.Play();
    }

    private void PlayGhostDeadAudio()
    {
        audioSource.clip = ghostDeadAudio;
        audioSource.loop = false;
        audioSource.Play();
    }

    private void PlayPelletEatAudio()
    {
        audioSource.PlayOneShot(pelletEatAudio);
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

    void TriggerDeadAnimation()
    {
        if (!audioSource.isPlaying)
        {
            PlayDeadSoundEffect();
            animator.SetTrigger("isDead");
            moveEffectSource.Stop();
            audioSource.Stop();
        }
    }
}
