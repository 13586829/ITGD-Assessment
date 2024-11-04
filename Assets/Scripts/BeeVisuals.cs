using UnityEngine;
using UnityEngine.Tilemaps;

public class BeeVisuals : MonoBehaviour
{
    public AudioSource moveEffectSource;
    public AudioClip moveSoundEffect;
    public AudioClip pelletEatAudio;
    public AudioClip wallHitAudio;

    public Tilemap pelletTilemap;
    public TileBase emptyTile;
    public TileBase pelletTile;

    private Animator animator;
    private AudioSource pelletAudioSource;
    private ParticleSystem dustEffect;
    private bool hasStartedMoving = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        dustEffect = GetComponentInChildren<ParticleSystem>();

        if (moveEffectSource != null && moveSoundEffect != null)
        {
            moveEffectSource.clip = moveSoundEffect;
            moveEffectSource.loop = true;
            moveEffectSource.playOnAwake = false;
        }

        pelletAudioSource = gameObject.AddComponent<AudioSource>();
        pelletAudioSource.playOnAwake = false;
    }

    public void StartMoving()
    {
        if (!hasStartedMoving && moveEffectSource != null && !moveEffectSource.isPlaying)
        {
            moveEffectSource.Play();
            hasStartedMoving = true;
            
            if (dustEffect != null && !dustEffect.isPlaying)
            {
                dustEffect.Play();
            }
        }
    }

    public void StopMoving()
    {
        if (moveEffectSource != null && moveEffectSource.isPlaying)
        {
            moveEffectSource.Stop();
            hasStartedMoving = false;
            
            if (dustEffect != null && dustEffect.isPlaying)
            {
                dustEffect.Stop();
            }
        }
    }

    public void CheckForPellet()
    {
        if (pelletTilemap == null) return;

        Vector3Int gridPosition = pelletTilemap.WorldToCell(transform.position);
        TileBase currentTile = pelletTilemap.GetTile(gridPosition);

        if (currentTile == pelletTile)
        {
            pelletTilemap.SetTile(gridPosition, emptyTile);
            PlayPelletEatAudio();
        }
    }

    private void PlayPelletEatAudio()
    {
        if (pelletAudioSource != null && pelletEatAudio != null)
        {
            pelletAudioSource.PlayOneShot(pelletEatAudio);
        }
    }

    public void PlayWallHitAudio()
    {
        if (moveEffectSource != null && wallHitAudio != null)
        {
            moveEffectSource.PlayOneShot(wallHitAudio);
        }
    }
}
