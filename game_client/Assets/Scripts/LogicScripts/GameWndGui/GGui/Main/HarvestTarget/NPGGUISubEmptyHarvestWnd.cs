
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一个空的资源收集目标对象
    /// 这个对象不会对粒子数量和目标之类的进行处理，只是单纯的作为收集粒子指向的目标
    /// </summary>
    public class NPGGUISubEmptyHarvestWnd : _ANPGGUISubHarvestWnd
    {
        public NPGGUISubEmptyHarvestWnd(EHarvestType _harvestResType, RectTransform _targetUIObj)
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
        }
    }
}