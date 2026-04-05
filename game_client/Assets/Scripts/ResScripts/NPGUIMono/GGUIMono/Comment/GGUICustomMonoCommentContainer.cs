using System;
using ALPackage;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOE
{
    public class GGUICustomMonoCommentContainer : MonoBehaviour
    {
        [ALHeader("评论容器")] 
        public GGUIMonoCommentContainer monoCommentContainer;
        [ALHeader("随机组id")]
        public long randomGroupId;
        [ALHeader("初始弹幕数量")]
        public int initCount;
        [ALHeader("最快自动添加时间（秒）")]
        [Range(0.1f, 60)]
        public float addTimeMinS = 0;
        [ALHeader("最慢自动添加时间（秒）")]
        [Range(0.1f, 60)]
        public float addTimeMaxS = 1;
        
#if NP_GAME
        private GGUIWndCommentItemContainer _m_wndCommentItemContainer;
        private long _m_serilizeId = 0;

        public void Awake()
        {
            if (null == _m_wndCommentItemContainer)
                _m_wndCommentItemContainer = new GGUIWndCommentItemContainer(monoCommentContainer);
            _m_wndCommentItemContainer.initWnd();

            _m_serilizeId++;
        }

        public void OnDestroy()
        {
            if (_m_wndCommentItemContainer != null) 
                _m_wndCommentItemContainer.discard();
            _m_wndCommentItemContainer = null;
            
            _m_serilizeId++;
        }

        public void OnEnable()
        {
            _m_serilizeId++;
            
            if (_m_wndCommentItemContainer != null)
            {
                _m_wndCommentItemContainer.clearAll();
                _m_wndCommentItemContainer.showWnd();
                for (int i = 0; i < initCount; i++)
                {
                    CommentRandomInfo commentRandomInfo = GRefdataCoreMgr.instance.getCommentRandomInfoByGroupId(randomGroupId);
                    _m_wndCommentItemContainer.addCommentItem(commentRandomInfo);
                }
            }
            
            //容错
            float addTime = Random.Range(addTimeMinS, addTimeMaxS);
            if (addTime <= 0)
                addTime = 1;
            
            ALCommonTaskController.CommonActionAddMonoTask(_tickAddCommentItem, addTime);
        }

        public void OnDisable()
        {
            _m_serilizeId++;
            
            if (_m_wndCommentItemContainer != null)
            {
                _m_wndCommentItemContainer.clearAll();
                _m_wndCommentItemContainer.hideWnd();
            }
        }
        
        //添加弹幕任务
        private void _tickAddCommentItem()
        {
            addCommentItem();
            
            long serilizeId = _m_serilizeId;
            
            //容错
            float addTime = Random.Range(addTimeMinS, addTimeMaxS);
            if (addTime <= 0)
                addTime = 1;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                if( serilizeId != _m_serilizeId)
                    return;
                _tickAddCommentItem();
            }, addTime);
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
#endif
        
    }
}