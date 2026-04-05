using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTreasureHuntLabSelectContainerItem : _ANPGGUIBasicSubWnd<GGUIMonoTreasureHuntLabSelectContainerItem>
    {
        private TreasureHuntLabRefObj _m_labRefObj;
        
        private NPGGuiWndTexture _m_wBannerImage;
        
        public GGUIWndTreasureHuntLabSelectContainerItem(GGUIMonoTreasureHuntLabSelectContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        public event Action<TreasureHuntLabRefObj> onSelect;
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.bannerImage != null)
                _m_wBannerImage = new NPGGuiWndTexture(wnd.bannerImage);

            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onClickSelect);
        }
        
        protected override void _onDiscard()
        {
            onSelect = null;
            
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onClickSelect);
            }
            
            _m_wBannerImage?.discard();
            _m_wBannerImage = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wBannerImage?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wBannerImage?.discardTexture();
        }

        public void setData(TreasureHuntLabRefObj _labRefObj)
        {
            _m_labRefObj = _labRefObj;
            
            _refreshWnd();
        }

        public void setSelect(bool _selected)
        {
            if(wnd == null)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.nowSelectShow, _selected);
        }
        
        private void _refreshWnd()
        {
            if (wnd == null || _m_labRefObj == null)
                return;

            // 设置实验室名称
            ALUGUICommon.setLabelTxt(wnd.txtLabName, TextTranslate.instance.getLanguage(_m_labRefObj.name));

            // 设置banner图
            if (_m_wBannerImage != null)
            {
                _m_wBannerImage.showWnd();
                _m_wBannerImage.setTexture(_m_labRefObj.banner_img);
            }

            int putInTreasureCount = 0;
            int totalTreasureCount = _m_labRefObj.treasureList?.Count ?? 0;
            if (_m_labRefObj.treasureList != null)
            {
                foreach (var treasureRefObj in _m_labRefObj.treasureList)
                {
                    if(treasureRefObj == null)
                        continue;

                    if (TreasureHuntUtil.getTreasureState(treasureRefObj.id) == ETreasureHuntTreasureState.GOT_ACTIVATED)
                        putInTreasureCount++;
                }
            }

            string txtPutInTreasureProgressKey = string.IsNullOrEmpty(wnd.txtPutInTreasureProgressKey)
                ? TransKeyConst.common_currentTotalNum_num_num
                : wnd.txtPutInTreasureProgressKey;
            ALUGUICommon.setLabelTxt(wnd.txtPutInTreasureProgress,
                TextTranslate.instance.getLanguage(txtPutInTreasureProgressKey, putInTreasureCount, totalTreasureCount));
            
            bool isUnlock = _m_labRefObj.isUnlock();
            ALUGUICommon.setGameObjEnable(wnd.unlockShow, isUnlock);
            ALUGUICommon.setGameObjEnable(wnd.lockShow, !isUnlock);
        }

        private void _onClickSelect(GameObject _go)
        {
            if (_m_labRefObj == null)
                return;
            
            onSelect?.Invoke(_m_labRefObj);
        }
    }
}