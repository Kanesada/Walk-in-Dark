using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 有关场景的重置

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathVFXPrefab;
    int trapsLayer; // 陷阱的碰撞层
    bool isDead = false; //判断角色是否死亡

    // Start is called before the first frame update
    void Start()
    {
        trapsLayer = LayerMask.NameToLayer("Traps");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("碰撞发生");
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Traps")
        {
            Debug.Log("碰撞发生");
            Instantiate(deathVFXPrefab, transform.position, transform.rotation);  // 释放死亡烟雾特效
            AudioManager.PlayDeathAudio(); // 发出死亡音效

            gameObject.SetActive(false);
            if (isDead != true) GameManager.PlayerDead();  //防止死亡多次触发
            isDead = true;

            //Invoke("ReloadScene", 0.8f); // 调用延时 防止音乐未播放完直接跳转场景
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)  //碰撞到traps层
    {
        if (collision.gameObject.layer == trapsLayer)
        {
            Instantiate(deathVFXPrefab, transform.position, transform.rotation);  // 释放死亡烟雾特效
            AudioManager.PlayDeathAudio(); // 发出死亡音效
            
            gameObject.SetActive(false);
            if(isDead != true) GameManager.PlayerDead();  //防止死亡多次触发
            isDead = true;

            //Invoke("ReloadScene", 0.8f); // 调用延时 防止音乐未播放完直接跳转场景
        }
    }

    private void ReloadScene()
    {
        isDead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
