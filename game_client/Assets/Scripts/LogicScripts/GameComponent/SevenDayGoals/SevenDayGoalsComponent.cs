using ALPackage;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 七日目标数据组件
    /// </summary>
    public class SevenDayGoalsComponent : _ANPBasicPlayerComponent
    {
        [NotNull] private readonly SevenDayGoalsData _m_data;
        
        
        public SevenDayGoalsComponent(NPPlayerComponentMgr _compMgr) 
            : base(_compMgr)
        {
            _m_data = new SevenDayGoalsData();
        }
        

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.SEVEN_DAY_GOALS; } }
        public override ENPPlayerCompType[] dependCompList { get { return new[] { ENPPlayerCompType.COMMON_ACTIVITY, ENPPlayerCompType.BASIC_INFO }; } }
        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }
        /// <summary>
        /// 七日目标数据
        /// </summary>
        public SevenDayGoalsData data { get { return _m_data; } }
        

        public override void presendInitProtocol()
        {
            dealPreInitFunc(setInitDone);
        }
        protected override void _dealInit()
        {
        }
        protected override void _onInitDone()
        {
            var activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByType(ECommonActivityType.SEVEN_DAY_GOALS);
            if (activityInfo is { isEnable: true })
                _m_data.init(activityInfo);
            
            WinMsg.RegisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_START, _onActivityChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_END, _onActivityChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_UPDATE, _onActivityChg);
        }
        protected override void _onInitFail()
        {
            ALLog.Error("SevenDayGoalsComponent init fail");
        }
        protected override void _discard()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_START, _onActivityChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_END, _onActivityChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_UPDATE, _onActivityChg);
            
            _m_data.discard();
        }

        /// <summary>
        /// 获取是否需要展示红点
        /// </summary>
        /// <param name="_day"></param>
        /// <param name="_tag"></param>
        public bool getNeedShowRedTip(long _day, ESevenDayGoalsRedTipType _tag)
        {
            if (_m_data.redTipDealer == null)
                return false;

            return _m_data.redTipDealer.getRedTipCanShow(_day, _tag);
        }

        /// <summary>
        /// 设置红点已读
        /// </summary>
        /// <param name="_day"></param>
        /// <param name="_tag"></param>
        public void setReadRedTip(long _day, ESevenDayGoalsRedTipType _tag)
        {
            if (_m_data.redTipDealer == null)
                return;

            _m_data.redTipDealer.setIsRead(_day, _tag);
        }


        private void _onActivityChg()
        {
            // 判断现在数据的活动是否还有效
            bool isActivityEnable = _m_data.activityInfo is { isEnable: true };
            // 判断数据是否已经初始化
            bool isDataInit = _m_data.isInit;
            // 如果数据已经初始化并且活动仍然有效，则不需要重新初始化
            if (isDataInit && isActivityEnable)
                return;

            // 如果活动还有效，但是数据没有初始化，就重新初始化
            if (isActivityEnable)
            {
                _ABaseActivityInfo activityInfo = _m_data.activityInfo;
                _m_data.discard();
                _m_data.init(activityInfo);
            }
            // 如果活动无效了，就尝试重新获取活动进行初始化
            else
            {
                _m_data.discard();
                
                _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByType(ECommonActivityType.SEVEN_DAY_GOALS);
                if (activityInfo is { isEnable: true })
                    _m_data.init(activityInfo);
            }
        }
    }
}