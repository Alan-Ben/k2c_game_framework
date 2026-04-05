using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using NPEnum;
using ChatPackage;
using UnityEngine.UI;

namespace GOE
{
    // 聊天频道会话列表
    public class GGUIWndChatChannleContainer : _ATNPGGUIWndShowAnimContainer <GGUIMonoChatChannleContainerItem, GGUIMonoChatChannleContainer, GGUIWndChatChannleContainerItem>
    {
        
        public List<GGUIWndChatChannleContainerItem> _m_lItemGroupList;//子控件列表

        //点击回调
        public event Action<_INPChatInfo> itemClickAction;

        public GGUIWndChatChannleContainer(GGUIMonoChatChannleContainer _containerMono) : base(_containerMono)
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

            _m_lItemGroupList = new List<GGUIWndChatChannleContainerItem>();
        }

        protected override GGUIWndChatChannleContainerItem _createItemWnd(GGUIMonoChatChannleContainerItem _itemMono)
        {
            return new GGUIWndChatChannleContainerItem(_itemMono, _clickItemWnd);
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        public void refresh(List<_INPChatInfo> _infoList)
        {
            if (null == _infoList)
                return;
            _INPChatInfo tempData = null;
            GGUIWndChatChannleContainerItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < _infoList.Count; ++i)
            {
                tempData = _infoList[i];
                if (tempData == null)
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

                tempItemWnd.setInfo(tempData);
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
        /// 外部调用设置选中
        /// </summary>
        /// <param name="_itemWnd"></param>
        public void setSelectItem(_INPChatInfo _info)
        {
            foreach (GGUIWndChatChannleContainerItem item in _m_lItemGroupList)
            {
                item?.setSelected(item.info == _info);
            }
        }
        

        /// <summary>
        /// 选中某一个item
        /// </summary>
        /// <param name="_info"></param>
        private void _clickItemWnd(_INPChatInfo _info)
        {
            itemClickAction?.Invoke(_info);
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
    }
}
