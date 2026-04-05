using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTreasureHuntTreasureInfo : _ANPGGUIBasicSubWnd<GGUIMonoTreasureHuntTreasureInfo>
    {
        private _ITreasureHuntTreasureInfo _m_iTreasureInfo;
        
        private NPGGuiWndTexture _m_wTreasureIcon;
        private GGuiWndSprite _m_wTreasureQualityBgIcon;
        private CommonUISfxObj _m_sfxObj;//品质特效
        private GGUISubWndQualityShowGo _m_wQualityShowGo;
        
        public GGUIWndTreasureHuntTreasureInfo(GGUIMonoTreasureHuntTreasureInfo _wnd) : base(_wnd)
        {
            initWnd();
        }

        public event Action<_ITreasureHuntTreasureInfo> onTreasureClick;//当奇物被点击
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.texIcon != null)
                _m_wTreasureIcon = new NPGGuiWndTexture(wnd.texIcon);
            
            if (wnd.qualityBg != null)
                _m_wTreasureQualityBgIcon = new GGuiWndSprite(wnd.qualityBg);
            
            if (wnd.qualityShowGoMono != null)
                _m_wQualityShowGo = new GGUISubWndQualityShowGo(wnd.qualityShowGoMono);
            
            ALUGUICommon.combineBtnClick(wnd.btnTreasure, _onClickTreasure);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnTreasure, _onClickTreasure);
            }

            onTreasureClick = null;
            
            _m_wTreasureIcon?.discard();
            _m_wTreasureIcon = null;
            
            _m_wTreasureQualityBgIcon?.discard();
            _m_wTreasureQualityBgIcon = null;
            
            _m_sfxObj?.forceDiscard();
            _m_sfxObj = null;
            
            _m_wQualityShowGo?.discard();
            _m_wQualityShowGo = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_TREASURE_OUTPUT_CHG, _onTreasureOutputChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_TREASURE_OUTPUT_CHG, _onTreasureOutputChg);
            
            _m_wTreasureIcon?.hideWnd();
            _m_wTreasureQualityBgIcon?.hideWnd();
            
            _m_sfxObj?.forceDiscard();
            _m_sfxObj = null;
            _m_wQualityShowGo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wTreasureIcon?.discardTexture();
            _m_wTreasureQualityBgIcon?.discardTexture();
            
            _m_sfxObj?.forceDiscard();
            _m_sfxObj = null;
            _m_wQualityShowGo?.resetWnd();
        }
        
        public void setData(_ITreasureHuntTreasureInfo _treasureInfo)
        {
            _m_iTreasureInfo = _treasureInfo;
            
            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if(wnd == null || _m_iTreasureInfo == null || _m_iTreasureInfo.treasureRefObj == null)
                return;

            TreasureHuntTreasureRefObj treasureRefObj = _m_iTreasureInfo.treasureRefObj;
            ETreasureHuntTreasureState treasureState = _m_iTreasureInfo.treasureState;
            
            if (_m_wTreasureIcon != null)
            {
                _m_wTreasureIcon.showWnd();
                _m_wTreasureIcon.setTexture(treasureRefObj.icon);
            }

            ALUGUICommon.setLabelTxt(wnd.txtTreasureName, TextTranslate.instance.getLanguage(treasureRefObj.name));
            ALUGUICommon.setLabelTxt(wnd.txtNotGetTreasureDesc, TextTranslate.instance.getLanguage(treasureRefObj.not_get_desc, treasureRefObj.not_get_desc_args_list));
            ALUGUICommon.setLabelTxt(wnd.txtTreasureDesc, TextTranslate.instance.getLanguage(treasureRefObj.desc));

            _refreshSfxShow();
            _refreshOutputShow();
            
            wnd.refreshTreasureState(treasureState);
        }

        /// <summary>
        /// 刷新特效显示
        /// </summary>
        private void _refreshSfxShow()
        {
            if(null == wnd || _m_iTreasureInfo == null || _m_iTreasureInfo.treasureRefObj == null)
                return;
            
            TreasureHuntTreasureRefObj treasureRef = _m_iTreasureInfo.treasureRefObj;

            NPQualityExtRefObj qualityExtRefObj = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((long) treasureRef.quality);

            if (qualityExtRefObj != null)
            {
                if (_m_wTreasureQualityBgIcon != null)
                {
                    _m_wTreasureQualityBgIcon.showWnd();
                    _m_wTreasureQualityBgIcon.setTexture(qualityExtRefObj.treasure_hunt_treasure_card_bg);
                }
            }
            else
            {
                _m_wTreasureQualityBgIcon?.hideWnd();
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

        /// <summary>
        /// 刷新产出显示
        /// </summary>
        private void _refreshOutputShow()
        {
            if (null == wnd || _m_iTreasureInfo == null)
                return;

            if (_m_iTreasureInfo.treasureOutputRefObj != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.isOutputTreasureShow, true);
                ALUGUICommon.setGameObjEnable(wnd.isOutputTreasureHide, false);
                
                TreasureHuntTreasureOutputInfo treasureOutputInfo = NPPlayer.instance.treasureHuntComponent.getTreasureOutputInfo(_m_iTreasureInfo.treasureId);
                if (treasureOutputInfo != null && treasureOutputInfo.canDraw)
                {
                    ALUGUICommon.setGameObjEnable(wnd.hasOutputCanDrawShow, true);
                    ALUGUICommon.setGameObjEnable(wnd.noOutputCanDrawShow, false);
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(wnd.hasOutputCanDrawShow, false);
                    ALUGUICommon.setGameObjEnable(wnd.noOutputCanDrawShow, true);    
                }
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.isOutputTreasureShow, false);
                ALUGUICommon.setGameObjEnable(wnd.isOutputTreasureHide, true);
                
                ALUGUICommon.setGameObjEnable(wnd.hasOutputCanDrawShow, false);
                ALUGUICommon.setGameObjEnable(wnd.noOutputCanDrawShow, false);
            }
        }
        
        private void _refreshOutputShow(TreasureHuntTreasureOutputInfo _outputInfo)
        {
            if (_outputInfo != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.isOutputTreasureShow, true);
                ALUGUICommon.setGameObjEnable(wnd.isOutputTreasureHide, false);
                
                if (_outputInfo.canDraw)
                {
                    ALUGUICommon.setGameObjEnable(wnd.hasOutputCanDrawShow, true);
                    ALUGUICommon.setGameObjEnable(wnd.noOutputCanDrawShow, false);
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(wnd.hasOutputCanDrawShow, false);
                    ALUGUICommon.setGameObjEnable(wnd.noOutputCanDrawShow, true);    
                }
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.isOutputTreasureShow, false);
                ALUGUICommon.setGameObjEnable(wnd.isOutputTreasureHide, true);
                
                ALUGUICommon.setGameObjEnable(wnd.hasOutputCanDrawShow, false);
                ALUGUICommon.setGameObjEnable(wnd.noOutputCanDrawShow, false);
            }
        }
        
        private void _onClickTreasure(GameObject _go)
        {
            if (null == wnd || _go == null)
                return;

            if (wnd.isClickShowDetailToolTip)
            {
                QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_TreasureHuntTreasureDetail(
                    wnd.detailToolTipAssetPath, _m_iTreasureInfo,
                    _go.GetComponent<RectTransform>(), wnd.detailToolTipInterval));       
            }
            
            onTreasureClick?.Invoke(_m_iTreasureInfo);
        }
        
        private void _onTreasureOutputChg(params object[] _objects)
        {
            if(_objects == null || _objects.Length < 1 || !( _objects[0] is TreasureHuntTreasureOutputInfo _treasureOutputInfo) 
               || _m_iTreasureInfo == null || _m_iTreasureInfo.treasureId != _treasureOutputInfo.treasureId)
                return;

            _refreshOutputShow(_treasureOutputInfo);
        }
    }
}