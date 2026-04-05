using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUISubWndInnSpecialGuestServeChoiceOptionContainer : _AGGUISubWndCommonContainer<GGUIMonoInnSpecialGuestServeChoiceOptionContainerItem, GGUIMonoInnSpecialGuestServeChoiceOptionContainer, GGUISubWndInnSpecialGuestServeChoiceOptionContainerItem>
    {
        [ItemNotNull, NotNull] private readonly List<InnSpecialGuestChoiceOptionRefObj> _m_optionList;
        private long _m_selectedOptionId = -1;


        public GGUISubWndInnSpecialGuestServeChoiceOptionContainer([NotNull] GGUIMonoInnSpecialGuestServeChoiceOptionContainer _containerMono)
            : base(_containerMono)
        {
            _m_optionList = new List<InnSpecialGuestChoiceOptionRefObj>();

            initWnd();
        }
        
        
        public long selectedOptionId { get { return _m_selectedOptionId; } }


        protected override GGUISubWndInnSpecialGuestServeChoiceOptionContainerItem _createItemWnd(GGUIMonoInnSpecialGuestServeChoiceOptionContainerItem _itemMono)
        {
            return new GGUISubWndInnSpecialGuestServeChoiceOptionContainerItem(_itemMono, _onOptionClick);
        }
        protected override void _refreshItemWnd(GGUISubWndInnSpecialGuestServeChoiceOptionContainerItem _itemWnd, int _index)
        {
            InnSpecialGuestChoiceOptionRefObj optionRef = _m_optionList.SafeGet(_index);
            if (optionRef == null)
                return;

            bool isSelected = optionRef.id == _m_selectedOptionId;
            _itemWnd.refreshWnd(optionRef, isSelected);
        }


        public void refreshWnd(InnSpecialGuestChoiceRefObj _choiceRefObj)
        {
            _m_selectedOptionId = -1;
            _m_optionList.Clear();
            if (_choiceRefObj?.option_id_list != null)
            {
                foreach (int optionId in _choiceRefObj.option_id_list)
                {
                    InnSpecialGuestChoiceOptionRefObj optionRef = GRefdataCoreMgr.instance.innSpecialGuestChoiceOptionRefCore.getRef(optionId);
                    if (optionRef != null)
                        _m_optionList.Add(optionRef);
                }
            }

            refreshWnd(_m_optionList.Count);
        }


        private void _onOptionClick(long _optionId)
        {
            if (_m_selectedOptionId == _optionId)
                return;

            _m_selectedOptionId = _optionId;
            refreshWnd();
        }
    }
}