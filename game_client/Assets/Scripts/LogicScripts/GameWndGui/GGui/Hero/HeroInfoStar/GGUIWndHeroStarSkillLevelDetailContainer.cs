using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 伙伴觉醒技能等级详情列表item容器
    /// </summary>
    public class GGUIWndHeroStarSkillLevelDetailContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoHeroStarSkillLevelDetailContainerItem, GGUIMonoHeroStarSkillLevelDetailContainer, GGUIWndHeroStarSkillLevelDetailContainerItem>
    {
        //item列表
        protected List<GGUIWndHeroStarSkillLevelDetailContainerItem> _m_lItemList;

        public GGUIWndHeroStarSkillLevelDetailContainer(GGUIMonoHeroStarSkillLevelDetailContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndHeroStarSkillLevelDetailContainerItem _createItemWnd(GGUIMonoHeroStarSkillLevelDetailContainerItem _itemMono)
        {
            return new GGUIWndHeroStarSkillLevelDetailContainerItem(_itemMono);
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
            _m_lItemList = new List<GGUIWndHeroStarSkillLevelDetailContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_list"></param>
        public void showItemList(HeroInfo _heroInfo, long _starSkillId)
        {
            if (_heroInfo == null || _m_lItemList == null)
                return;

            //获取等级配置列表
            List<HeroStarSkillLevelRefObj> levelRefList = new List<HeroStarSkillLevelRefObj>();
            GRefdataCoreMgr.instance.heroStarSkillLevelRefCore.dealAllRef(_levelRef =>
            {
                if(_levelRef != null && _levelRef.skill_id == _starSkillId)
                    levelRefList.Add(_levelRef);
            });

            GGUIWndHeroStarSkillLevelDetailContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < levelRefList.Count; i++)
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
                itemWnd.setInfo(_heroInfo, levelRefList[i]);
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
