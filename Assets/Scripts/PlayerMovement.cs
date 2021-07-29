using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("移动设置")]
    public float moveSpeed = 3f;
    public float crouchSpeedDivisor = 3f;//下蹲（折叠）速度


    [Header("跳跃设置")]
    [Tooltip("分别设置小跳与大跳")]
    public float jumpForce = 10.3f;
    public float crouchJumpBoost = 15.5f;
    public float hangingJumpForce = 14.5f;
    public int maxJumpCount = 2;  //最大跳跃数量 用来实现二段跳
    private int jumpCount = 1;


    [Header("角色状态")]
    public bool isCrouch;//是否下蹲
    public bool isOnGround; //判断是否站在地面
    public bool isjump; //判断是否在跳跃
    public bool isHeadBlocked; //判断头顶是否有遮挡
    public bool isHanging; //判断是否在悬挂
    public float xVelocity; // x轴的受力方向


    [Header("环境检测")]
    public float headClearance = 0.2f;
    public float footOffest = 0.45f;
    float playerHeight;
    public float eyeHeight = 1.5f;  //悬挂相关
    public float grabDistance = 0.5f;
    public float reachOffset = 0.8f;

    public LayerMask groundLayer;
    

    bool jumpPressed;  // 跳跃键是否按下
    bool crouchPressed; //下蹲键是否按下
    bool crouchHeld;  //下蹲键是否长按

    //碰撞体尺寸 预留变量
    Vector2 colliderStandSize;//站立状态
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;//下蹲状态
    Vector2 colliderCrouchOffset;





    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  //引用角色刚体
        coll = GetComponent<BoxCollider2D>();  //引用角色碰撞

        colliderStandSize = coll.size;   //用预留的变量接收碰撞体属性 定义好站立与下蹲状态的碰撞体大小 方便之后使用
        colliderStandOffset = coll.offset;
        colliderCrouchSize = new Vector2(coll.size.x, coll.size.y / 2f);
        colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y / 2f);
        playerHeight = coll.size.y;
        footOffest = coll.size.x / 2f;

        jumpCount = maxJumpCount;  //跳跃次数赋初值
    }


    void Update()  //与按键手感相关 放在update里调用
    {
        if (GameManager.GameOver())
        {
            rb.velocity = new Vector2(0f, 0f);
            return;
        }
        if (Input.GetButtonDown("Jump") && jumpCount> 0) jumpPressed = true;
        crouchHeld = Input.GetButton("Crouch");
        if (Input.GetButtonDown("Crouch")) crouchPressed = true;
        



    }

    private void FixedUpdate()  //与物理相关 放在Fixupdate里调用
    {
        if (GameManager.GameOver())
        {
            rb.velocity = new Vector2(0f, 0f);
            return;
        }
        PhysicsCheck(); //检测是否站在地上
        GroundMovement(); //控制角色进行左右移动
        MidAirMovement();  //控制角色进行跳跃
    }





    //判断角色是否有碰撞
    void PhysicsCheck()
    {
        //判断脚下
        isOnGround = Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer);
        //射线判断头顶
        isHeadBlocked = OffsetRaycast(new Vector2(0f, coll.size.y), Vector2.up, headClearance, groundLayer); 
        // 射线判断头顶的悬挂方向
        Vector2 grabDirection = new Vector2(transform.localScale.x, 0f);
        RaycastHit2D blockCheck = OffsetRaycast(new Vector2(footOffest * transform.localScale.x, playerHeight), grabDirection, grabDistance, groundLayer);
        //射线判断眼前的悬挂方向
        RaycastHit2D wallCheck = OffsetRaycast(new Vector2(footOffest * transform.localScale.x, eyeHeight), grabDirection, grabDistance, groundLayer);
        //射线判断有无抓檐
        RaycastHit2D ledgeCheck = OffsetRaycast(new Vector2(reachOffset * transform.localScale.x, playerHeight), Vector2.down, grabDistance, groundLayer);
        // 判断能否悬挂 条件：下落 头顶无障碍 有抓檐
        if(!isOnGround && rb.velocity.y<0 && !blockCheck && wallCheck && ledgeCheck)
        {
            //修正模型的位置
            Vector3 pos = transform.position;
            pos.x += (wallCheck.distance -0.1f) * transform.localScale.x;
            pos.y -= ledgeCheck.distance;
            transform.position = pos;
            
            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }



    }

    //控制角色进行左右移动函数 先判断是否是下蹲状态
    void GroundMovement()
    {
        if (isHanging)  //悬挂状态不能左右移动
        {
            jumpCount = 1; //使悬挂角色可以向上跳一下
            if (jumpPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.AddForce(new Vector2(rb.velocity.x, hangingJumpForce), ForceMode2D.Impulse);
                isHanging = false;
                jumpPressed = false;
            }
            else if (crouchPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                isHanging = false;
                crouchPressed = false;
            }
            else return;
        }
        if (crouchHeld && !isCrouch && isOnGround) Crouch();   //判断有无下蹲指令
        else if (!crouchHeld && isCrouch && !isHeadBlocked) StandUp();
        else if (!isOnGround && isCrouch) StandUp();  //空中取消下蹲状态

        xVelocity = Input.GetAxis("Horizontal");  //获取水平方向移动指令。-1f~1f 不按的时候自动归0 因此不会出现滑动

        if (isCrouch) xVelocity = xVelocity / crouchSpeedDivisor;
        rb.velocity = new Vector2(xVelocity * moveSpeed, rb.velocity.y);

        FlipDirection();  //控制角色行动时贴图的方向
    }

    void FlipDirection()  //控制角色行动时贴图的方向
    {
        if (xVelocity > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (xVelocity < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Crouch()  //下蹲函数
    {
        isCrouch = true;
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }

    void StandUp()  //从下蹲中恢复
    {
        isCrouch = false;
        crouchPressed = false;
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
    }
    void MidAirMovement()  //跳跃函数
    {
        if (isOnGround)
        {
            isjump = false;
            jumpCount = maxJumpCount;
        }
        if (jumpPressed && isOnGround && !isHeadBlocked)   //起跳阶段 判断符合起跳条件
        {
            if(isCrouch) //下蹲蓄力跳
            {
                StandUp();
                isjump = true;
                rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);  //小跳
                isjump = true;
            }

            jumpCount--;
            jumpPressed = false;
            AudioManager.PlayJumpAudio();
        }
        else if(jumpPressed && isjump && jumpCount > 0)  //空中 二段跳
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
            //AudioManager.PlayJumpAudio();

        }
    }

    
    RaycastHit2D OffsetRaycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)  //装饰一个射线碰撞检测函数
    {
        Vector2 pos = transform.position;
        float direction = transform.localScale.x;
        Vector2 correct = new Vector2(0.1f, 0.1f);
        RaycastHit2D hit = Physics2D.Raycast(pos + offset - correct*direction, rayDirection, length, layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection * length, color);
        return hit;
    } 


}
