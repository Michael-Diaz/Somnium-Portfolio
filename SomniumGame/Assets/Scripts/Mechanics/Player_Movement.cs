using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float jumpForce = 3.0f;
    private float moveInput;

    private Rigidbody rb;
    
    private bool rightOriented = true;

    private bool isGrounded = true;

    private bool isStealthed = false;
    private bool isSprinting = false;
    private float speedMult = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, 1.1f);
        moveInput = Input.GetAxisRaw("Horizontal");

        if (isStealthed)
            speedMult = 0.5f;
        else if (isSprinting)
            speedMult = 1.75f;
        else
            speedMult = 1.0f;

        rb.velocity = new Vector2(moveInput * moveSpeed * speedMult, rb.velocity.y);

        if ((!rightOriented && moveInput > 0) || (rightOriented && moveInput < 0))
        {
            Flip();
        }
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            isStealthed = !isStealthed;
            isSprinting = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = !isSprinting;
            isStealthed = false;
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
        
    }

    void Flip()
    {
        rightOriented = !rightOriented;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1.5f, 1.0f);
    }

}
