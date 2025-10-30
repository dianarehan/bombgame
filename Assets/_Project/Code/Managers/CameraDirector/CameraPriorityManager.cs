using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

public class CameraPriorityManager : MonoBehaviour
{
    [SerializeField] private List<CinemachineCamera> allCameras;

    [SerializeField] private int activePriority = 100;
    [SerializeField] private int inactivePriority = 10;

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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActivateCamera(allCameras[0]);
        }
    }
    
    void Start()
    {
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

    public void ActivateCamera(CinemachineCamera cameraToActivate)
    {
        if (cameraToActivate == null || !allCameras.Contains(cameraToActivate))
        {
            Debug.LogWarning("The camera to activate is not in the manager's list.");
            return;
        }

        foreach (var cam in allCameras)
        {
            cam.Priority = (cam == cameraToActivate) ? activePriority : inactivePriority;
        }
        
    }
}