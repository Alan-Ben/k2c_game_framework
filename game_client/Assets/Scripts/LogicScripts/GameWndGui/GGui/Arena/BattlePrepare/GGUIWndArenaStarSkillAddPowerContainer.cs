using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 竞技场伙伴觉醒技能实力加成列表
    /// </summary>
    public class GGUIWndArenaStarSkillAddPowerContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoArenaStarSkillAddPowerContainerItem, GGUIMonoArenaStarSkillAddPowerContainer, GGUIWndArenaStarSkillAddPowerContainerItem>
    {
        //item列表
        protected List<GGUIWndArenaStarSkillAddPowerContainerItem> _m_lItemList;
        //点击item
        private Action<GGUIWndArenaStarSkillAddPowerContainerItem> _m_aOnClickItem;

        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndArenaStarSkillAddPowerContainerItem> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }

        public GGUIWndArenaStarSkillAddPowerContainer(GGUIMonoArenaStarSkillAddPowerContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndArenaStarSkillAddPowerContainerItem _createItemWnd(GGUIMonoArenaStarSkillAddPowerContainerItem _itemMono)
        {
            GGUIWndArenaStarSkillAddPowerContainerItem item = new GGUIWndArenaStarSkillAddPowerContainerItem(_itemMono);
            return item;
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            _m_lItemList?.Clear();
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<GGUIWndArenaStarSkillAddPowerContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void showItemList(List<HeroStarSkillInfo> _infoList)
        {
            if (wnd == null || _infoList == null || _m_lItemList == null)
                return;

            bool isSelect = false;
            GGUIWndArenaStarSkillAddPowerContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _infoList.Count; i++)
            {
                //如果容器内部个数不足则新增视图
                if (i >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                    itemWnd = _m_lItemList[i];

                itemWnd.showWnd();
                itemWnd.setInfo(_infoList[i]);
                count++;
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }
        }
    }
}
