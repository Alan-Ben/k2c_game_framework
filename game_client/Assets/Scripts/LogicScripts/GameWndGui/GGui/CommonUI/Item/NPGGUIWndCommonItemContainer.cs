using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用物品容器container
    /// </summary>
    public class NPGGUIWndCommonItemContainer : _ATNPGGUIWndShowAnimContainer<NPGGUIMonoCommonItem, NPGGUIMonoCommonItemContainer, NPGGUIWndCommonItem>
    {
        private List<NPGGUIWndCommonItem> _m_lItemGroupList;//子控件列表

        public NPGGUIWndCommonItemContainer(NPGGUIMonoCommonItemContainer _containerMono)
            : base(_containerMono)
        {
            initWnd();
        }

        protected override NPGGUIWndCommonItem _createItemWnd(NPGGUIMonoCommonItem _itemMono)
        {
            return new NPGGUIWndCommonItem(_itemMono);
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
        }

        protected override void _onDiscard()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
            _m_lItemGroupList = null;
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            _m_lItemGroupList = new List<NPGGUIWndCommonItem>();
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_itemDataList"></param>
        public void showItemList<T>(List<T> _itemDataList)
            where T : _IItem
        {
            if (_itemDataList == null)
                return;

            //设置是否居中展示列表
            _dealSetCenter(_itemDataList.Count);

            T tempData = default(T);
            NPGGUIWndCommonItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < _itemDataList.Count; ++i)
            {
                tempData = _itemDataList[i];
                if (tempData == null)
                    continue;

                //判断该道具类型是否需要展示
                if(!GCommon.itemCanShowInRewardPreview(tempData.getItemType(), tempData.subId))
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

                tempItemWnd.setItem(tempData);
                count++;
            }

            for (int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }

            _refreshContentLayout();
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_itemDataList"></param>
        public void showItemList<T>(T[] _itemDataArray)
            where T : _IItem
        {
            if (_itemDataArray == null)
                return;

            //设置是否居中展示列表
            _dealSetCenter(_itemDataArray.Length);

            T tempData = default(T);
            NPGGUIWndCommonItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < _itemDataArray.Length; ++i)
            {
                tempData = _itemDataArray[i];
                if (tempData == null)
                    continue;

                //判断该道具类型是否需要展示
                if (!GCommon.itemCanShowInRewardPreview(tempData.getItemType(), tempData.subId))
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

                tempItemWnd.setItem(tempData);
                count++;
            }
            for (int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }

            _refreshContentLayout();
        }

        /// <summary>
        /// 刷新容器布局
        /// </summary>
        public void _refreshContentLayout()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (wnd == null || wnd.itemContainer == null)
                    return;

                LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
            });
        }

        public NPGGUIWndCommonItem getIndexItemWnd(int _idx)
        {
            if (null == _m_lItemGroupList)
                return null;

            return _m_lItemGroupList.SafeGet(_idx);
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
