
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一个空的资源收集目标对象
    /// 这个对象不会对粒子数量和目标之类的进行处理，只是单纯的作为收集粒子指向的目标
    /// </summary>
    public abstract class _ANPGGUISubHarvestWnd : _IHarvestTarget
    {
        //收获资源对象
        private readonly EHarvestType _m_eHarvestType;
        //目标的UI对象
        private readonly RectTransform _m_rtTargetUIObj;

        private bool _m_bIsInited;

        protected _ANPGGUISubHarvestWnd(EHarvestType _harvestResType, RectTransform _targetUIObj)
        {
            _m_eHarvestType = _harvestResType;
            _m_rtTargetUIObj = _targetUIObj;

            _m_bIsInited = false;
        }

        /// <summary>
        /// 初始化相关的监控和管理
        /// </summary>
        public void init()
        {
            //if (_m_bIsInited || _m_eHarvestType == EHarvestType.NONE)
            if (_m_bIsInited)
                return;

            //设置为已初始化
            _m_bIsInited = true;
            GGUIHarvestCore.instance.regHarvestTarget(this);
            
            _onInit();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void discard()
        {
            if (!_m_bIsInited)
                return;

            _m_bIsInited = false;
            GGUIHarvestCore.instance.unregHarvestTarget(this);
            
            _onDiscard();
        }

        protected abstract void _onInit();
        protected abstract void _onDiscard();

        #region 收获接口部分
        /// <summary>
        /// 对应的收获资源类型
        /// </summary>
        public EHarvestType harvestType { get { return _m_eHarvestType; } }

        /// <summary>
        /// 获取粒子收集结束对象
        /// </summary>
        public RectTransform targetUIObj { get { return _m_rtTargetUIObj; } }

        /// <summary>
        /// 粒子开始表现时的处理
        /// 此时需要设置好相关的状态以及可能触发消息的处理，避免错误的刷新操作
        /// </summary>
        public abstract void particleStart(long _serialize);

        /// <summary>
        /// 粒子表现结束的处理
        /// </summary>
        public abstract void particleComplete(long _serialize);

        /// <summary>
        /// 单个粒子飞行到位的处理
        /// </summary>
        /// <param name="_itemCount">单个粒子代表数量</param>
        public abstract void particleItemDone(long _serialize, long _itemCount);
        #endregion
    }
}