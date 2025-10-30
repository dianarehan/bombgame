using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

public class CameraPriorityManager : MonoBehaviour
{
    [Header("Camera Control")]
    [Tooltip("Add all your virtual cameras to this list.")]
    public List<CinemachineCamera> allCameras;

    [Header("Priority Settings")]
    public int activePriority = 100;
    public int inactivePriority = 10;

    public static CameraPriorityManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Make sure we have cameras
        if (allCameras == null || allCameras.Count == 0)
        {
            Debug.LogError("No cameras assigned to the CameraPriorityManager!");
            return;
        }

        for (int i = 0; i < allCameras.Count; i++)
        {
            allCameras[i].Priority = (i == 0) ? activePriority : inactivePriority;
        }
    }

    /// <summary>
    /// This is the main function. It activates one camera and deactivates all others.
    /// </summary>
    public void ActivateCamera(CinemachineCamera cameraToActivate)
    {
        if (cameraToActivate == null || !allCameras.Contains(cameraToActivate))
        {
            Debug.LogWarning("The camera to activate is not in the manager's list.");
            return;
        }

        // Loop through all cameras and set priorities
        foreach (var cam in allCameras)
        {
            cam.Priority = (cam == cameraToActivate) ? activePriority : inactivePriority;
        }
        
    }
}