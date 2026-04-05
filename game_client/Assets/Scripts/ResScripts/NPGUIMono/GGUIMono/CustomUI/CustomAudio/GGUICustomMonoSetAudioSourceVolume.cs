using GOE;
using UnityEngine;

/// <summary>
/// 设置AudioSource音量
/// </summary>
public class GGUICustomMonoSetAudioSourceVolume : MonoBehaviour
{
    [ALInfo("该脚本用于设置AudioSource音量跟随游戏内背景音量设置")]
    [ALHeader("音频源")]
    public AudioSource audioSource;
    
    private void OnEnable()
    {
        if (audioSource == null)
            return;

#if NP_GAME
        float bgAudioValue = AudioMixerMgr.getCurBgVolume();
        audioSource.volume = bgAudioValue;
#endif
    }

    private void OnDisable()
    {
    }
}
