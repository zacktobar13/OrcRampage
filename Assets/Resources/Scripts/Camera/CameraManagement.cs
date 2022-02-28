using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManagement : MonoBehaviour
{
    CinemachineVirtualCamera cvCamera;
    void Start()
    {
        cvCamera = GetComponent<CinemachineVirtualCamera>();
        cvCamera.Follow = PlayerManagement.player.transform;
    }
}
