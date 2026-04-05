using ALPackage;

namespace GOE
{
    /// <summary>
    /// 伙伴星辉等级列表item
    /// </summary>
    public class GGUIWndConsortHaloLvlContainerItem : _ATNPGGUIWndSingleChoiceItem<GGUIMonoConsortHaloLvlContainerItem, GGUIWndConsortHaloLvlContainerItem>
    {
        private long _m_lConsortId;//所属妃子id
        private ConsortHaloLvlRefObj _m_rHaloLvlRefObj;

        private NPGGuiWndTexture _m_icon;
        
        public GGUIWndConsortHaloLvlContainerItem(GGUIMonoConsortHaloLvlContainerItem _mono) : base(_mono)
        {
        }
        
        public long consortId { get { return _m_lConsortId; } }
        public ConsortHaloLvlRefObj haloLvlRefObj { get { return _m_rHaloLvlRefObj; } }

        protected override void _onWndInitDoneEx()
        {
            if(wnd == null)
                return;

            if (wnd.icon != null)
                _m_icon = new NPGGuiWndTexture(wnd.icon);
        }
        
        protected override void _onDiscardEx()
        {
            _m_icon?.discard();
            _m_icon = null;
        }
        
        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
            _m_icon?.hideWnd();
        }

        protected override void _onResetEx()
        {
            _m_icon?.discardTexture();
        }

        public void setData(long _consortId, ConsortHaloLvlRefObj _consortHaloRefObj)
        {
            _m_lConsortId = _consortId;
            _m_rHaloLvlRefObj = _consortHaloRefObj;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(_m_rHaloLvlRefObj == null || wnd == null)
                return;

            if (wnd.txtLevelList != null)
            {
                string levelStr = string.IsNullOrEmpty(wnd.txtLevelKey)
                    ? TextTranslate.instance.getLanguage(TransKeyConst.common_level2_num, _m_rHaloLvlRefObj.level)
                    : TextTranslate.instance.getLanguage(wnd.txtLevelKey, _m_rHaloLvlRefObj.level);
                
                foreach (var txtLevel in wnd.txtLevelList)
                {
                    ALUGUICommon.setLabelTxt(txtLevel, levelStr);
                }
            }

            GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_lConsortId);
            if (consortInfo == null || consortInfo.haloInfo == null || !consortInfo.haloInfo.isUnlock)
            {
                ALUGUICommon.setGameObjEnable(wnd.haloUnlockShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.haloLockShowList, true);
                GGameCommonInfo.grayImage(wnd.lockGrayList);
                
                ALUGUICommon.setGameObjEnable(wnd.goLevelReachShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goLevelReachHideList, false);
                
                if (_m_icon != null)
                {
                    _m_icon.showWnd();
                    _m_icon.setTexture(_m_rHaloLvlRefObj.unreach_icon);
                }
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.haloUnlockShowList, true);
                ALUGUICommon.setGameObjEnable(wnd.haloLockShowList, false);
                GGameCommonInfo.disgrayImage(wnd.lockGrayList);

                bool levelReach = consortInfo.haloInfo.level >= _m_rHaloLvlRefObj.level;
                
                ALUGUICommon.setGameObjEnable(wnd.goLevelReachShowList, levelReach);
                ALUGUICommon.setGameObjEnable(wnd.goLevelReachHideList, levelReach);
                
                if (_m_icon != null)
                {
                    NPGTextureIndex iconIndex = levelReach ? _m_rHaloLvlRefObj.reach_icon : _m_rHaloLvlRefObj.unreach_icon;
                    _m_icon.showWnd();
                    _m_icon.setTexture(iconIndex);
                }
            }
        }
    }
}