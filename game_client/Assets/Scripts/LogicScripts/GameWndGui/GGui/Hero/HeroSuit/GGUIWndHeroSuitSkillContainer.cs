using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 伙伴套系技能列表item容器
    /// </summary>
    public class GGUIWndHeroSuitSkillContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoHeroSuitSkillContainerItem, GGUIMonoHeroSuitSkillContainer, GGUIWndHeroSuitSkillContainerItem>
    {
        //item列表
        protected List<GGUIWndHeroSuitSkillContainerItem> _m_lItemList;

        public GGUIWndHeroSuitSkillContainer(GGUIMonoHeroSuitSkillContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndHeroSuitSkillContainerItem _createItemWnd(GGUIMonoHeroSuitSkillContainerItem _itemMono)
        {
            return new GGUIWndHeroSuitSkillContainerItem(_itemMono);
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
            _m_lItemList = new List<GGUIWndHeroSuitSkillContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_suitInfo"></param>
        /// <param name="_suitSkillIdList"></param>
        public void showItemList(HeroSuitInfo _suitInfo, HeroInfo _heroInfo, List<long> _suitSkillIdList)
        {
            if (_suitSkillIdList == null || _m_lItemList == null || _suitSkillIdList.Count == 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, true);
                return;
            }
            ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, false);

            GGUIWndHeroSuitSkillContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _suitSkillIdList.Count; i++)
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
                itemWnd.setInfo(_suitInfo, _heroInfo, _suitSkillIdList[count]);
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
