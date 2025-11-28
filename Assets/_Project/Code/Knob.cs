using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Knob : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float maxDistance = 3.0f;
    public float rotationSpeed = 5f;
    
    [Header("Rotation Axis")]
    [Tooltip("Check ONE box to choose which way it spins.")]
    public bool rotateAroundX = false;
    public bool rotateAroundY = false;
    public bool rotateAroundZ = true;

    [Tooltip("Angle to turn per click (e.g. 90 or -90)")]
    public float angleIncrement = 90f;

    [Header("Logic")]
    public bool isLocked = false;

    private bool isRotating = false;
    private Quaternion targetRotation;
    private PlayerMovement player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        targetRotation = transform.localRotation;
    }

    private void OnMouseDown()
    {
        if (isRotating || player == null) return;

        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist > maxDistance)
        {
            if (GameUIManager.Instance != null) GameUIManager.Instance.ShowMessage("Too far away.");
            return;
        }

        if (isLocked)
        {
            if (GameUIManager.Instance != null) GameUIManager.Instance.ShowMessage("It's locked.");
            return;
        }


        float x = rotateAroundX ? angleIncrement : 0;
        float y = rotateAroundY ? angleIncrement : 0;
        float z = rotateAroundZ ? angleIncrement : 0;

        targetRotation = targetRotation * Quaternion.Euler(x, y, z);

        StartCoroutine(RotateSmoothly());
    }

    private IEnumerator RotateSmoothly()
    {
        isRotating = true;

        Quaternion startRotation = transform.localRotation;
        float time = 0;

        while (time < 1f)
        {
            time += Time.deltaTime * rotationSpeed;
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, time);
            yield return null;
        }
        transform.localRotation = targetRotation;
        isRotating = false;
        
        Debug.Log("Knob Rotated!");
        SceneTransitionManager.Instance.LoadScene("Game");
    }
}