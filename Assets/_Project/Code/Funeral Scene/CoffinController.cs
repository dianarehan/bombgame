using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CoffinController : MonoBehaviour
{
    [Header("The Hidden Flower")]
    public GameObject placedFlowerModel;

    [Header("Settings")]
    public float maxDistance = 3.0f;
    public float animationDelay = 0.5f;

    private bool hasFlower = false;
    private bool isFlowerPlaced = false;
    private PlayerMovement player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();

        if (placedFlowerModel != null)
            placedFlowerModel.SetActive(false);
    }

    public void AcquireFlower()
    {
        hasFlower = true;
    }

    private void OnMouseDown()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.transform.position, transform.position);

        if (dist > maxDistance)
        {
            GameUIManager.Instance.ShowMessage("Too far away.");
            return; 
        }

        if (hasFlower && !isFlowerPlaced)
        {
            player.PlayPutDownAnimation();
            Invoke("PlaceFlowerSequence", animationDelay);
        }
        else if (isFlowerPlaced)
        {
             GameUIManager.Instance.ShowMessage("I already placed the flowers.");
        }
        else if (!hasFlower)
        {
            GameUIManager.Instance.ShowMessage("I need to find some flowers first.");
        }
    }

    private void PlaceFlowerSequence()
    {
        placedFlowerModel.SetActive(true);
        isFlowerPlaced = true;
        GameUIManager.Instance.ShowMessage("Flowers placed successfully.");
        Invoke("LoadNextLevel", 2.0f);
    }

    private void LoadNextLevel()
    {
         SceneTransitionManager.Instance.LoadScene("Hallway");
    }
}