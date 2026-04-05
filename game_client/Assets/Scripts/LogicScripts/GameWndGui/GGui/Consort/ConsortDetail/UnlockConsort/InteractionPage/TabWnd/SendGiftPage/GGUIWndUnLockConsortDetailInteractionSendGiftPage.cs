using System.Collections.Generic;
using ALPackage;
using Common.BagItemUseEnum;
using GS2GC.p006_BagItemOp;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndUnLockConsortDetailInteractionSendGiftPage : _AGGUIWndUnLockConsortDetailInteractionPageTabPageWnd<GGUIMonoUnlockConsortDetailInteractionSendGiftPage>
    {
        private GGUIWndConsortBagItemContainer _m_wConsortBagItemContainer;
        private GGUIWndConsortBagItem _m_wSelectConsortBagItem;//选中的item
        private List<ConsortSendGiftDrawShowTypeShowSfxMgr> _m_lDrawShowTypeShowSfxMgrList;//特效管理列表
        
        [NotNull] private List<BagItemRefObj> _m_bagItemRefObjList = new List<BagItemRefObj>();//展示的道具列表

        public GGUIWndUnLockConsortDetailInteractionSendGiftPage(_IGGUIWndUnlockConsortDetailInteractionPageParam _pageParam, NPCommonAssetPathInfo _commonAssetPathInfo, Transform _parent) : base(_pageParam, _commonAssetPathInfo, _parent)
        {
        }

        public override EUnlockConsortDetailWndInteractionPageTabType tabPageType
        {
            get { return EUnlockConsortDetailWndInteractionPageTabType.SEND_GIFT; }
        }
        
        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.itemContainer != null)
            {
                _m_wConsortBagItemContainer = new GGUIWndConsortBagItemContainer(wnd.itemContainer);
                _m_wConsortBagItemContainer.onSelectItemChg += _onSelectItemChg;
            }

            if (wnd.monoSelectItem != null)
                _m_wSelectConsortBagItem = new GGUIWndConsortBagItem(wnd.monoSelectItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnGive, _onSendGiftBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnBatchGive, _onBatchSendGiveBtnClick);
        }

        protected override void _onDiscardSub()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnGive, _onSendGiftBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnBatchGive, _onBatchSendGiveBtnClick);                
            }
            
            if (_m_wConsortBagItemContainer != null)
            {
                _m_wConsortBagItemContainer.onSelectItemChg -= _onSelectItemChg;
                _m_wConsortBagItemContainer.discard();
            }
            _m_wConsortBagItemContainer = null;

            if(_m_wSelectConsortBagItem != null)
                _m_wSelectConsortBagItem.discard();
            _m_wSelectConsortBagItem = null;

            _discardAllDrawShowTypeSfxMgr();
        }

        protected override void _onShowWndSub()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_GIVE_CONSORT_GIFT, _simulateClickGiveConsortGift);//模拟点击赠送知己礼物，默认选择当前选中的
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_QUIT_CONSORT_GIVE_GIFT, _simulateClickQuitGiveGift);//模拟点击退出家人送礼页面
        }

        protected override void _onHideWndSub()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_GIVE_CONSORT_GIFT, _simulateClickGiveConsortGift);//模拟点击赠送知己礼物，默认选择当前选中的
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_QUIT_CONSORT_GIVE_GIFT, _simulateClickQuitGiveGift);//模拟点击退出家人送礼页面
            _m_wConsortBagItemContainer?.hideWnd();
            _m_wSelectConsortBagItem?.hideWnd();
            
            _discardAllDrawShowTypeSfxMgr();
        }

        protected override void _onResetSub()
        {
            _m_wConsortBagItemContainer?.resetWnd();
            _m_wSelectConsortBagItem?.resetWnd();
            
            _discardAllDrawShowTypeSfxMgr();
        }

        protected override void _setDataSub()
        {
            _m_bagItemRefObjList.Clear();
            
            GRefdataCoreMgr.instance.bagItemConsortCore.dealAllRef(_refObj =>
            {
                //是否是指定类型的道具，并且是指定情人的道具EBagItemUse_TargetType.SELECT
                if (_refObj != null && _refObj.isConsortDetailSendGiftBagItem())
                {
                    BagItemRefObj bagItemRefObj = GRefdataCoreMgr.instance.bagItemCore.getRef(_refObj.id);
                    if(bagItemRefObj != null)
                        _m_bagItemRefObjList.Add(bagItemRefObj);
                }
            });
        }
        
        protected override void _refreshWndSub()
        {
            if(wnd == null)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.listEmptyShow,_m_bagItemRefObjList.Count == 0);
            ALUGUICommon.setGameObjEnable(wnd.listEmptyHide,_m_bagItemRefObjList.Count != 0);
            
            int selectItemIndex = _m_PageParam?.interactionSendGiftPageSelectIndex ?? -1;
            if (selectItemIndex < 0 || selectItemIndex >= _m_bagItemRefObjList.Count)
            {
                if(_m_PageParam != null)
                    _m_PageParam.interactionSendGiftPageSelectIndex = 0;

                selectItemIndex = 0;
            }
            
            if (null != _m_wConsortBagItemContainer)
            {
                _m_wConsortBagItemContainer.showWnd();
                _m_wConsortBagItemContainer.showItemList(_m_bagItemRefObjList, (_bagItemRefObj) =>
                {
                    if (_bagItemRefObj == null)
                        return false;

                    return NPPlayer.instance.consortComp.needShowConsortSendGiftBagItemRedTip(_bagItemRefObj.id);
                });
                
                if (_m_PageParam != null)
                {
                    _m_wConsortBagItemContainer.setSelectByIndexWithOutCallBack(selectItemIndex);
                }
            }

            if (selectItemIndex >= _m_bagItemRefObjList.Count)
            {
                _m_wSelectConsortBagItem?.hideWnd();   
            }
            else
            {
                if (_m_wSelectConsortBagItem != null)
                {
                    _m_wSelectConsortBagItem.showWnd();
                    _m_wSelectConsortBagItem.setInfo(_m_bagItemRefObjList[selectItemIndex], selectItemIndex);
                }
            }
        }

        /// <summary>
        /// 当item被选中
        /// </summary>
        private void _onSelectItemChg(GGUIWndConsortBagItem _selectItem)
        {
            if (_selectItem == null || _m_PageParam == null)
            {
                _m_wSelectConsortBagItem?.hideWnd();
                return;
            }

            if (_selectItem.index < 0 || _selectItem.index >= _m_bagItemRefObjList.Count)
            {
                _m_wSelectConsortBagItem?.hideWnd();
                return;
            }

            if (_m_PageParam.interactionSendGiftPageSelectIndex == _selectItem.index)//重复点击
            {
                _selectItem.showItemDetail(_selectItem.getGameObj());
                return;
            }
            
            _m_PageParam.interactionSendGiftPageSelectIndex = _selectItem.index;
            if (_m_wSelectConsortBagItem != null)
            {
                _m_wSelectConsortBagItem.showWnd();
                _m_wSelectConsortBagItem.setInfo(_m_bagItemRefObjList[_m_PageParam.interactionSendGiftPageSelectIndex], _m_PageParam.interactionSendGiftPageSelectIndex);
            }
        }
        
        /// <summary>
        /// 赠送礼物按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onSendGiftBtnClick(GameObject _go)
        {
            if (_m_iConsortInfo == null || !_checkCanSendGift(true))
                return;

            int selectItemIndex = _m_PageParam?.interactionSendGiftPageSelectIndex ?? -1;
            if(selectItemIndex < 0 || selectItemIndex >= _m_bagItemRefObjList.Count)
                return;

            BagItemRefObj selectBagItem = _m_bagItemRefObjList[selectItemIndex];
            if (selectBagItem == null)
                return;

            NPPlayer.instance.bagComp.reqBagUseItemForSelectConsort(_m_iConsortInfo.consortId,selectBagItem.id, 1, _msg =>
            {
                _retUseItem(selectItemIndex, _msg);
            });
        }

        /// <summary>
        /// 批量赠送礼物按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onBatchSendGiveBtnClick(GameObject _go)
        {
            if (_m_iConsortInfo == null || !_checkCanSendGift(true))
                return;

            int selectItemIndex = _m_PageParam?.interactionSendGiftPageSelectIndex ?? -1;
            if(selectItemIndex < 0 || selectItemIndex >= _m_bagItemRefObjList.Count)
                return;
            
            BagItemRefObj selectBagItem = _m_bagItemRefObjList[selectItemIndex];
            if (selectBagItem == null)
                return;

            GCommon.showBagBatchUse(ENPItemType.BAG_ITEM, selectBagItem.id, 999, (_useCount) =>
            {
                NPPlayer.instance.bagComp.reqBagUseItemForSelectConsort(_m_iConsortInfo.consortId,selectBagItem.id, (int)_useCount, _msg =>
                {
                    _retUseItem(selectItemIndex, _msg);
                });
            });
        }

        /// <summary>
        /// 收到使用道具回包
        /// </summary>
        private void _retUseItem(int _selectItemIndex, GS2GC_006_010_RetBagUseItemForSelectConsort _msg)
        {
            BagItemRefObj selectBagItem = _m_bagItemRefObjList.SafeGet(_selectItemIndex);
            BagItemConsortRefObj bagItemConsortRefObj = GRefdataCoreMgr.instance.bagItemConsortCore.getRef(selectBagItem?.id ?? 0);
            WinMsg.SendMsg(WinMsgType.RET_CONSORT_SEND_GIFT, bagItemConsortRefObj);
            
            //展示相关tip
            if (_msg != null)
            {
                GCommon.showConsortChgTip(_msg.getItemList());
                _playDrawShowTypeSfx(_msg.getItemList());
            }
            
            _m_wConsortBagItemContainer?.forceRefreshItem(_selectItemIndex);
        }
        
        /// <summary>
        /// 检查是否可以赠送礼物
        /// </summary>
        /// <returns></returns>
        private bool _checkCanSendGift(bool _showTip)
        {
            if (_m_bagItemRefObjList.Count <= 0)
            {
                GCommon.dealItemNotEnough(wnd.emptyListItem.itemType, wnd.emptyListItem.itemId);
                return false;
            }

            if (_m_PageParam == null || _m_PageParam.interactionSendGiftPageSelectIndex < 0 || _m_PageParam.interactionSendGiftPageSelectIndex >= _m_bagItemRefObjList.Count)
                return false;

            BagItemRefObj selectBagItem = _m_bagItemRefObjList[_m_PageParam.interactionSendGiftPageSelectIndex];
            if (selectBagItem == null)
                return false;

            if (NPPlayer.instance.bagComp.getItemCount(selectBagItem.id) <= 0)
            {
                GCommon.dealItemNotEnough(ENPItemType.BAG_ITEM, selectBagItem.id);
                return false;
            }

            return true;
        }

        //模拟点击赠送知己礼物，默认选择当前选中的
        private void _simulateClickGiveConsortGift(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _m_wConsortBagItemContainer == null)
                return;
            
            //点击赠送
            _onSendGiftBtnClick(null);
        }

        //模拟点击退出家人送礼页面
        private void _simulateClickQuitGiveGift()
        {
            _onCloseBtnClick(null);
        }

        #region 特效

        private class ConsortSendGiftDrawShowTypeShowSfxMgr
        {
            private ConsortSendGiftDrawShowTypeShowSfxConfig _m_config;//显示的配置数据
            
            private List<CommonUISfxObj> _m_lSfxObjList;//特效对象列表

            public Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType drawShowType
            {
                get { return _m_config?.showType ?? EBagItemUse_ConsortDrawShowType.NONE; }
            }

            public ConsortSendGiftDrawShowTypeShowSfxMgr(ConsortSendGiftDrawShowTypeShowSfxConfig _config)
            {
                _m_config = _config;
            }

            public void discard()
            {
                if (_m_lSfxObjList != null)
                {
                    foreach (CommonUISfxObj sfxObj in _m_lSfxObjList)
                    {
                        sfxObj?.forceDiscard();
                    }
                    _m_lSfxObjList.Clear();
                }
                _m_lSfxObjList = null;

                _m_config = null;
            }

            /// <summary>
            /// 播放特效
            /// </summary>
            public void playSfx()
            {
                if(_m_config == null)
                    return;
                
                if(_m_lSfxObjList == null)
                    _m_lSfxObjList = new List<CommonUISfxObj>();

                CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(_m_config.sfxId, _m_config.sfxParent);
                if(null == sfxObj)
                    return;
                
                _m_lSfxObjList.Add(sfxObj);

                // 判断特效数量是否超过上限, 若是的话从头开始移除特效
                if (_m_lSfxObjList.Count > 0 && _m_lSfxObjList.Count > _m_config.sfxLimit)
                {
                    int needRemoveCount = _m_lSfxObjList.Count - _m_config.sfxLimit;
                    for (int i = 0; i < needRemoveCount; i++)
                    {
                        _m_lSfxObjList[i]?.forceDiscard();
                    }
                    _m_lSfxObjList.RemoveRange(0, needRemoveCount);
                }
            }
        }

        private void _discardAllDrawShowTypeSfxMgr()
        {
            if (_m_lDrawShowTypeShowSfxMgrList == null)
                return;

            foreach (var mgr in _m_lDrawShowTypeShowSfxMgrList)
            {
                mgr?.discard();
            }
        }

        private void _playDrawShowTypeSfx(EBagItemUse_ConsortDrawShowType _type)
        {
            if(wnd == null)
                return;

            if (_m_lDrawShowTypeShowSfxMgrList == null)
                _m_lDrawShowTypeShowSfxMgrList = new List<ConsortSendGiftDrawShowTypeShowSfxMgr>();
            
            ConsortSendGiftDrawShowTypeShowSfxMgr mgr = _m_lDrawShowTypeShowSfxMgrList.Find(_mgr => _mgr != null && _mgr.drawShowType == _type);
            if (mgr == null)
            {
                ConsortSendGiftDrawShowTypeShowSfxConfig config = wnd.getConsortSendGiftDrawShowTypeShowSfxConfig(_type);
                if(config == null)
                    return;

                mgr = new ConsortSendGiftDrawShowTypeShowSfxMgr(config);
                _m_lDrawShowTypeShowSfxMgrList.Add(mgr);
            }
            
            mgr.playSfx();
        }

        private void _playDrawShowTypeSfx(Common.BagItemUseObj.BagItemUse_ConsortShowInfo _showInfo)
        {
            if(_showInfo == null)
                return;
            
            _playDrawShowTypeSfx(_showInfo.getType());
        }
        
        private void _playDrawShowTypeSfx(List<Common.BagItemUseObj.BagItemUse_ConsortShowInfo> _showInfoList)
        {
            if(_showInfoList == null)
                return;

            foreach (var showInfo in _showInfoList)
            {
                _playDrawShowTypeSfx(showInfo);
            }
        }
        
        #endregion
    }
}