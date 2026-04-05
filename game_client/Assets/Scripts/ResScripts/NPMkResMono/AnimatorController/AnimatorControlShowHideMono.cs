using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 再显示隐藏时候注册全局动画的customMono
    /// </summary>
    public class AnimatorControlShowHideMono :MonoBehaviour
    {
        /// <summary>
        /// 需要注册的数据列表
        /// </summary>
        public List<AnimatorControlRegInfo> dataList;

        private void OnEnable()
        {
            if(null == dataList)
                return;
            
            foreach (AnimatorControlRegInfo animatorControlRegInfo in dataList)
            {
                if(null == animatorControlRegInfo || string.IsNullOrEmpty(animatorControlRegInfo.controlNameTag))
                    continue;
                
                AnimatorControllerMgr.instance.regMono(animatorControlRegInfo);
            }
        }

        private void OnDisable()
        {
            if(null == dataList)
                return;
            
            foreach (AnimatorControlRegInfo animatorControlRegInfo in dataList)
            {
                if(null == animatorControlRegInfo || string.IsNullOrEmpty(animatorControlRegInfo.controlNameTag))
                    continue;
                
                AnimatorControllerMgr.instance.unregMono(animatorControlRegInfo);
            }
        }
        
        private void OnDestroy()
        {
            if(null == dataList)
                return;
            
            foreach (AnimatorControlRegInfo animatorControlRegInfo in dataList)
            {
                if(null == animatorControlRegInfo || string.IsNullOrEmpty(animatorControlRegInfo.controlNameTag))
                    continue;
                
                AnimatorControllerMgr.instance.unregMono(animatorControlRegInfo);
            }
        }
    }
}