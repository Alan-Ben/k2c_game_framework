using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public interface _IGGUIWndUnLockConsortDetailHaloPageParam
    {
        /// <summary>
        /// 选中的星辉等级
        /// </summary>
        int selectHaloLvl { get; set; }
    }
    
    /// <summary>
    /// 妃子解锁详情页面星辉page
    /// </summary>
    public class GGUIWndUnLockConsortDetailHaloPage : _AGGUIWndUnLockConsortDetailTabPage<GGUIMonoUnLockConsortDetailHaloPage>
    {
        private _IGGUIWndUnLockConsortDetailHaloPageParam _m_param;
     
        [NotNull] private List<ConsortHaloLvlRefObj> _m_lShowHaloLvlRefList = new List<ConsortHaloLvlRefObj>();//需要显示的星辉等级列表
        
        //星辉等级列表
        private GGUIWndConsortHaloLvlContainer _m_wConsortHaloLvlContainer;
        
        /// <summary>
        /// 星辉等级变更子窗口
        /// </summary>
        private GGUISubWndConsortHaloLvlChg _m_wConsortHaloLvlChg;
        
        // 升级消耗道具
        private NPGGUIWndCommonItem _m_LevelUpCost;
        
        public GGUIWndUnLockConsortDetailHaloPage(_IGGUIWndUnLockConsortDetailHaloPageParam _param, NPCommonAssetPathInfo _commonAssetPathInfo, Transform _parent) : base(_commonAssetPathInfo, _parent)
        {
            _m_param = _param;
        }

        /// <summary>
        /// 本窗口对应的页签类型
        /// </summary>
        public override EUnLockConsortDetailWndTabType tabPageType { get { return EUnLockConsortDetailWndTabType.HALO; } }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.monoHaloLevelContainer != null)
            {
                _m_wConsortHaloLvlContainer = new GGUIWndConsortHaloLvlContainer(wnd.monoHaloLevelContainer);
                _m_wConsortHaloLvlContainer.onSelectItemChg += _onSelectHaliLvlItemChg;
            }

            if (wnd.monoConsortHaloLvlChg != null)
                _m_wConsortHaloLvlChg = new GGUISubWndConsortHaloLvlChg(wnd.monoConsortHaloLvlChg);
            
            if (wnd.monoHaloLevelUpCostItem != null)
                _m_LevelUpCost = new NPGGUIWndCommonItem(wnd.monoHaloLevelUpCostItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnUnlockHalo, _onHaloUnlockBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnHaloLevelUp, _onLevelUpBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnTotalEffectView, _onTotalEffectViewBtnClick);
        }

        protected override void _onDiscardSub()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnUnlockHalo, _onHaloUnlockBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnHaloLevelUp, _onLevelUpBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnTotalEffectView, _onTotalEffectViewBtnClick);
            }
            
            if (_m_wConsortHaloLvlContainer != null)
            {
                _m_wConsortHaloLvlContainer.onSelectItemChg -= _onSelectHaliLvlItemChg;
                _m_wConsortHaloLvlContainer.discard();
                _m_wConsortHaloLvlContainer = null;    
            }
            
            _m_wConsortHaloLvlChg?.discard();
            _m_wConsortHaloLvlChg = null;
            
            _m_LevelUpCost?.discard();
            _m_LevelUpCost = null;
        }

        protected override void _onShowWndSub()
        {
            
        }

        protected override void _onHideWndSub()
        {
            _m_wConsortHaloLvlContainer?.hideWnd();
            _m_wConsortHaloLvlChg?.hideWnd();
            
            _m_LevelUpCost?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wConsortHaloLvlContainer?.resetWnd();
            _m_wConsortHaloLvlChg?.resetWnd();

            _m_LevelUpCost?.resetWnd();
        }

        protected override void _setDataSub()
        {
            _m_lShowHaloLvlRefList.Clear();

            if (_m_iConsortShowInfo != null && _m_iConsortShowInfo.consortRefObj != null &&
                _m_iConsortShowInfo.consortRefObj.haloLvlRefList != null)
            {
                foreach (var item in _m_iConsortShowInfo.consortRefObj.haloLvlRefList)
                {
                    if(item != null && item.level > 0)//等级大于0的才需要显示
                        _m_lShowHaloLvlRefList.Add(item);
                }
            }
        }
        
        protected override void _refreshWndSub()
        {
            if (_m_wConsortHaloLvlContainer != null && _m_iConsortShowInfo != null && _m_iConsortShowInfo.consortRefObj != null)
            {
                _m_wConsortHaloLvlContainer.showWnd();
                _m_wConsortHaloLvlContainer.setData(_m_iConsortShowInfo.consortId, _m_lShowHaloLvlRefList);
            }
            
            _refreshConsortNowInHaloInfo();
            
            // 刷新选中星辉等级列表
            _refreshSelectHaloLvlInfo();
        }

        /// <summary>
        /// 刷新妃子当前所在星辉的等级信息
        /// </summary>
        private void _refreshConsortNowInHaloInfo()
        {
            if(wnd == null || _m_iConsortShowInfo == null)
                return;

            if (_m_iConsortShowInfo.haloInfo == null || !_m_iConsortShowInfo.haloInfo.isUnlock)
            {
                ALUGUICommon.setGameObjEnable(wnd.haloUnlockShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.haloLockShowList, true);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.haloUnlockShowList, true);
                ALUGUICommon.setGameObjEnable(wnd.haloLockShowList, false);

                if (string.IsNullOrEmpty(wnd.txtNowHaloLevelKey))
                {
                    ALUGUICommon.setLabelTxt(wnd.txtNowHaloLevel, TextTranslate.instance.getLanguage(wnd.txtNowHaloLevelKey, _m_iConsortShowInfo.haloInfo.level));
                }
                else
                {
                    ALUGUICommon.setLabelTxt(wnd.txtNowHaloLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level2_num, _m_iConsortShowInfo.haloInfo.level));
                }
            }
        }

        /// <summary>
        /// 刷新选中的星辉等级信息
        /// </summary>
        private void _refreshSelectHaloLvlInfo()
        {
            if(wnd == null || _m_param == null || _m_iConsortShowInfo == null || _m_iConsortShowInfo.consortRefObj == null)
                return;

            #region 数据获取以及矫正部分
            
            ConsortHaloLvlRefObj selectHaloLvlRefObj = _m_iConsortShowInfo.consortRefObj.getHaloLvlRefObj(_m_param.selectHaloLvl);//从所有星辉等级数据中获取当前选中星辉等级
            bool canFindInShowList = _m_lShowHaloLvlRefList.Find((_refObj) => _refObj != null && selectHaloLvlRefObj != null && _refObj.level == selectHaloLvlRefObj.level) != null;//当前选中星辉等级是否在显示列表中
            
            if (selectHaloLvlRefObj == null || (!canFindInShowList && _m_iConsortShowInfo.haloIsUnlock))//若从数据中找不到需要显示的等级 或 需要显示的等级不在显示列表中但是星辉已经解锁时
            {
                // 找显示列表中的第一个有数据的值
                foreach (var item in _m_lShowHaloLvlRefList)
                {
                    if (item != null)
                    {
                        selectHaloLvlRefObj = item;
                        break;
                    }
                }
                
                // 若一个都找不到, 说明等级配置为空, 直接返回
                if (selectHaloLvlRefObj == null)
                {
                    Debug.LogError($"妃子:{_m_iConsortShowInfo.consortId} 星辉:{_m_iConsortShowInfo.consortRefObj.consort_halo_id} 等级配置为空");
                    return;
                }
                
                _m_param.selectHaloLvl = selectHaloLvlRefObj.level;
            }
            
            if(_m_wConsortHaloLvlContainer != null)
                _m_wConsortHaloLvlContainer.selectItem(_m_param.selectHaloLvl, false);
            
            //走到这里时selectHaloLvlRefObj一定不为空
            EConsortDetailHaloPageSelectHaloLvlState selectHaloLvlState = getSelectHaloLvlState(selectHaloLvlRefObj);
            wnd.setSelectHaloLvlState(selectHaloLvlState);
            
            ConsortHaloLvlRefObj selectHaloLvlPreLvlRefObj = _m_iConsortShowInfo.consortRefObj.getHaloLvlRefObj(selectHaloLvlRefObj.level - 1);//获取当前选中星辉等级的前一个等级
            
            #endregion

            if (_m_wConsortHaloLvlChg != null)
            {
                _m_wConsortHaloLvlChg.showWnd();
                _m_wConsortHaloLvlChg.setData(selectHaloLvlPreLvlRefObj, selectHaloLvlRefObj);
            }

            // 刷新升级消耗
            if (_m_LevelUpCost != null)
            {
                _m_LevelUpCost.showWnd();
                _m_LevelUpCost.setItem(selectHaloLvlRefObj.cost_item);
            }
        }

        /// <summary>
        /// 获取选中的星辉等级状态
        /// </summary>
        /// <returns></returns>
        private EConsortDetailHaloPageSelectHaloLvlState getSelectHaloLvlState([NotNull] ConsortHaloLvlRefObj _haloLvlRefObj)
        {
            int playerHaloLvl = _m_iConsortShowInfo?.haloInfo?.level ?? 0;//玩家当前星辉等级
            if (_haloLvlRefObj.level <= playerHaloLvl)
                return EConsortDetailHaloPageSelectHaloLvlState.LessEqual_PlayerNowHaloLvl;
            else if (_haloLvlRefObj.level == playerHaloLvl + 1)
                return EConsortDetailHaloPageSelectHaloLvlState.Equal_PlayerNowHaloLvlNextLvl;
            else
                return EConsortDetailHaloPageSelectHaloLvlState.Large_PlayerNowHaloLvlNextLvl;
        }

        /// <summary>
        /// 当星辉等级item点击
        /// </summary>
        private void _onSelectHaliLvlItemChg(GGUIWndConsortHaloLvlContainerItem _itemWnd)
        {
            if(_itemWnd == null || _itemWnd.haloLvlRefObj == null || _m_param == null)
                return;

            // 若星辉还未解锁, 不能选中
            if (_m_iConsortShowInfo == null || !_m_iConsortShowInfo.haloIsUnlock)
            {
                _m_param.selectHaloLvl = 0;
                _m_wConsortHaloLvlContainer?.resetSelect();//重置选中item
            }
            else
            {
                _m_param.selectHaloLvl = _itemWnd.haloLvlRefObj.level;
            }
            _refreshSelectHaloLvlInfo();
        }

        /// <summary>
        /// 星辉解锁按钮被点击时
        /// </summary>
        private void _onHaloUnlockBtnClick(GameObject _go)
        {
            // 找不到数据 或 星辉已解锁时, 直接返回
            if(_m_iConsortShowInfo == null || _m_iConsortShowInfo.haloIsUnlock)
                return;
            
            NPPlayer.instance.consortComp.reqUnlockHalo(_m_iConsortShowInfo.consortId, (_msg) =>
            {
                _refreshWnd();
            }, null);
        }
        
        /// <summary>
        /// 升级按钮被点击时
        /// </summary>
        private void _onLevelUpBtnClick(GameObject _go)
        {
            if(_m_param == null || _m_iConsortShowInfo == null || _m_iConsortShowInfo.consortRefObj == null || !_m_iConsortShowInfo.haloIsUnlock)
                return;
            
            ConsortHaloLvlRefObj selectHaloLvlRefObj = _m_iConsortShowInfo.consortRefObj.getHaloLvlRefObj(_m_param.selectHaloLvl);//获取当前选中星辉等级
            if(selectHaloLvlRefObj == null)
                return;
            
            EConsortDetailHaloPageSelectHaloLvlState selectHaloLvlState = getSelectHaloLvlState(selectHaloLvlRefObj);
            if(selectHaloLvlState is EConsortDetailHaloPageSelectHaloLvlState.LessEqual_PlayerNowHaloLvl or EConsortDetailHaloPageSelectHaloLvlState.Large_PlayerNowHaloLvlNextLvl)
                return;
            
            // 只有当选中item是玩家当前星辉等级的下一级时才能升级
            if(!GCommon.isItemEnough(selectHaloLvlRefObj.cost_item, true))
                return;
            
            NPPlayer.instance.consortComp.reqUpgradeHaloLvl(_m_iConsortShowInfo.consortId, (_msg) =>
            {
                _refreshWnd();
            }, null);
        }

        /// <summary>
        /// 效果总览按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onTotalEffectViewBtnClick(GameObject _go)
        {
            if(_m_iConsortShowInfo == null)
                return;
            
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndConsortHaloEffect.instance, () =>
            {
                GGUIWndConsortHaloEffect.instance.showWnd();
                GGUIWndConsortHaloEffect.instance.setData(_m_iConsortShowInfo.haloInfo);
            }, UINodeTagConst.C_CONSORT_HALO_EFFECT_DETAIL);
        }
    }
}