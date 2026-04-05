using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUIWndEveningDungeonFinalAttackRecordContainer : _AGGUISubWndCommonContainer<GGUIMonoEveningDungeonFinalAttackRecordItem, GGUIMonoEveningDungeonFinalAttackRecordContainer, GGUIWndEveningDungeonFinalAttackRecordItem>
    {
        private List<Common.DungeonObj.EveningDungeon_DefeatInfo> _m_defeatInfoList;
        
        public GGUIWndEveningDungeonFinalAttackRecordContainer(GGUIMonoEveningDungeonFinalAttackRecordContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndEveningDungeonFinalAttackRecordItem _createItemWnd(GGUIMonoEveningDungeonFinalAttackRecordItem _itemMono)
        {
            GGUIWndEveningDungeonFinalAttackRecordItem itemWnd = new GGUIWndEveningDungeonFinalAttackRecordItem(_itemMono);
            return itemWnd;
        }

        protected override void _refreshItemWnd(GGUIWndEveningDungeonFinalAttackRecordItem _itemWnd, int _index)
        {
            if(_m_defeatInfoList == null || _index < 0 || _index >= _m_defeatInfoList.Count)
                return;
            
            _itemWnd.setData(_m_defeatInfoList[_index]);
        }

        public void setData(List<Common.DungeonObj.EveningDungeon_DefeatInfo> _defeatInfoList)
        {
            _m_defeatInfoList = _defeatInfoList;
            _m_defeatInfoList?.Sort((_a, _b) =>
            {
                if (_b == null) return -1;
                if (_a == null) return 1;
                if (object.ReferenceEquals(_a, _b)) return 0;

                // 尾刀记录的近的排在上面
                return -_a.getDefeatTimeMs().CompareTo(_b.getDefeatTimeMs());
            });

            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, _m_defeatInfoList == null || _m_defeatInfoList.Count <= 0);
            }
            
            refreshWnd(_m_defeatInfoList?.Count ?? 0);
        }
    }
}