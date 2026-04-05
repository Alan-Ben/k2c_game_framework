using ALPackage;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonEnum;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

namespace GOE
{
    //关卡气泡容器
    public class GGUIWndCommentSubWnd : _ATALBasicUISubWnd<GGUIMonoCommentSubWnd>
    {
        //随机组id列表
        [NotNull]private List<long> _m_randomGroupIdList = new List<long>();
        private GGUIWndCommentItemContainer _m_wndCommentItemContainer;
        private long _m_serilizeId = 0;

        public GGUIWndCommentSubWnd(GGUIMonoCommentSubWnd _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            _m_wndCommentItemContainer = new GGUIWndCommentItemContainer(wnd.monoCommentContainer);
            _m_wndCommentItemContainer.initWnd();
            _m_serilizeId++;
        }

        protected override void _onShowWnd()
        {
            _m_serilizeId++;
            if(null == wnd)
                return;
            
            if (_m_wndCommentItemContainer != null)
            {
                _m_wndCommentItemContainer.clearAll();
                _m_wndCommentItemContainer.showWnd();
                for (int i = 0; i < wnd.initCount; i++)
                {
                    CommentRandomInfo commentRandomInfo = GRefdataCoreMgr.instance.getCommentRandomInfoByGroupId(randomGroupId);
                    _m_wndCommentItemContainer.addCommentItem(commentRandomInfo);
                }
            }
            
            //容错
            float addTime = Random.Range(wnd.addTimeMinS, wnd.addTimeMaxS);
            if (addTime <= 0)
                addTime = 1;
            
            ALCommonTaskController.CommonActionAddMonoTask(_tickAddCommentItem, addTime);
        }


        protected override void _onHideWnd()
        {
            _m_serilizeId++;
            
            if (_m_wndCommentItemContainer != null)
            {
                _m_wndCommentItemContainer.clearAll();
                _m_wndCommentItemContainer.hideWnd();
            }
        }

        protected override void _onReset()
        {
            _m_serilizeId++;

        }

        protected override void _onDiscard()
        {
            if (_m_wndCommentItemContainer != null) 
                _m_wndCommentItemContainer.discard();
            _m_wndCommentItemContainer = null;
            _m_serilizeId++;

        }
        
        //添加弹幕任务
        private void _tickAddCommentItem()
        {
            if(null == wnd)
                return;
            
            addCommentItem();
            
            long serilizeId = _m_serilizeId;
            
            //容错
            float addTime = Random.Range(wnd.addTimeMinS, wnd.addTimeMaxS);
            if (addTime <= 0)
                addTime = 1;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                if( serilizeId != _m_serilizeId)
                    return;
                _tickAddCommentItem();
            }, addTime);
        }

        //设置随机组id
        public void setGroupId(List<long> _groupIdList)
        {
            if (null == _groupIdList)
                return;
            _m_randomGroupIdList.Clear();
            _m_randomGroupIdList.AddRange(_groupIdList);
        }
        
        private long randomGroupId
        {
            get
            {
                if (_m_randomGroupIdList.Count == 0)
                    return 0;
                return _m_randomGroupIdList.GetRandomItem();
            }
        }
        
        //添加一条弹幕
        public void addCommentItem()
        {
            CommentRandomInfo commentRandomInfo = GRefdataCoreMgr.instance.getCommentRandomInfoByGroupId(randomGroupId);
            if(null == commentRandomInfo)
                return;
            
            if (_m_wndCommentItemContainer != null) 
                _m_wndCommentItemContainer.addCommentItem(commentRandomInfo);
        }
      
        
        //添加一条指定样式弹幕
        public void addCommentItem(NPCommonAssetPathInfo _assetPathInfo)
        {
            CommentRandomInfo commentRandomInfo = GRefdataCoreMgr.instance.getCommentRandomInfoByGroupId(randomGroupId);
            if(null == commentRandomInfo)
                return;
            //设置指定样式
            commentRandomInfo.assetPathInfo = _assetPathInfo;
            
            if (_m_wndCommentItemContainer != null) 
                _m_wndCommentItemContainer.addCommentItem(commentRandomInfo);
        }
    }
}
