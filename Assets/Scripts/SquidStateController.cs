using UnityEngine;

public class SquidStateController : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;

    // Audio clips for different states
    public AudioClip normalAudio;
    public AudioClip scaredOrRecoveringAudio;
    public AudioClip deadAudio;

    // Static variable to track if any squid is in the dead state
    private static bool anySquidInDeadState = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Automatically play the normal state sound when the game scene starts
        PlayNormalAudio();
    }

    void Update()
    {
        // Test state transitions using number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EnterNormalState();
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
    }

    // Normal state
    public void EnterNormalState()
    {
        animator.SetTrigger("Normal");

        // Only play normal audio if no squid is in dead state
        if (!anySquidInDeadState)
        {
            PlayNormalAudio();
        }
    }

    // Scared state
    public void EnterScaredState()
    {
        animator.SetTrigger("Scared");
        PlayScaredOrRecoveringAudio();
    }

    // Recovering state
    public void EnterRecoveringState()
    {
        animator.SetTrigger("Recovering");

        // Continue playing scared/recovering audio if already playing
        if (!audioSource.isPlaying || audioSource.clip != scaredOrRecoveringAudio)
        {
            PlayScaredOrRecoveringAudio();
        }
    }

    // Dead state
    public void EnterDeadState()
    {
        animator.SetTrigger("Dead");

        // Stop current audio to play dead audio exclusively
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (!anySquidInDeadState)
        {
            anySquidInDeadState = true;
            PlayDeadAudio();
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

    private void PlayScaredOrRecoveringAudio()
    {
        if (audioSource != null && scaredOrRecoveringAudio != null)
        {
            audioSource.clip = scaredOrRecoveringAudio;
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

    // Static method to enable normal state audio for all squids in the scene
    public static void EnableSquidAudio()
    {
        foreach (var squid in FindObjectsOfType<SquidStateController>())
        {
            squid.PlayNormalAudio();
        }
    }

    public static void ResetDeadState()
    {
        anySquidInDeadState = false;
    }
}
