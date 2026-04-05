using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子皮肤窗口
    /// </summary>
    public class GGUIWndConsortSkinMain : _ANPGGUIBasicResBarWnd<GGUIMonoConsortSkinMain>
    {
        private static GGUIWndConsortSkinMain _g_instance;
        public static GGUIWndConsortSkinMain instance { get { return _g_instance ??= new GGUIWndConsortSkinMain(); } }

        private GGUIWndConsortSkinItemContainer _m_wSkinContainer;//皮肤列表
        private NPGGUIWndCommonShowCase _m_wSelectSkinShowCase;//选中皮肤展示
        private NPGGUIWndCommonItem _m_wUnlockCostItem;//解锁消耗道具item

        private GConsortRefObj _m_rConsortRefObj;//妃子配表数据
        private long _m_lSelectSkinId;//选中皮肤id
        
        public GGUIWndConsortSkinMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoConsortSkinMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortSkinMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoSkinContainer != null)
            {
                _m_wSkinContainer = new GGUIWndConsortSkinItemContainer(wnd.monoSkinContainer);
                _m_wSkinContainer.onSelectItemChg += _onSelectSkinChg;
            }

            if (wnd.monoSelectSkinShowCaseWnd != null)
                _m_wSelectSkinShowCase = new NPGGUIWndCommonShowCase(wnd.monoSelectSkinShowCaseWnd);

            if (wnd.unlock_cost_item != null)
                _m_wUnlockCostItem = new NPGGUIWndCommonItem(wnd.unlock_cost_item);
            
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnUnlock, _onUnlockBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnWearing, _onWearBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnUnlock, _onUnlockBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnWearing, _onWearBtnClick);
            }
            
            if (_m_wSkinContainer != null)
            {
                _m_wSkinContainer.onSelectItemChg -= _onSelectSkinChg;
                _m_wSkinContainer.discard();
                _m_wSkinContainer = null;    
            }
            
            _m_wSelectSkinShowCase?.discard();
            _m_wSelectSkinShowCase = null;
            
            _m_wUnlockCostItem?.discard();
            _m_wUnlockCostItem = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wSkinContainer?.hideWnd();
            _m_wSelectSkinShowCase?.hideWnd();
            _m_wUnlockCostItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wSkinContainer?.resetWnd();
            _m_wSelectSkinShowCase?.resetWnd();
            _m_wUnlockCostItem?.resetWnd();
        }

        public void setConsort(long _consortId)
        {
            _m_rConsortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(_consortId);

            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if(wnd == null || _m_rConsortRefObj == null)
                return;
            
            if (_m_wSkinContainer != null)
            {
                _m_wSkinContainer.showWnd();
                _m_wSkinContainer.showItemList(_m_rConsortRefObj.skinList);
            }

            ALUGUICommon.setLabelTxt(wnd.txtConsortName, _m_rConsortRefObj.transName);
            
            _refreshSelectSkinShow();
        }

        /// <summary>
        /// 刷新选中皮肤展示数据
        /// </summary>
        private void _refreshSelectSkinShow()
        {
            if(_m_rConsortRefObj == null || wnd == null)
                return;
            
            GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_rConsortRefObj.id);//获取已解锁的妃子信息
            GConsortSkinRefObj selectSkinRef = _m_rConsortRefObj.getSkinRefObj(_m_lSelectSkinId);
            if (selectSkinRef == null)//若找不到选中皮肤数据
            {
                _m_lSelectSkinId = consortInfo?.consortSkinShowInfo?.skinId ?? _m_rConsortRefObj.default_skin_id;//若妃子已解锁, 选中妃子穿戴皮肤, 若未解锁, 选中默认皮肤
                selectSkinRef = _m_rConsortRefObj.getSkinRefObj(_m_lSelectSkinId);
                if (selectSkinRef == null && _m_rConsortRefObj.skinList != null)//若还是找不到
                {
                    foreach (var item in _m_rConsortRefObj.skinList)
                    {
                        if (item != null)
                        {
                            _m_lSelectSkinId = item.id;
                            selectSkinRef = item;
                            break;
                        }
                    }
                }
            }

            if (selectSkinRef == null)
            {
                Debug.LogError($"_refreshSelectSkinShow: 妃子 : {_m_rConsortRefObj.id} 找不到任何皮肤数据");
                return;
            }

            if (wnd.txtSelectSkinName != null)
            {
                foreach (var text in wnd.txtSelectSkinName)
                {
                    ALUGUICommon.setLabelTxt(text, GCommon.getItemName(ENPItemType.CONSORT_SKIN, selectSkinRef.id));
                }
            }
            ALUGUICommon.setLabelTxt(wnd.txtSelectSkinDesc, GCommon.getItemDesc(ENPItemType.CONSORT_SKIN, selectSkinRef.id));
            
            if (_m_wSkinContainer != null)
            {
                _m_wSkinContainer.setSelect(selectSkinRef, false);
            }

            if (_m_wSelectSkinShowCase != null)
            {
                //展示形象
                _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[4];
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(selectSkinRef.td_show), 0);
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(selectSkinRef.td_bg_index), 1);
                _m_wSelectSkinShowCase.showWnd(showCaseUnitInfoObjList);
            }

            EConsortSkinStateType unlockState = NPPlayer.instance.consortComp.getConsortSkinUnlockType(_m_rConsortRefObj.id, selectSkinRef.id);
            NPCommonEnumStatInfo<EConsortSkinStateType>.setStat(wnd.selectSKinStateInfos, unlockState);

            if (_m_wUnlockCostItem != null)
            {
                if (selectSkinRef.unlock_cost_item != null && selectSkinRef.unlock_cost_item.IsValid)
                {
                    _m_wUnlockCostItem.showWnd();
                    _m_wUnlockCostItem.setItem(selectSkinRef.unlock_cost_item);
                }
                else
                {
                    _m_wUnlockCostItem.hideWnd();
                }
            }
        }

        /// <summary>
        /// 设置选中皮肤
        /// </summary>
        public void setSelectSkinId(long _skinId, bool _forceSelect)
        {
            if(_skinId == _m_lSelectSkinId && !_forceSelect)
                return;
            
            _m_lSelectSkinId = _skinId;
            
            if(wnd == null || !isShow)
                return;

            _refreshSelectSkinShow();
        }

        /// <summary>
        /// 选中皮肤变更
        /// </summary>
        /// <param name="_itemWnd"></param>
        private void _onSelectSkinChg(GGUIWndConsortSkinItem _itemWnd)
        {
            if(_itemWnd == null || _itemWnd.skinRef == null)
                return;
            
            setSelectSkinId(_itemWnd.skinRef.id, false);
        }

        /// <summary>
        /// 返回按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_SKIN_MAIN);
        }

        /// <summary>
        /// 点击解锁按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onUnlockBtnClick(GameObject _go)
        {
            if(_m_rConsortRefObj == null)
                return;
            
            GConsortSkinRefObj selectSkinRef = _m_rConsortRefObj.getSkinRefObj(_m_lSelectSkinId);
            if(selectSkinRef == null)
                return;

            if(!NPPlayer.instance.consortComp.checkSkinCanUnlock(selectSkinRef, true))
                return;
            
            NPPlayer.instance.consortComp.reqUnlockSkin(selectSkinRef.id, (_msg) =>
            {
                _refreshWnd();
            }, null);
        }
        
        /// <summary>
        /// 穿戴按钮点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onWearBtnClick(GameObject _go)
        {
            if(_m_rConsortRefObj == null)
                return;
            
            GConsortSkinRefObj selectSkinRef = _m_rConsortRefObj.getSkinRefObj(_m_lSelectSkinId);
            if(selectSkinRef == null)
                return;

            EConsortSkinStateType skinStateType = NPPlayer.instance.consortComp.getConsortSkinUnlockType(_m_rConsortRefObj.id, selectSkinRef.id);
            if(skinStateType == EConsortSkinStateType.UNLOCK_NOT_WEARING)
            {
                NPPlayer.instance.consortComp.reqSetCurSkin(_m_rConsortRefObj.id, selectSkinRef.id, (_msg) =>
                {
                    _refreshSelectSkinShow();
                }, null);
            }
        }
    }
}