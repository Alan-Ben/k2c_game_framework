using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 运动粒子对象
    /// </summary>
    public class BurstParticleObj
    {
        //是否开始粒子运动，未开始情况下不需要进行tick运算
        private bool _m_bIsStart;
        //开始进行粒子运动的时间
        private float _m_fStartTimeS;

        //显示对象，从Group创建，无效的时候放回group
        private ParticleCacheItem _m_piParticleItem;

        private BurstParticleGroupInfo _m_bBurstGroupInfo;//粒子信息
        private RectTransform _m_rtControlRect;//控制的RectTransform
        private Vector2 _m_vStartPos;  //初始坐标
        private Vector2 _m_vBurstPos;  //炸裂的目标坐标
        private Vector2 _m_vEndPos;    //飞行的目标坐标
        private float _m_fTotalBurstTime;      //炸裂运动总时长
        private float _m_fTotalFlyTime;      //飞行运动总时长
        private float _m_fBurstFristAccSpeedTimeScale;  //炸裂运动加速段占比
        private float _m_fFlyFristAccSpeedTimeScale;  //飞行运动加速段占比
        private float _m_alphaStartTime;  //粒子开始改变透明度的时间
        private float _m_alphaEndValue;  //粒子alpha最终的值
        private float _m_totalAlphaTime;  //粒子alpha变化总时间

        private ALRealTimeFloatFadeController _m_cBurstXcontroller;   //X轴运动计算对象
        private ALRealTimeFloatFadeController _m_cBurstYcontroller;   //Y轴运动计算对象

        private bool _m_bIsBurstPhase;         //是否是炸裂运动阶段
        private float _m_fPhaseEndTime;      //当前运动阶段结束时间

        public BurstParticleObj()
        {
            _m_bIsStart = false;
            _m_fStartTimeS = 0f;

            _m_piParticleItem = null;

            _m_bBurstGroupInfo = null;
            _m_fPhaseEndTime = 0f;
            _m_fTotalBurstTime = 0f;
            _m_fTotalFlyTime = 0f;
            _m_fFlyFristAccSpeedTimeScale = 0f;
            _m_vStartPos = Vector2.zero;
            _m_vBurstPos = Vector2.zero;
            _m_vEndPos = Vector2.zero;
            _m_rtControlRect = null;
            _m_bIsBurstPhase = true;
            _m_alphaStartTime = 0;
            _m_alphaEndValue = 0;
            _m_totalAlphaTime = 0;

            _m_cBurstXcontroller = new ALRealTimeFloatFadeController(1f, 1f, 1f, 1f, 1f);
            _m_cBurstYcontroller = new ALRealTimeFloatFadeController(1f, 1f, 1f, 1f, 1f);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_startTime">激活时间点</param>
        /// <param name="_groupInfo">粒子组信息</param>
        /// <param name="_startPos">起点</param>
        /// <param name="_burstPos">炸裂终点</param>
        /// <param name="_endPos">飞行终点</param>
        /// <param name="_totalBurstTime">炸裂运动总时长</param>
        /// <param name="_totalFlyTime">飞行运动总时长</param>
        /// <param name="_firstBurstAccSpeedTimeScale">炸裂运动加速段占比</param>
        /// <param name="_firstFlyAccSpeedTimeScale">飞行运动加速段占比</param>
        public void setValue(
            float _startTime,
            BurstParticleGroupInfo _groupInfo,
            Vector2 _startPos,
            Vector2 _burstPos,
            Vector2 _endPos,
            float _totalBurstTime,
            float _totalFlyTime,
            float _firstBurstAccSpeedTimeScale,
            float _firstFlyAccSpeedTimeScale,
            float _alphaStartTime,
            float _alphaEndValue)
        {
            _m_bIsStart = false;
            _m_fStartTimeS = _startTime;
            _m_bBurstGroupInfo = _groupInfo;
            _m_vStartPos = _startPos;
            _m_vBurstPos = _burstPos;
            _m_vEndPos = _endPos;
            _m_fTotalBurstTime = _totalBurstTime;
            _m_fTotalFlyTime = _totalFlyTime;
            _m_fBurstFristAccSpeedTimeScale = _firstBurstAccSpeedTimeScale;
            _m_fFlyFristAccSpeedTimeScale = _firstFlyAccSpeedTimeScale;
            _m_alphaStartTime = _alphaStartTime;
            _m_alphaEndValue = _alphaEndValue;
            _m_totalAlphaTime = _totalBurstTime + _totalFlyTime - _alphaStartTime;
            //尝试获取Item
            if (null != _m_bBurstGroupInfo)
            {
                _m_piParticleItem = _m_bBurstGroupInfo._popParticleItem();
                if (null != _m_piParticleItem)
                {
                    _m_rtControlRect = _m_piParticleItem.mono.GetComponent<RectTransform>();
                    if (null != _m_rtControlRect)
                    {
                        //初始化粒子位置
                        _m_rtControlRect.anchoredPosition = new Vector2(_m_vStartPos.x, _m_vStartPos.y);
                    }
                }
            }
        }

        /// <summary>
        /// 粒子运动处理
        /// </summary>
        /// <param name="_processTimeS">经过的时间</param>
        /// <returns>粒子是否还有效，如粒子无效外部需要进行回收</returns>
        public bool tick(float _processTimeS)
        {
            //判断是否开始
            if(!_m_bIsStart)
            {
                //判断时间是否达到需要开始的时间，如未达到不需要做后续处理
                if (_processTimeS < _m_fStartTimeS)
                    return true;

                //此时设置开始，并显示粒子
                _m_bIsStart = true;
                _m_piParticleItem?.showItem();

                //开始炸裂需要执行的事件
                _m_bBurstGroupInfo?._onStartBurst();

                //进入炸裂运动阶段
                _m_bIsBurstPhase = true;
                _m_fPhaseEndTime = _m_fStartTimeS + _m_fTotalBurstTime;
                _m_cBurstXcontroller.resetVariables(_m_vStartPos.x, _m_vBurstPos.x, 0, _m_fTotalBurstTime, _m_fTotalBurstTime * _m_fBurstFristAccSpeedTimeScale);
                _m_cBurstYcontroller.resetVariables(_m_vStartPos.y, _m_vBurstPos.y, 0, _m_fTotalBurstTime, _m_fTotalBurstTime * _m_fBurstFristAccSpeedTimeScale);
            }

            //判断阶段变化
            while (_processTimeS >= _m_fPhaseEndTime)
            {
                //炸裂完毕，进入飞行阶段
                if (_m_bIsBurstPhase)
                {
                    _m_bIsBurstPhase = false;

                    //炸裂完毕需要执行的事件
                    _m_bBurstGroupInfo?._onBurstDone();

                    //重置计算对象
                    _m_cBurstXcontroller.resetVariables(_m_rtControlRect.localPosition.x, _m_vEndPos.x, 0, _m_fTotalFlyTime, _m_fTotalFlyTime * _m_fFlyFristAccSpeedTimeScale);
                    _m_cBurstYcontroller.resetVariables(_m_rtControlRect.localPosition.y, _m_vEndPos.y, 0, _m_fTotalFlyTime, _m_fTotalFlyTime * _m_fFlyFristAccSpeedTimeScale);

                    //阶段结束时间变化
                    _m_fPhaseEndTime += _m_fTotalFlyTime;
                }
                //飞行结束
                else
                {
                    //执行飞行结束事件
                    _m_bBurstGroupInfo?._onFlyDone();

                    //返回false，外围会进行回收
                    return false;
                }
            }

            if (null == _m_rtControlRect)
                return false;

            //运动处理
            _m_rtControlRect.anchoredPosition = new Vector2(_m_cBurstXcontroller.curValue, _m_cBurstYcontroller.curValue);

            //处理改变透明度
            if (_m_alphaStartTime > 0 && null != _m_piParticleItem)
            {
                //alpha变化时间
                float alphaTime = _processTimeS - _m_fStartTimeS - _m_alphaStartTime;
                if (alphaTime > 0)
                {
                    float alpha = Mathf.Lerp(1, _m_alphaEndValue, Mathf.InverseLerp(0, _m_totalAlphaTime, alphaTime));
                    alpha = Mathf.Clamp01(alpha);
                    _m_piParticleItem.setAlpha(alpha);
                }
            }
            
            return true;
        }

        /// <summary>
        /// 重置信息
        /// </summary>
        public void reset()
        {
            _m_bIsStart = false;
            _m_fStartTimeS = 0f;

            //回收粒子对象
            if(null != _m_bBurstGroupInfo)
            {
                _m_bBurstGroupInfo._pushBackCacheItem(_m_piParticleItem);
            }

            _m_bBurstGroupInfo = null;
            _m_fPhaseEndTime = 0f;
            _m_fTotalFlyTime = 0f;
            _m_fFlyFristAccSpeedTimeScale = 0f;
            _m_vStartPos = Vector2.zero;
            _m_vBurstPos = Vector2.zero;
            _m_vEndPos = Vector2.zero;
            _m_alphaStartTime = 0;
            _m_alphaEndValue = 0;
            _m_totalAlphaTime = 0;
            _m_rtControlRect = null;
            _m_bIsBurstPhase = true;
        }
    }

    /// <summary>
    /// 粒子任务缓存池
    /// </summary>
    public class BurstParticleObjCache : _AALUnsafeThreadCacheController<BurstParticleObj, BurstParticleObj>
    {
        private static BurstParticleObjCache _g_instance = new BurstParticleObjCache();
        public static BurstParticleObjCache instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new BurstParticleObjCache();
                return _g_instance;
            }
        }

        public BurstParticleObjCache() : base(16, 128)
        {
            init(new BurstParticleObj());
        }

        protected override BurstParticleObj _createItem(BurstParticleObj _template)
        {
            return new BurstParticleObj();
        }

        //警告信息文字
        protected override string _warningTxt { get { return "BurstParticleObjCache"; } }

        protected override void _discardItem(BurstParticleObj _item)
        {
            _item.reset();
            return;
        }

        protected override void _onInit(BurstParticleObj _template)
        {
        }

        protected override void _resetItem(BurstParticleObj _item)
        {
            _item.reset();
        }
    }
}