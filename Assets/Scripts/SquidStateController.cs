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

    // Static variables for controlling audio and dead state
    private static bool anySquidInDeadState = false;
    private static bool squidAudioEnabled = false; // Flag to control when squid audio can start

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Only proceed with audio and state transitions if squid audio is enabled
        if (!squidAudioEnabled)
            return;

        // State transitions controlled by number keys for testing purposes
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

        // Directional control using arrow keys
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

    // Sets the direction for the squid's animation based on an integer value
    public void SetDirection(int direction)
    {
        currentDirection = direction;
        animator.SetInteger("Direction", direction);
    }

    // Static method to enable squid audio and start the normal state audio automatically
    public static void EnableSquidAudio()
    {
        squidAudioEnabled = true;

        // Play normal state audio when squid audio is enabled for the first time
        foreach (var squid in FindObjectsOfType<SquidStateController>())
        {
            squid.PlayNormalAudio();
        }
    }

    // Transition the squid to the scared state
    public void EnterScaredState()
    {
        animator.SetTrigger("Scared");
        PlayScaredAudio();
    }

    // Transition the squid to the recovering state
    public void EnterRecoveringState()
    {
        animator.SetTrigger("Recovering");
        PlayNormalAudio();
    }

    // Transition the squid to the dead state
    public void EnterDeadState()
    {
        animator.SetTrigger("Dead");
        if (!anySquidInDeadState)
        {
            anySquidInDeadState = true;
            PlayDeadAudio();
        }
    }

    // Return the squid to the normal state with the last known direction
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
