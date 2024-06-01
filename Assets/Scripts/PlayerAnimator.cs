using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    
    private const string IS_WALKING = "isWalking";

    private PlayerScript player;

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponentInParent<PlayerScript>();
    }

    void Update()
    {
        anim.SetBool(IS_WALKING, player.is_Walking());
    }
}
