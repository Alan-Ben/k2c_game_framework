using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIWndCommonToolTip_TreasureHuntOreDetail : _ATNPGGUIWndCommonItemToolTip<NPGGUIMonoCommonToolTip_TreasureHuntOreDetail>
    {
        private NPGGuiWndTexture _m_wNormalOreIcon;
        private NPGGuiWndTexture _m_wAdvanceOreIcon;
        private GGuiWndSprite _m_wQualityBgIcon;
        private CommonUISfxObj _m_sfxObj;
        private GGUISubWndQualityShowGo _m_wQualityShowGo;

        public NPGGUIWndCommonToolTip_TreasureHuntOreDetail(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            
            if(wnd == null)
                return;

            if (wnd.imgNormalOreIcon != null)
                _m_wNormalOreIcon = new NPGGuiWndTexture(wnd.imgNormalOreIcon);
            if (wnd.imgAdvancedOreIcon != null)
                _m_wAdvanceOreIcon = new NPGGuiWndTexture(wnd.imgAdvancedOreIcon);
            
            if (wnd.qualityBg != null)
                _m_wQualityBgIcon = new GGuiWndSprite(wnd.qualityBg);
            
            if (wnd.qualityShowGoMono != null)
                _m_wQualityShowGo = new GGUISubWndQualityShowGo(wnd.qualityShowGoMono);
        }
        
        protected override void _onDiscard()
        {
            base._onDiscard();
            
            _m_wNormalOreIcon?.discard();
            _m_wNormalOreIcon = null;
            _m_wAdvanceOreIcon?.discard();
            _m_wAdvanceOreIcon = null;
            
            _m_wQualityBgIcon?.discard();
            _m_wQualityBgIcon = null;
            _m_sfxObj?.forceDiscard();
            _m_sfxObj = null;
            _m_wQualityShowGo?.discard();
            _m_wQualityShowGo = null;
        }
        
        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            
            _m_wNormalOreIcon?.hideWnd();
            _m_wAdvanceOreIcon?.hideWnd();
            
            _m_wQualityBgIcon?.hideWnd();
            _m_sfxObj?.forceDiscard();
            _m_sfxObj = null;
            _m_wQualityShowGo?.hideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
            
            _m_wNormalOreIcon?.discardTexture();
            _m_wAdvanceOreIcon?.discardTexture();
            
            _m_wQualityBgIcon?.discardTexture();
            _m_sfxObj?.forceDiscard();
            _m_sfxObj = null;
            _m_wQualityShowGo?.resetWnd();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(_ITreasureHuntOreInfo _oreInfo, RectTransform _targetTransRoot, Vector2 _interval)
        {
            if(null == wnd || _oreInfo == null || _oreInfo.oreRefObj == null)
                return;

            TreasureHuntOreRefObj oreRefObj = _oreInfo.oreRefObj;
            ETreasureHuntOreState oreState = _oreInfo.oreState;

            if (_m_wNormalOreIcon != null)
            {
                _m_wNormalOreIcon.showWnd();
                _m_wNormalOreIcon.setTexture(oreRefObj.icon);
            }

            if (_m_wAdvanceOreIcon != null)
            {
                _m_wAdvanceOreIcon.showWnd();
                _m_wAdvanceOreIcon.setTexture(oreRefObj.advanced_ore_icon);
            }
            
            ALUGUICommon.setLabelTxt(wnd.txtOreName, TextTranslate.instance.getLanguage(oreRefObj.name));
            
            ALUGUICommon.setLabelTxt(wnd.txtNotGetOreDesc, TextTranslate.instance.getLanguage(oreRefObj.not_get_desc, oreRefObj.not_get_desc_args_list));
            ALUGUICommon.setLabelTxt(wnd.txtOreDesc, TextTranslate.instance.getLanguage(oreRefObj.desc));

            string numKey = string.IsNullOrEmpty(wnd.txtNumKey) ? TransKeyConst.common_value : wnd.txtNumKey;
            ALUGUICommon.setLabelTxt(wnd.txtNum, TextTranslate.instance.getLanguage(numKey, _oreInfo.num));
            
            wnd.refreshOreState(oreState);
            
            // 刷新品质显示
            _refreshQualityShow(oreRefObj);
            
            //设置位置
            setPos(_targetTransRoot, _interval.x, _interval.y);
        }
        
        /// <summary>
        /// 刷新品质显示
        /// </summary>
        private void _refreshQualityShow(TreasureHuntOreRefObj _oreRefObj)
        {
            if (wnd == null || _oreRefObj == null)
                return;
            
            NPQualityExtRefObj qualityExtRefObj = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((long)_oreRefObj.quality);
            
            // 刷新品质背景
            if (qualityExtRefObj != null)
            {
                if (_m_wQualityBgIcon != null)
                {
                    _m_wQualityBgIcon.showWnd();
                    _m_wQualityBgIcon.setTexture(qualityExtRefObj.treasure_hunt_ore_card_bg);
                }
            }
            else
            {
                _m_wQualityBgIcon?.hideWnd();
            }
            
            // 刷新品质GO显示
            if (qualityExtRefObj != null)
            {
                if (_m_wQualityShowGo != null)
                {
                    _m_wQualityShowGo.showWnd();
                    _m_wQualityShowGo.setData(qualityExtRefObj);
                }
            }
            else
            {
                _m_wQualityShowGo?.hideWnd();
            }
            
            // 刷新品质特效
            if (wnd.needQualityItemSfx && wnd.qualityItemSfxParent != null && qualityExtRefObj != null)
            {
                if (_m_sfxObj != null)
                {
                    _m_sfxObj.forceDiscard();
                    _m_sfxObj = null;
                }
                
                _m_sfxObj = PlaySfxMgr.instance.playUISfx(qualityExtRefObj.item_sfx_id, wnd.qualityItemSfxParent);
            }
        }
        
        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(NPGNodeCommonToolTip_TreasureHuntOreDetail));
        }
    }
}