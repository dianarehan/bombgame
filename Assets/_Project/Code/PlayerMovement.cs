using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f; 
    public float gravity = -9.81f;

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
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        float horizontal = Input.GetAxis("Vertical"); 
        float vertical = Input.GetAxis("Horizontal");   
        
        Vector3 direction = new Vector3(horizontal, 0f, -vertical).normalized;

        if (direction.magnitude >= 0.1f && canMove)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            controller.Move(direction * moveSpeed * Time.deltaTime);

            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

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