using UnityEngine;

public class ChefCatMovement : MonoBehaviour
{
    public float speed = 5f; 
    private Animator animator; 
    private Vector2 direction; 
    private SpriteRenderer spriteRenderer; 
    private bool isDead = false; 
    public Transform mouse;
    public float triggerDistance = 2f;

    void Start()
    {
   
        animator = GetComponent<Animator>();  
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isDead)
        {
            GetInput();
            Move();
            UpdateAnimation();
            FlipSprite();

            if (Vector3.Distance(transform.position, mouse.position) <= triggerDistance)
            {
                EnterDeadState();
            }
        }
    }

    void GetInput()
    {
        direction = Vector2.zero;

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
    Vector3 moveDirection = new Vector3(direction.x, direction.y, 0);
    transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
	}


    void UpdateAnimation()
    {
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }

    void FlipSprite()
	{
    	if (direction.x > 0) 
    	{
        	spriteRenderer.flipX = false; 
    	}	
    	else if (direction.x < 0) 
    	{
        	spriteRenderer.flipX = true; 
    	}
	}


    void EnterDeadState()
    {
        isDead = true; 
        animator.SetBool("isDead", true); 
    }
}

