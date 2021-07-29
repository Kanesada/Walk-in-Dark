using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    SceneFader fader;
    List<Orb> orbs;  // orb列表 记录orb

    [Header("场景信息")]
    //public int orbNum;  // 场景现有orb
    public int deathNum; //玩家死亡数量
    Door lockedDoor;
    public float gameTime; //记录时间
    public bool gameIsOver = false;  //判断游戏是否结束

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        orbs = new List<Orb>();

        DontDestroyOnLoad(this);
    }

    public static void RegisterOrb(Orb orb)  // 注册记录Orb
    {
        if (instance == null) return;
        if (!instance.orbs.Contains(orb)) instance.orbs.Add(orb);   //检测orbs列表中是否包含orb，如果不包含，将场景中orb添加
        UImanager.UpdateOrbUI(instance.orbs.Count);
    }

    public static void PlayerGrabbedOrb(Orb orb)
    {
        if (!instance.orbs.Contains(orb)) return;
        instance.orbs.Remove(orb);
        UImanager.UpdateOrbUI(instance.orbs.Count);


        if (instance.orbs.Count == 0)
        {
            instance.lockedDoor.Open();
        }
    }

    public static void RegisterDoor(Door door)
    {
        instance.lockedDoor = door;
    }

    public static void RegisterSceneFader(SceneFader obj)  // 注册fader，生成实例
    {
        instance.fader = obj;
    }

    public static void PlayerDead() //角色死亡
    {
        instance.fader.FadeOut();
        instance.deathNum++;
        UImanager.UpdateDeathUI(instance.deathNum);
        instance.Invoke("RestartScene", 1.5f);
    }

    void RestartScene()  //重置场景
    {
        instance.orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public static void PlayerWon()
    {
        instance.gameIsOver = true;
        UImanager.DisplayGameOver();
    }

    public static bool GameOver()
    {
        return instance.gameIsOver;
    }

    private void Update()
    {
        if (gameIsOver) return;
        //orbNum = instance.orbs.Count;
        gameTime += Time.deltaTime;
        UImanager.UpdateTimeUI(gameTime);
    }

}
