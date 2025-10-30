using UnityEngine;

public class Key : MonoBehaviour
{
    public Lock lockController;

    private void Start()
    {
        if (lockController == null)
        {
            Debug.LogError("Key is missing a reference to the Lock!", this);
        }
    }

    private void OnMouseDown()
    {
        lockController.AcquireKey();
        
        gameObject.SetActive(false);
    }
}
