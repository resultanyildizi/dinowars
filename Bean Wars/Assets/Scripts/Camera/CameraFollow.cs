using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachine;

    private Transform playerToFollow;

    private void Awake()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();
        Player.OnPlayerCreatedEvent += OnPlayerCreated;
    }
    private void FixedUpdate()
    {
        if (playerToFollow != null)
        {
            cinemachine.LookAt = playerToFollow.transform;
            cinemachine.Follow = playerToFollow.transform;
        }
    }

    private void OnPlayerCreated(Player player)
    {
        if (player.isLocalPlayer) playerToFollow = player.transform;
    }
}
