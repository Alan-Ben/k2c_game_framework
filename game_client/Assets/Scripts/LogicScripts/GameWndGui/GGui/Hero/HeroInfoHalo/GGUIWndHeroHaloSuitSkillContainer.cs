using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 伙伴套系技能列表item容器
    /// </summary>
    public class GGUIWndHeroHaloSuitSkillContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoHeroHaloSuitSkillContainerItem, GGUIMonoHeroHaloSuitSkillContainer, GGUIWndHeroHaloSuitSkillContainerItem>
    {
        //item列表
        protected List<GGUIWndHeroHaloSuitSkillContainerItem> _m_lItemList;

        public GGUIWndHeroHaloSuitSkillContainer(GGUIMonoHeroHaloSuitSkillContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndHeroHaloSuitSkillContainerItem _createItemWnd(GGUIMonoHeroHaloSuitSkillContainerItem _itemMono)
        {
            return new GGUIWndHeroHaloSuitSkillContainerItem(_itemMono);
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
            _m_lItemList = new List<GGUIWndHeroHaloSuitSkillContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_infoList"></param>
        public void showItemList(List<WCGPairInt> _infoList, bool _onlyShowCurValue = false)
        {
            if (_infoList == null || _m_lItemList == null)
                return;

            GGUIWndHeroHaloSuitSkillContainerItem itemWnd = null;
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
                itemWnd.setInfo(_infoList[i].first(), _infoList[i].second(), _onlyShowCurValue);
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

        /// <summary>
        /// 播放升级激活动画
        /// </summary>
        public void playUpgradeAni()
        {
            foreach (GGUIWndHeroHaloSuitSkillContainerItem _item in _m_lItemList)
            {
                _item?.playUpgradeAni();
            }
        }
    }
}
