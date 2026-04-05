using System;
using ALPackage;
using CommonEnum;
using GC2GS.p007_CommOp;
using GS2GC.p002_InitOp;
using GS2GC.p007_CommOp;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 通用红点组件
    /// </summary>
    public class RedDotComponent : _ANPBasicPlayerComponent
    {
        //需要展示的红点列表
        [NotNull] private Dictionary<ERedDotType, RedDotInfo> _m_lRedDotDic = new Dictionary<ERedDotType, RedDotInfo>();
        //定时任务
        private ALCommonEnableTaskController _m_iTickTask;

        //构造函数
        public RedDotComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        protected static ENPPlayerCompType[] _g_DependComp = { ENPPlayerCompType.GUILD };
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.RED_DOT; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            reqRedDotInit();
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_JOIN_GUILD, _onJoinGuild);//加入联盟
            WinMsg.RegisterMsg(WinMsgType.ON_LEAVE_GUILD, _onLeaveGuild);//退出联盟
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_MEMBER_WEEK_DATA_CHG, _onGuildMemberWeekDataChg);//联盟成员每周数据变更
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("RedDotComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_JOIN_GUILD, _onJoinGuild);//加入联盟
            WinMsg.UnregisterMsg(WinMsgType.ON_LEAVE_GUILD, _onLeaveGuild);//退出联盟
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_MEMBER_WEEK_DATA_CHG, _onGuildMemberWeekDataChg);//联盟成员每周数据变更
            _clear();
        }

        //析构函数
        private void _clear()
        {
            _m_lRedDotDic.Clear();
            _m_iTickTask.setDisable();
        }

        //处理红点显示
        private void _dealRedDot(ERedDotType _redDotType, bool _isAdd)
        {
            // 红点id
            long redTipId = 0;
            // 额外的系统显示条件
            bool sysCanShowCondition = true;

            // 根据红点类型获取对应的红点id
            switch (_redDotType)
            {
                case ERedDotType.NONE:
                    break;
            }

            // 设置红点显示与否
            if (redTipId > 0)
                RedTipMgr.instance.setCountByRefRedTipId(redTipId, _isAdd && sysCanShowCondition ? 1 : 0);
        }

        #region 检查过期时间

        /// <summary>
        /// 检查是否需要开启定时任务
        /// </summary>
        private void _checkNeedStartTickTask()
        {
            _m_iTickTask.setDisable();

            bool needCheck = false;
            foreach (RedDotInfo redDotInfo in _m_lRedDotDic.Values)
            {
                if (redDotInfo != null && redDotInfo.needCheckTimeMs > 0)
                {
                    needCheck = true;
                    break;
                }
            }

            if (needCheck)
                _m_iTickTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tickCheckTime, 1.0f);
        }

        /// <summary>
        /// 定时检查时间
        /// </summary>
        private void _tickCheckTime()
        {
            foreach (RedDotInfo redDotInfo in _m_lRedDotDic.Values)
            {
                redDotInfo?.check();
            }
        }

        #endregion

        #region 消息事件

        /// <summary>
        /// 加入联盟
        /// </summary>
        /// <param name="_objects"></param>
        private void _onJoinGuild(params object[] _objects)
        {
          
        }

        /// <summary>
        /// 退出联盟
        /// </summary>
        /// <param name="_objects"></param>
        private void _onLeaveGuild(params object[] _objects)
        {
            
        }

        /// <summary>
        /// 联盟成员每周数据变更
        /// </summary>
        private void _onGuildMemberWeekDataChg()
        {
           
        }

        #endregion

        #region S2C

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_msg"></param>
        public void retRedDotInit(GS2GC_002_080_RetRedDotInit _msg)
        {
            if (_msg == null)
                return;

            for (int i = 0; i < _msg.getRedDotList().Count; i++)
            {
                RedDotInfo info = new RedDotInfo(_msg.getRedDotList()[i]);
                _m_lRedDotDic[_msg.getRedDotList()[i].getRedDotType()] = info;
            }

            // 遍历所有红点类型，处理红点显示
            for (int i = 0; i < ERedDotTypeComparer.g_iEnumCount; i++)
            {
                ERedDotType type = (ERedDotType) i;
                if (type != ERedDotType.NONE)
                {
                    _dealRedDot(type, _m_lRedDotDic.ContainsKey(type));
                }
            }

            // 检查是否需要开启定时任务
            _checkNeedStartTickTask();
            setInitDone();
        }

        /// <summary>
        /// 新增红点推送
        /// </summary>
        /// <param name="_msg"></param>
        public void pushRedDotChg(GS2GC_007_059_PushRedDotChg _msg)
        {
            if (_msg == null)
                return;

            if(_m_lRedDotDic.ContainsKey(_msg.getRedDotInfo().getRedDotType()))
                _m_lRedDotDic[_msg.getRedDotInfo().getRedDotType()].updateInfo(_msg.getRedDotInfo());
            else
            {
                RedDotInfo info = new RedDotInfo(_msg.getRedDotInfo());
                _m_lRedDotDic[_msg.getRedDotInfo().getRedDotType()] = info;
            }

            // 检查是否需要开启定时任务
            _checkNeedStartTickTask();
            _dealRedDot(_msg.getRedDotInfo().getRedDotType(), true);
        }

        /// <summary>
        /// 移除红点推送
        /// </summary>
        /// <param name="_msg"></param>
        public void pushRedDotRemove(GS2GC_007_060_PushRedDotRemove _msg)
        {
            if (_msg == null)
                return;

            ERedDotType redDotType = _msg.getRedDotType();
            if (_m_lRedDotDic.ContainsKey(redDotType))
                _m_lRedDotDic.Remove(redDotType);

            // 检查是否需要开启定时任务
            _checkNeedStartTickTask();
            _dealRedDot(redDotType, false);
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求红点初始化
        /// </summary>
        public void reqRedDotInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_080_ReqRedDotInit());
        }

        /// <summary>
        /// 红点-清除红点请求
        /// </summary>
        /// <param name="_redDotTypes"></param>
        public void reqClearRedDot(List<ERedDotType> _redDotTypes)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_007_031_ReqClearRedDot(_redDotTypes),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_007_031_RetClearRedDot>(null));
        }

        /// <summary>
        /// 红点-检查红点状态请求
        /// </summary>
        /// <param name="_redDotType"></param>
        public void reqCheckRedDot(ERedDotType _redDotType, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_007_032_ReqCheckRedDot(_redDotType),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_007_032_RetCheckRedDot>((_isSuc, _msg) =>
                {
                    _callback?.Invoke();
                }));
        }

        #endregion
    }
}
