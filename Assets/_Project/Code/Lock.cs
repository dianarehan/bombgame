using TMPro;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField] private GameObject keyGameObject;
    [SerializeField] private Box boxController;
    [SerializeField] private TextMeshProUGUI promptText;
    private Animator animator;
    private bool hasKey = false;
    private bool isLocked = true;

    private void Start()
    {
        animator = gameObject.GetComponentInParent<Animator>();

        if (keyGameObject == null || boxController == null)
        {
            Debug.LogError("Lock is missing references!", this);
        }
    }

    public void AcquireKey()
    {
        hasKey = true;
    }

    private void OnMouseDown()
    {
        // Only do this if we have the key AND the lock is still locked
        if (hasKey && isLocked)
        {
            // 1. Re-activate the key so it can be part of the animation
            keyGameObject.SetActive(true);
            
            // 2. Play the "Open" animation (e.g., key turning in lock)
            animator.SetTrigger("Open");
            promptText.text = "";

            // The rest of the logic happens in OnLockAnimationFinished()
            // which is called by an Animation Event (see setup below)
        }
        else if (!hasKey)
        {
            Debug.Log("You need the key!");
            promptText.text = "You need the key!";
            // Optional: Play a "locked" sound
        }
    }

    public void OnLockAnimationFinished()
    {
        // 1. Hide the key again now that the animation is done
        
        // 2. Mark the lock as "open"
        isLocked = false;
        
        // 3. Tell the box that it is now unlocked
        boxController.UnlockBox();
        gameObject.SetActive(false);
        // 4. Optional: Hide the lock itself
        // gameObject.SetActive(false);
    }
}
