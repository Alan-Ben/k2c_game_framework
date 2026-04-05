using System.Collections.Generic;

namespace GOE
{
    public class GGUIWndConsortFetterEffectDescItemContainer : _AGGUISubWndCommonContainer<GGUIMonoConsortFetterEffectDescItem, GGUIMonoConsortFetterEffectDescItemContainer, GGUIWndConsortFetterEffectDescItem>
    {
        private List<ConsortFettersLvlRefObj> _m_lConsortFettersLvlRefObjList;//显示的羁绊等级列表
        private int _m_iNowFettersLvl;//当前羁绊等级
        
        public GGUIWndConsortFetterEffectDescItemContainer(GGUIMonoConsortFetterEffectDescItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndConsortFetterEffectDescItem _createItemWnd(GGUIMonoConsortFetterEffectDescItem _itemMono)
        {
            GGUIWndConsortFetterEffectDescItem itemWnd = new GGUIWndConsortFetterEffectDescItem(_itemMono);
            return itemWnd;
        }

        protected override void _refreshItemWnd(GGUIWndConsortFetterEffectDescItem _itemWnd, int _index)
        {
            if(_m_lConsortFettersLvlRefObjList == null || _index < 0 || _index >= _m_lConsortFettersLvlRefObjList.Count)
                return;

            ConsortFettersLvlRefObj fettersLvlRefObj = _m_lConsortFettersLvlRefObjList[_index];
            _itemWnd.setData(fettersLvlRefObj, fettersLvlRefObj != null && fettersLvlRefObj.lvl == _m_iNowFettersLvl);
        }
        
        public void setData(List<ConsortFettersLvlRefObj> _consortFettersLvlRefObjList, int _nowFettersLvl)
        {
            _m_lConsortFettersLvlRefObjList = _consortFettersLvlRefObjList;
            _m_iNowFettersLvl = _nowFettersLvl;

            refreshWnd(_m_lConsortFettersLvlRefObjList?.Count ?? 0);
        }
    }
}