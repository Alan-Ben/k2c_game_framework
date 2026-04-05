using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 进度奖励item列表容器
    /// </summary>
    public class GGUIWndChapterMainDialogSliderTipContainer : _ANPGGUIBasicSubWndContainer<GGUIMonoChapterMainDialogSliderTipItem, GGUIMonoChapterMainDialogSliderTipContainer, GGUIWndChapterMainDialogSliderTipItem>
    {
        //窗口容器
        protected List<GGUIWndChapterMainDialogSliderTipItem> _m_lItemList;
        
        public GGUIWndChapterMainDialogSliderTipContainer(GGUIMonoChapterMainDialogSliderTipContainer _containerMono) : base(_containerMono)
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
            _m_lItemList = new List<GGUIWndChapterMainDialogSliderTipItem>();
        }

        protected override GGUIWndChapterMainDialogSliderTipItem _createItemWnd(GGUIMonoChapterMainDialogSliderTipItem _itemMono)
        {
            // 创建对象
            GGUIWndChapterMainDialogSliderTipItem item = new GGUIWndChapterMainDialogSliderTipItem(_itemMono);
            return item;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_itemPosList"></param>
        public void showItemList(List<long> _list, List<float> _itemPosList)
        {
            if (null == _list || _itemPosList == null || _list.Count != _itemPosList.Count)
                return;
            
            GGUIWndChapterMainDialogSliderTipItem itemWnd = null;
            int count = 0;
            //根据是否展示最后一个遍历数据
            int itemCount = _list.Count;
            for (int i = 0; i < itemCount; i++)
            {
                long item = _list[i];
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
                    itemWnd.rectTransform.localPosition = new Vector3(_itemPosList[i], 0);

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
        /// 播放进度条气泡动画
        /// </summary>
        public void showDialogSliderTip(long _pointId, Action _onDone)
        {
            bool hasFind = false;
            foreach (GGUIWndChapterMainDialogSliderTipItem gguiWndChapterMainDialogSliderTipItem in _m_lItemList)
            {
                if (null != gguiWndChapterMainDialogSliderTipItem && gguiWndChapterMainDialogSliderTipItem.point == _pointId)
                {
                    hasFind = true;
                    gguiWndChapterMainDialogSliderTipItem.playShowAni(_onDone);
                    break;
                }
            }

            if (!hasFind)
            {
                if (_onDone != null) 
                    _onDone();
            }
        }
    }
}
