using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static AudioManager current;

    [Header("环境声音")]
    public AudioClip ambientClip;
    public AudioClip musicClip;

    [Header("主角音效")]
    public AudioClip[] walkStepClips;
    public AudioClip[] crouchStepClips;
    public AudioClip jumpClip;
    public AudioClip jumpVoiceClip;
    public AudioClip deathClip;
    public AudioClip deathVoiceClip;
    public AudioClip orbVoiceClip;

    [Header("FX音效")]
    public AudioClip deathFXclip;
    public AudioClip orbFXClip;
    public AudioClip doorFXClip;


    AudioSource ambientSource;
    AudioSource musicSource;
    AudioSource fxSource;
    AudioSource playerSource;
    AudioSource voiceSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        if (current != null) return;  // 防止重复添加
        current = this;
        DontDestroyOnLoad(gameObject); //切换场景重新加载时不删除

        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        fxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();
        StartLevelAudio();
    }

    void StartLevelAudio()
    {
        current.ambientSource.clip = current.ambientClip; // 环境音效
        current.ambientSource.loop = true;
        current.ambientSource.Play();

        current.musicSource.clip = current.musicClip;
        current.musicSource.loop = true;
        current.musicSource.Play();
    }


    public static void PlayFootstepAudio() // 移动脚步
    {
        int index = Random.Range(0, current.walkStepClips.Length); // 在脚步声数组中随机选择脚步声进行播放
        current.playerSource.clip = current.walkStepClips[index];
        current.playerSource.Play();
    }

    public static void PlayCrouchFootstepAudio()  //下蹲脚步
    {
        int index = Random.Range(0, current.crouchStepClips.Length); // 在脚步声数组中随机选择脚步声进行播放
        current.playerSource.clip = current.crouchStepClips[index];
        current.playerSource.Play();
    }

    public static void PlayJumpAudio()
    {
        current.playerSource.clip = current.jumpClip;
        current.playerSource.Play();

        current.playerSource.clip = current.jumpVoiceClip;
        current.playerSource.Play();
    }

    public static void PlayDeathAudio()
    {
        Debug.Log("enter death audio");
        current.playerSource.clip = current.deathClip;
        current.playerSource.Play();

        current.voiceSource.clip = current.deathVoiceClip;
        current.voiceSource.Play();

        current.fxSource.clip = current.deathFXclip;
        current.fxSource.Play();
    }

    public static void PlayOrbAudio()
    {
        current.fxSource.clip = current.orbFXClip;
        current.fxSource.Play();

        current.voiceSource.clip = current.orbVoiceClip;
        current.voiceSource.Play();
    }

    public static void PlayDoorOpenAudio()
    {
        current.fxSource.clip = current.doorFXClip;
        current.fxSource.PlayDelayed(1f);
    }

}
