using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerSkillController : NetworkBehaviour
{
    [SerializeField] double healAmount;
    [SerializeField] double skillDelay;

    private double lastSkillUsageTime = 0;

    private Controls controls;

    private Controls Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new Controls();
        }
    }

    public override void OnStartAuthority()
    {
        enabled = true;

        Controls.Player.Heal.performed += ctx => HealIfSanya();
    }

    [ClientCallback]
    private void OnEnable() => Controls.Enable();
    [ClientCallback]
    private void OnDisable() => Controls.Disable();

    [Command]
    private void HealIfSanya()
    {
        var player = GetComponent<Player>();
        Debug.Log("AM I SANYA?");

        if (player.Dino == DinowarsNetworkRoomPlayer.Dino.Sanya)
        {
            Debug.Log("TRYING TO HEAL");
            var currentTime = Time.time;
            if (lastSkillUsageTime + skillDelay < currentTime)
            {
                Debug.Log("HEALING MYSELF");
                player.Heal(healAmount);
                lastSkillUsageTime = currentTime;

            }
        }
    }
}
