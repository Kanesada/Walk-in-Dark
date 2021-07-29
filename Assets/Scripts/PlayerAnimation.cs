using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    PlayerMovement movement;
    Rigidbody2D rb;
    //AudioManager audioManager;
    

    int fallID;

    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponentInParent<PlayerMovement>();
        fallID = Animator.StringToHash("verticalVelocity");
        rb = GetComponentInParent<Rigidbody2D>();
        //audioManager = GetComponent<AudioManager>();
    }

    void Update()
    {
        // 将状态与动画进行绑定
        anim.SetFloat("speed", Mathf.Abs(movement.xVelocity));
        anim.SetBool("isOnGround", movement.isOnGround);
        anim.SetBool("isHanging", movement.isHanging);
        anim.SetBool("isJumping", movement.isjump);
        anim.SetBool("isCrouching", movement.isCrouch);
        anim.SetFloat(fallID, rb.velocity.y);
    }

    public void StepAudio() //脚步声 作为事件在Animation中调用 可查看running的Animation
    {
        AudioManager.PlayFootstepAudio();
    }

    public void CrouchStepAudio()  //下蹲脚步声
    {
        AudioManager.PlayCrouchFootstepAudio();
    }

}
