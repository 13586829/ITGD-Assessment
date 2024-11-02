using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidStateController : MonoBehaviour
{
    private Animator animator;
    private int currentDirection = 0; // Default direction (up)
    private AudioSource audioSource;

    // Audio clips for different states
    public AudioClip normalAudio;
    public AudioClip scaredAudio;
    public AudioClip deadAudio;

    // Static variable to track if any squid is in the dead state
    private static bool anySquidInDeadState = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Start in the normal state with the default audio
        SetDirection(currentDirection);
        PlayNormalAudio();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for state transition inputs using number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ReturnToNormalState();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EnterScaredState();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EnterRecoveringState();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EnterDeadState();
        }

        // Check for directional inputs using arrow keys
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetDirection(0); // Up
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetDirection(1); // Down
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetDirection(2); // Left
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetDirection(3); // Right
        }
    }

    public void SetDirection(int direction)
    {
        currentDirection = direction;
        animator.SetInteger("Direction", direction);
    }

    public void EnterScaredState()
    {
        animator.SetTrigger("Scared");
        PlayScaredAudio();
    }

    public void EnterRecoveringState()
    {
        animator.SetTrigger("Recovering");
        PlayNormalAudio();
    }

    public void EnterDeadState()
    {
        animator.SetTrigger("Dead");
        if (!anySquidInDeadState)
        {
            anySquidInDeadState = true;
            PlayDeadAudio();
        }
    }

    public void ReturnToNormalState()
    {
        animator.SetTrigger("BackToNormal");
        animator.SetInteger("Direction", currentDirection); // Return to the last known direction

        // Only play normal audio if no squid is in dead state
        if (!anySquidInDeadState)
        {
            PlayNormalAudio();
        }
    }

    // Audio handling methods
    private void PlayNormalAudio()
    {
        if (audioSource != null && normalAudio != null)
        {
            audioSource.clip = normalAudio;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void PlayScaredAudio()
    {
        if (audioSource != null && scaredAudio != null)
        {
            audioSource.clip = scaredAudio;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void PlayDeadAudio()
    {
        if (audioSource != null && deadAudio != null)
        {
            Debug.Log("Playing Dead Audio");
            audioSource.clip = deadAudio;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // Static method to reset dead state when needed (e.g., when all squids respawn)
    public static void ResetDeadState()
    {
        anySquidInDeadState = false;
    }
}
