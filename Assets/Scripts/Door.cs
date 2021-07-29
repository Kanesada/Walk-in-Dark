using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;
    int openID;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        openID = Animator.StringToHash("Open");
        GameManager.RegisterDoor(this); // 在game manager中注册自己
        
    }

    public void Open()
    {
        anim.SetTrigger(openID);  // 播放开门动画
        AudioManager.PlayDoorOpenAudio(); //播放开门声音
    }

    
}
