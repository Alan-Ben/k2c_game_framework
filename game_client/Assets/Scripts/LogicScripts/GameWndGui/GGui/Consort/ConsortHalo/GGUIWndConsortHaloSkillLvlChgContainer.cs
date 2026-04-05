using System.Collections.Generic;

namespace GOE
{
    public class GGUIWndConsortHaloSkillLvlChgContainer : _AGGUISubWndCommonContainer<GGUIMonoConsortHaloSkillLvlChgItem, GGUIMonoConsortHaloSkillLvlChgContainer, GGUIWndConsortHaloSkillLvlChgItem>
    {
        public List<ConsortHaloSkillLvlChgInfo> _m_lChgSkillInfoList;//变化的技能数据表
        
        public GGUIWndConsortHaloSkillLvlChgContainer(GGUIMonoConsortHaloSkillLvlChgContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndConsortHaloSkillLvlChgItem _createItemWnd(GGUIMonoConsortHaloSkillLvlChgItem _itemMono)
        {
            GGUIWndConsortHaloSkillLvlChgItem itemWnd = new GGUIWndConsortHaloSkillLvlChgItem(_itemMono);
            return itemWnd;
        }

        protected override void _refreshItemWnd(GGUIWndConsortHaloSkillLvlChgItem _itemWnd, int _index)
        {
            if(_m_lChgSkillInfoList == null || _index < 0 || _index >= _m_lChgSkillInfoList.Count)
                return;
            
            _itemWnd.setData(_m_lChgSkillInfoList[_index]);
        }
        
        public void setData(List<ConsortHaloSkillLvlChgInfo> _lChgSkillInfoList)
        {
            _m_lChgSkillInfoList = _lChgSkillInfoList;
            
            refreshWnd(_m_lChgSkillInfoList?.Count ?? 0);
        }
    }
}