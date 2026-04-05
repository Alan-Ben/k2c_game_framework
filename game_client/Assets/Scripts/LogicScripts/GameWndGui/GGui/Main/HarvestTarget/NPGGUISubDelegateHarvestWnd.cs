
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一个带回调资源收集目标对象
    /// 这个对象不会对粒子数量和目标之类的进行处理，只是单纯的作为收集粒子指向的目标
    /// </summary>
    public class NPGGUISubDelegateHarvestWnd : _ANPGGUISubHarvestWnd
    {
        public event Action onParticleItemDoneAction;
        
        public NPGGUISubDelegateHarvestWnd(EHarvestType _harvestResType, RectTransform _targetUIObj)
            : base(_harvestResType, _targetUIObj)
        {
        }

        protected override void _onInit()
        {
        }

        protected override void _onDiscard()
        {
        }

        public override void particleStart(long _serialize)
        {
        }

        public override void particleComplete(long _serialize)
        {
        }

        public override void particleItemDone(long _serialize, long _itemCount)
        {
            if (onParticleItemDoneAction != null) 
                onParticleItemDoneAction();
        }
    }
}