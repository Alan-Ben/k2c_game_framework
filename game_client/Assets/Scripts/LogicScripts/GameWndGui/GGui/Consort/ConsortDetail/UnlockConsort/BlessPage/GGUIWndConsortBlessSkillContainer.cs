using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 加护技能列表
    /// </summary>
    public class GGUIWndConsortBlessSkillContainer : _AGGUISubWndCommonContainer<GGUIMonoConsortBlessSkillContainerItem, GGUIMonoConsortBlessSkillContainer, GGUIWndConsortBlessSkillContainerItem>
    {
        private GGottenConsortInfo _m_iConsortInfo;//妃子信息
        private List<ConsortBlessSkillInfo> _m_iBlessSkillInfoList;//加护技能信息列表
        
        public GGUIWndConsortBlessSkillContainer(GGUIMonoConsortBlessSkillContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndConsortBlessSkillContainerItem _createItemWnd(GGUIMonoConsortBlessSkillContainerItem _itemMono)
        {
            return new GGUIWndConsortBlessSkillContainerItem(_itemMono);
        }

        protected override void _refreshItemWnd(GGUIWndConsortBlessSkillContainerItem _itemWnd, int _index)
        {
            if(_m_iBlessSkillInfoList == null || _index < 0 || _index >= _m_iBlessSkillInfoList.Count)
                return;
            
            _itemWnd.showWnd();
            _itemWnd.setData(_m_iConsortInfo, _m_iBlessSkillInfoList[_index]);
        }

        public void setData(GGottenConsortInfo _consortInfo)
        {
            _m_iConsortInfo = _consortInfo;
            
            if(_m_iBlessSkillInfoList == null)
                _m_iBlessSkillInfoList = new List<ConsortBlessSkillInfo>();
            _m_iConsortInfo?.getBlessSkillInfoList(_m_iBlessSkillInfoList);
            
            refreshWnd(_m_iBlessSkillInfoList?.Count ?? 0);
        }

        /// <summary>
        /// 设置点击升级
        /// </summary>
        /// <param name="_index"></param>
        public void setClickUpgrade(int _index)
        {
            GGUIWndConsortBlessSkillContainerItem item = getItem(_index);
            if (item != null)
                item.setClickUpgrade();
        }
    }
}