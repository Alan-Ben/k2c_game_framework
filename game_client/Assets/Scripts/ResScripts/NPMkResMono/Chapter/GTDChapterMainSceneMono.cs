using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 关卡表现状态枚举
    /// </summary>
    public enum EChapterMainSceneState
    {
        Idle,//默认
        Forward,//前进
        QuickForward,//快速前进
        PerBoss,//boss前状态，那个只出现一次的动画
        WaitBoss,//等待boss战
    }
    
    /// <summary>
    /// 关卡主场景mono
    /// </summary>
    public class GTDChapterMainSceneMono : MonoBehaviour
    {
        [ALHeader("玩家形象加载位置")]
        public Transform playerParent;
        [ALHeader("场景动画")]
        public Animator bgAnimator;
        
        [ALHeader("不同状态下的表现参数")]
        public List<GTDChapterMainSceneStateInfo> stateInfos;
        
        public GTDChapterMainSceneStateInfo getStateInfo(EChapterMainSceneState _state)
        {
            if (null == stateInfos)
                return null;
            
            foreach (GTDChapterMainSceneStateInfo gtdChapterMainSceneStateInfo in stateInfos)
            {
                if(null == gtdChapterMainSceneStateInfo)
                    continue;

                if (gtdChapterMainSceneStateInfo.state == _state)
                    return gtdChapterMainSceneStateInfo;
            }

            return null;
        }
    }
    
    [System.Serializable]
    public class GTDChapterMainSceneStateInfo
    {
        [ALHeader("对应状态")]
        public EChapterMainSceneState state;
        [ALHeader("背景速度参数")]
        public float speed;
        [ALHeader("玩家动画名,列表内随机")]
        public List<string> playerAniNameList;
        [ALHeader("小车动画名")]
        public string carAniName;
    }
}