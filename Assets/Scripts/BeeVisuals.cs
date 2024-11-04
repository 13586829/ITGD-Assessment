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

    void Start()
    {
        animator = GetComponent<Animator>();
        
        if (moveEffectSource != null && moveSoundEffect != null)
        {
            moveEffectSource.clip = moveSoundEffect;
            moveEffectSource.loop = true;
        }
    }

    public void StartMoving()
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", true);
        }

        if (moveEffectSource != null && !moveEffectSource.isPlaying)
        {
            moveEffectSource.Play();
        }
    }

    public void StopMoving()
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", false);
        }

        if (moveEffectSource != null && moveEffectSource.isPlaying)
        {
            moveEffectSource.Stop();
        }
    }

    public void PlayPelletEatAudio()
    {
        if (moveEffectSource != null && pelletEatAudio != null)
        {
            moveEffectSource.PlayOneShot(pelletEatAudio);
        }
    }

    public void PlayWallHitAudio()
    {
        if (moveEffectSource != null && wallHitAudio != null)
        {
            moveEffectSource.PlayOneShot(wallHitAudio);
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
}