using System.Collections.Generic;
using ALPackage;
using Common.MarsObj;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 前往火星留言容器
    /// </summary>
    public class GGUIWndMarsGoToSubMsgContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoMarsGoToSubMsgContainerItem, GGUIMonoMarsGoToSubMsgContainer, GGUIWndMarsGoToSubMsgContainerItem>
    {
        //item列表
        protected List<GGUIWndMarsGoToSubMsgContainerItem> _m_lItemList;
        //玩家留言信息列表
        private List<Mars_GoRoute_StageMsg> _m_lInfoList;
        //当前阶段id
        private int _m_iStageId;
        //显示序列号
        private long _m_lShowSerialize;
        //dotweener的列表移动处理
        private Tweener _m_tweener;

        public GGUIWndMarsGoToSubMsgContainer(GGUIMonoMarsGoToSubMsgContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndMarsGoToSubMsgContainerItem _createItemWnd(GGUIMonoMarsGoToSubMsgContainerItem _itemMono)
        {
            GGUIWndMarsGoToSubMsgContainerItem item = new GGUIWndMarsGoToSubMsgContainerItem(_itemMono);
            return item;
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_tweener?.Kill();
        }

        protected override void _onReset()
        {
            _m_lItemList?.Clear();
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;
            _m_tweener?.Kill();
            _m_tweener = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<GGUIWndMarsGoToSubMsgContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_infoList"></param>
        /// <param name="_stageId"></param>
        public void showItemList(List<Mars_GoRoute_StageMsg> _infoList, int _stageId)
        {
            if (wnd == null || _infoList == null || _m_lItemList == null)
                return;

            //设置信息
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_lInfoList = new List<Mars_GoRoute_StageMsg>();
            _m_lInfoList.AddRange(_infoList);
            _m_iStageId = _stageId;
            _m_tweener?.Kill();

            //先对item按照列表展示顺序排序
            if (_m_lItemList != null && _m_lItemList.Count > 0)
            {
                _m_lItemList.Sort((_a, _b) =>
                {
                    if (_a == null || _b == null || _a.rectTransform == null || _b.rectTransform == null)
                        return 0;

                    int aIndex = _a.rectTransform.GetSiblingIndex();
                    int bIndex = _b.rectTransform.GetSiblingIndex();
                    return aIndex.CompareTo(bIndex);
                });
            }

            //加载列表
            GGUIWndMarsGoToSubMsgContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _m_lInfoList.Count; i++)
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
                itemWnd.setInfo(_m_lInfoList[i], _stageId);
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

            //计算当前列表是否全部在可视范围，如果超出则开启自动滚动
            float itemHeight = 0;
            float totalItemHeight = 0;
            if (wnd.itemTemplate != null && wnd.itemContainer != null)
            {
                RectTransform itemRect = wnd.itemTemplate.transform as RectTransform;
                if (itemRect != null)
                    itemHeight = itemRect.rect.height;

                //计算item列表总高度
                VerticalLayoutGroup layoutGroup = wnd.itemContainer as VerticalLayoutGroup;
                if (layoutGroup != null)
                    itemHeight += layoutGroup.spacing;
                totalItemHeight = itemHeight * _m_lInfoList.Count;
                totalItemHeight += layoutGroup.padding.top - layoutGroup.spacing;

                //列表可视范围
                RectTransform scrollRect = (RectTransform)wnd.scrollRect.transform;
                float scrollRectHeight = scrollRect.rect.height;

                //如果列表总高度超过可视范围则开启自动滚动
                if (totalItemHeight > scrollRectHeight)
                {
                    //新增一个item在最后，展示内容跟第一个一样，让滚动看起来是循环的
                    itemWnd = addItemWnd();
                    _m_lInfoList.Add(_m_lInfoList[0]);
                    _m_lItemList.Add(itemWnd);
                    itemWnd.showWnd();
                    itemWnd.setInfo(_m_lInfoList[0], _stageId);

                    //下一帧开启自动滚动
                    ALCommonActionMonoTask.addNextFrameTask(_checkAutoScroll);
                }
            }

            //列表为空时显隐
            ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, _m_lInfoList.Count <= 0);
            ALUGUICommon.setGameObjEnable(wnd.goEmptyHideList, _m_lInfoList.Count > 0); 
        }

        /// <summary>
        /// 检查是否自动滚动
        /// </summary>
        private  void _checkAutoScroll()
        {
            if (wnd == null || _m_lItemList == null || _m_lItemList.Count <= 1 || wnd.itemContainer == null || wnd.scrollRect == null)
                return;

            //判断所有content的高度是否超过容器展示的高度，如果没有超过则不自动滚动
            RectTransform contentRect = (RectTransform)wnd.itemContainer.transform;
            float contentHeight = contentRect.rect.height;
            RectTransform scrollRect = (RectTransform)wnd.scrollRect.transform;
            VerticalLayoutGroup layoutGroup = wnd.itemContainer as VerticalLayoutGroup;
            float scrollRectHeight = scrollRect.rect.height;
            if (contentHeight < scrollRectHeight)
                return;

            //开启自动循环滚动
            long curSerialize = _m_lShowSerialize;
            ALProcess scrollProcess = ALProcess.CreateProcess();
            scrollProcess
                .addProcess(() =>
                {
                    //移动到顶部
                    moveToTop();
                })
                .addDelegateProcess(_onDone =>
                {
                    if (curSerialize != _m_lShowSerialize)
                        return;

                    //计算第一个item滚动出去时verticalNormalizedPosition的值
                    RectTransform itemRectTransform = wnd.itemTemplate.transform as RectTransform;
                    float firstItemHeight = itemRectTransform != null ? itemRectTransform.rect.height : 0;
                    firstItemHeight += (layoutGroup != null ? layoutGroup.spacing + layoutGroup.padding.top : 0);
                    float scrollNormalised = firstItemHeight / (contentHeight - scrollRectHeight);
                    float endNormalised = 1 - scrollNormalised;
                    float targetNormalised = 1;
                    _m_tweener = DOTween.To(_value => targetNormalised = _value, targetNormalised, endNormalised, wnd.moveItemDuration)
                        .OnUpdate(() =>
                        {
                            if (curSerialize != _m_lShowSerialize || wnd == null || wnd.scrollRect == null)
                                return;

                            wnd.scrollRect.verticalNormalizedPosition = targetNormalised;
                        })
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            if (curSerialize != _m_lShowSerialize)
                                return;

                            //移动完成，将最上面的item移动到最下面，形成循环滚动的效果
                            if (_m_lItemList != null && _m_lItemList.Count > 0 && _m_lItemList[0] != null)
                            {
                                //先移动列表到顶部
                                moveToTop();

                                //移动item
                                GGUIWndMarsGoToSubMsgContainerItem tempItem = _m_lItemList[0];
                                tempItem.rectTransform.SetAsLastSibling();
                                _m_lItemList.RemoveAt(0);
                                _m_lItemList.Add(tempItem);

                                //移动数据
                                _m_lInfoList.RemoveAt(0);
                                _m_lInfoList.Add(_m_lInfoList[0]);

                                //刷新最下面item展示
                                tempItem.setInfo(_m_lInfoList[0], _m_iStageId);

                                //强制刷新布局
                                if (wnd != null && wnd.itemContainer != null)
                                    LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
                            }
                            _onDone?.Invoke();
                        });
                })
                .addProcess(() =>
                {
                    //继续循环滚动
                    _checkAutoScroll();
                })
                .deal();
        }
    }
}
