using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    private Transform playerToFollow;
    [SerializeField]
    private float smoothAmount;
    [SerializeField]
    private Vector3 offsetVector;

    private CinemachineVirtualCamera cinemachine;


    private void Awake()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();
        Player.OnPlayerCreatedEvent += OnPlayerCreated;
    }
    private void FixedUpdate()
    {
        if(playerToFollow != null)
        {
            //Vector3 desiredPosition = transform.position = playerToFollow.position + offsetVector;
            //Vector3 smoothPosition =  Vector3.Lerp(transform.position, desiredPosition, smoothAmount * Time.deltaTime);
            //transform.position = smoothPosition;
            //transform.LookAt(playerToFollow);

            cinemachine.LookAt = playerToFollow.transform;
            cinemachine.Follow = playerToFollow.transform;
        }
    }

    private void OnPlayerCreated(Player player)
    {
        if (player.isLocalPlayer) playerToFollow = player.transform;
    }
}
