using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted
    }

    [SerializeField] Mode mode;


    void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 directionToCamera = Camera.main.transform.position - transform.position;
                transform.LookAt(directionToCamera);
                break;
            case Mode.CameraForward:
                transform.forward = -Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = Camera.main.transform.forward;
                break;
        }
    }
}
