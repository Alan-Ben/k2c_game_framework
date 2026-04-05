using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 伙伴资质列表item容器
    /// </summary>
    public class GGUIWndHeroTalentSkillContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoHeroTalentSkillContainerItem, GGUIMonoHeroTalentSkillContainer, GGUIWndHeroTalentSkillContainerItem>
    {
        //item列表
        protected List<GGUIWndHeroTalentSkillContainerItem> _m_lItemList;
        //当前选中的item
        private GGUIWndHeroTalentSkillContainerItem _m_wCurSelect;
        //选中item事件
        private Action<GGUIWndHeroTalentSkillContainerItem> _m_aOnSelectItem;

        /// <summary>
        /// 选中item事件
        /// </summary>
        public Action<GGUIWndHeroTalentSkillContainerItem> onSelectItem { get { return _m_aOnSelectItem; } set { _m_aOnSelectItem = value; } }

        public GGUIWndHeroTalentSkillContainer(GGUIMonoHeroTalentSkillContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndHeroTalentSkillContainerItem _createItemWnd(GGUIMonoHeroTalentSkillContainerItem _itemMono)
        {
            GGUIWndHeroTalentSkillContainerItem item = new GGUIWndHeroTalentSkillContainerItem(_itemMono);
            item.onSelectItem += _onSelectItem;
            return item;
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
            _m_lItemList = new List<GGUIWndHeroTalentSkillContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_list"></param>
        public void showItemList(long _heroId, List<long> _talentSkillIdList)
        {
            if (_talentSkillIdList == null || _m_lItemList == null)
                return;

            GGUIWndHeroTalentSkillContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _talentSkillIdList.Count; i++)
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
                itemWnd.setInfo(_heroId, _talentSkillIdList[i]);
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
        /// 设置选中item
        /// </summary>
        /// <param name="_id"></param>
        public void setSelectItem(long _id)
        {
            if (_m_lItemList == null)
                return;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                if (_m_lItemList[i] != null && _m_lItemList[i].talentSkillRef != null &&
                    _m_lItemList[i].talentSkillRef.id == _id)
                {
                    _m_wCurSelect?.setSelect(false);
                    _m_wCurSelect = _m_lItemList[i];
                    _m_wCurSelect?.setSelect(true);
                    _m_aOnSelectItem?.Invoke(_m_wCurSelect);
                }
            }
        }

        /// <summary>
        /// 当前选中item播放特效
        /// </summary>
        public void playSelectItemSfx(long _talentSkillId)
        {
            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                if (_m_lItemList[i] != null && _m_lItemList[i].talentSkillRef != null &&
                    _m_lItemList[i].talentSkillRef.id == _talentSkillId)
                {
                    _m_lItemList[i].playSfx();
                    break;
                }
            }
        }

        /// <summary>
        /// 根据播放资质技能id播放解锁动画
        /// </summary>
        /// <param name="_id"></param>
        public void playUnlockAniByTalentSkillId(long _id)
        {
            if (_m_lItemList == null)
                return;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                if (_m_lItemList[i] != null && _m_lItemList[i].talentSkillRef != null &&
                    _m_lItemList[i].talentSkillRef.id == _id)
                {
                    _m_lItemList[i].playUnlockAni();
                    return;
                }
            }
        }

        /// <summary>
        /// 刷新所有item显示
        /// </summary>
        public void refreshAllTalenItemShow()
        {
            if (_m_lItemList == null)
                return;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                _m_lItemList[i]?.refreshWnd();
            }
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        public void refreshRedTip()
        {
            if (_m_lItemList == null)
                return;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                _m_lItemList[i]?.refreshRedTip();
            }
        }

        //选中item事件
        private void _onSelectItem(GGUIWndHeroTalentSkillContainerItem _item)
        {
            if (_item == null || _m_wCurSelect == _item)
                return;

            _m_wCurSelect?.setSelect(false);
            _m_wCurSelect = _item;
            _m_wCurSelect?.setSelect(true);
            _m_aOnSelectItem?.Invoke(_m_wCurSelect);
        }
    }
}
