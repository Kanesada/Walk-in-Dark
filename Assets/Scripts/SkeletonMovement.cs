using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMovement : MonoBehaviour
{
    [Header("Left and Right Bound")]
    public Transform LeftBound;
    public Transform RightBound;
    public float leftBound;
    public float rightBound;


    [Header("Moving Parameter")]
    public float roamingSpeed;
    public float chasingSpeed;
    public float lightedSpeed;

    
    public enum State
    {
        Roaming,
        Chasing,
        Lighted,
        Dead
    }

    [Header("Enemy State")]
    public float maxHp = 2;  // enemy's max hp
    public State state;
    public bool isLightOn;   // wheter the light is on
    public bool isDetected;  // wheter the player entered the detection area
    public GameObject robbie;  // get player


    bool faceLeft = true;
    float hp;
    Rigidbody2D rb;   // enemy's rigid body
    BoxCollider2D detectionArea;  // the detection area
    


    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        rb = GetComponent<Rigidbody2D>();
        robbie = GameObject.Find("Robbie");
        

        leftBound = LeftBound.position.x;
        rightBound = RightBound.position.x;




        Destroy(LeftBound.gameObject);
        Destroy(RightBound.gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        isLightOn = GameObject.Find("Spot Light").GetComponent<MovingLight>().turnOn;  // Get the light state

        
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                EnemyRoaming();


                break;
            case State.Chasing:


                break;
            case State.Lighted:

                break;

            case State.Dead:

                break;

        }
    }

    public void EnemyRoaming()
    {
        if(faceLeft == true)  // if the enemy is facing left
        {
            rb.velocity = new Vector2(-roamingSpeed, rb.velocity.y);  // move toward left
            if(transform.position.x < leftBound)
            {
                //transform.localScale = new Vector3(-1* transform.localScale.x,transform.localScale.y,transform.localScale.z);
                faceLeft = false;
            }
           
        }
        else if(faceLeft == false)
        {
            rb.velocity = new Vector2(roamingSpeed, rb.velocity.y);  // move toward right
            if (transform.position.x > rightBound)
            {
                //transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
                faceLeft = true;
            }
        }

    }

    public void EnemyChasing()
    {
        
    }

    public void EnemyLighted()
    {

    }

    public void EnemyDead()
    {

    }



}
