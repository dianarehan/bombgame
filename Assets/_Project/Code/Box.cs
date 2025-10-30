using UnityEngine;
using TMPro;
    
public class Box : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    private Animator animator;
    private bool isLocked = true;
    private bool isOpen = false;

    private void Start()
        => animator = GetComponentInParent<Animator>(); 
    
    public void UnlockBox()
    {
        isLocked = false;
    }

    private void OnMouseDown()
    {
        if (!isLocked && !isOpen)
        {
            isOpen = true;
            animator.SetTrigger("Open");
            promptText.text = "";
        }
        else if (isLocked)
        {
            Debug.Log("The box is locked!");
            promptText.text = "The box is locked!";
        }
    }
}