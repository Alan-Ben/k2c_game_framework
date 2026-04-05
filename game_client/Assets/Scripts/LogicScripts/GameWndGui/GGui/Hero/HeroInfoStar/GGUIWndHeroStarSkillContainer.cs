using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴觉醒技能列表item容器
    /// </summary>
    public class GGUIWndHeroStarSkillContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoHeroStarSkillContainerItem, GGUIMonoHeroStarSkillContainer, GGUIWndHeroStarSkillContainerItem>
    {
        //item列表
        protected List<GGUIWndHeroStarSkillContainerItem> _m_lItemList;

        public GGUIWndHeroStarSkillContainer(GGUIMonoHeroStarSkillContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndHeroStarSkillContainerItem _createItemWnd(GGUIMonoHeroStarSkillContainerItem _itemMono)
        {
            return new GGUIWndHeroStarSkillContainerItem(_itemMono);
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {
            if (_m_lItemList != null)
            {
                for (int i = 0; i < _m_lItemList.Count; i++)
                {
                    _m_lItemList[i]?.hideWnd();
                }
            }
        }

        protected override void _onReset()
        {
            if (_m_lItemList != null)
            {
                for (int i = 0; i < _m_lItemList.Count; i++)
                {
                    _m_lItemList[i]?.resetWnd();
                }
            }
        }

        protected override void _onDiscard()
        {
            if (_m_lItemList != null)
            {
                for (int i = 0; i < _m_lItemList.Count; i++)
                {
                    _m_lItemList[i]?.discard();
                }
                _m_lItemList.Clear();
                _m_lItemList = null;
            }
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<GGUIWndHeroStarSkillContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <param name="_starSkillIdList"></param>
        public void showItemList(HeroInfo _heroInfo, List<long> _starSkillIdList, bool _needShowNextLevel = false)
        {
            if (_starSkillIdList == null || _m_lItemList == null)
                return;

            GGUIWndHeroStarSkillContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _starSkillIdList.Count; i++)
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
                itemWnd.setInfo(_heroInfo, _starSkillIdList[i], _needShowNextLevel);
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

            //处理列表是否居中
            _dealSetCenter(_starSkillIdList.Count);
        }

        /// <summary>
        /// 所有item播放升级特效
        /// </summary>
        public void playUpgradeSfxWithAllItem()
        {
            if (_m_lItemList == null)
                return;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                _m_lItemList[i]?.playUpgradeSfx();
            }
        }

        /// <summary>
        /// 设置自定义等级
        /// </summary>
        /// <param name="_level"></param>
        public void setCustomLevel(long _level)
        {
            if (_m_lItemList == null)
                return;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                _m_lItemList[i]?.setCustomLevel(_level);
            }
        }

        //处理列表是否居中
        private void _dealSetCenter(long _count)
        {
            if (wnd == null || _count <= 0)
                return;

            if (wnd.needSetCenterWhenNotExceed)
            {
                RectTransform containerRT = (RectTransform)wnd.transform;
                RectTransform itemRT = (RectTransform)wnd.itemTemplate.transform;
                HorizontalLayoutGroup horizontalLayoutGroup = wnd.itemContainer as HorizontalLayoutGroup;
                RectTransform itemContainerRT = (RectTransform)wnd.itemContainer.transform;

                if (containerRT != null && itemRT != null && horizontalLayoutGroup != null && itemContainerRT != null)
                {
                    //判断列表是否超过容器
                    if (containerRT.rect.width < (itemRT.rect.width * _count + horizontalLayoutGroup.spacing * (_count - 1)))
                        itemContainerRT.pivot = new Vector2(0f, 1f);//超出设置居左
                    else
                        itemContainerRT.pivot = new Vector2(0.5f, 1f);//未超出设置居中
                }
            }
        }
    }
}
