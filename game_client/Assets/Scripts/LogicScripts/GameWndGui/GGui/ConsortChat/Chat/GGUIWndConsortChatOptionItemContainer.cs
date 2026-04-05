using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item容器
    /// </summary>
    public class GGUIWndConsortChatOptionItemContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoConsortChatOptionItem,GGUIMonoConsortChatOptionItemContainer,GGUIWndConsortChatOptionItem>
    {
        public List<GGUIWndConsortChatOptionItem> _m_lItemGroupList;//子控件列表
		
        public GGUIWndConsortChatOptionItemContainer(GGUIMonoConsortChatOptionItemContainer _containerMono) : base(_containerMono)
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

            _m_lItemGroupList = new List<GGUIWndConsortChatOptionItem>();
        }

        protected override GGUIWndConsortChatOptionItem _createItemWnd(GGUIMonoConsortChatOptionItem _itemMono)
        {
            GGUIWndConsortChatOptionItem itemWnd = new GGUIWndConsortChatOptionItem(_itemMono);
            return itemWnd;
        }


        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(long _consortId, long _dialogueId, ConsortChatDialogueSentenceRefObj _sentence, Action<long, long, long> _onChooseOption)
        {
            if (_sentence == null || _sentence.response_sentence_list == null|| _sentence.response_sentence_list.Count <=0)
                return;

            GGUIWndConsortChatOptionItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < _sentence.response_sentence_list.Count; ++i)
            {
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
                    tempItemWnd = _m_lItemGroupList[i];
                }

                tempItemWnd.setInfo(_consortId, _dialogueId, _sentence.id, _sentence.response_sentence_list[i], _onChooseOption);
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
    }
}
