using ALPackage;
using NPEnum;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    [Serializable]
    public class LevySilverPointItemStatusMono
    {
        [ALHeader("百分比 小于这个值展示")]
        public float percent;

        [ALHeader("需要显示的GoList")]
        public List<GameObject> showGoList;
    }
    
    /// <summary>
    /// 征收银币功能入口点的Mono
    /// </summary>
    public class GTDHomeEntryPointMono_Silver : _AGTDHomeEntryPointMono_Base
    {
        [ALHeader("银币阶段表现")]
        public List<LevySilverPointItemStatusMono> statusMonoList;


        private void Awake()
        {
            if(null == statusMonoList)
                return;
            
            statusMonoList.Sort((_x, _y) => _x.percent.CompareTo(_y.percent));
        }

        /// <summary>
        /// 根据进度获取对应要加载的资源
        /// </summary>
        /// <param name="_percent"></param>
        /// <returns></returns>
        public LevySilverPointItemStatusMono getResPathIdByPercent(float _percent,List<LevySilverPointItemStatusMono> _monoList)
        {
            LevySilverPointItemStatusMono temp = null;
#if NP_GAME
            if (null == _monoList || _monoList.Count == 0)
                return null;

            _percent *= 100;

            foreach (LevySilverPointItemStatusMono levySilverPointItemStatusMono in _monoList)
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
