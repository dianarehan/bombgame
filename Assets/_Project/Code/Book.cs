using UnityEngine;

public class Book : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void OnMouseDown()
    {
        if (!isOpen)
        {
            isOpen = true;
            animator.SetTrigger("Open");
        }
        else
        {
            isOpen = false;
            animator.SetTrigger("Close");
        }
    }
}