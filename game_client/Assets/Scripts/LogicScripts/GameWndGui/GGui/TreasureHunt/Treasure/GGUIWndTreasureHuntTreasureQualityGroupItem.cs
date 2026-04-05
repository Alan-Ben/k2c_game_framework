using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 奇物品质组item
    /// </summary>
    public class GGUIWndTreasureHuntTreasureQualityGroupItem : _ANPGGUIBasicSubWnd<GGUIMonoTreasureHuntTreasureQualityGroupItem>
    {
        private EQuality _m_eQuality;
        private List<_ITreasureHuntTreasureInfo> _m_lTreasureInfoList;
        
        private GGUIWndTreasureHuntTreasureItemSizeChangeableContainer _m_wTreasureContainer;
        
        public GGUIWndTreasureHuntTreasureQualityGroupItem(GGUIMonoTreasureHuntTreasureQualityGroupItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        public event Action<_ITreasureHuntTreasureInfo> onTreasureClick; // 当奇物被点击 

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoTreasureContainer != null)
            {
                _m_wTreasureContainer = new GGUIWndTreasureHuntTreasureItemSizeChangeableContainer(wnd.monoTreasureContainer);
                _m_wTreasureContainer.onTreasureClick += _onClickTreasure;
            }
        }
        
        protected override void _onDiscard()
        {
            onTreasureClick = null;

            if (_m_wTreasureContainer != null)
            {
                _m_wTreasureContainer.onTreasureClick -= _onClickTreasure;
                _m_wTreasureContainer.discard();
                _m_wTreasureContainer = null;
            }
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wTreasureContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wTreasureContainer?.resetWnd();
        }

        public void setData(EQuality _quality, List<_ITreasureHuntTreasureInfo> _treasureInfoList)
        {
            _m_eQuality = _quality;
            _m_lTreasureInfoList = _treasureInfoList;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            NPQualityRefObj qualityRefObj = GCommon.getItemQualityRef(ENPItemType.NONE, (long) _m_eQuality);
            if(qualityRefObj != null)
                ALUGUICommon.setLabelTxt(wnd.txtQualityName, TextTranslate.instance.getLanguage(qualityRefObj.name));

            // 设置奇物列表
            if (_m_wTreasureContainer != null)
            {
                _m_wTreasureContainer.showWnd();
                _m_wTreasureContainer.setData(_m_lTreasureInfoList);
            }
        }

        private void _onClickTreasure(_ITreasureHuntTreasureInfo _treasureInfo)
        {
            onTreasureClick?.Invoke(_treasureInfo);
        }
    }
}