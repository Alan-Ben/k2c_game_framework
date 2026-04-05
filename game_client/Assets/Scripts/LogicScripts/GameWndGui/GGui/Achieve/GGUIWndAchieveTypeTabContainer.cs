using System.Collections.Generic;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 成就页签容器
    /// </summary>
    public class GGUIWndAchieveTypeTabContainer : _ATNPGGUIWndSingleChoiceContainer<GGUIMonoAchieveTypeTabContainerItem, GGUIMonoAchieveTypeTabContainer, GGUIWndAchieveTypeTabContainerItem>
    {
        private List<GGUIWndAchieveTypeTabContainerItem> _m_lItemGroupList;
        public GGUIWndAchieveTypeTabContainer(GGUIMonoAchieveTypeTabContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
            foreach (GGUIWndAchieveTypeTabContainerItem iconItem in _m_lItemGroupList)
                iconItem?.discard();
            _m_lItemGroupList?.Clear();
        }

        protected override void _onResetEx()
        {
            _m_lItemGroupList?.Clear();
        }

        protected override void _onDiscardEx()
        {
            _m_lItemGroupList?.Clear();
            _m_lItemGroupList = null;
        }

        protected override void _onWndInitDoneEx()
        {
            _m_lItemGroupList = new List<GGUIWndAchieveTypeTabContainerItem>();
        }


        protected override GGUIWndAchieveTypeTabContainerItem _createItemWnd(GGUIMonoAchieveTypeTabContainerItem _itemMono)
        {
            GGUIWndAchieveTypeTabContainerItem itemWnd = new GGUIWndAchieveTypeTabContainerItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_typeRefList"></param>
        public void setInfo(List<AchieveTypeRefObj> _typeRefList)
        {
            GGUIWndAchieveTypeTabContainerItem tempItemWnd = null;
            AchieveTypeRefObj itemData;
            int count = 0;
            for (int i = 0; i < _typeRefList.Count; i++)
            {
                itemData = _typeRefList[i];
                if (null == itemData)
                    continue;

                if (count >= _m_lItemGroupList.Count)
                {
                    tempItemWnd = addItemWnd();
                    if (tempItemWnd == null)
                        return;
                    //放入数据队列
                    _m_lItemGroupList.Add(tempItemWnd);
                }
                else
                {
                    tempItemWnd = _m_lItemGroupList[count];
                }
                tempItemWnd.showWnd();
                tempItemWnd.setInfo(itemData);
                count++;
            }

            for (int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_refObj"></param>
        public void setSelect(AchieveTypeRefObj _refObj)
        {
            if (_refObj == null)
                return;

            for (int i = 0; i < _m_lItemGroupList.Count; i++)
            {
                if (_m_lItemGroupList[i] != null && _m_lItemGroupList[i].typeRef != null &&
                    _m_lItemGroupList[i].typeRef.type == _refObj.type)
                {
                    setSelectItem(_m_lItemGroupList[i]);
                    break;
                }
            }
        }

        /// <summary>
        /// 刷新列表红点
        /// </summary>
        public void refreshTabRedTip()
        {
            for (int i = 0; i < _m_lItemGroupList.Count; i++)
            {
                if (_m_lItemGroupList[i] != null)
                {
                    _m_lItemGroupList[i].refreshRedTip();
                }
            }
        }
    }
}
