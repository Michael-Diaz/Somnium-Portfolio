using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private float moveInput;

    private Rigidbody2D rb;
    
    private bool rightOriented = true;

    private bool isGrounded;
    private bool isStealthed = false;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    private int numJumps;
    public int extraJumps;


    // Start is called before the first frame update
    void Start()
    {
        numJumps = extraJumps;
        rb = GetComponent<Rigidbody2D>();    
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");
        if (isStealthed)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed * 0.5F, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }

        if (!rightOriented && moveInput > 0)
        {
            Flip();
        }
        else if (rightOriented && moveInput < 0)
        {
            Flip();
        }
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            isStealthed = !isStealthed;
        }

        if(isGrounded)
        {
            numJumps = extraJumps;
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (numJumps > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                numJumps--;
            }
            else if (numJumps == 0 && isGrounded)
            {
                rb.velocity = Vector2.up * jumpForce;
            }
        }
        
    }

    void Flip()
    {
        rightOriented = !rightOriented;
        Vector3 Scales = transform.localScale;

        Scales.x *= -1;
        transform.localScale = Scales;
    }

}
