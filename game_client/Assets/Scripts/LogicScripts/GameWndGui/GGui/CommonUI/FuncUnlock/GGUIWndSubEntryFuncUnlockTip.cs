using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 功能入口解锁提示附加窗口
    /// </summary>
    public class GGUIWndSubEntryFuncUnlockTip : _ATALBasicUISubWnd<GGUIMonoSubEntryFuncUnlockTip>
    {
        //功能类型
        private ENPFunctionType _m_eType;
        //目标开服天数
        private long _m_lTargetServerStartDay;
        //CD任务
        private ALCommonEnableTaskController _m_cdTask;

        public GGUIWndSubEntryFuncUnlockTip(GGUIMonoSubEntryFuncUnlockTip _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _stopCD();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_type"></param>
        public void setInfo(ENPFunctionType _type)
        {
            if (wnd == null || _type == ENPFunctionType.NONE)
                return;

            _m_eType = _type;
            FuncUnlockInfo funcUnlockInfo = NPPlayer.instance.funcUnlockComp.getFuncUnlockInfo(_m_eType);
            if (funcUnlockInfo == null)
                return;

            //获取目标开服天数
            _m_lTargetServerStartDay = _getTargetStartDay();

            //设置提示文本
            ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(TransKeyConst.funcUnlock_nextUnlock_str, funcUnlockInfo.funcName));
            //先隐藏倒计时
            ALUGUICommon.setGameObjEnable(wnd.goNoCDHideList, false);
            //开始倒计时
            if (_m_lTargetServerStartDay > 0)
                _startCD();
        }

        /// <summary>
        /// 开始CD任务
        /// </summary>
        private void _startCD()
        {
            _stopCD();
            _m_cdTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_refreshCD, 0.2f);
        }

        /// <summary>
        /// 关闭CD任务
        /// </summary>
        private void _stopCD()
        {
            _m_cdTask.setDisable();
        }

        /// <summary>
        /// 刷新倒计时
        /// </summary>
        private void _refreshCD()
        {
            if (wnd == null)
                return;

            //已开服天数
            long curServerStartDay = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.SERVER_START_DAYS);
            if (_m_lTargetServerStartDay <= 0)
            {
                _stopCD();
                ALUGUICommon.setGameObjEnable(wnd.goNoCDHideList, false);
            }
            else
            {
                long remainingDays = _m_lTargetServerStartDay - curServerStartDay;
                if (remainingDays > 0)
                {
                    ALUGUICommon.setGameObjEnable(wnd.goNoCDHideList, true);
                    //距离24点的剩余毫秒数
                    long remainingMs = TimeUtil.getNextAssignTimeRemainMs(FpsAndPingMgr.instance.serverTimeTag, 24, 0);
                    //加上剩余天数的毫秒数
                    remainingMs += ((remainingDays - 1) * 24 * 60 * 60 * 1000);
                    ALUGUICommon.setLabelTxt(wnd.txtCountdown, TimeUtil.millisecondsToTime_hms(remainingMs));
                }
                else
                {
                    _stopCD();
                    ALUGUICommon.setGameObjEnable(wnd.goNoCDHideList, false);
                    GCommon.reloadCustomLoadPrefab();
                    GCommon.triggerTutorial();
                }
            }
        }

        #region 根据条件字符串获取开服天数

        /// <summary>
        /// 获取目标开服天数
        /// </summary>
        /// <returns></returns>
        private long _getTargetStartDay()
        {
            FuncUnlockInfo funcUnlockInfo = NPPlayer.instance.funcUnlockComp.getFuncUnlockInfo(_m_eType);
            if (funcUnlockInfo == null)
                return 0;

            NPSimpleUnlockRef simpleUnlockRefObj = funcUnlockInfo.simpleUnlockRefObj;
            if (simpleUnlockRefObj == null || simpleUnlockRefObj.condition_info == null || simpleUnlockRefObj.condition_info.isEmpty || simpleUnlockRefObj.condition_info.condition == null)
                return 0;

            if (simpleUnlockRefObj.condition_info.condition._m_cConditionObj != null)
            {
                NPPlayerCondition_CS_VARIABLE temp = _getVariableConditionObj(simpleUnlockRefObj.condition_info.condition);
                if (temp != null && temp.countRng != null && temp.playerValueType != null && _isServerStartDaysVariable(temp.playerValueType.variable))
                    return temp.countRng.min;
            }

            if (simpleUnlockRefObj.condition_info.condition._m_lChildConditionList != null)
            {
                for (int i = 0; i < simpleUnlockRefObj.condition_info.condition._m_lChildConditionList.Count; i++)
                {
                    NPPlayerConditionGroupObj childCondition = simpleUnlockRefObj.condition_info.condition._m_lChildConditionList[i];
                    NPPlayerCondition_CS_VARIABLE temp = _getVariableConditionObj(childCondition);
                    if (temp != null && temp.countRng != null && temp.playerValueType != null && _isServerStartDaysVariable(temp.playerValueType.variable))
                        return temp.countRng.min;
                }
            }

            return 0;
        }

        /// <summary>
        /// 获取计算高级公式的条件
        /// </summary>
        /// <param name="_conditionObj"></param>
        /// <returns></returns>
        private NPPlayerCondition_CS_VARIABLE _getVariableConditionObj(NPPlayerConditionGroupObj _conditionObj)
        {
            if (_conditionObj == null || _conditionObj._m_cConditionObj == null || !_conditionObj.hasCondition())
                return null;

            if (_conditionObj._m_cConditionObj.conditionType == ENPPlayerConditionType.CS_VARIABLE)
            {
                NPPlayerCondition_CS_VARIABLE csVariableObj = _conditionObj._m_cConditionObj as NPPlayerCondition_CS_VARIABLE;
                return csVariableObj;
            }

            return null;
        }

        /// <summary>
        /// 判断是否是计算开服天数的高级公式
        /// </summary>
        /// <param name="_variableObj"></param>
        /// <returns></returns>
        private bool _isServerStartDaysVariable(NPPlayerVariableGroupObj _variableObj)
        {
            if (_variableObj == null || !_variableObj.hasVariable())
                return false;

            if (_variableObj._m_vVariableObj != null && _variableObj._m_vVariableObj.variableType == ENPPlayerVariableType.CS_PARAM)
            {
                NPPlayerVariablePlayerParam variablePlayerParm = _variableObj._m_vVariableObj as NPPlayerVariablePlayerParam;
                if (variablePlayerParm != null && variablePlayerParm.paramType == ENPPlayerParam.SERVER_START_DAYS)
                    return true;
            }

            return false;
        }

        #endregion
    }
}