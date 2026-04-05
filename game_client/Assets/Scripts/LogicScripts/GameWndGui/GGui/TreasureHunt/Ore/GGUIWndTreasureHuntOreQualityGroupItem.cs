using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 矿石品质组item
    /// </summary>
    public class GGUIWndTreasureHuntOreQualityGroupItem : _ANPGGUIBasicSubWnd<GGUIMonoTreasureHuntOreQualityGroupItem>
    {
        private EQuality _m_eQuality;
        private List<_ITreasureHuntOreInfo> _m_lOreInfoList;
        
        private GGUIWndTreasureHuntOreItemSizeChangeableContainer _m_wOreContainer;
        
        public GGUIWndTreasureHuntOreQualityGroupItem(GGUIMonoTreasureHuntOreQualityGroupItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoOreContainer != null)
            {
                _m_wOreContainer = new GGUIWndTreasureHuntOreItemSizeChangeableContainer(wnd.monoOreContainer);
            }
        }
        
        protected override void _onDiscard()
        {
            if (_m_wOreContainer != null)
            {
                _m_wOreContainer.discard();
                _m_wOreContainer = null;
            }
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wOreContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wOreContainer?.resetWnd();
        }

        public void setData(EQuality _quality, List<_ITreasureHuntOreInfo> _oreInfoList)
        {
            _m_eQuality = _quality;
            _m_lOreInfoList = _oreInfoList;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            NPQualityRefObj qualityRefObj = GCommon.getItemQualityRef(ENPItemType.NONE, (long) _m_eQuality);
            if(qualityRefObj != null)
                ALUGUICommon.setLabelTxt(wnd.txtQualityName, TextTranslate.instance.getLanguage(qualityRefObj.name));

            // 设置矿石列表
            if (_m_wOreContainer != null)
            {
                _m_wOreContainer.showWnd();
                _m_wOreContainer.setData(_m_lOreInfoList);
            }
        }
    }
}