using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星 - ai智能控制效果配置
    /// </summary>
    [Serializable]
    public class MonoMarsIntelligentControlEffectConfig
    {
        [ALHeader("智能控制配表ID")]
        public long intelligentControlId;

        [ALHeader("效果使用的动画名")]
        public string showAniName;
    }
    
    /// <summary>
    /// 火星 - ai智能控制效果表现
    /// </summary>
    [Serializable]
    public class MonoMarsIntelligentControlEffectShow
    {
        [ALHeader("表现使用的动画")]
        public Animation ani;

        [ALHeader("初始化动画名(用来恢复状态显示)")]
        public string initAniName;
        
        [ALHeader("效果配置列表")]
        public List<MonoMarsIntelligentControlEffectConfig> effectConfigList;
        
        /// <summary>
        /// 播放初始化动画
        /// </summary>
        public void playInitAni()
        {
            if (ani != null && !string.IsNullOrEmpty(initAniName))
            {
                ani.ForcePlay(initAniName);
            }
        }
        
        /// <summary>
        /// 进行效果表现
        /// </summary>
        /// <param name="_intelligentControlId"></param>
        public void playEffect(long _intelligentControlId, Action _complete = null)
        {
            if (effectConfigList == null || effectConfigList.Count <= 0)
            {
                _complete?.Invoke();
                return;
            }

            MonoMarsIntelligentControlEffectConfig showEffectConfig = null;
            foreach (var config in effectConfigList)
            {
                if(config != null && config.intelligentControlId == _intelligentControlId)
                {
                    showEffectConfig = config;
                    break;
                }
            }

            if (showEffectConfig != null)
            {
                if (ani != null && !string.IsNullOrEmpty(showEffectConfig.showAniName))
                {
                    ani.ForcePlay(showEffectConfig.showAniName, 0f, () =>
                    {
                        // 播放完成后，重新播放初始化动画
                        playInitAni();
                        _complete?.Invoke();
                    });
                }
                else
                {
                    _complete?.Invoke();
                }
            }
            else
            {
                _complete?.Invoke();
            }
        }
    }
}