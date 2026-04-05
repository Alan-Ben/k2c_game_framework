
using System.Collections.Generic;

namespace GOE
{
    public class GGUISubWndInnGuestUnlockTipContainer : _AGGUISubWndCommonContainer<GGUIMonoInnGuestUnlockTipContainerItem, GGUIMonoInnGuestUnlockTipContainer, GGUISubWndInnGuestUnlockTipContainerItem>
    {
        private List<_NPPlayerConditionSerializeInfo> _m_conditionList;
        private List<string> _m_tipKeyList;
        private List<CommonStringList> _m_tipParamsList;
        
        
        public GGUISubWndInnGuestUnlockTipContainer(GGUIMonoInnGuestUnlockTipContainer _containerMono) 
            : base(_containerMono)
        {
            initWnd();
        }
        
        
        protected override GGUISubWndInnGuestUnlockTipContainerItem _createItemWnd(GGUIMonoInnGuestUnlockTipContainerItem _itemMono)
        {
            return new GGUISubWndInnGuestUnlockTipContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndInnGuestUnlockTipContainerItem _itemWnd, int _index)
        {
            _NPPlayerConditionSerializeInfo condition = _m_conditionList.SafeGet(_index);
            string tipKey = _m_tipKeyList.SafeGet(_index);
            CommonStringList tipParams = _m_tipParamsList.SafeGet(_index);
            _itemWnd.refreshWnd(condition, tipKey, tipParams);
        }


        public void refreshWnd(InnNormalGuestHandbookInfo _guestInfo)
        {
            _m_conditionList = _guestInfo?.refObj.unlock_condition_list;
            _m_tipKeyList = _guestInfo?.refObj.unlock_condition_desc_list;
            _m_tipParamsList = _guestInfo?.refObj.unlock_condition_desc_params_list;
            refreshWnd(_m_tipKeyList?.Count ?? 0);
        }
        public void refreshWnd(InnSpecialGuestHandbookInfo _guestInfo)
        {
            _m_conditionList = _guestInfo?.refObj.unlock_condition_list;
            _m_tipKeyList = _guestInfo?.refObj.unlock_condition_desc_list;
            _m_tipParamsList = _guestInfo?.refObj.unlock_condition_desc_params_list;
            refreshWnd(_m_tipKeyList?.Count ?? 0);
        }
    }
}