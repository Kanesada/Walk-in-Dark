using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MovingLight : MonoBehaviour
{
    [Header("Light Control Parameter")]
    public Light headLight;  //Player head light
    public Light torsoLight; //Player torso light
    public Light spotlight; // the spot light
    public bool turnOn;   // if the light is turned on

    CircleCollider2D lightCollider;  // Light collider

    Vector3 screenPosition;//将物体从世界坐标转换为屏幕坐标
    Vector3 mousePositionOnScreen;//获取到点击屏幕的屏幕坐标
    Vector3 mousePositionInWorld;//将点击屏幕的屏幕坐标转换为世界坐标


    // Start is called before the first frame update
    void Start()
    {
        spotlight = GetComponent<Light>();
        turnOn = true;
        lightCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MouseFollow();
        if(Input.GetButtonDown("Fire1"))
        {
            TurnOnOff();
        }

    }

    void MouseFollow()

    {
        //获取鼠标在相机中（世界中）的位置，转换为屏幕坐标；

        screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        //获取鼠标在场景中坐标

        mousePositionOnScreen = Input.mousePosition;

        //让场景中的Z=鼠标坐标的Z

        mousePositionOnScreen.z = screenPosition.z;

        //将相机中的坐标转化为世界坐标

        mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);

        //物体跟随鼠标移动

        //transform.position = mousePositionInWorld;

        //物体跟随鼠标X轴移动

        transform.position = new Vector3(mousePositionInWorld.x, mousePositionInWorld.y, transform.position.z);
    }

    public void TurnOnOff()   //控制灯光亮灭
    {
        if (turnOn == true)  // if the light is on, turn off the light
        {
            spotlight.enabled = false;
            lightCollider.enabled = false;
            headLight.intensity = 1.5f;
            headLight.range = 0.6f;

            torsoLight.intensity = 1f;
            torsoLight.range = 0.6f;
            turnOn = false;
            return;
        }
        else if (turnOn == false)   // if the light is off, turn on the light
        {
            spotlight.enabled = true;
            lightCollider.enabled = true;
            headLight.intensity = 3.2f;
            headLight.range = 1.2f;

            torsoLight.intensity = 3f;
            torsoLight.range = 1f;
            turnOn = true;
            return;
        }

    }

        
    }
