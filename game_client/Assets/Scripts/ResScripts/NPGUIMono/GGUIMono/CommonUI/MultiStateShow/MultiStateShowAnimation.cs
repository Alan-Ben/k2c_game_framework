using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    [Serializable]
    public class MultiStateShowAnimation<T> where T : Enum 
    {
        /// <summary>
        /// 内部的显示类
        /// </summary>
        [Serializable]
        public class StateShowData
        {
            [NotNull] public T type;
            [NotNull] public List<GameObject> showList;
            public string animationName;
        }

        [ALHeader("不同状态下播放动画的animation")]
        public Animation animation;
        // 不同状态下的显示列表
        [NotNull][ItemNotNull] public List<StateShowData> stateShowDataList;

        /// <summary>
        /// 根据传入的枚举，显示对应的内容
        /// </summary>
        public void setShowData(T _type, Action _aniPlayDone)
        {
            StateShowData stateShowData = null;
            for (int i = 0; i < stateShowDataList.Count; i++)
            {
                StateShowData showData = stateShowDataList[i];
                if (showData.type.Equals(_type))
                    stateShowData = showData;
                else
                    ALUGUICommon.setGameObjEnable(showData.showList, false);
            }

            if (stateShowData != null)
            {
                ALUGUICommon.setGameObjEnable(stateShowData.showList, true);
                if (animation != null && !string.IsNullOrEmpty(stateShowData.animationName))
                {
                    animation.Play(stateShowData.animationName, _aniPlayDone);
                }
                else
                {
                    _aniPlayDone?.Invoke();
                }
            }
        }
    }
}