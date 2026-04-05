using System.Collections.Generic;
using ALPackage;
using Common;
using GS2GC.p002_InitOp;
using GS2GC.p007_CommOp;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 跑马灯管理器
    /// </summary>
    public class MarqueeComponent : _ANPBasicPlayerComponent
    {
        //跑马灯记录数据
        [NotNull] private MarqueeRemarkInfo _m_marqueeRemarkInfo;
        //跑马灯队列信息
        [NotNull] private Dictionary<int, MarqueeQueueInfo> _m_dMarqueeQueueInfo;
        //跑马灯记录展示信息
        [NotNull] private MarqueeRecordShowInfo _m_recordShowInfo;

        //构造函数
        public MarqueeComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_marqueeRemarkInfo = new MarqueeRemarkInfo();
            _m_dMarqueeQueueInfo = new Dictionary<int, MarqueeQueueInfo>();
            _m_recordShowInfo = new MarqueeRecordShowInfo();
        }

        /// <summary>
        /// 跑马灯记录展示信息
        /// </summary>
        public MarqueeRecordShowInfo recordShowInfo { get { return _m_recordShowInfo; } }

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.MARQUEE; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return false; } }
        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
        }

        protected override void _dealInit()
        {
            //先获取记录已看的实例id，再请求初始化
            _m_marqueeRemarkInfo.sendRequest(reqMarqueeInit);
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("MarqueeComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            _clear();
        }

        //析构函数
        private void _clear()
        {
            _m_dMarqueeQueueInfo.Clear();
            _m_recordShowInfo.clear();
        }

        /// <summary>
        /// 检查设置跑马灯已读并移除数据
        /// </summary>
        /// <param name="_showPosId"></param>
        /// <param name="_instanceId"></param>
        public void checkRemoveMarquee(int _showPosId, long _instanceId)
        {
            if (_m_dMarqueeQueueInfo.TryGetValue(_showPosId, out MarqueeQueueInfo _queueInfo))
            {
                if (_queueInfo == null)
                    return;

                if (!_queueInfo.checkIsValid(_instanceId))
                {
                    //移除数据
                    _queueInfo.removeMarquee(_instanceId);
                }
            }
        }

        /// <summary>
        /// 设置跑马灯已读并强制移除
        /// </summary>
        /// <param name="_showPosId"></param>
        /// <param name="_instanceId"></param>
        public void forceRemoveMarquee(int _showPosId, long _instanceId)
        {
            if (_m_dMarqueeQueueInfo.TryGetValue(_showPosId, out MarqueeQueueInfo _queueInfo))
            {
                if (_queueInfo == null)
                    return;

                //移除数据
                _queueInfo.removeMarquee(_instanceId);
            }
        }

        /// <summary>
        /// 移除指定展示位所有跑马灯
        /// </summary>
        /// <param name="_showPosId"></param>
        public void forceRemoveAllMarquee(int _showPosId)
        {
            if (_m_dMarqueeQueueInfo.TryGetValue(_showPosId, out MarqueeQueueInfo _queueInfo))
            {
                if (_queueInfo == null || _queueInfo.getMarqueeCount() == 0)
                    return;

                //移除数据
                _queueInfo.removeAllMarquee();
            }
        }

        /// <summary>
        /// 获取指定展示位跑马灯数量
        /// </summary>
        /// <param name="_showPosId"></param>
        /// <returns></returns>
        public long getMarqueeCount(int _showPosId)
        {
            if (_m_dMarqueeQueueInfo.TryGetValue(_showPosId, out MarqueeQueueInfo _queueInfo))
            {
                if (_queueInfo == null)
                    return 0;

                return _queueInfo.getMarqueeCount();
            }

            return 0;
        }

        /// <summary>
        /// 根据位置获取跑马灯信息
        /// </summary>
        /// <param name="_showPosId"></param>
        /// <returns></returns>
        public MarqueeInfo getMarqueeInfo(int _showPosId)
        {
            if (_m_dMarqueeQueueInfo.TryGetValue(_showPosId, out MarqueeQueueInfo _queueInfo))
                return _queueInfo?.getNextMarquee();
            else
                return null;
        }

        /// <summary>
        /// 根据记录数据检查是否有效可展示
        /// </summary>
        /// <param name="_showPosId"></param>
        /// <param name="_priorityId"></param>
        /// <param name="_dbId"></param>
        /// <returns></returns>
        public bool checkIsValidByRecord(int _showPosId, long _priorityId, long _dbId)
        {
            return _m_marqueeRemarkInfo.checkIsValid(_showPosId, _priorityId, _dbId);
        }

        /// <summary>
        /// 新增跑马灯
        /// </summary>
        /// <param name="_showPosId"></param>
        /// <param name="_info"></param>
        private void _addMarquee(int _showPosId, Common_MarqueeInfo _info)
        {
            if (_showPosId <= 0)
                return;

            if (_m_dMarqueeQueueInfo.TryGetValue(_showPosId, out MarqueeQueueInfo _queueInfo))
                _queueInfo?.addMarquee(_showPosId, _info);
            else
            {
                MarqueeQueueInfo queueInfo = new MarqueeQueueInfo(_showPosId, _info, _m_marqueeRemarkInfo);
                _m_dMarqueeQueueInfo[_showPosId] = queueInfo;
            }
        }

        /// <summary>
        /// 后台命令手动操作移除跑马灯
        /// </summary>
        /// <param name="_instanceId"></param>
        private void _delMarqueeWithNoRecord(long _instanceId)
        {
            foreach (KeyValuePair<int, MarqueeQueueInfo> marqueeQueueInfo in _m_dMarqueeQueueInfo)
            {
                MarqueeQueueInfo queueInfo = marqueeQueueInfo.Value;
                if (queueInfo == null)
                    continue;

                if(queueInfo.removeMarqueeWithNoRecord(_instanceId))
                    return;
            }
        }

        #region S2C

        /// <summary>
        /// 初始化跑马灯
        /// </summary>
        /// <param name="_msg"></param>
        public void retMarqueeInit(GS2GC_002_033_RetMarqueeInit _msg)
        {
            if (_msg == null)
                return;

            _m_dMarqueeQueueInfo.Clear();
            if (_msg.getMarqueeList() != null)
            {
                for (int i = 0; i < _msg.getMarqueeList().Count; i++)
                {
                    Common_MarqueeShowPosInfo temp = _msg.getMarqueeList()[i];
                    if(temp == null)
                        continue;

                    if(temp.getShowPosId() > 0)
                        _m_dMarqueeQueueInfo[temp.getShowPosId()] = new MarqueeQueueInfo(temp, _m_marqueeRemarkInfo);
                }
            }

            setInitDone();
        }

        /// <summary>
        /// 新增跑马灯
        /// </summary>
        public void onMarqueeAdd(GS2GC_007_056_OnMarqueeAdd _msg)
        {
            if (_msg == null)
                return;

            _addMarquee(_msg.getShowPosId(), _msg.getMarqueeInfo());

            WinMsg.SendMsg(WinMsgType.ON_ADD_MARQUEE, _msg.getShowPosId());
        }

        /// <summary>
        /// 跑马灯删除推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onMarqueeDel(GS2GC_007_058_OnMarqueeDel _msg)
        {
            if (_msg == null)
                return;

            _delMarqueeWithNoRecord(_msg.getMarqueeDbid());

            WinMsg.SendMsg(WinMsgType.ON_DEL_MARQUEE, _msg.getMarqueeDbid());
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求初始化跑马灯
        /// </summary>
        public void reqMarqueeInit()
        {
            List<Common_MarqueeShowPosReadInfo> marqueeReadList = _m_marqueeRemarkInfo.getInitRecordDataList();
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_033_ReqMarqueeInit(marqueeReadList));
        }

        #endregion
    }
}
