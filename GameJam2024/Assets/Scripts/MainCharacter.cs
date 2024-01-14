using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class MainCharacter : MonoBehaviour
{
    private int teleportCount = 0;
    private TeleportationManager teleportationManager;
    public delegate void PlayerEnterExitEvent(Transform exitPoint);
    public static event PlayerEnterExitEvent OnPlayerEnterExit;

     // Singleton instance
    public static MainCharacter Instance { get; private set; }

    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public GameObject orbPosObj;
    public LayerMask IgnoreMe;
    public GameObject[] orbsPrefab;
    


    Animator animCtrl;
    Camera playerCamera;
    public GameObject cameraParent;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;


    [HideInInspector]
    public bool canMove = true;

    float speed;

void Awake()
    {
        // Set the singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerCamera = Camera.main;
        characterController = GetComponent<CharacterController>();
        animCtrl = GetComponent<Animator>();    
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        teleportationManager = FindObjectOfType<TeleportationManager>();
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            Vector3 dir;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreMe))
            {
                dir = (hit.point - orbPosObj.transform.position).normalized;
            }
            else {
                dir = playerCamera.transform.forward;
            }
            //Instantiate(orbsPrefab[0], new Vector3(0, 0, 0), Quaternion.identity);


        }
        movement();
        applyAnim();

        // Check if the player walked through an exit point
        // Check if the player walked through an exit point
        if (teleportCount < teleportationManager.maxTeleportCount)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f, LayerMask.GetMask("ExitPoint"));
            if (colliders.Length > 0)
            {
                // Player walked through an exit point
                TeleportPlayer();
            }
        }
    }

    void movement() {
        speed = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;

        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = ((forward * curSpeedX) + (right * curSpeedY)).normalized * (isRunning ? runningSpeed : walkingSpeed);


        //if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        //{
        //    // moveDirection.y = jumpSpeed;
        //}
        if (!characterController.isGrounded)
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            cameraParent.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
    void applyAnim() {
        if (speed <= 0.1f)
        {
            animCtrl.SetBool("idle", true);
            animCtrl.SetBool("walk", false);
            animCtrl.SetBool("run", false);
        }
        else if (speed <= walkingSpeed + 0.1)
        {
            animCtrl.SetBool("idle", false);
            animCtrl.SetBool("walk", true);
            animCtrl.SetBool("run", false);
        }
        else if(speed <= runningSpeed+0.1){
            animCtrl.SetBool("idle", false);
            animCtrl.SetBool("walk", false);
            animCtrl.SetBool("run", true);

        }

    }

    private void TeleportPlayer()
    {
        int randomIndex = Random.Range(0, teleportationManager.entryPoints.Length);
        Vector3 teleportPosition = teleportationManager.entryPoints[randomIndex].position;

        // Teleport the player to the random entry point
        transform.position = teleportPosition;

        if(teleportationManager.teleportCount < teleportationManager.maxTeleportCount)
            teleportationManager.teleportCount++;

        if (teleportationManager.teleportCount >= teleportationManager.maxTeleportCount)
        {
            // Teleport to the boss room if max teleport count is reached
            // spawn the portal door   
        }
    }

}

