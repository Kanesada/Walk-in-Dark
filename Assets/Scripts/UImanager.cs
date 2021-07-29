using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UImanager : MonoBehaviour
{
    static UImanager instance;

    public TextMeshProUGUI orbText, timeText, deathText, gameoverText; 

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static void UpdateOrbUI(int orbCount)  //更新orb UI
    {
        instance.orbText.text = orbCount.ToString();  //将int转换为string
    }

    public static void UpdateDeathUI(int deathCount) //更新死亡次数UI
    {
        instance.deathText.text = deathCount.ToString();
    }

    public static void UpdateTimeUI(float time) //更新时间UI
    {
        int minute = (int)(time) / 60;
        float second = time % 60;
        instance.timeText.text = time.ToString();
        instance.timeText.text = minute.ToString("00") + ":" + second.ToString("00");  // 秒和分钟只显示两位
    }
    public static void DisplayGameOver()
    {
        instance.gameoverText.enabled = true;
    }

}
