using UnityEngine;

public class SquidStateController : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;


    public AudioClip normalAudio;
    public AudioClip scaredOrRecoveringAudio;
    public AudioClip deadAudio;
    
    private static bool anySquidInDeadState = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        PlayNormalAudio();
    }

    void Update()
    {
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
    
    public void EnterNormalState()
    {
        animator.SetTrigger("Normal");
        
        if (!anySquidInDeadState)
        {
            PlayNormalAudio();
        }
    }
    
    public void EnterScaredState()
    {
        animator.SetTrigger("Scared");
        PlayScaredOrRecoveringAudio();
    }
    
    public void EnterRecoveringState()
    {
        animator.SetTrigger("Recovering");
        
        if (!audioSource.isPlaying || audioSource.clip != scaredOrRecoveringAudio)
        {
            PlayScaredOrRecoveringAudio();
        }
    }
    
    public void EnterDeadState()
    {
        animator.SetTrigger("Dead");
        
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
    
    public static void EnableSquidAudio()
    {
        var squids = FindObjectsByType<SquidStateController>(FindObjectsSortMode.None);
        foreach (var squid in squids)
        {
            squid.PlayNormalAudio();
        }
    }

    public static void ResetDeadState()
    {
        anySquidInDeadState = false;
    }
}
