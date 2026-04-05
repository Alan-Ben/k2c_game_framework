using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndRecruitShopNew : _ANPGGUIBasicWnd<GGUIMonoRecruitShopNew>
    {
        private RecruitShopInfo _m_iShopInfo;//招募商店ID
        private List<_ARecruitItemInfo> _m_iRecruitItemInfoList;//招募商店物品列表
        private int _m_iNowSelectItemIndex = 0;//当前选中物品的索引
        private long _m_lUIResPathId;
        
        private CommonItemData _m_iRecruitCostItemData;//招募消耗物品数据
        
        private NPGGUIWndCommonItem _m_wRecruitCostItem;
        private GGUIWndRecruitShopIconItemContainer _m_wRecruitShopIconItemContainer;
        private Dictionary<ERecruitItemType, _IGGUIPrefabWndRecruitItemDetail> _m_dDetailInfoPageDic;
        private GGUISubWndRecruitExchangeBtn _m_wndRecruitExchangeBtn;
        
        public GGUIWndRecruitShopNew(RecruitShopInfo _shopInfo, long _uiResPathId) : base(EALUIWndLayer.NORMAL)
        {
            _m_iShopInfo = _shopInfo;
            _m_lUIResPathId = _uiResPathId;

            if (_m_iShopInfo != null && _m_iShopInfo.recruitShopRefObj != null && _m_iShopInfo.recruitShopRefObj.recruit_cost_show_item != null)
                _m_iRecruitCostItemData = new CommonItemData(_m_iShopInfo.recruitShopRefObj.recruit_cost_show_item, 0);

            if (_m_iShopInfo != null)
            {
                _m_iRecruitItemInfoList = new List<_ARecruitItemInfo>();
                _m_iShopInfo.getRecruitItemInfoList(_m_iRecruitItemInfoList);
            }
            _m_iNowSelectItemIndex = 0;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResPathId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            if(wnd.monoRecruitCostItem != null)
                _m_wRecruitCostItem = new NPGGUIWndCommonItem(wnd.monoRecruitCostItem);

            if (wnd.monoRecruitShopIconItemContainer != null)
            {
                _m_wRecruitShopIconItemContainer = new GGUIWndRecruitShopIconItemContainer(wnd.monoRecruitShopIconItemContainer);
                _m_wRecruitShopIconItemContainer.onItemClick += _onRecruitItemClick;
            }
            
            if (wnd.monoExchangeBtn != null)
                _m_wndRecruitExchangeBtn = new GGUISubWndRecruitExchangeBtn(wnd.monoExchangeBtn);
            
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnLeft, _onLeftBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnRight, _onRightBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnLeft, _onLeftBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnRight, _onRightBtnClick);
            }
            
            _m_wRecruitCostItem?.discard();
            _m_wRecruitCostItem = null;

            if (_m_wRecruitShopIconItemContainer != null)
            {
                _m_wRecruitShopIconItemContainer.onItemClick -= _onRecruitItemClick;
                _m_wRecruitShopIconItemContainer.discard();
                _m_wRecruitShopIconItemContainer = null;       
            }

            _dealAllDetailInfoPage((_detailInfoPage) =>
            {
                _detailInfoPage?.discard();
            });
            _m_dDetailInfoPageDic?.Clear();
            _m_dDetailInfoPageDic = null;
            
            _m_wndRecruitExchangeBtn?.discard();
            _m_wndRecruitExchangeBtn = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            
            WinMsg.RegisterMsgAct(WinMsgType.ON_RECRUIT_EXCHANGE_SUCC, _onRecruitExchangeSucc);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            
            WinMsg.UnregisterMsgAct(WinMsgType.ON_RECRUIT_EXCHANGE_SUCC, _onRecruitExchangeSucc);
            
            _m_wRecruitCostItem?.hideWnd();
            
            _m_wRecruitShopIconItemContainer?.hideWnd();
            
            _dealAllDetailInfoPage((_detailInfoPage) =>
            {
                _detailInfoPage?.hideWnd();
            });
            
            _m_wndRecruitExchangeBtn?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRecruitCostItem?.resetWnd();
            
            _m_wRecruitShopIconItemContainer?.resetWnd();
            
            _dealAllDetailInfoPage((_detailInfoPage) =>
            {
                _detailInfoPage?.resetWnd();
            });
            
            _m_wndRecruitExchangeBtn?.resetWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if(wnd == null || _m_iShopInfo == null || _m_iShopInfo.recruitShopRefObj == null)
                return;

            _refreshRecruitCostItem();
            _refreshRecruitContainer();
            _refreshLeftRightBtn();
        }
        
        private void _refreshRecruitContainer()
        {
            _sortRecruitItemInfoList();
            if (_m_wRecruitShopIconItemContainer != null)
            {
                _m_wRecruitShopIconItemContainer.showWnd();
                _m_wRecruitShopIconItemContainer.setData(_m_iShopInfo, _m_iRecruitItemInfoList);
                
                _refreshSelectItem();
            }
        }

        private void _refreshSelectItem()
        {
            _m_wRecruitShopIconItemContainer?.setItemSelect(_m_iRecruitItemInfoList?.SafeGet(_m_iNowSelectItemIndex));
            _refreshNowSelectItemDetailInfoWnd();
            _refreshExchangeBtn();
        }

        private void _refreshSelectItem(int _index)
        {
            if (_index < 0 || _index >= _m_iRecruitItemInfoList?.Count)
                return;
            
            _m_iNowSelectItemIndex = _index;
            _refreshSelectItem();
            
            _refreshLeftRightBtn();
        }

        /// <summary>
        /// 刷新左右切按钮
        /// </summary>
        private void _refreshLeftRightBtn()
        {
            if(wnd == null)
                return;
            
            if (_m_iRecruitItemInfoList == null || _m_iRecruitItemInfoList.Count <= 1)
            {
                ALUGUICommon.setGameObjEnable(wnd.btnLeft, false);
                ALUGUICommon.setGameObjEnable(wnd.btnRight, false);
                return;
            }

            if (_m_iNowSelectItemIndex <= 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.btnLeft, false);
                ALUGUICommon.setGameObjEnable(wnd.btnRight, true);
                return;
            }

            if (_m_iNowSelectItemIndex >= _m_iRecruitItemInfoList.Count - 1)
            {
                ALUGUICommon.setGameObjEnable(wnd.btnLeft, true);
                ALUGUICommon.setGameObjEnable(wnd.btnRight, false);
            }
        }

        /// <summary>
        /// 对_m_iRecruitItemInfoList列表进行排序
        /// </summary>
        private void _sortRecruitItemInfoList()
        {
            if(_m_iRecruitItemInfoList == null)
                return;
            
            Dictionary<long, ERecruitState> tmpRecruitStateDic = new Dictionary<long, ERecruitState>();
            
            _m_iRecruitItemInfoList.Sort((_a, _b) =>
            {
                if (_b == null)
                    return -1;
                if (_a == null)
                    return 1;
                if (ReferenceEquals(_a, _b))
                    return 0;

                if (!tmpRecruitStateDic.TryGetValue(_a.recruitId, out ERecruitState _recruitStateA))
                {
                    _recruitStateA = _m_iShopInfo?.getRecruitState(_a, false) ?? ERecruitState.LOCK;
                    tmpRecruitStateDic[_a.recruitId] = _recruitStateA;
                }
                
                if(!tmpRecruitStateDic.TryGetValue(_b.recruitId, out ERecruitState _recruitStateB))
                {
                    _recruitStateB = _m_iShopInfo?.getRecruitState(_b, false) ?? ERecruitState.LOCK;
                    tmpRecruitStateDic[_b.recruitId] = _recruitStateB;
                }

                if (_recruitStateA != _recruitStateB)
                    return ERecruitStateComparer.Compare(_recruitStateA, _recruitStateB);
                
                if(_b.recruitRefObj == null)
                    return -1;
                if (_a.recruitRefObj == null)
                    return 1;
                if (ReferenceEquals(_a.recruitRefObj, _b.recruitRefObj))
                    return 0;

                if (_a.recruitRefObj.sort_id != _b.recruitRefObj.sort_id)
                    return _a.recruitRefObj.sort_id.CompareTo(_b.recruitRefObj.sort_id);

                return _a.recruitId.CompareTo(_b.recruitId);
            });
        }
        
        private void _refreshRecruitCostItem()
        {
            if(wnd == null)
                return;
            
            if (_m_iRecruitCostItemData != null)
            {
                _m_iRecruitCostItemData.setCount(GCommon.getItemCount(_m_iRecruitCostItemData.getItemType(), _m_iRecruitCostItemData.subId));
                if (_m_wRecruitCostItem != null)
                {
                    _m_wRecruitCostItem.showWnd();
                    _m_wRecruitCostItem.setItem(_m_iRecruitCostItemData);
                }
            }
            else
            {
                _m_wRecruitCostItem?.hideWnd();
            }
        }
        
        /// <summary>
        /// 刷新交换按钮
        /// </summary>
        private void _refreshExchangeBtn()
        {
            if(_m_iRecruitItemInfoList == null || _m_wndRecruitExchangeBtn == null)
                return;
            
            _m_wndRecruitExchangeBtn.showWnd();
            _m_wndRecruitExchangeBtn.setData(_m_iShopInfo, _m_iRecruitItemInfoList?.SafeGet(_m_iNowSelectItemIndex));
        }
        
        private void _onRecruitItemClick(_IRecruitShopIconItemWnd _itemWnd)
        {
            if (_itemWnd == null || _itemWnd.recruitItemInfo == null || _m_iRecruitItemInfoList == null)
                return;

            int selectIndex = -1;
            for (int i = 0; i < _m_iRecruitItemInfoList.Count; i++)
            {
                if(_m_iRecruitItemInfoList[i] == null)
                    continue;
                
                if (_m_iRecruitItemInfoList[i].recruitId == _itemWnd.recruitItemInfo.recruitId)
                {
                    selectIndex = i;
                    break;
                }
            }
            
            _refreshSelectItem(selectIndex);
        }

        /// <summary>
        /// 兑换成功消息
        /// </summary>
        private void _onRecruitExchangeSucc()
        {
            // 刷新窗口
            _refreshWnd();
        }
        
        /// <summary>
        /// 当背包物品变化
        /// </summary>
        /// <param name="_objs"></param>
        private void _onBagItemChg(params object[] _objs)
        {
            if(_m_iRecruitCostItemData == null || _m_iRecruitCostItemData.getItemType() != ENPItemType.BAG_ITEM)
                return;
            
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is BagItem))
                return;

            BagItem chgBagItem = (BagItem) _objs[0];
            if (chgBagItem != null && chgBagItem.itemId == _m_iRecruitCostItemData.subId)
            {
                _refreshRecruitCostItem();
            }
        }

        #region 点击事件

        /// <summary>
        /// 当向左按钮被点击时
        /// </summary>
        /// <param name="_go"></param>
        private void _onLeftBtnClick(GameObject _go)
        {
            if(_m_iRecruitItemInfoList == null || _m_iRecruitItemInfoList.Count <= 0)
                return;

            _m_iNowSelectItemIndex = ((_m_iNowSelectItemIndex - 1) + _m_iRecruitItemInfoList.Count) % _m_iRecruitItemInfoList.Count;
            _refreshSelectItem();
            _refreshLeftRightBtn();
        }
        
        /// <summary>
        /// 当向右按钮被点击时
        /// </summary>
        /// <param name="_go"></param>
        private void _onRightBtnClick(GameObject _go)
        {
            if(_m_iRecruitItemInfoList == null || _m_iRecruitItemInfoList.Count <= 0)
                return;

            _m_iNowSelectItemIndex = (_m_iNowSelectItemIndex + 1) % _m_iRecruitItemInfoList.Count;
            _refreshSelectItem();
            _refreshLeftRightBtn();
        }
        
        /// <summary>
        /// 返回按钮被点击
        /// </summary>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_RECRUIT_SHOP);
        }

        #endregion

        #region 详细信息预制窗口

        /// <summary>
        /// 处理所有详细信息子窗口
        /// </summary>
        /// <param name="_action"></param>
        private void _dealAllDetailInfoPage(Action<_IGGUIPrefabWndRecruitItemDetail> _action)
        {
            if(_m_dDetailInfoPageDic == null || _action == null)
                return;

            foreach (var page in _m_dDetailInfoPageDic.Values)
            {
                _action(page);
            }
        }

        /// <summary>
        /// 获取详细信息子窗口
        /// </summary>
        /// <param name="_getDone"></param>
        private void _getDetailInfoPage(ERecruitItemType _recruitItemType, Action<_IGGUIPrefabWndRecruitItemDetail> _getDone)
        {
            if(_getDone == null || wnd == null)
                return;

            if (_m_dDetailInfoPageDic == null)
                _m_dDetailInfoPageDic = new Dictionary<ERecruitItemType, _IGGUIPrefabWndRecruitItemDetail>();

            if (!_m_dDetailInfoPageDic.TryGetValue(_recruitItemType, out _IGGUIPrefabWndRecruitItemDetail detailInfoWnd) || detailInfoWnd == null)
            {
                NPCommonAssetPathInfo assetPathInfo = wnd.getRecruitItemDetailInfoWndAssetPath(_recruitItemType);
                if (assetPathInfo == null)
                {
                    Debug.LogError($"[GGUIWndRecruitShopNew _getDetailInfoPage] 没有从mono配置中找到类型:{_recruitItemType} 对应的详细信息预制窗口加载路径", wnd);
                    _getDone(null);
                    return;
                }
                
                switch (_recruitItemType)
                {
                    case ERecruitItemType.HERO:
                        detailInfoWnd = new GGUIPrefabWndRecruitItemDetail_Hero(assetPathInfo, wnd.detailInfoParent);
                        break;
                    
                    case ERecruitItemType.CONSORT:
                        detailInfoWnd = new GGUIPrefabWndRecruitItemDetail_Consort(assetPathInfo, wnd.detailInfoParent);
                        break;
                    
                    default:
                        Debug.LogError($"[GGUIWndRecruitShopNew _getDetailInfoPage] 代码没有实现类型:{_recruitItemType} 对应的详细信息预制窗口");
                        break;
                }

                _m_dDetailInfoPageDic[_recruitItemType] = detailInfoWnd;
            }
            
            _getDone.Invoke(detailInfoWnd);
        }

        /// <summary>
        /// 刷新当前选中物品的详细信息窗口
        /// </summary>
        private void _refreshNowSelectItemDetailInfoWnd()
        {
            if(_m_wRecruitShopIconItemContainer == null)
                return;

            _ARecruitItemInfo recruitItemInfo = _m_iRecruitItemInfoList?.SafeGet(_m_iNowSelectItemIndex);
            if(recruitItemInfo == null)
                return;
            
            _dealAllDetailInfoPage((_detailInfoWnd) =>
            {
                _detailInfoWnd?.hideWnd();
            });
            
            _getDetailInfoPage(recruitItemInfo.recruitItemType, (_detailInfoWnd) =>
                {
                    if(_detailInfoWnd == null)
                        return;
                    
                    //此处确保加载操作，生命周期实际由dic控制
                    if(!_detailInfoWnd.isLoaded)
                        _detailInfoWnd.load();
                    
                    _detailInfoWnd.regLoadDoneDelegate(() =>
                    {
                        _detailInfoWnd.showWnd();
                        _detailInfoWnd.setData(_m_iShopInfo, recruitItemInfo);
                    });
                });
        }

        #endregion
    }
}