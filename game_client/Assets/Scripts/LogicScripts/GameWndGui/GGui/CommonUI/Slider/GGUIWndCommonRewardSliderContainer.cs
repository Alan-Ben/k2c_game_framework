using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 进度奖励item列表容器
    /// </summary>
    public class GGUIWndCommonRewardSliderContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoCommonRewardSliderContainerItem, GGUIMonoCommonRewardSliderContainer, GGUIWndCommonRewardSliderContainerItem>
    {
        //窗口容器
        protected List<GGUIWndCommonRewardSliderContainerItem> _m_lItemList;
        //点击item
        private Action<GGUIWndCommonRewardSliderContainerItem> _m_aOnClickItem;
        //领奖状态变化
        private Action<GGUIWndCommonRewardSliderContainerItem> _m_aOnRewardStateChg;

        /// <summary>
        /// 点击item
        /// </summary>
        public Action<GGUIWndCommonRewardSliderContainerItem> onClickItem
        {
            get { return _m_aOnClickItem; }
            set { _m_aOnClickItem = value; }
        }

        /// <summary>
        /// 领奖状态变化
        /// </summary>
        public Action<GGUIWndCommonRewardSliderContainerItem> onRewardStateChg
        {
            get { return _m_aOnRewardStateChg; }
            set { _m_aOnRewardStateChg = value; }
        }

        public GGUIWndCommonRewardSliderContainer(GGUIMonoCommonRewardSliderContainer _containerMono) : base(_containerMono)
        {
            initWnd();
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
            _m_lItemList = new List<GGUIWndCommonRewardSliderContainerItem>();
        }

        protected override GGUIWndCommonRewardSliderContainerItem _createItemWnd(GGUIMonoCommonRewardSliderContainerItem _itemMono)
        {
            // 创建对象
            GGUIWndCommonRewardSliderContainerItem item = new GGUIWndCommonRewardSliderContainerItem(_itemMono);
            item.onClickItem += _onClickItem;
            item.onRewardStateChg += _onRewardStateChg;
            return item;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_itemPosList"></param>
        public void showItemList(List<_ISliderRewardItemInfo> _list, List<float> _itemPosList, bool _needShowLast)
        {
            if (null == _list || _itemPosList == null || _list.Count != _itemPosList.Count)
                return;
            
            GGUIWndCommonRewardSliderContainerItem itemWnd = null;
            int count = 0;
            //根据是否展示最后一个遍历数据
            int itemCount = _list.Count - (_needShowLast ? 0 : 1);
            for (int i = 0; i < itemCount; i++)
            {
                _ISliderRewardItemInfo item = _list[i];
                if (null == item)
                    continue;
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
                
                //设置信息
                itemWnd.setInfo(item);
                //设置位置
                if (itemWnd.rectTransform != null)
                    itemWnd.rectTransform.localPosition = new Vector3(_itemPosList[i] - itemWnd.rectTransform.rect.width/2, 0);

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
        /// 刷新全部领奖状态
        /// </summary>
        public void refreshState()
        {
            if (_m_lItemList == null)
                return;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                if(_m_lItemList[i] != null)
                    _m_lItemList[i].refreshState();
            }
        }

        /// <summary>
        /// 根据分数刷新小于该分数的宝箱状态
        /// </summary>
        public void refreshStateByScore(long _score)
        {

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                if (_m_lItemList[i] != null && _m_lItemList[i].itemInfo != null &&
                    (( _m_lItemList[i].itemInfo.score <= _score) || 
                    (_m_lItemList[i].itemInfo.score > _score && _m_lItemList[i].lastState != ESliderRewardState.CAN_NOT_GET)))
                    _m_lItemList[i].refreshState();
            }
        }

        //点击item事件
        private void _onClickItem(GGUIWndCommonRewardSliderContainerItem _item)
        {
            if (_m_aOnClickItem != null)
                _m_aOnClickItem(_item);
        }

        //领奖状态变化事件
        private void _onRewardStateChg(GGUIWndCommonRewardSliderContainerItem _item)
        {
            if (_m_aOnRewardStateChg != null)
                _m_aOnRewardStateChg(_item);
        }
    }

    /// <summary>
    /// 进度奖励item信息接口
    /// </summary>
    public interface _ISliderRewardItemInfo
    {
        /// <summary>
        /// id
        /// </summary>
        long id { get; }
        /// <summary>
        /// 分数
        /// </summary>
        long score { get; }
        /// <summary>
        /// 显示数值使用的格式化类型
        /// </summary>
        EValueFormatType valueFormatType { get; }
        /// <summary>
        /// 显示使用的分数(若为null或string.empty, 会显示score值, 否则显示该字符串)
        /// </summary>
        string showScore { get; }
        /// <summary>
        /// 奖励领取状态
        /// </summary>
        ESliderRewardState rewardState { get; }
        /// <summary>
        /// 宝箱图标
        /// </summary>
        NPGTextureIndex icon { get; }
        /// <summary>
        /// 需要展示的奖励item
        /// </summary>
        _IItem showRewardItem { get; }
    }
}
