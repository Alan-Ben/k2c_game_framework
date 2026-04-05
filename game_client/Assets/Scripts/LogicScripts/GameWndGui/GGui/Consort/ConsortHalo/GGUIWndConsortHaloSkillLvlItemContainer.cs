using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 星辉技能itemContainer
    /// </summary>
    public class GGUIWndConsortHaloSkillLvlItemContainer : _AGGUISubWndCommonContainer<GGUIMonoConsortHaloSkillLvlItem, GGUIMonoConsortHaloSkillLvlItemContainer, GGUIWndConsortHaloSkillLvlItem>
    {
        private List<ConsortHaloSkillInfo> _m_lHaloSkillInfoList;//星辉技能数据表
        
        public GGUIWndConsortHaloSkillLvlItemContainer(GGUIMonoConsortHaloSkillLvlItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndConsortHaloSkillLvlItem _createItemWnd(GGUIMonoConsortHaloSkillLvlItem _itemMono)
        {
            GGUIWndConsortHaloSkillLvlItem itemWnd = new GGUIWndConsortHaloSkillLvlItem(_itemMono);
            return itemWnd;
        }

        protected override void _refreshItemWnd(GGUIWndConsortHaloSkillLvlItem _itemWnd, int _index)
        {
            if(_m_lHaloSkillInfoList == null || _index < 0 || _index >= _m_lHaloSkillInfoList.Count)
                return;
            
            _itemWnd.setData(_m_lHaloSkillInfoList[_index]);
        }

        public void setData(List<ConsortHaloSkillInfo> _haloSKillInfoList)
        {
            _m_lHaloSkillInfoList = _haloSKillInfoList;

            refreshWnd(_m_lHaloSkillInfoList?.Count ?? 0);
        }
    }
}