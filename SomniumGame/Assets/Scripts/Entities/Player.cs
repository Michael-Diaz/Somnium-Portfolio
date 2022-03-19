using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator playerAnim;

    [Header("Player Initializers")]
    [SerializeField] private int health;
    [SerializeField] private float moveSpeed = 1.5f, 
                                jumpForce = 3.0f, 
                                sightRange, 
                                hearingRange, 
                                soundRadius, 
                                visibilityRadius;

    
    [Header("Player Movement")]
    private Rigidbody rb;
    private float moveInput;
    private float speedMult = 1.0f;
    private bool rightOriented = true;
    private Transform lowerBound;
    private Transform upperBound;


    [Header("Player State Changes")]
    public bool byStairs = false;
    [SerializeField] private int currentFloor;
    [SerializeField] public bool isMoving = false,
                                isStealthed = false, 
                                isSprinting = false,
                                isGrounded = true, 
                                isInDanger = false;


    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();    
        lowerBound = GameObject.FindGameObjectWithTag("lowerBound").GetComponent<Transform>();
        upperBound = GameObject.FindGameObjectWithTag("upperBound").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, 1.1f);
        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
            isMoving = true;
        else
            isMoving = false;
        playerAnim.SetBool("Moving", isMoving);

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
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isStealthed = !isStealthed;
            isSprinting = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = !isSprinting;
            isStealthed = false;
        }

        playerAnim.SetBool("Sprinting", isSprinting);
        playerAnim.SetBool("Crouching", isStealthed);

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

        // if !isInDanger play default else play dangerMusic
        //  player has to be safe for x amount of time before returning to default music
        
    }

    void Flip()
    {
        rightOriented = !rightOriented;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1.5f, 1.0f);
    }

}
