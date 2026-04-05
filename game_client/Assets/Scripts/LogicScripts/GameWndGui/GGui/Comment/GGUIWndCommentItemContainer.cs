using ALPackage;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    //关卡气泡容器
    public class GGUIWndCommentItemContainer : _ATALBasicUISubWnd<GGUIMonoCommentContainer>
    {
        private long _m_serialId = ALSerializeOpMgr.next();
        //item 列表
        [NotNull]private List<GGUIWndCommentContainerItem> _m_lItemWndList = new List<GGUIWndCommentContainerItem>();

        public GGUIWndCommentItemContainer(GGUIMonoCommentContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            
            _m_serialId = ALSerializeOpMgr.next();
        }

        protected override void _onShowWnd()
        {

        }


        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            GGUIWndCommentContainerItem temp = null;
            for (int i = 0; i < _m_lItemWndList.Count; ++i)
            {
                temp = _m_lItemWndList[i];
                if (temp == null)
                    continue;
                temp.discard();
            }
            
            _m_lItemWndList.Clear();
        }

        protected override void _onDiscard()
        {
            GGUIWndCommentContainerItem temp = null;
            for (int i = 0; i < _m_lItemWndList.Count; ++i)
            {
                temp = _m_lItemWndList[i];
                if (temp == null)
                    continue;
                
                GGUIWndCommentContainerItemCacheMgr.instance.pushBackItem(temp.info.assetPathInfo, temp);
            }
            _m_lItemWndList.Clear();
            
            _m_serialId = ALSerializeOpMgr.next();
        }
        
        public void addCommentItem(CommentRandomInfo _info)
        {
            if (null == _info)
                return;
            
            long serialId = _m_serialId;
            
            GGUIWndCommentContainerItemCacheMgr.instance.popItem(_info.assetPathInfo, (_indexInfo, _item) =>
            {
                if (serialId != _m_serialId)
                {
                    GGUIWndCommentContainerItemCacheMgr.instance.pushBackItem(_info.assetPathInfo, _item);
                    return;
                }
                if(null == _item || null == _item.wnd)
                {
                    GGUIWndCommentContainerItemCacheMgr.instance.pushBackItem(_info.assetPathInfo, _item);
                    return;
                }
                
                //将子窗口添加到容器中
                _item.wnd.transform.SetParent(wnd.itemContainer.transform);
                _item.wnd.transform.localPosition = Vector3.zero;
                _item.wnd.transform.localScale = Vector3.one;
                _item.wnd.transform.localRotation = Quaternion.identity;

                //调用子窗口的初始化函数
                _item.initWnd();
                _item.setInfo(_info);
                //调用子窗口的显示函数
                _item.showWnd();
                
                _m_lItemWndList.Add(_item);

                _checkLimit();
            });
        }

        public void clearAll()
        {
            _m_serialId = ALSerializeOpMgr.next();
            
            GGUIWndCommentContainerItem temp = null;
            for (int i = 0; i < _m_lItemWndList.Count; ++i)
            {
                temp = _m_lItemWndList[i];
                if (temp == null)
                    continue;
                GGUIWndCommentContainerItemCacheMgr.instance.pushBackItem(temp.info.assetPathInfo, temp);
            }
            _m_lItemWndList.Clear();
        }

        //删除超过数量的item
        private void _checkLimit()
        {
            if(null == wnd)
                return;

            if(_m_lItemWndList.Count <= wnd.maxCount)
                return;

            int removeCount = _m_lItemWndList.Count - wnd.maxCount;
            
            GGUIWndCommentContainerItem temp = null;
            for (int i = 0; i < removeCount; i++)
            {
                temp = _m_lItemWndList[i];
                if (temp == null)
                    continue;
                
                GGUIWndCommentContainerItemCacheMgr.instance.pushBackItem(temp.info.assetPathInfo, temp);

                _m_lItemWndList.RemoveAt(i);
            }
        }
    }
}
