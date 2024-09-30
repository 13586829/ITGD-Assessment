using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    private Animator animator; 
    private float stateTimer = 0f; 
    private int currentState = 0; 
    private float stateDuration = 3f; 

    void Start()
    { 
        animator = GetComponent<Animator>();
        EnterWalkingLeftState();
    }

    void Update()
    {
        stateTimer += Time.deltaTime;
        
        if (stateTimer >= stateDuration)
        {
            CycleNextState();
            stateTimer = 0f;
        }
    }

    void CycleNextState()
    {
        switch (currentState)
        {
            case 0:
                EnterScaredState();
                break;
            case 1:
                EnterRecoveringState();
                break;
            case 2:
                EnterDeadState();
                break;
            case 3:
                EnterWalkingLeftState();
                break;
        }
    }
    
    void EnterWalkingLeftState()
    {
        animator.Play("Walk_Left"); 
        currentState = 0; 
    }

    void EnterScaredState()
    {
        animator.Play("Scared_State"); 
        currentState = 1;
    }

    void EnterRecoveringState()
    {
        animator.Play("Recovering_State"); 
        currentState = 2; 
    }

    void EnterDeadState()
    {
        animator.Play("Dead_State"); 
        currentState = 3;
    }
}
