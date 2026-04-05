using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星科技信息窗口
    /// </summary>
    public class GGUIWndMarsTechnologyInfo : _ANPGGUIBasicSubWnd<GGUIMonoMarsTechnologyInfo>
    {
        private long _m_lTechnologyId;
        
        private MarsTechnologyInfo _m_iTechnologyInfo;
        private EMarsTechnologyState _m_eTechnologyState;
        
        private NPGGuiWndTexture _m_wIcon;


        public GGUIWndMarsTechnologyInfo(GGUIMonoMarsTechnologyInfo _wnd) : base(_wnd)
        {
            initWnd();
        }

        public long technologyId { get { return _m_lTechnologyId; } }
        public MarsTechnologyInfo technologyInfo { get { return _m_iTechnologyInfo; } }
        public EMarsTechnologyState technologyState { get { return _m_eTechnologyState; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            // 构建图标显示
            if (wnd.monoIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.monoIcon);
        }

        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;
            
            _m_lTechnologyId = 0;
            _m_iTechnologyInfo = null;
        }

        protected override void _onShowWnd()
        {
            refreshWnd();
            
            // 注册科技变化消息
            WinMsg.RegisterMsg(WinMsgType.ON_MARS_TECHNOLOGY_CHG, _onTechnologyChg);
        }

        protected override void _onHideWnd()
        {
            // 解除科技变化消息注册
            WinMsg.UnregisterMsg(WinMsgType.ON_MARS_TECHNOLOGY_CHG, _onTechnologyChg);
            
            _m_wIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
        }

        /// <summary>
        /// 通过科技ID刷新窗口
        /// </summary>
        /// <param name="_technologyId">科技ID</param>
        public void setData(long _technologyId)
        {
            _m_lTechnologyId = _technologyId;
            _m_iTechnologyInfo = NPPlayer.instance?.marsComp?.technologySubComponent.getTechnologyInfoById(_m_lTechnologyId);
            if (_m_iTechnologyInfo == null)
                _m_iTechnologyInfo = new MarsTechnologyInfo(_m_lTechnologyId);
            
            _updateTechnologyState();
            
            refreshWnd();
        }

        /// <summary>
        /// 通过科技配表对象刷新窗口
        /// </summary>
        /// <param name="_technologyRefObj">科技配表对象</param>
        public void setData(MarsTechnologyRefObj _technologyRefObj)
        {
            _m_lTechnologyId = _technologyRefObj?.id ?? 0;
            _m_iTechnologyInfo = NPPlayer.instance?.marsComp?.technologySubComponent?.getTechnologyInfoById(_m_lTechnologyId);
            if(_m_iTechnologyInfo == null)
                _m_iTechnologyInfo = new MarsTechnologyInfo(_technologyRefObj);
            
            _updateTechnologyState();
            
            refreshWnd();
        }
        
        public void setData(MarsTechnologyInfo _technologyInfo)
        {
            _m_iTechnologyInfo = _technologyInfo;
            _m_lTechnologyId = _m_iTechnologyInfo?.technologyId ?? 0;
            _updateTechnologyState();
            
            refreshWnd();
        }

        /// <summary>
        /// 更新科技状态
        /// </summary>
        private void _updateTechnologyState()
        {
            _m_eTechnologyState = MarsUtil.getTechnologyState(_m_iTechnologyInfo);
        }
        
        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !isShow || _m_iTechnologyInfo == null)
                return;

            _refreshRefShow();
            _refreshInfoShow();
        }

        /// <summary>
        /// 刷新配表信息相关显示(只和配表数据有关的信息)
        /// </summary>
        private void _refreshRefShow()
        {
            if(wnd == null || _m_iTechnologyInfo == null)
                return;

            // 刷新图标
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(_m_iTechnologyInfo.technologyRefObj?.icon);
            }
            
            // 刷新名称
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_iTechnologyInfo.technologyRefObj?.transName);
            ALUGUICommon.setLabelTxt(wnd.txtDesc, _m_iTechnologyInfo.technologyRefObj?.transDesc);
            
        }

        /// <summary>
        /// 刷新具体信息显示(会随具体数据变化的信息)
        /// </summary>
        private void _refreshInfoShow()
        {
            if(wnd == null || _m_iTechnologyInfo == null || _m_iTechnologyInfo.technologyRefObj == null)
                return;
            
            int curLvl = _m_iTechnologyInfo.lvl;
            ALUGUICommon.setLabelTxt(wnd.txtLvl, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, curLvl, _m_iTechnologyInfo.technologyRefObj.max_level));

            // 刷新状态显示
            NPCommonEnumStatMutexShowInfo<EMarsTechnologyState>.setStat(wnd.stateShowInfoList, _m_eTechnologyState);
            
            bool canUpgrade = _m_iTechnologyInfo.checkCanUpgrade();
            ALUGUICommon.setGameObjEnable(wnd.canUpgradeShowList, canUpgrade);
        }

        /// <summary>
        /// 科技变化消息回调
        /// </summary>
        /// <param name="_objs">参数：_objs[0] 为 MarsTechnologyInfo</param>
        private void _onTechnologyChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is MarsTechnologyInfo technologyInfo))
                return;
            
            if(technologyInfo.technologyId == _m_lTechnologyId)
                _m_iTechnologyInfo = technologyInfo;
            
            // 因为其他科技变化可能会影响当前科技状态，所以都要更新状态
            _updateTechnologyState();
            
            _refreshInfoShow();
        }
    }
}
