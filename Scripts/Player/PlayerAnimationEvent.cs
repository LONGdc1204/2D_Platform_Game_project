using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private Player player;
    public void Awake() {
        player = GetComponentInParent<Player>();
    }
    // Ko cho player dieu khien khi dang chay hoat anh hoi sinh
    public void FinishedRespawn() => player.RespawnFinished(true);
}
