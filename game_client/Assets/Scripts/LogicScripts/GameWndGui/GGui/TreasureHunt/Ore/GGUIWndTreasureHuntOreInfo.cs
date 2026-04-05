using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTreasureHuntOreInfo : _ANPGGUIBasicSubWnd<GGUIMonoTreasureHuntOreInfo>
    {
        private _ITreasureHuntOreInfo _m_iOreInfo;

        private NPGGuiWndTexture _m_wNormalOreIcon;
        private NPGGuiWndTexture _m_wAdvanceOreIcon;
        private GGuiWndSprite _m_wOreQualityBgIcon;
        private CommonUISfxObj _m_sfxObj;//品质特效
        private GGUISubWndQualityShowGo _m_wQualityShowGo;

        public GGUIWndTreasureHuntOreInfo(GGUIMonoTreasureHuntOreInfo _wnd) : base(_wnd)
        {
            initWnd();
        }

        public event Action<_ITreasureHuntOreInfo> onOreClick;//当矿石被点击
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.texNormalIcon != null)
                _m_wNormalOreIcon = new NPGGuiWndTexture(wnd.texNormalIcon);
            if (wnd.texAdvancedIcon != null)
                _m_wAdvanceOreIcon = new NPGGuiWndTexture(wnd.texAdvancedIcon);
            
            if (wnd.qualityBg != null)
                _m_wOreQualityBgIcon = new GGuiWndSprite(wnd.qualityBg);
            
            if (wnd.qualityShowGoMono != null)
                _m_wQualityShowGo = new GGUISubWndQualityShowGo(wnd.qualityShowGoMono);
            
            ALUGUICommon.combineBtnClick(wnd.btnOre, _onClickOre);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnOre, _onClickOre);
            }

            onOreClick = null;
            
            _m_wNormalOreIcon?.discard();
            _m_wNormalOreIcon = null;
            _m_wAdvanceOreIcon?.discard();
            _m_wAdvanceOreIcon = null;
            
            _m_wOreQualityBgIcon?.discard();
            _m_wOreQualityBgIcon = null;
            
            _m_sfxObj?.forceDiscard();
            _m_sfxObj = null;
            
            _m_wQualityShowGo?.discard();
            _m_wQualityShowGo = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wNormalOreIcon?.hideWnd();
            _m_wAdvanceOreIcon?.hideWnd();
            _m_wOreQualityBgIcon?.hideWnd();
            
            _m_sfxObj?.forceDiscard();
            _m_sfxObj = null;
            _m_wQualityShowGo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wNormalOreIcon?.discardTexture();
            _m_wAdvanceOreIcon?.discardTexture();
            _m_wOreQualityBgIcon?.discardTexture();
            
            _m_sfxObj?.forceDiscard();
            _m_sfxObj = null;
            _m_wQualityShowGo?.resetWnd();
        }

        public void setData(_ITreasureHuntOreInfo _oreInfo)
        {
            _m_iOreInfo = _oreInfo;
            
            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if(wnd == null || _m_iOreInfo == null || _m_iOreInfo.oreRefObj == null)
                return;

            TreasureHuntOreRefObj oreRef = _m_iOreInfo.oreRefObj;
            ETreasureHuntOreState oreState = _m_iOreInfo.oreState;
            
            if (_m_wNormalOreIcon != null)
            {
                _m_wNormalOreIcon.showWnd();
                _m_wNormalOreIcon.setTexture(oreRef.icon);
            }
            if (_m_wAdvanceOreIcon != null)
            {
                _m_wAdvanceOreIcon.showWnd();
                _m_wAdvanceOreIcon.setTexture(oreRef.advanced_ore_icon);
            }

            ALUGUICommon.setLabelTxt(wnd.txtOreName, TextTranslate.instance.getLanguage(oreRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtNotGetOreDesc, TextTranslate.instance.getLanguage(oreRef.not_get_desc, oreRef.not_get_desc_args_list));
            ALUGUICommon.setLabelTxt(wnd.txtOreDesc, TextTranslate.instance.getLanguage(oreRef.desc));

            if (string.IsNullOrEmpty(wnd.txtNumKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtNum, _m_iOreInfo.num);
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtNum, TextTranslate.instance.getLanguage(wnd.txtNumKey, _m_iOreInfo.num));
            }
            
            if (string.IsNullOrEmpty(wnd.txtMassKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtMass, TreasureHuntUtil.getOreMassShowStr(_m_iOreInfo.mass));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtMass, TextTranslate.instance.getLanguage(wnd.txtMassKey, TreasureHuntUtil.getOreMassShowStr(_m_iOreInfo.mass)));
            }

            _refreshSfxShow();
            
            wnd.refreshOreState(oreState);
        }

        /// <summary>
        /// 刷新特效显示
        /// </summary>
        private void _refreshSfxShow()
        {
            if(null == wnd || _m_iOreInfo == null || _m_iOreInfo.oreRefObj == null)
                return;
            
            TreasureHuntOreRefObj oreRef = _m_iOreInfo.oreRefObj;

            NPQualityExtRefObj qualityExtRefObj = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((long) oreRef.quality);

            if (qualityExtRefObj != null)
            {
                if (_m_wOreQualityBgIcon != null)
                {
                    _m_wOreQualityBgIcon.showWnd();
                    _m_wOreQualityBgIcon.setTexture(qualityExtRefObj.treasure_hunt_ore_card_bg);
                }                
            }
            else
            {
                _m_wOreQualityBgIcon?.hideWnd();
            }

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
            
            // 显示品质特效
            if (wnd.needQualityItemSfx && null != wnd.qualityItemSfxParent && qualityExtRefObj  != null)
            {
                if (null != _m_sfxObj)
                {
                    _m_sfxObj.forceDiscard();
                    _m_sfxObj = null;
                }

                _m_sfxObj = PlaySfxMgr.instance.playUISfx(qualityExtRefObj.item_sfx_id, wnd.qualityItemSfxParent);
            }
        }
        
        private void _onClickOre(GameObject _go)
        {
            if (wnd == null)
                return;

            _showItemDetail(_go);
        }
        
        /// <summary>
        /// 展示详情
        /// </summary>
        private void _showItemDetail(GameObject _go)
        {
            if (null == wnd || _go == null)
                return;

            if (wnd.isClickShowDetailToolTip)
            {
                QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_TreasureHuntOreDetail(
                    wnd.detailToolTipAssetPath, _m_iOreInfo,
                    _go.GetComponent<RectTransform>(), wnd.detailToolTipInterval));       
            }
            
            onOreClick?.Invoke(_m_iOreInfo);
        }
    }
}