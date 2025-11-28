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
    public bool rotateAroundZ = true; // Default to Z (Forward)

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
        // Set the starting target to whatever we are now
        targetRotation = transform.localRotation;
    }

    private void OnMouseDown()
    {
        if (isRotating || player == null) return;

        // 1. Distance Check
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

        // 2. Play Player Animation (Look at the knob)


        // 3. Calculate New Rotation based on selected Axis
        float x = rotateAroundX ? angleIncrement : 0;
        float y = rotateAroundY ? angleIncrement : 0;
        float z = rotateAroundZ ? angleIncrement : 0;

        // Multiply the CURRENT target by the NEW offset (this stacks the rotation)
        targetRotation = targetRotation * Quaternion.Euler(x, y, z);

        // 4. Start Coroutine
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
            // Slerp rotates from Start to Target using the pivot point automatically
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, time);
            yield return null;
        }
        // Snap to exact finish
        transform.localRotation = targetRotation;
        isRotating = false;
        
        Debug.Log("Knob Rotated!");
    }
}