using Unity.Cinemachine;
using UnityEngine;

public class CameraDirector : MonoBehaviour
{
    public CinemachineCamera cameraA;
    public CinemachineCamera cameraB;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Switch to the other camera
            if (cameraA.Priority > cameraB.Priority)
            {
                cameraB.Priority = 10;
                cameraA.Priority = 0;
            }
            else
            {
                cameraB.Priority = 0;
                cameraA.Priority = 10;
            }
        }
    }

}
