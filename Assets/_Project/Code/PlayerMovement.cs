using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f; 
    public float gravity = -9.81f;
[Header("Movement Settings")]
    public float smoothTime = 0.1f; // How long it takes to turn (0.1 is snappy, 0.3 is slow)

    // ADD THIS PRIVATE VARIABLE
    private float rotationVelocity;

    [Header("Interaction Settings")]
    public bool canMove = true; 

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Gravity Logic
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        // 2. Get Standard Inputs (Don't swap them!)
        float horizontal = Input.GetAxis("Horizontal"); // A & D
        float vertical = Input.GetAxis("Vertical");     // W & S
        
        // 3. Calculate Direction Relative to Camera
        Transform camTransform = Camera.main.transform;
        
        // Get camera forward and right vectors, but flatten them (ignore Y)
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Create the direction based on camera view
        Vector3 direction = (camForward * vertical + camRight * horizontal).normalized;

        if (direction.magnitude >= 0.1f && canMove)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        
        // CORRECTED LINE: Use 'rotationVelocity' (the private var), NOT 'rotationSpeed' or 'smoothTime'
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, smoothTime);
        
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        controller.Move(direction * moveSpeed * Time.deltaTime);
        animator.SetBool("IsWalking", true);
    }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        // 4. Gravity Application
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void SitDown()
    {
        canMove = false; 
        animator.SetBool("IsSitting", true);
        animator.SetBool("IsWalking", false);
    }

    public void StandUp()
    {
        canMove = true; 
        animator.SetBool("IsSitting", false);
    }

    public void PlayPushAnimation()
    {
        canMove = false; 
        animator.SetTrigger("PushButton");
        // You'll need to set canMove = true after the animation finishes
        // (You can use an Animation Event for that, like we did with the Lock)
    }

    public void PlayTakeAnimation()
    {
        canMove = false;
        animator.SetTrigger("TakeItem");
        Invoke("EnableMovement", 1.5f); // Simple timer to re-enable movement
    }

    private void EnableMovement()
    {
        canMove = true;
    }
}