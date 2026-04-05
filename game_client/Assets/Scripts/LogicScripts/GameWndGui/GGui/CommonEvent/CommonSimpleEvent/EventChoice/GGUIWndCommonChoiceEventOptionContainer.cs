using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 选项事件格 - 选项Container
    /// </summary>
    public class GGUIWndCommonChoiceEventOptionContainer : _AGGUIWndOptionContainer<GGUIMonoCommonChoiceEventOptionItem, GGUIMonoCommonChoiceEventOptionContainer, GGUIWndCommonChoiceEventOptionItem>
    {
        private List<CommonEventChoiceOptionRefObj> _m_lOptionRefObjList;//选项列表
        
        public GGUIWndCommonChoiceEventOptionContainer(GGUIMonoCommonChoiceEventOptionContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndCommonChoiceEventOptionItem _createItemWnd(GGUIMonoCommonChoiceEventOptionItem _itemMono)
        {
            GGUIWndCommonChoiceEventOptionItem itemWnd = new GGUIWndCommonChoiceEventOptionItem(_itemMono);
            return itemWnd;
        }

        protected override void _showItemWnd(int _index, GGUIWndCommonChoiceEventOptionItem _itemWnd)
        {
            if(_index < 0 || _m_lOptionRefObjList == null || _index >= _m_lOptionRefObjList.Count || _itemWnd == null)
                return;

            CommonEventChoiceOptionRefObj refObj = _m_lOptionRefObjList[_index];
            if (refObj == null)
            {
                _itemWnd.hideWnd();
            }
            else
            {
                _itemWnd.showWnd();
                _itemWnd.setData(refObj);
            }
        }

        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
        }

        protected override void _onResetEx()
        {
        }

        protected override void _onDiscardEx()
        {
        }

        protected override void _onWndInitDoneEx()
        {
        }

        public void setChoiceOption(List<CommonEventChoiceOptionRefObj> _optionList)
        {
            _m_lOptionRefObjList = _optionList;
            if (_optionList == null || _optionList.Count <= 0)
            {
                _setOptionCount(0);
                return;
            }
            
            _setOptionCount(_optionList.Count);
        }
    }
}