using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 自定义显示玩家icon信息的customMono
    /// </summary>
    public class NPGGUICustomMonoPlayerIconWnd : _ANPGGUIMonoCustomBasicWnd
    {
        [ALHeader("玩家信息")]
        public NPGGUIMonoPlayerIcon monoPlayerIcon;

#if NP_GAME
        //玩家信息窗口
        private NPGGUIWndPlayerIcon _m_selfPlayerIconWnd;
#endif
        protected override void _onCustomUIEnable()
        {
#if NP_GAME
            if(null == monoPlayerIcon)
                return;
            
            if(null == _m_selfPlayerIconWnd)
                _m_selfPlayerIconWnd = new NPGGUIWndPlayerIcon(monoPlayerIcon);
            
            _m_selfPlayerIconWnd.setSelfInfo();
            _m_selfPlayerIconWnd.showWnd();

            NPPlayer.instance.specialItemComp.goldData.onEarningsChg += _onEarningsChg;
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_POWER_CHG, _onHeroPowerChg);
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChanged);
#endif
        }

        protected override void _onCustomUIDisable()
        {
#if NP_GAME
            if (_m_selfPlayerIconWnd != null) 
                _m_selfPlayerIconWnd.discard();
            _m_selfPlayerIconWnd = null;

            NPPlayer.instance.specialItemComp.goldData.onEarningsChg -= _onEarningsChg;
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_POWER_CHG, _onHeroPowerChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChanged);
#endif
        }
#if NP_GAME

        //赚速变化
        private void _onEarningsChg()
        {
            _m_selfPlayerIconWnd?.setSelfInfo();
        }

        //伙伴实力变更
        private void _onHeroPowerChg(params object[] _objs)
        {
            _m_selfPlayerIconWnd?.setSelfInfo();
        }

        private void _onPlayerParamChanged(params object[] _objs)
        {
            if (null == _m_selfPlayerIconWnd)
                return;

            ENPPlayerParam paramType = (ENPPlayerParam)_objs[0];
            if (paramType == ENPPlayerParam.ICON || paramType == ENPPlayerParam.ICON_BGK || paramType == ENPPlayerParam.LEVEL || paramType == ENPPlayerParam.VIP_LVL) 
            {
                _m_selfPlayerIconWnd.setSelfInfo();
            }
        }
#endif
    }
}