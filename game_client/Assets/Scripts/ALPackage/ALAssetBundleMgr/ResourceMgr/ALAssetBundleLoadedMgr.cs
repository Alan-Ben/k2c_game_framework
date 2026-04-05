using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /******************
     * 已经加载的资源中对象的总管理对象
     * 管理对象按照最后访问时间，根据时间区域将所有访问的对象划分到不定，使用Dictionnary对区域整体对象进行访问
     * 根据对象最长有效期进行管理
     **/
    public class ALAssetBundleLoadedMgr
    {
        private static ALAssetBundleLoadedMgr _g_instance;
        public static ALAssetBundleLoadedMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ALAssetBundleLoadedMgr(300, 500, 0.1f);

                return _g_instance;
            }
        }
        public static long getTimeTag() { return ALCommon.getNowTimeMill(); }

        /** 划分区域的时间跨度 */
        private int _m_iTimeRange;
        /** 加载对象在内存无访问的最长存活时间 */
        private int _m_iObjLiveTimeRange;
        /** 检查资源任务的执行周期 */
        private float _m_fCheckCoroutineCycleTime;
        /** 按照时间区域将加载对象划分到不同区域内 */
        private Dictionary<long, HashSet<ALAssetBundleLoadedObj>> _m_dLoadedObjTimerDic;

        /** 之前检查的时间标记位置*/
        private long _m_iPreCheckTimeTag;

        protected ALAssetBundleLoadedMgr(int _timeRangeMS, int _objLiveTimeMS, float _checkCycleTime)
        {
            _m_iTimeRange = _timeRangeMS;
            if (_m_iTimeRange < 100)
                _m_iTimeRange = 100;

            _m_iObjLiveTimeRange = _objLiveTimeMS;
            if (_m_iObjLiveTimeRange < _m_iTimeRange)
                _m_iObjLiveTimeRange = _m_iTimeRange;

            _m_fCheckCoroutineCycleTime = _checkCycleTime;
            if (_m_fCheckCoroutineCycleTime < 0.1f)
                _m_fCheckCoroutineCycleTime = 0.1f;

            _m_dLoadedObjTimerDic = new Dictionary<long, HashSet<ALAssetBundleLoadedObj>>();
            //设置当前时间标记为最后一次检查标记
            _m_iPreCheckTimeTag = getTimeTag();

            //添加检查的周期任务
            ALCoroutineDealerMgr.instance.addCoroutine(new ALAssetBundleResourceCheckCoroutine());
        }

        public float checkCycleTime { get { return _m_fCheckCoroutineCycleTime; } }

        /*********************
         * 刷新资源加载对象在管理对象中的访问位置
         **/
        public void refreshLoadedObj(ALAssetBundleLoadedObj _obj)
        {
            if (null == _obj || null == _obj.obj)
                return;

            //获取当前所在时间
            long preTime = _obj.lastViewTime;

            //刷新访问时间片
            _obj._refreshLastViewTime();

            //获取最新访问时间
            long curTime = _obj.lastViewTime;

            //如果前后的访问时间相同不进行处理
            if (curTime == preTime)
                return;

            if (-1 == preTime)
            {
                //插入新对象
                long curTimeRangeIdx = _getTimeRange(curTime);

                //放入当前时间区域
                _addToTimeRange(curTimeRangeIdx, _obj);
            }
            else
            {
                //分别计算所在时间区域
                long preTimeRangeIdx = _getTimeRange(preTime);
                long curTimeRangeIdx = _getTimeRange(curTime);

                //所在时间区域不变则不进行处理
                if (preTimeRangeIdx == curTimeRangeIdx)
                    return;

                //从原先时间区域取出
                _removeFromTimeRange(preTimeRangeIdx, _obj);

                //放入当前时间区域
                _addToTimeRange(curTimeRangeIdx, _obj);
            }
        }

        /********************
         * 检查加载的资源的生存时间是否有效
         **/
        public void checkLoadedObjLiveTime()
        {
            //获取原先检查的时间区域序号
            long preCheckTimeRangeIdx = _getTimeRange(_m_iPreCheckTimeTag - _m_iObjLiveTimeRange);
            //设置原先检查时间标记为当前标记
            _m_iPreCheckTimeTag = getTimeTag();
            //获取当前时间区域序号
            long curCheckTimeRangeIdx = _getTimeRange(_m_iPreCheckTimeTag - _m_iObjLiveTimeRange);

            //将检测区间内的时间区域都提取出来进行检测
            for (long i = preCheckTimeRangeIdx; i <= curCheckTimeRangeIdx; i++)
            {
                //该时间点无集合则继续下一集合
                if (!_m_dLoadedObjTimerDic.ContainsKey(i))
                    continue;

                //逐个区域序号进行检查
                HashSet<ALAssetBundleLoadedObj> releaseSet = _m_dLoadedObjTimerDic[i];
                //删除节点数据集合
                _m_dLoadedObjTimerDic.Remove(i);

                //逐个对象释放资源
                foreach (ALAssetBundleLoadedObj obj in releaseSet)
                {
                    if (null == obj.obj)
                        continue;

                    if (obj.obj.loadingOpCount > 0)
                    {
                        obj._resetLastViewTime();
                        continue;
                    }

                    obj._release();
                }
            }
        }

        /*****************
         * 从指定的时间区域内将指定对象移除
         **/
        protected void _removeFromTimeRange(long _range, ALAssetBundleLoadedObj _obj)
        {
            if (!_m_dLoadedObjTimerDic.ContainsKey(_range))
                return;

            HashSet<ALAssetBundleLoadedObj> hashset = _m_dLoadedObjTimerDic[_range];

            //从集合移除
            hashset.Remove(_obj);
        }

        /*****************
         * 将对象添加到指定的时间区域内
         **/
        protected void _addToTimeRange(long _range, ALAssetBundleLoadedObj _obj)
        {
            if (null == _obj || null == _obj.obj)
                return;

            if (!_m_dLoadedObjTimerDic.ContainsKey(_range))
            {
                //原先不包含该事件区域集合则创建
                HashSet<ALAssetBundleLoadedObj> objSet = new HashSet<ALAssetBundleLoadedObj>();
                _m_dLoadedObjTimerDic[_range] = objSet;
            }

            HashSet<ALAssetBundleLoadedObj> hashset = _m_dLoadedObjTimerDic[_range];

            //添加到集合中
            hashset.Add(_obj);
        }

        /***************
         * 根据时间获取所在时间片的编号
         **/
        protected long _getTimeRange(long _timeTag)
        {
            long rangeIdx = _timeTag / _m_iTimeRange;

            return rangeIdx;
        }
    }
}
