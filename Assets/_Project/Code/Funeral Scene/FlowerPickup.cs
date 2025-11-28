using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlowerPickup : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float maxDistance = 3.0f;
    public float animationDelay = 0.5f;

    [Tooltip("Drag the Coffin object here.")]
    public CoffinController coffinController;

    private PlayerMovement player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void OnMouseDown()
    {
        if (player == null) return;

        // 1. DISTANCE CHECK
        float dist = Vector3.Distance(player.transform.position, transform.position);
        
        if (dist > maxDistance)
        {
            GameUIManager.Instance.ShowMessage("Too far away to pick that up.");
            return;
        }

        player.PlayTakeAnimation();

        Invoke("PickUpSequence", animationDelay);
    }

    private void PickUpSequence()
    {
        if (coffinController != null)
        {
            coffinController.AcquireFlower();
            GameUIManager.Instance.ShowMessage("Picked up the flowers.");
            gameObject.SetActive(false);
        }
    }
}