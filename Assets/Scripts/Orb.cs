using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public GameObject explosionVFXPrefab;
    int player;
    // Start is called before the first frame update
    void Start()
    {
        player = LayerMask.NameToLayer("Player");
        GameManager.RegisterOrb(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)  //检测trigger碰撞 播放动画
    {
        if(collision.gameObject.layer == player)
        {
            Instantiate(explosionVFXPrefab, transform.position, transform.rotation);
            AudioManager.PlayOrbAudio();
            GameManager.PlayerGrabbedOrb(this);
            gameObject.SetActive(false);
        }
    }

}
