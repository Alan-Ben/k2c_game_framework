using System;
using System.Collections.Generic;
using ALPackage;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 情人item容器
    /// </summary>
    public class GGUIWndConsortCardItemContainer: _ATNPGGUIWndSingleChoiceContainer<GGUIMonoConsortCardItem,GGUIMonoConsortCardItemContainer,GGUIWndConsortCardItem>
    {
        public List<GGUIWndConsortCardItem> _m_lItemGroupList;//子控件列表
        public Action<_IConsortShowInfo> clickDelegate;
        private Tweener _m_tweener;
        private RectTransform _m_itemContainer;

        public GGUIWndConsortCardItemContainer(GGUIMonoConsortCardItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWndEx()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_CHARM_CHG,_onConsortChg);
            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_INTIMACY_CHG,_onConsortChg);
            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_CUR_SKIN_INFO_CHG,_onConsortChg);
            moveToLeft();
        }

        protected override void _onHideWndEx()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_CHARM_CHG,_onConsortChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_INTIMACY_CHG,_onConsortChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_CUR_SKIN_INFO_CHG,_onConsortChg);
            _m_tweener?.Kill();
        }

        protected override void _onResetEx()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
        }

        protected override void _onDiscardEx()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
            _m_lItemGroupList = null;
        }

        protected override void _onWndInitDoneEx()
        {
            if(wnd == null)
                return;

            _m_lItemGroupList = new List<GGUIWndConsortCardItem>();
            if(null != wnd.itemContainer)
                _m_itemContainer = wnd.itemContainer.GetComponent<RectTransform>();
        }

        protected override GGUIWndConsortCardItem _createItemWnd(GGUIMonoConsortCardItem _itemMono)
        {
            return new GGUIWndConsortCardItem(_itemMono,clickDelegate);
        }
        
    
        private void _onConsortChg(params object[] __objs)
        {
            if (__objs.Length == 0)
                return;
            long consortId = (long)__objs[0];
            foreach (GGUIWndConsortCardItem cardItem in _m_lItemGroupList)
            {
                if (cardItem.consortCardInfo.consortId != consortId)
                    continue;
                cardItem.refreshWnd();
            }
        }
        
        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        /// <param name="_defaultClickFirst">是否默认点击第一个item</param>
        public void showItemList(List<_IConsortShowInfo> _itemDataList, long _m_defaultConsortId = 0)
        {
            if (_itemDataList == null)
                return;

            _IConsortShowInfo tempData = null;
            GGUIWndConsortCardItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < _itemDataList.Count; ++i)
            {
                tempData = _itemDataList[i];
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
                if (_m_defaultConsortId == tempData.consortId)
                {
                    tempItemWnd.forceClick();
                }
                count++;
            }

            for (int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }

            ALUGUICommon.setGameObjEnable(wnd.goListEmpterShow, _itemDataList.Count == 0);
            ALUGUICommon.setGameObjEnable(wnd.goListEmpterHide, _itemDataList.Count != 0);
            _refreshContentLayout();
        }
            
        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        /// <param name="_type"></param>
        public void showItemList(List<_IConsortShowInfo> _itemDataList, EGameCommonUnlockType _type, long _m_defaultConsortId = 0)
        {
            if (_itemDataList == null)
                return;

            _IConsortShowInfo tempData = null;
            GGUIWndConsortCardItem tempItemWnd = null;
            EGameCommonUnlockType itemUnlockType = EGameCommonUnlockType.LOCK;
            int count = 0;
            for (int i = 0; i < _itemDataList.Count; ++i)
            {
                tempData = _itemDataList[i];
                if (tempData == null)
                    continue;
                itemUnlockType = NPPlayer.instance.consortComp.getConsortUnlockType(tempData.consortId);
                if(itemUnlockType != _type)
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
                if (_m_defaultConsortId == tempData.consortId)
                {
                    tempItemWnd.forceClick();
                }
                count++;
            }

            for (int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }

            ALUGUICommon.setGameObjEnable(wnd.goListEmpterShow, _itemDataList.Count == 0);
            ALUGUICommon.setGameObjEnable(wnd.goListEmpterHide, _itemDataList.Count != 0);
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

        /// <summary>
        /// 设置移动类型
        /// </summary>
        /// <param name="_moveType"></param>
        public void setScrollType(ScrollRect.MovementType _moveType)
        {
            wnd.scrollRect.movementType = _moveType;
        }

        /// <summary>
        /// 移动对应的item到中间位置
        /// </summary>
        /// <param name="_consortId"></param>
        public void moveConsortToCenter(long _consortId, Action _moveDone, Action _dealDone)
        {
            GGUIWndConsortCardItem cardItem = null;
            for (int i = 0; i < _m_lItemGroupList.Count; i++)
            {
                cardItem = _m_lItemGroupList[i]; 
                if(null == cardItem)
                    continue;
                if (cardItem.consortCardInfo.consortId == _consortId)
                {
                    _moveToItemIdx(cardItem, () =>
                    {
                        _moveDone?.Invoke();
                    });
                    return;
                }
            }
        }

        /// <summary>
        /// 根据下标模拟点击item
        /// </summary>
        /// <param name="_index"></param>
        public void setClickItemByIndex(int _index)
        {
            if (_m_lItemGroupList == null || _index < 0 || _m_lItemGroupList.Count <= _index)
                return;

            setSelectItem(_m_lItemGroupList[_index]);
        }
        
        private void _moveToItemIdx(GGUIWndConsortCardItem _itemWnd, Action _dealDone)
        {
            if (null == _m_itemContainer || null == _itemWnd || null == wnd.areaMaskObj)
            {
                _dealDone?.Invoke();
                return;
            }
            _m_tweener?.Kill();
            ALCommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                float curItemX =  _m_itemContainer.anchoredPosition.x + _itemWnd.rectTransform.localPosition.x;
                float targetX =  wnd.areaMaskObj.anchoredPosition.x + wnd.areaMaskObj.rect.width / 2;
                float mergeX = targetX - curItemX;
                _m_tweener?.Kill();
                _m_tweener = null;
                
                Vector2 anchoredPosition = _m_itemContainer.anchoredPosition;
                float nowHorizontal = anchoredPosition.x;
                float newHorizontal = nowHorizontal;
                float endX = nowHorizontal + mergeX;
                float time = (wnd.itemMoveTime == 0) ? 0 : Math.Clamp(Mathf.Abs(endX - nowHorizontal) / 1080f, 0, 1) * wnd.itemMoveTime;
                // if (_isSmooth)
                // {
                _m_tweener = DOTween.To(_value => newHorizontal = _value, nowHorizontal, endX, time).OnUpdate(() =>
                {
                    _m_itemContainer.anchoredPosition = new Vector2(newHorizontal ,anchoredPosition.y);
                }).SetAutoKill(true).OnKill(() =>
                {
                    _dealDone?.Invoke();
                });
            });
        }

        /// <summary>
        /// 所有item跳到动画最后一帧
        /// </summary>
        public void playAllItemAniToEnd()
        {
            refreshAllItem(_itemWnd =>
            {
                if (null == _itemWnd)
                    return;
                _playItemRefreshAnim(_itemWnd,false);
            });
        }
    }
}