using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一定时间内值从start 到 end的变化
    /// 支持从大到小和从小到大
    /// </summary>
    public class NPMonoTaskLerpStartEndValueByTime : _IALBaseMonoTask
    {
         /// <summary>
        /// 直接开始任务执行
        /// </summary>
        /// <param name="_mDCheckStopFun"></param>
        /// <param name="_mLSerializeId"></param>
        /// <param name="_mIStartNum"></param>
        /// <param name="_mIEndNum"></param>
        /// <param name="_mFDurationTime"></param>
        /// <param name="_mDNumChg"></param>
        /// <param name="_mDGetOrigSerialize"></param>
        /// <param name="_mDOnNumChgDone"></param>
        public static void startLerpTask(Func<bool> _mDCheckStopFun, long _mLSerializeId, long _mIStartNum, long _mIEndNum, float _mFDurationTime, Action<long> _mDNumChg, Func<long> _mDGetOrigSerialize, Action _mDOnNumChgDone = null)
        {
            NPMonoTaskLerpStartEndValueByTime task = MonoTaskLerpStartEndValueByTimeCache.instance.popItem();
            task.setValue(_mDCheckStopFun, _mLSerializeId, _mIStartNum, _mIEndNum, _mFDurationTime, _mDNumChg, _mDGetOrigSerialize, _mDOnNumChgDone);

            ALMonoTaskMgr.instance.addMonoTask(task);
        }

        private long _m_iStartNum;//开始值
        private long _m_iNowNum;//当前值
        private long _m_iEndNum;//结束值
        private Func<bool> _m_dCheckStopFun;//退出的判断函数
        private Action<long> _m_dNumChg;//数值变化的时候干啥
        private Action _m_dOnNumChgDone;//数值达到最大后干啥
        private float _m_fDurationTime;//持续时间
        private float _m_fStartTime;//开始时间
        private double _m_fSpeed;//变化速度
        private long _m_lSerializeId;
        private Func<long> _m_dGetOrigSerialize;//原来的操作值

        public long serializeId { get { return _m_lSerializeId; } }

        protected NPMonoTaskLerpStartEndValueByTime()
        {
            _m_dCheckStopFun = null;
            _m_lSerializeId = 0;
            _m_iStartNum = 0;
            _m_iNowNum = 0;
            _m_iEndNum = 0;
            _m_dNumChg = null;
            _m_fDurationTime = 0f;
            _m_fSpeed = 0;
            _m_fStartTime = Time.realtimeSinceStartup;
            _m_dOnNumChgDone = null;
            _m_dGetOrigSerialize = null;
        }

        public void setValue(Func<bool> _mDCheckStopFun, long _mLSerializeId, long _mIStartNum, long _mIEndNum, float _mFDurationTime, Action<long> _mDNumChg, Func<long> _mDGetOrigSerialize, Action _mDOnNumChgDone = null)
        {
            _m_dCheckStopFun = _mDCheckStopFun;
            _m_lSerializeId = _mLSerializeId;
            _m_iStartNum = _mIStartNum;
            _m_iNowNum = _mIStartNum;
            _m_iEndNum = _mIEndNum;
            _m_dNumChg = _mDNumChg;
            _m_fDurationTime = _mFDurationTime;
            if (_m_iEndNum > _m_iStartNum)
                _m_fSpeed = (_m_iEndNum - _m_iStartNum) / _m_fDurationTime;
            else
                _m_fSpeed = (_m_iStartNum - _m_iEndNum) / _m_fDurationTime;
            _m_fStartTime = Time.realtimeSinceStartup;
            _m_dOnNumChgDone = _mDOnNumChgDone;
            _m_dGetOrigSerialize = _mDGetOrigSerialize;
        }

        public void deal()
        {
            //判断已经失效，或者序列号对不上了，或者值已经到了就不处理
            if (_m_dCheckStopFun() || _m_dGetOrigSerialize == null || _m_dGetOrigSerialize() != _m_lSerializeId ||
                (_m_iEndNum >= _m_iStartNum && _m_iNowNum >= _m_iEndNum) || (_m_iEndNum < _m_iStartNum && _m_iNowNum <= _m_iEndNum))
            {
                if (_m_dOnNumChgDone != null)
                    _m_dOnNumChgDone();

                //回收任务
                MonoTaskLerpStartEndValueByTimeCache.instance.pushBackCacheItem(this);
                return;
            }

            if(_m_iEndNum >= _m_iStartNum)
            {
                _m_iNowNum = _m_iStartNum + (long)Math.Round(_m_fSpeed * (Time.realtimeSinceStartup - _m_fStartTime));
                if(_m_iNowNum > _m_iEndNum)
                    _m_iNowNum = _m_iEndNum;
            }
            else
            {
                _m_iNowNum = _m_iStartNum - (long)Math.Round(_m_fSpeed * (Time.realtimeSinceStartup - _m_fStartTime));
                if (_m_iNowNum < _m_iEndNum)
                    _m_iNowNum = _m_iEndNum;
            }
            if (_m_dNumChg != null)
                _m_dNumChg(_m_iNowNum);

            //加入下一帧
            ALMonoTaskMgr.instance.addNextFrameTask(this);
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        protected void _reset()
        {
            _m_dCheckStopFun = null;
            _m_lSerializeId = 0;
            _m_iStartNum = 0;
            _m_iNowNum = 0;
            _m_iEndNum = 0;
            _m_dNumChg = null;
            _m_fDurationTime = 0f;
            _m_fSpeed = 0;
            _m_fStartTime = Time.realtimeSinceStartup;
            _m_dOnNumChgDone = null;
            _m_dGetOrigSerialize = null;
        }

        /// <summary>
        /// 任务的缓存池
        /// </summary>
        protected class MonoTaskLerpStartEndValueByTimeCache : _AALUnsafeThreadCacheController<NPMonoTaskLerpStartEndValueByTime, NPMonoTaskLerpStartEndValueByTime>
        {
            private static MonoTaskLerpStartEndValueByTimeCache _g_instance = new MonoTaskLerpStartEndValueByTimeCache();
            public static MonoTaskLerpStartEndValueByTimeCache instance
            {
                get
                {
                    if (null == _g_instance)
                        _g_instance = new MonoTaskLerpStartEndValueByTimeCache();
                    return _g_instance;
                }
            }

            public MonoTaskLerpStartEndValueByTimeCache() : base(5, 10)
            {
                init(new NPMonoTaskLerpStartEndValueByTime());
            }

            protected override NPMonoTaskLerpStartEndValueByTime _createItem(NPMonoTaskLerpStartEndValueByTime _template)
            {
                return new NPMonoTaskLerpStartEndValueByTime();
            }

            //警告信息文字
            protected override string _warningTxt { get { return "NPMonoTaskLerpStartEndValueByTimeCache"; } }

            protected override void _discardItem(NPMonoTaskLerpStartEndValueByTime _item)
            {
                _item._reset();
                return;
            }

            protected override void _onInit(NPMonoTaskLerpStartEndValueByTime _template)
            {
            }

            protected override void _resetItem(NPMonoTaskLerpStartEndValueByTime _item)
            {
                _item._reset();
            }
        }
    }
}