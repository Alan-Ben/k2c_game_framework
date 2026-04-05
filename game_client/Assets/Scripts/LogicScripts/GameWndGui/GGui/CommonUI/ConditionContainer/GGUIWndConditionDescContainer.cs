using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 条件描述Container
    /// </summary>
    public class GGUIWndConditionDescContainer : _ANPGGUIBasicSubWndControlContainer<GGUIMonoConditionDescItem, GGUIMonoConditionDescContainer, GGUIWndConditionDescItem>
    {
        public GGUIWndConditionDescContainer(GGUIMonoConditionDescContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
        }
        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        protected override GGUIWndConditionDescItem _createItemWnd(GGUIMonoConditionDescItem _itemMono)
        {
            GGUIWndConditionDescItem itemWnd = new GGUIWndConditionDescItem(_itemMono);
            return itemWnd;
        }

        public void setShowData(List<_IConditionDescShow> _infoList)
        {
            showItemList(_infoList, (_itemWnd, _data) =>
            {
                if (_itemWnd == null || _data == null)
                    return false;

                _itemWnd.setItemIndex(_infoList?.IndexOf(_data) ?? 0);
                _itemWnd.setData(_data);
                return true;
            });
        }
    }
}