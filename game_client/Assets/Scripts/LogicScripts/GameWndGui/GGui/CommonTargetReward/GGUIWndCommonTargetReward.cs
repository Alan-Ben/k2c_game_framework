using ALPackage;
using Common.NpChatObj;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    // 通用目标奖励弹窗
    public class GGUIWndCommonTargetReward : _ANPGGUIBasicWnd<GGUIMonoCommonTargetReward>
    {
        //资源id
        private long _m_resPathId;
        //每个item
        [NotNull]private List<GGUISubWndCommonTargetRewardItem> _m_itemList = new List<GGUISubWndCommonTargetRewardItem>();
        //当前选中item
        private GGUISubWndCommonTargetRewardItem _m_curSelectItem;
        //当前展示的形象窗帘
        private GGUIWndCommonTargetRewardShow _m_curShowWnd;
        
        public GGUIWndCommonTargetReward(long _resPathId)
            : base(EALUIWndLayer.ADDITION)
        {
            _m_resPathId = _resPathId;
        }
        
        protected override string _monoAssetPath { get { return  UIResPathAssistant.getAssetPath(_m_resPathId);  } }
        protected override string _monoObjName { get { return  UIResPathAssistant.getObjName(_m_resPathId);  } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (wnd.itemList != null)
            {
                for(int i = 0; i < wnd.itemList.Count; ++i)
                {
                    GGUIMonoCommonTargetRewardItem itemMono = wnd.itemList[i];
                    if(itemMono == null)
                        continue;
                    
                    GGUISubWndCommonTargetRewardItem itemWnd = new GGUISubWndCommonTargetRewardItem(itemMono, i);
                    itemWnd.onClick += _onClickItem;
                    _m_itemList.Add(itemWnd);
                }
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnGet, _onClickGet);
            ALUGUICommon.combineBtnClick(wnd.btnGo, _onClickGo);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            foreach (GGUISubWndCommonTargetRewardItem item in _m_itemList)
            {
                if (item != null) 
                    item.discard();
            }
            _m_itemList.Clear();
            
            if(null != _m_curShowWnd)
                _m_curShowWnd.discard();
            _m_curShowWnd = null;
        }

        private int _getRewardTypeSortOrder(ECommonRewardType _rewardType)
        {
            switch (_rewardType)
            {
                case ECommonRewardType.CAN_GET_REWARD://可领取优先
                    return 0;
                
                case ECommonRewardType.NOT_GET_REWARD://未达成其次
                    return 1;
                
                case ECommonRewardType.HAS_GET_REWARD://已领取最后
                    return 2;
                
                default:
                    return 999;
            }
        }
        
        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            // 对item排序
            _m_itemList.Sort((_a, _b) =>
            {
                if (_b == null) return -1;
                if (_a == null) return 1;
                if (ReferenceEquals(_a, _b)) return 0;

                int aRewardTypeSortOrder = _getRewardTypeSortOrder(_a.getRewardType());
                int bRewardTypeSortOrder = _getRewardTypeSortOrder(_b.getRewardType());
                if(aRewardTypeSortOrder != bRewardTypeSortOrder)
                    return aRewardTypeSortOrder.CompareTo(bRewardTypeSortOrder);

                return _a.initIndex.CompareTo(_b.initIndex);
            });

            GGUISubWndCommonTargetRewardItem selectItem = null;
            GGUISubWndCommonTargetRewardItem firstVisibleItem = null;
            for(int i = 0;i < _m_itemList.Count; ++i)
            {
                GGUISubWndCommonTargetRewardItem rewardItem = _m_itemList[i];
                if(null == rewardItem)
                    continue;

                bool isVisible = rewardItem.isShowConditionMet();
                if (isVisible)
                    rewardItem.showWnd();
                else
                    rewardItem.hideWnd();

                if (rewardItem.wnd != null)
                {
                    rewardItem.wnd.transform.SetSiblingIndex(i);
                }

                if (!isVisible)
                    continue;

                if (null == firstVisibleItem)
                    firstVisibleItem = rewardItem;

                if (selectItem == null && rewardItem.getRewardType() == ECommonRewardType.CAN_GET_REWARD)
                {
                    selectItem = rewardItem;
                }
            }

            //选中找到的，找不到选中第一个可见的
            _selectItem(selectItem ?? firstVisibleItem);
        }
        
        //点击item
        private void _onClickItem(GGUISubWndCommonTargetRewardItem _item)
        {
            _selectItem(_item);
        }

        //点击领取奖励
        private void _onClickGet(GameObject _go)
        {
            if(null == wnd || null == _m_curSelectItem || null == _m_curSelectItem.wnd || null == _m_curSelectItem.info)
                return;
            
            NPPlayer.instance.commonTargetRewardComp.reqDrawTargetReward(_m_curSelectItem.info.id, () =>
            {
                _refreshWnd();
            });
        }
        
        //点击挑战
        private void _onClickGo(GameObject _go)
        {
            if(null == wnd)
                return;
            
            _m_curSelectItem?.info?.targetRewardRefObj?.go_to?.dealEffect();
        }
        
        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_COMMON_TARGET_REWARD);
        }

        //选中item
        private void _selectItem(GGUISubWndCommonTargetRewardItem _item)
        {
            if(null == _item)
                return;
            
            if(_item == _m_curSelectItem)
                return;

            foreach (GGUISubWndCommonTargetRewardItem rewardItem in _m_itemList)
            {
                if (rewardItem != null) 
                    rewardItem.setIsSelected(false);
            }
            
            //设置选中
            _m_curSelectItem = _item;
            _m_curSelectItem.setIsSelected(true);
            
            ALCommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                _moveScrollRectToItemCenter(_m_curSelectItem);
            });
            
            _refreshSelectItemWnd();
        }

        //将ScrollRect平移，使目标item位于视口中央
        private void _moveScrollRectToItemCenter(GGUISubWndCommonTargetRewardItem _item)
        {
            if (null == wnd || null == _item || null == _item.wnd)
                return;

            ScrollRect scrollRect = wnd.targetRewardScrollRect;
            if (null == scrollRect)
                return;

            RectTransform content = scrollRect.content;
            if (null == content)
                return;

            RectTransform viewport = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.transform as RectTransform;
            if (null == viewport)
                return;

            RectTransform itemRect = _item.wnd.GetComponent<RectTransform>();
            if (null == itemRect)
                return;

            Vector3 itemWorldCenter = itemRect.TransformPoint(itemRect.rect.center);
            Vector3 itemCenterInContent = content.InverseTransformPoint(itemWorldCenter);

            //Vector3 viewportWorldCenter = viewport.TransformPoint(viewport.rect.center);
            //Vector3 viewportCenterInContent = content.InverseTransformPoint(viewportWorldCenter);
            
            if (scrollRect.horizontal)
            {
                float contentWidth = content.rect.width;
                float viewportWidth = viewport.rect.width;
                if (contentWidth - viewportWidth > Mathf.Epsilon)
                {
                    float normalizedX = (itemCenterInContent.x - (viewportWidth / 2)) / (contentWidth - viewportWidth);
                    normalizedX = Mathf.Clamp01(normalizedX);
                    scrollRect.horizontalNormalizedPosition = normalizedX;
                }
            }

            if (scrollRect.vertical)
            {
                float contentHeight = content.rect.height;
                float viewportHeight = viewport.rect.height;
                if (contentHeight - viewportHeight > Mathf.Epsilon)
                {
                    float normalizedY = ((contentHeight / 2) + itemCenterInContent.y) / (contentHeight - viewportHeight);
                    normalizedY = Mathf.Clamp01(normalizedY);
                    scrollRect.verticalNormalizedPosition = normalizedY;
                }
            }
        }

        //刷新窗口
        private void _refreshSelectItemWnd()
        {
            if(null == wnd || null == _m_curSelectItem || null == _m_curSelectItem.wnd || null == _m_curSelectItem.info)
                return;

            if (_m_curSelectItem.info.targetRewardRefObj != null)
            {
                long curProgress = _m_curSelectItem.info.getCurCount();
                long targetProgress = _m_curSelectItem.info.getTargetCount();
                
                Color progressColor = curProgress >= targetProgress ? wnd.progressReachColor : wnd.progressUnReachColor;
                
                //显示描述
                ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_curSelectItem.info.targetRewardRefObj.target_txt, 
                    GCommon.addColorForRichText(curProgress.ToString(), progressColor), targetProgress));
            }
            
            //设置状态
            NPCommonEnumStatInfo<ECommonRewardType>.setStat(wnd.rewardStatList, _m_curSelectItem.getRewardType());

            //刷新itemWnd
            foreach (GGUISubWndCommonTargetRewardItem itemWnd in _m_itemList)
            {
                if (itemWnd != null) 
                    itemWnd.refresh();
            }
            
            //形象展示
            if(null != _m_curShowWnd)
                _m_curShowWnd.discard();
            _m_curShowWnd = null;

            _m_curShowWnd = new GGUIWndCommonTargetRewardShow(_m_curSelectItem.wnd.uiResPathId, wnd.modelParent);
            _m_curShowWnd.load();
            _m_curShowWnd.showWnd();
            _m_curShowWnd.setInfo(_m_curSelectItem.getRewardType());
        }
    }
}
