using UnityEngine;
using UnityEngine.UI;

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
    private int footstepCounter = 0;

    [SerializeField] private Transform projectileLaunchOffset;
    [SerializeField] private GameObject[] itemPrefabs;

    private Light sight;

    [Header("Player State Changes")]
    public bool byInteract = false;
    public bool lightOn = false;
    private bool usageConflict = false;
    public bool _usageConflict = false;
    [SerializeField] private int currentFloor;
    [SerializeField] public bool isMoving = false,
                                isStealthed = false, 
                                isSprinting = false,
                                isGrounded = true,
                                miniAnim = false;
    public int hiddenState = 0;
    private Vector3 returnPos;

    private int invSelect = 0;
    public GameObject[] inventory;
    public RectTransform selectionArrows;
    public Sprite emptyIcon;
    private Image field1;
    private Image field2;

    public bool keyHeld;
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

        inventory = new GameObject[2];
        field1 = GameObject.Find("Slot 1").transform.GetChild(0).GetComponent<Image>();
        field2 = GameObject.Find("Slot 2").transform.GetChild(0).GetComponent<Image>();

        GameObject states = GameObject.Find("SettingsLoader");
        winState = states.GetComponent<WinGame>();
        loseState = states.GetComponent<LoseGame>();
        keyHeld = false;
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

            if (!miniAnim)    
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
    }

    void Update() 
    {
        _usageConflict = usageConflict;
        if (hiddenState == 0)
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.LeftControl))
            {
                isStealthed = true;
                isSprinting = false;
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                isSprinting = true;
                isStealthed = false;
            }
            else 
            {
                isSprinting = false;
                isStealthed = false;
            }

            playerAnim.SetBool("Sprinting", isSprinting);
            playerAnim.SetBool("Crouching", isStealthed);

            if (Input.GetKeyDown(KeyCode.E))
            {
                UseItem(inventory, invSelect);
            }

            if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
        }
        else if ((Input.GetKeyDown(KeyCode.I) && hiddenState == 1) || (Input.GetKeyDown(KeyCode.K) && (hiddenState == 2 || hiddenState == 3)))
        {
            hiddenState = 0;
            playerAnim.SetInteger("Hiding Type", hiddenState);
            
            transform.position = returnPos;

            rb.useGravity = true;
            hurtbox.enabled = true;

            sight.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (invSelect == 0)
                invSelect = 1;
            else
                invSelect = 0;

            selectionArrows.anchoredPosition = new Vector2(-112 + (invSelect * 224), 0);

        }

        // if !isInDanger play default else play dangerMusic
        //  player has to be safe for x amount of time before returning to default music
        
    }

    public void Flip()
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

    public void TempStairsFunc(bool active)
    {
        rb.useGravity = !active;
        hurtbox.enabled = !active;
        sight.enabled = !active;

        miniAnim = active;
        playerAnim.SetBool("Moving", active);
    }

    public void UseItem(GameObject[] held, int hand)
    {
        if (held[hand] != null)
        {
            Item itemSpecs = held[hand].GetComponent<Item>();
            itemSpecs.ItemUsage(hand);

            switch (itemSpecs.itemType)
            {
                case 0: // it's a flashlight
                    // double light range for 5 seconds
                    if (!lightOn)
                    {
                        lightOn = true;
                        GameObject.Find("Vision").GetComponent<Light>().range = 10.6f;
                        // revert light after duration
                        Invoke("revertLight", 5);
                    }
                    else
                    {
                        // prevents consumption of an item that cannot be used at the moment
                        usageConflict = true;
                        
                    }

                    break;
                case 1: // it's a music box, subtract 1 from case # for prefab index

                    usageConflict = false;

                    GameObject musicBox = (GameObject) Instantiate(itemPrefabs[0], projectileLaunchOffset.position, Quaternion.identity);
                    Rigidbody musicBoxRB = musicBox.GetComponent<Rigidbody>();

                    break;

                case 2: // it's a cup, subtract 1 from case # for prefab index
                    usageConflict = false;

                    GameObject cup = (GameObject) Instantiate(itemPrefabs[1], projectileLaunchOffset.position, Quaternion.identity);
                    Rigidbody cupRB = cup.GetComponent<Rigidbody>();
                    cupRB.AddForce(new Vector3(4.0f * (rightOriented ? 1 : -1), 2.0f, 0.0f), ForceMode.Impulse);

                    break;
            }

            if (hand == 0)
                field1.sprite = emptyIcon;
            else
                field2.sprite = emptyIcon;
        }

        if (hand == 0 && held[1] != null)
            invSelect = 1;
        if (hand == 1 && held[0] != null)
            invSelect = 0;

        selectionArrows.anchoredPosition = new Vector2(-112 + (invSelect * 224), 0);
    }

    void OnCollisionEnter(Collision entity)
    {
        if (entity.gameObject.tag == "Enemy")
        {
            loseState.loseCondition = true;
        }
    }

    void revertLight()
    {
        lightOn = false;
        GameObject.Find("Vision").GetComponent<Light>().range = 5.3f;
    }

    public void callPlayFootstep(int footstepCounter)
    {
        GameObject.Find("SFX Source").GetComponent<SoundFX>().playFootstep(footstepCounter);
        footstepCounter++;
    }
}
