using UnityEngine;
using System.Collections;
public class BombMsg : MonoBehaviour
{
    [SerializeField] private GameObject messageObject;
    private bool isShown = false;
    void OnMouseDown()
    {
        StartCoroutine(ShowMessage());
    }

    IEnumerator ShowMessage()
    {
        yield return new WaitForSeconds(2);
        if (!isShown)
        {
            messageObject.SetActive(true);
            isShown = true;
        }
        yield return new WaitForSeconds(5);
        messageObject.SetActive(false);
    }
}
