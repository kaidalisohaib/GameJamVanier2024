using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

 
public class MainCharacter : MonoBehaviour
{
    public static int score = 0;
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
    public float maxHealth = 100;
    public float shotCooldown = 1;
    
    public TMP_Text textDisplayed;
    float health;
    public LayerMask IgnoreMe;
    public GameObject orbsPrefab;
    public GameObject[] orbsPrefab;
    


    GameObject orbPosObj;
    Animator animCtrl;
    Camera playerCamera;
    GameObject cameraParent;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    Transform rootTrsf;
    RectTransform MCLifeShow;
    RectTransform MCManaShow;



    [HideInInspector]
    public bool canMove = true;
    public static bool dead = false;


    float speed;
    private float time =-1;


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
        dead = false;
        health = maxHealth;
        cameraParent = GameObject.Find("camParPos");
        MCLifeShow = GameObject.Find("MCLifeShow").GetComponent<RectTransform>();
        MCManaShow = GameObject.Find("MCManaShow").GetComponent<RectTransform>();
        orbPosObj = GameObject.Find("orbPos");
        playerCamera = Camera.main;
        characterController = GetComponent<CharacterController>();
        animCtrl = GetComponent<Animator>();
        rootTrsf = transform.Find("CharacterArmature/Root").transform;
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        teleportationManager = FindObjectOfType<TeleportationManager>();
        
    }

    void Update()
    {
        textDisplayed.text = score +"";

        if (!dead && Input.GetButtonDown("Fire1")&&(Time.time - time >= shotCooldown)) {
            time = Time.time;
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
            Instantiate(orbsPrefab, orbPosObj.transform.position, Quaternion.LookRotation(dir));


        }
        movement();
        applyAnim();
        MCLifeShow.localScale = new Vector3(health / maxHealth, 1, 1);
        if (time != -1 && Time.time - time <= shotCooldown)
        {
            MCManaShow.localScale = new Vector3((shotCooldown - (Time.time - time)), 1, 1);
        }
        else {
            MCManaShow.localScale = new Vector3(1, 1, 1);
        }
        

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
        if (dead)
        {
            var c = cameraParent.transform;
            c.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            c.rotation = Quaternion.Euler(Mathf.Clamp(cameraParent.transform.rotation.eulerAngles.x, -lookXLimit, lookXLimit), cameraParent.transform.rotation.eulerAngles.y, 0);
            return;
        }
        speed = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;

        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);


        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? Input.GetAxis("Horizontal") : 0;
        float angleToCenter = Vector2.SignedAngle(Vector2.right,new Vector2(curSpeedX, curSpeedY));
        float movementDirectionY = moveDirection.y;
        moveDirection = ((forward * curSpeedX) + (right * curSpeedY)).normalized * (isRunning ? runningSpeed : walkingSpeed);
      
        rootTrsf.localRotation = Quaternion.Lerp(rootTrsf.localRotation, Quaternion.Euler(90 + angleToCenter, -90, -90), Time.deltaTime * 5);



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
        if(dead) return;
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

    public void removeHealth(float damage) {
        health -= damage;
        if (health <= 0) {
            health = 0;
            // death screen
            dead = true;
            animCtrl.SetBool("dead", true);
            StartCoroutine(die());
            //Destroy(gameObject);
        }
    }

    IEnumerator die()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Die", LoadSceneMode.Single);

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

