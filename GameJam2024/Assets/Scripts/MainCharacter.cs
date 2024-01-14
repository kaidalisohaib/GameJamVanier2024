using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainCharacter : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float maxHealth = 100;
    float health;
    public LayerMask IgnoreMe;
    public GameObject orbsPrefab;


    GameObject orbPosObj;
    Animator animCtrl;
    Camera playerCamera;
    GameObject cameraParent;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    Transform rootTrsf;
    RectTransform MCLifeShow;

    [HideInInspector]
    public bool canMove = true;
    public static bool dead = false;


    float speed;
    void Start()
    {
        health = maxHealth;
        cameraParent = GameObject.Find("camParPos");
        MCLifeShow = GameObject.Find("MCLifeShow").GetComponent<RectTransform>();
        orbPosObj = GameObject.Find("orbPos");
        playerCamera = Camera.main;
        characterController = GetComponent<CharacterController>();
        animCtrl = GetComponent<Animator>();
        rootTrsf = transform.Find("CharacterArmature/Root").transform;
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        
        if (!dead && Input.GetButtonDown("Fire1")) {
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

    }

    void movement() {
        if (dead)
        {
            var c = cameraParent.transform;
            c.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            c.Rotate(-Input.GetAxis("Mouse Y") * lookSpeed, 0, 0);
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
            //Destroy(gameObject);
        }
    }
}
