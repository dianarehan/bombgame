using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

// This object MUST have a Collider component to be clicked
[RequireComponent(typeof(Collider))]
public class ClickableCameraLink : MonoBehaviour
{
    [SerializeField] private CinemachineCamera targetCamera;

    private CameraPriorityManager cameraManager;

    void Start()
    {
        cameraManager = CameraPriorityManager.Instance;
        
        if (cameraManager == null)
        {
            Debug.LogError("No CameraPriorityManager found in the scene!");
        }
    }

    private void OnMouseDown()
    {
        if (targetCamera != null && cameraManager != null)
        {
            cameraManager.ActivateCamera(targetCamera);
        }
        else
        {
            Debug.LogWarning("Clickable object is missing its target camera or manager.", this);
        }
    }
}