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
    private Collider hurtbox;
    private float moveInput;
    private float speedMult = 1.0f;
    public bool rightOriented = true;
    private Transform lowerBound;
    private Transform upperBound;

    private Light sight;

    [Header("Player State Changes")]
    public bool byInteract = false;
    [SerializeField] private int currentFloor;
    [SerializeField] public bool isMoving = false,
                                isStealthed = false, 
                                isSprinting = false,
                                isGrounded = true;
    public int hiddenState = 0;
    private Vector3 returnPos;

    public WinGame winState;
    public LoseGame loseState;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();  
        hurtbox = GetComponent<Collider>();  

        sight = this.gameObject.transform.GetChild(2).GetComponent<Light>();

        lowerBound = GameObject.FindGameObjectWithTag("lowerBound").GetComponent<Transform>();
        upperBound = GameObject.FindGameObjectWithTag("upperBound").GetComponent<Transform>();

        GameObject states = GameObject.Find("SettingsLoader");
        winState = states.GetComponent<WinGame>();
        loseState = states.GetComponent<LoseGame>();

    }

    void FixedUpdate()
    {
        if (hiddenState == 0)
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
        else if (Input.GetKeyDown(KeyCode.W))
        {
            hiddenState = 0;
            playerAnim.SetInteger("Hiding Type", hiddenState);
            
            transform.position = returnPos;

            rb.useGravity = true;
            hurtbox.enabled = true;

            sight.enabled = true;
        }
    }

    void Update() 
    {
        if (hiddenState == 0)
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
        }

        // if !isInDanger play default else play dangerMusic
        //  player has to be safe for x amount of time before returning to default music
        
    }

    void Flip()
    {
        rightOriented = !rightOriented;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1.5f, 1.0f);
    }

    public void Hide(int stateChange, Vector3 newPos)
    {
        hiddenState = stateChange;
        playerAnim.SetInteger("Hiding Type", hiddenState);

        rb.velocity = new Vector2(0.0f, 0.0f);
        isMoving = false;
        isStealthed = false; 
        isSprinting = false;
        playerAnim.SetBool("Moving", isMoving);
        playerAnim.SetBool("Crouching", isStealthed);
        playerAnim.SetBool("Sprinting", isSprinting);

        rb.useGravity = false;
        hurtbox.enabled = false;

        sight.enabled = false;

        returnPos = newPos;
    }

    void OnCollisionEnter(Collision entity)
    {
        if (entity.gameObject.name == "Stalker(Clone)")
        {
            loseState.loseCondition = true;
        }
    }
}
