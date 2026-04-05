using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 火星基地 - ai智能控制信息窗口
    /// </summary>
    public class GGUIWndMarsIntelligentControlInfo : _ANPGGUIBasicSubWnd<GGUIMonoMarsIntelligentControlInfo>
    {
        private _IMarsIntelligentControlInfo _m_iInfo;
        private EMarsIntelligentControlState _m_ePrevState = EMarsIntelligentControlState.NONE;//上一次的状态
        
        private NPGGuiWndTexture _m_wIcon;
        private ALCommonEnableTaskController _m_TickTask;
        
        public GGUIWndMarsIntelligentControlInfo(GGUIMonoMarsIntelligentControlInfo _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.icon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.icon);
        }

        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;

            _discardTickTask();
            
            _m_iInfo = null;
            _m_ePrevState = EMarsIntelligentControlState.NONE;
        }

        protected override void _onShowWnd()
        {
            refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_MARS_INTELLIGENT_CHG, _onIntelligentChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_MARS_INTELLIGENT_CHG, _onIntelligentChg);
            
            _m_wIcon?.hideWnd();
            
            _discardTickTask();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
            
            _discardTickTask();
        }
        
        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info">智能控制信息</param>
        public void setInfo(_IMarsIntelligentControlInfo _info)
        {
            _m_iInfo = _info;
            _m_ePrevState = _info?.getState() ?? EMarsIntelligentControlState.NONE;
            refreshWnd();
        }
        
        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !isShow || _m_iInfo == null || _m_iInfo.refObj == null)
                return;

            // 设置图标
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(_m_iInfo.refObj.icon);
            }

            // 设置名称
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_iInfo.refObj.transName);
            
            // 设置描述
            if (string.IsNullOrEmpty(wnd.txtEffectDescKey))
                ALUGUICommon.setLabelTxt(wnd.txtEffectDesc, _m_iInfo.refObj.transDesc);
            else
                ALUGUICommon.setLabelTxt(wnd.txtEffectDesc, TextTranslate.instance.getLanguage(wnd.txtEffectDescKey, _m_iInfo.refObj.transDesc));

            // 设置冷却时间显示
            if (string.IsNullOrEmpty(wnd.txtCoolingTimeKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtCoolingTime, TimeUtil.millisecondsToTime_Two(_m_iInfo.refObj.coolingTimeMs));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtCoolingTime, TextTranslate.instance.getLanguage(
                    wnd.txtCoolingTimeKey, TimeUtil.millisecondsToTime_Two(_m_iInfo.refObj.coolingTimeMs)));    
            }
            
            // 刷新生效时间倒计时显示
            _refreshEffectiveTimeCountDown();
            // 刷新冷却时间倒计时显示
            _refreshCoolingTimeCountDown();
            
            // 刷新状态显示
            _refreshStateShow();
            
            _initTickTask();
        }
        
        /// <summary>
        /// 刷新生效中时间显示
        /// </summary>
        private void _refreshEffectiveTimeCountDown()
        {
            // 添加空值检查
            if (_m_iInfo == null || wnd == null)
                return;
            
            long effectiveTimeCountDown = _m_iInfo?.buffInfo?.curLeftTimeMS ?? 0;
            // 使用hh:mm:ss格式显示生效中时间
            if (string.IsNullOrEmpty(wnd.txtEffectiveTimeCountDownKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtEffectiveTimeCountDown, TimeUtil.millisecondsToTime_hms(effectiveTimeCountDown));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtEffectiveTimeCountDown, 
                    TextTranslate.instance.getLanguage(wnd.txtEffectiveTimeCountDownKey, TimeUtil.millisecondsToTime_hms(effectiveTimeCountDown)));       
            }
        }
        
        /// <summary>
        /// 刷新冷却倒计时显示
        /// </summary>
        private void _refreshCoolingTimeCountDown()
        {
            // 添加空值检查
            if (_m_iInfo == null || wnd == null)
                return;
            
            long coolingTimeCountDown = _m_iInfo.coolingLeftTimeMs;
            // 使用hh:mm:ss格式显示冷却时间
            if (string.IsNullOrEmpty(wnd.txtCoolingTimeCountDownKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtCoolingTimeCountDown, TimeUtil.millisecondsToTime_hms(coolingTimeCountDown));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtCoolingTimeCountDown, 
                    TextTranslate.instance.getLanguage(wnd.txtCoolingTimeCountDownKey, TimeUtil.millisecondsToTime_hms(coolingTimeCountDown)));
            }
        }

        private void _refreshStateShow()
        {
            // 添加空值检查
            if (_m_iInfo == null || wnd == null)
                return;
            
            // 设置状态显示
            if (wnd.stateShow != null)
            {
                NPCommonEnumStatMutexShowInfo<EMarsIntelligentControlState>.setStat(wnd.stateShow, _m_iInfo.getState());
            }
        }
        
        private void _refreshStateShow(EMarsIntelligentControlState _state)
        {
            if (wnd != null && wnd.stateShow != null)
            {
                NPCommonEnumStatMutexShowInfo<EMarsIntelligentControlState>.setStat(wnd.stateShow, _state);
            }
        }
        
        #region 循环任务

        /// <summary>
        /// 
        /// </summary>
        private void _initTickTask()
        {
            _discardTickTask();
            
            _m_TickTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(() =>
            {
                if(_m_iInfo == null)
                    return;

                EMarsIntelligentControlState state = _m_iInfo.getState();
                if (_m_ePrevState != state)
                {
                    _m_ePrevState = state;
                    _refreshStateShow();
                }
                
                if(state == EMarsIntelligentControlState.COOLING_DOWN)
                    _refreshCoolingTimeCountDown();
                
                if(state == EMarsIntelligentControlState.EFFECTIVE)
                    _refreshEffectiveTimeCountDown();
                
            }, 1f);
        }

        /// <summary>
        /// 
        /// </summary>
        private void _discardTickTask()
        {
            _m_TickTask.setDisable();
        }

        #endregion

        #region 事件监听

        /// <summary>
        /// 智能控制信息变化
        /// </summary>
        /// <param name="_objs"></param>
        private void _onIntelligentChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is MarsIntelligentControlInfo intelligentControlInfo) || _m_iInfo == null)
                return;

            if (intelligentControlInfo.id == _m_iInfo.id)
            {
                setInfo(intelligentControlInfo);
            }
        }

        #endregion
    }
}