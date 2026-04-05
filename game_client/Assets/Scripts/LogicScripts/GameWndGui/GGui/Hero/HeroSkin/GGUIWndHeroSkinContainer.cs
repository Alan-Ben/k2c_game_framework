using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 伙伴皮肤列表
    /// </summary>
    public class GGUIWndHeroSkinContainer : _ATNPGGUIWndSingleChoiceContainer<GGUIMonoHeroSkinContainerItem, GGUIMonoHeroSkinContainer, GGUIWndHeroSkinContainerItem>
    {
        private List<GGUIWndHeroSkinContainerItem> _m_lItemGroupList;
        public GGUIWndHeroSkinContainer(GGUIMonoHeroSkinContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
            foreach (GGUIWndHeroSkinContainerItem iconItem in _m_lItemGroupList)
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
            _m_lItemGroupList = new List<GGUIWndHeroSkinContainerItem>();
        }


        protected override GGUIWndHeroSkinContainerItem _createItemWnd(GGUIMonoHeroSkinContainerItem _itemMono)
        {
            GGUIWndHeroSkinContainerItem itemWnd = new GGUIWndHeroSkinContainerItem(_itemMono);
            return itemWnd;
        }

        public void showItemList(List<HeroSkinRefObj> _skinList)
        {
            GGUIWndHeroSkinContainerItem tempItemWnd = null;
            HeroSkinRefObj itemData = null;
            int count = 0;
            for (int i = 0; i < _skinList.Count; i++)
            {
                itemData = _skinList[i];
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
        public void setSelect(HeroSkinRefObj _refObj)
        {
            if (_refObj == null)
                return;

            for (int i = 0; i < _m_lItemGroupList.Count; i++)
            {
                if (_m_lItemGroupList[i] != null && _m_lItemGroupList[i].skinRef != null &&
                    _m_lItemGroupList[i].skinRef.id == _refObj.id)
                {
                    setSelectItem(_m_lItemGroupList[i]);
                    break;
                }
            }
        }
    }
}
