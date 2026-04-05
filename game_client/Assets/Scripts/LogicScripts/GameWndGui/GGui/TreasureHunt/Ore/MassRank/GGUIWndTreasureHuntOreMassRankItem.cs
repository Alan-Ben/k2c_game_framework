using ALPackage;

namespace GOE
{
    public class GGUIWndTreasureHuntOreMassRankItem : _ANPGGUIBasicSubWnd<GGUIMonoTreasureHuntOreMassRankItem>
    {
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;
        private Common.TreasureHuntObj.TreasureHunt_OreRankItem _m_iRankInfo;
        
        public GGUIWndTreasureHuntOreMassRankItem(GGUIMonoTreasureHuntOreMassRankItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化玩家图标子窗口
            if (wnd.monoPlayerIcon != null)
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoPlayerIcon);
        }
        
        protected override void _onDiscard()
        {
            _m_wPlayerIcon?.discard();
            _m_wPlayerIcon = null;
            
            _m_iRankInfo = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wPlayerIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wPlayerIcon?.resetWnd();
        }

        /// <summary>
        /// 设置排行数据
        /// </summary>
        /// <param name="_rankInfo">排行信息</param>
        public void setData(Common.TreasureHuntObj.TreasureHunt_OreRankItem _rankInfo)
        {
            _m_iRankInfo = _rankInfo;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || !isShow)
                return;

            if (_m_iRankInfo == null)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasPlayerInfoShowList,false);
                ALUGUICommon.setGameObjEnable(wnd.noPlayerInfoShowList,true);
                return;
            }
            
            ALUGUICommon.setGameObjEnable(wnd.hasPlayerInfoShowList, true);
            ALUGUICommon.setGameObjEnable(wnd.noPlayerInfoShowList, false);
            
            GCommon.reqPlayerInfo(_m_iRankInfo.getCid(), (_simplePlayerInfo) =>
            {
                if (wnd == null || _simplePlayerInfo == null || _m_wPlayerIcon == null)
                    return;
                
                _m_wPlayerIcon.showWnd();
                _m_wPlayerIcon.setPlayerInfo(_simplePlayerInfo, false);
            });
            
            // 刷新矿石质量显示
            string massValue = TreasureHuntUtil.getOreMassShowStr(_m_iRankInfo.getWeight());
            if (string.IsNullOrEmpty(wnd.txtMassKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtMass, massValue);
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtMass, TextTranslate.instance.getLanguage(wnd.txtMassKey, massValue));
            }
        }
    }
}