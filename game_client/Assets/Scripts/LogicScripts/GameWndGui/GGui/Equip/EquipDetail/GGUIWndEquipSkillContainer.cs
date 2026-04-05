using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 藏品技能列表item容器
    /// </summary>
    public class GGUIWndEquipSkillContainer : _ATNPGGUIWndSingleChoiceContainer<GGUIMonoEquipSkillContainerItem,GGUIMonoEquipSkillContainer,GGUIWndEquipSkillContainerItem>
    {
        //子控件列表
        private List<GGUIWndEquipSkillContainerItem> _m_lItemGroupList;

        public GGUIWndEquipSkillContainer(GGUIMonoEquipSkillContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWndEx()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_EQUIP_Skill_CHG, _onSkillInfoChg);//技能信息变更
        }

        protected override void _onHideWndEx()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_EQUIP_Skill_CHG, _onSkillInfoChg);//技能信息变更
            if (_m_lItemGroupList != null)
            {
                for (int i = 0; i < _m_lItemGroupList.Count; i++)
                {
                    _m_lItemGroupList[i]?.hideWnd();
                }
            }
        }

        protected override void _onResetEx()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
        }

        protected override void _onDiscardEx()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
            _m_lItemGroupList = null;
        }

        protected override void _onWndInitDoneEx()
        {
            if(wnd == null)
                return;

            _m_lItemGroupList = new List<GGUIWndEquipSkillContainerItem>();
        }

        protected override GGUIWndEquipSkillContainerItem _createItemWnd(GGUIMonoEquipSkillContainerItem _itemMono)
        {
            return new GGUIWndEquipSkillContainerItem(_itemMono);
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        public void showItemList(List<EquipSkillInfo> _infoList)
        {
            if (_infoList == null || _infoList.Count == 0)
                return;

            GGUIWndEquipSkillContainerItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < _infoList.Count; ++i)
            {
                EquipSkillInfo equipSkillInfo = _infoList[i];
                if (equipSkillInfo == null)
                    continue;
                if (count >= _m_lItemGroupList.Count)
                {
                    tempItemWnd = addItemWnd();
                    if (tempItemWnd == null)
                        continue;
                    //放入数据队列
                    _m_lItemGroupList.Add(tempItemWnd);
                }
                else
                {
                    tempItemWnd = _m_lItemGroupList[count];
                }

                tempItemWnd.showWnd();
                tempItemWnd.setInfo(equipSkillInfo);
                count++;
            }

            for (int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }
        }

        /// <summary>
        /// 根据技能位置选中item
        /// </summary>
        /// <param name="_skillIndex"></param>
        public void setSelectItemBySkillIndex(int _skillIndex)
        {
            if (_m_lItemGroupList == null)
                return;

            for (int i = 0; i < _m_lItemGroupList.Count; i++)
            {
                if (_m_lItemGroupList[i] != null && _m_lItemGroupList[i].equipSkillInfo != null &&
                    _m_lItemGroupList[i].equipSkillInfo.index == _skillIndex)
                {
                    setSelectItem(_m_lItemGroupList[i]);
                    break;
                }
            }
        }

        /// <summary>
        /// 选中的item发生了变化
        /// </summary>
        /// <param name="_itemWnd"></param>
        protected override void _onSelectItemChg(GGUIWndEquipSkillContainerItem _itemWnd)
        {
            base._onSelectItemChg(_itemWnd);
            GCommon.setContainerMoveItemWithinRangeInHorizontal(_itemWnd?.rectTransform, rectTransform, (RectTransform)wnd?.itemContainer?.transform);
        }

        //技能信息变更
        private void _onSkillInfoChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 2)
                return;

            long dbId = (long) _objects[0];
            long skillIndex = (int) _objects[1];

            if (_m_lItemGroupList == null)
                return;

            for (int i = 0; i < _m_lItemGroupList.Count; i++)
            {
                GGUIWndEquipSkillContainerItem item = _m_lItemGroupList[i];
                if (item != null && 
                    item.equipSkillInfo != null && 
                    item.equipSkillInfo.equipDbId == dbId &&
                    item.equipSkillInfo.index == skillIndex)
                {
                    item.refreshWnd();
                    item.playSfx();
                    break;
                }
            }
        }
    }
}