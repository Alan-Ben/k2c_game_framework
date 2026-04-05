using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 银币入口跟随tip
    /// </summary>
    public class GGUIMonoLevySilverPointItem : _AGGUIMonoEntryPointFollowItemBase
    {
        [ALHeader("征收按钮")]
        public GameObject levyBtn;

        [ALHeader("征收动画")]
        public Animation levyAnimation;

        [ALHeader("征收动画名称")]
        public string levyAnimationStr;

        [ALHeader("加载预制体位置")]
        public Transform loadStatusPrefabPos;

        [ALHeader("银币阶段表现")]
        public List<LevySilverPointItemStatusMono> statusMonoList;

        [ALHeader("弹出tip位置")]
        public RectTransform tipStartPos;

        [ALHeader("上浮提示显示参数")]
        [ALHeader("最高速率")]
        public float tipAccMaxRate = 1.0f;

        [ALHeader("待显示tip达到这个数开始加速")]
        public int tipAccMinNum;

        [ALHeader("待显示tip达到这个数达到最高速")]
        public int tipAccMaxNum;

        private void Awake()
        {
            if (null == statusMonoList)
                return;

            statusMonoList.Sort((_x, _y) => _x.percent.CompareTo(_y.percent));
        }
        
        /// <summary>
        /// 根据进度获取对应要加载的资源
        /// </summary>
        /// <param name="_percent"></param>
        /// <returns></returns>
        public LevySilverPointItemStatusMono getResPathIdByPercent(float _percent)
        {
            LevySilverPointItemStatusMono temp = null;
#if NP_GAME
            if (null == statusMonoList || statusMonoList.Count == 0)
                return null;

            _percent *= 100;

            foreach (LevySilverPointItemStatusMono levySilverPointItemStatusMono in statusMonoList)
            {
                if (null == levySilverPointItemStatusMono)
                    continue;

                if (null == temp && _percent < levySilverPointItemStatusMono.percent && !Mathf.Approximately(_percent, levySilverPointItemStatusMono.percent))
                {
                    temp = levySilverPointItemStatusMono;
                }

                ALUGUICommon.setGameObjEnable(levySilverPointItemStatusMono.showGoList, false);
            }      
#endif
            return temp;
        }
    }
}