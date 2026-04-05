using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTreasureHuntSpecimenConvertItem : _ANPGGUIBasicGridItemWnd<GGUIMonoTreasureHuntSpecimenConvertItem>
    {
        private TreasureHuntCommonOreInfo _m_iOreInfo;
        
        private GGUIWndTreasureHuntOreInfo _m_wOreInfo;
        private GGUISubWndCommonItemDetail _m_wSkillPointItem;

        public GGUIWndTreasureHuntSpecimenConvertItem(GGUIMonoTreasureHuntSpecimenConvertItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoOreInfo != null)
            {
                _m_wOreInfo = new GGUIWndTreasureHuntOreInfo(wnd.monoOreInfo);
            }

            if (wnd.monoSkillPointItem != null)
            {
                _m_wSkillPointItem = new GGUISubWndCommonItemDetail(wnd.monoSkillPointItem);
            }
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
            }

            _m_iOreInfo = null;
            
            _m_wOreInfo?.discard();
            _m_wOreInfo = null;

            _m_wSkillPointItem?.discard();
            _m_wSkillPointItem = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wOreInfo?.hideWnd();
            _m_wSkillPointItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wOreInfo?.resetWnd();
            _m_wSkillPointItem?.resetWnd();
        }

        protected override void _resetGridItem()
        {
            _m_wOreInfo?.resetWnd();
            _m_wSkillPointItem?.resetWnd();
        }
        
        public void setData(TreasureHuntCommonOreInfo _oreInfo)
        {
            _m_iOreInfo = _oreInfo;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || !isShow || _m_iOreInfo == null)
                return;
            
            // 刷新矿石信息显示
            if (_m_wOreInfo != null)
            {
                _m_wOreInfo.showWnd();
                _m_wOreInfo.setData(_m_iOreInfo);
            }

            if (_m_wSkillPointItem != null)
            {
                if (_m_iOreInfo.oreState is ETreasureHuntOreState.ACTIVATED_NORMAL or ETreasureHuntOreState.NOT_ACTIVATE_NORMAL)
                {
                    _m_wSkillPointItem.showWnd();
                    _m_wSkillPointItem.setShowData(GRefdataCoreMgr.instance.npGeneral.treasure_hunt_ore_normal_skill_point_item, _m_iOreInfo.num);
                }
                else if (_m_iOreInfo.oreState is ETreasureHuntOreState.ACTIVATED_ADVANCED or ETreasureHuntOreState.NOT_ACTIVATE_ADVANCED)
                {
                    _m_wSkillPointItem.showWnd();
                    _m_wSkillPointItem.setShowData(GRefdataCoreMgr.instance.npGeneral.treasure_hunt_ore_advanced_skill_point_item, _m_iOreInfo.num);
                }
                else
                {
                    _m_wSkillPointItem.hideWnd();
                }
            }
        }
    }
}