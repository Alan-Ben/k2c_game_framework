using ALPackage;
using UnityEngine;
using UnityEngine.Scripting;

namespace GOE
{
    /// <summary>
    /// 游戏帧率处理控制对象
    /// </summary>
    public class FrameRateController
    {
        private static FrameRateController _g_instance = new FrameRateController();
        public static FrameRateController instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new FrameRateController();

                return _g_instance;
            }
        }

        //操作序列号
        private long _m_lOpSerialize;
        //是否在高帧率模式下
        private bool _m_bIsHighFrame;

        protected FrameRateController()
        {
            _m_lOpSerialize = 1;
            _m_bIsHighFrame = false;
        }

        public bool isHighFrame { get { return _m_bIsHighFrame; } }
        public long opSerialize { get { return _m_lOpSerialize; } }

        public void setHighFrameOpen()
        {
            //如果是高帧率模式不需要额外处理
            if(GameSetting.instance.usingHighFrame)
                return;
            
            //累加操作序号
            _m_lOpSerialize++;

            if(_m_bIsHighFrame)
                return;

            //在Editor下关闭这个的时候，再启动，会导致未响应，操作时要谨慎
            _m_bIsHighFrame = true;
            Game.instance.setTargetFrameRate(true);
#if !UNITY_EDITOR
        //Unity在Editor下不再支持设置这个值，有的人电脑（Randy）开了这个模式，在Editor下会报错导致不能拖拽，先在Editor下不打开
        //https://unity.cn/releases/full/2018/2018.3.3     1103095
        GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
#endif
        }
        public void setHighFrameClose()
        {
            //如果是高帧率模式不需要额外处理
            if(GameSetting.instance.usingHighFrame)
                return;
            
            _m_lOpSerialize++;

            if(!_m_bIsHighFrame)
                return;

            _m_bIsHighFrame = false;
            if(GameSetting.instance.usingHighFrame == false)
            {
                //如果不是高帧率，就恢复成30帧
                Game.instance.setTargetFrameRate(false);
            }
#if !UNITY_EDITOR
           //Unity在Editor下不再支持设置这个值，有的人电脑（Randy）开了这个模式，在Editor下会报错导致不能拖拽，先在Editor下不打开
        //https://unity.cn/releases/full/2018/2018.3.3     1103095
        GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
#endif
        }

        /// <summary>
        /// 开启高帧率模式，并监控对应的Go对象是否停止位移
        /// </summary>
        public void setHighFrameOpenAndMonitGo(Transform _go)
        {
            //如果是高帧率模式不需要额外处理
            if(GameSetting.instance.usingHighFrame)
                return;
            
            //设置开启高帧率
            setHighFrameOpen();

            //开启监控
            MGFrameRateGoMonitorTask task = FrameRateGoMonitorTaskCache.instance.popItem();
            task.setInfo(_go, _m_lOpSerialize);
            //注册任务
            ALMonoTaskMgr.instance.addNextFrameLaterTask(task);
        }

        /// <summary>
        /// 针对某个Go进行监控，当对象停止移动的时候恢复高帧率状态
        /// </summary>
        public class MGFrameRateGoMonitorTask : _IALBaseMonoTask
        {
            //监控的GO对象
            private Transform _m_go;
            //监控对象的位置
            private Vector3 _m_vPrePos;
            private Quaternion _m_vPreRotation;
            //操作序列号
            private long _m_lOpSerialize;

            public MGFrameRateGoMonitorTask()
            {
                _m_go = null;
                _m_vPrePos = Vector3.zero;
                _m_vPreRotation = Quaternion.identity;
                _m_lOpSerialize = 0;
            }

            /// <summary>
            /// 设置信息
            /// </summary>
            /// <param name="_go"></param>
            /// <param name="_opSerialize"></param>
            public void setInfo(Transform _go, long _opSerialize)
            {
                _m_go = _go;
                _m_lOpSerialize = _opSerialize;
                if (null != _m_go)
                {
                    _m_vPrePos = _m_go.position;
                    _m_vPreRotation = _m_go.rotation;
                }
            }

            /// <summary>
            /// 重置信息
            /// </summary>
            public void reset()
            {
                _m_go = null;
                _m_vPrePos = Vector3.zero;
                _m_vPreRotation = Quaternion.identity;
                _m_lOpSerialize = 0;
            }

            /// <summary>
            /// 处理函数
            /// </summary>
            public void deal()
            {
                if(FrameRateController.instance.opSerialize != _m_lOpSerialize)
                {
                    //直接回收任务
                    FrameRateGoMonitorTaskCache.instance.pushBackCacheItem(this);
                    return;
                }

                if(null == _m_go || (_m_go.gameObject != null && !_m_go.gameObject.activeInHierarchy))
                {
                    //停止继续检测
                    FrameRateController.instance.setHighFrameClose();
                    //回收任务
                    FrameRateGoMonitorTaskCache.instance.pushBackCacheItem(this);
                    return;
                }

                //获取新位置，判断位移
                Vector3 newPos = _m_go.position;
                Quaternion newRotation = _m_go.rotation;
                if(Vector3.Distance(_m_vPrePos, newPos) < 0.0001f && Quaternion.Angle(_m_vPreRotation, newRotation) < 0.01f)
                {
                    //停止继续检测
                    FrameRateController.instance.setHighFrameClose();
                    //回收任务
                    FrameRateGoMonitorTaskCache.instance.pushBackCacheItem(this);
                    return;
                }

                _m_vPrePos = newPos;
                _m_vPreRotation = newRotation;

                //放入下一帧later处理
                ALMonoTaskMgr.instance.addNextFrameLaterTask(this);
            }
        }
        public class FrameRateGoMonitorTaskCache : _AALUnsafeThreadCacheController<MGFrameRateGoMonitorTask, MGFrameRateGoMonitorTask>
        {
            private static FrameRateGoMonitorTaskCache _g_instance = new FrameRateGoMonitorTaskCache();
            public static FrameRateGoMonitorTaskCache instance
            {
                get
                {
                    if(null == _g_instance)
                        _g_instance = new FrameRateGoMonitorTaskCache();
                    return _g_instance;
                }
            }

            public FrameRateGoMonitorTaskCache() : base(2, 4)
            {
                init(new MGFrameRateGoMonitorTask());
            }

            protected override MGFrameRateGoMonitorTask _createItem(MGFrameRateGoMonitorTask _template)
            {
                return new MGFrameRateGoMonitorTask();
            }

            //警告信息文字
            protected override string _warningTxt { get { return "FrameRateGoMonitorTaskCache"; } }

            protected override void _discardItem(MGFrameRateGoMonitorTask _item)
            {
                _item.reset();
                return;
            }

            protected override void _onInit(MGFrameRateGoMonitorTask _template)
            {
            }

            protected override void _resetItem(MGFrameRateGoMonitorTask _item)
            {
                _item.reset();
            }
        }
    }
}