﻿using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 实验室
    /// </summary>
    public class GGUIWndTreasureHuntLab : _ANPGGUIBasicResBarWnd<GGUIMonoTreasureHuntLab>
    {
        private static GGUIWndTreasureHuntLab _g_instance;
        public static GGUIWndTreasureHuntLab instance { get { return _g_instance ??= new GGUIWndTreasureHuntLab(); } }

        private TreasureHuntLabRefObj _m_rLabRefObj;
        private List<TreasureHuntLabRefObj> _m_lCanChgLabRefObjList;//可切换的实验室列表
        private TreasureHuntGotTreasureInfo _m_iCanPutInTreasureInfo;//可放入的奇物信息
        private List<_ITreasureHuntTreasureInfo> _m_lShowTreasureList;//显示的奇物列表
        private long _m_lSelectedTreasureId;//选中的奇物id
        
        private GGUIWndTreasureHuntTreasureInfo _m_wCanPutInTreasureInfoWnd;//可放入的奇物信息窗口
        private GGUIWndTreasureHuntTreasureInfo _m_wSelectedTreasureInfoWnd;//选中的奇物信息窗口
        private GGUIWndTreasureHuntTreasureSelectItemContainer _m_wTreasureContainerWnd;//奇物列表容器窗口
        
        public GGUIWndTreasureHuntLab() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntLab.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntLab.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance;} }
        
        public TreasureHuntGotTreasureInfo canPutInTreasureInfo { get { return _m_iCanPutInTreasureInfo; } }
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.canPutInTreasureInfoMono != null)
                _m_wCanPutInTreasureInfoWnd = new GGUIWndTreasureHuntTreasureInfo(wnd.canPutInTreasureInfoMono);
            
            if (wnd.selectedTreasureInfoMono != null)
                _m_wSelectedTreasureInfoWnd = new GGUIWndTreasureHuntTreasureInfo(wnd.selectedTreasureInfoMono);
            
            if (wnd.monoTreasureContainer != null)
            {
                _m_wTreasureContainerWnd = new GGUIWndTreasureHuntTreasureSelectItemContainer(wnd.monoTreasureContainer);
                _m_wTreasureContainerWnd.onSelectTreasure += _onSelectTreasure;
            }
            
            // 绑定按钮事件
            ALUGUICommon.combineBtnClick(wnd.btnPutIn, _onClickPutIn);
            ALUGUICommon.combineBtnClick(wnd.btnChgLab, _onClickChgLab);
            ALUGUICommon.combineBtnClick(wnd.btnPreLab, _onClickPreLab);
            ALUGUICommon.combineBtnClick(wnd.btnNextLab, _onClickNextLab);
            ALUGUICommon.combineBtnClick(wnd.btnCatalog, _onClickCatalog);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onClickReturn);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                // 解绑按钮事件
                ALUGUICommon.uncombineBtnClick(wnd.btnPutIn, _onClickPutIn);
                ALUGUICommon.uncombineBtnClick(wnd.btnChgLab, _onClickChgLab);
                ALUGUICommon.uncombineBtnClick(wnd.btnPreLab, _onClickPreLab);
                ALUGUICommon.uncombineBtnClick(wnd.btnNextLab, _onClickNextLab);
                ALUGUICommon.uncombineBtnClick(wnd.btnCatalog, _onClickCatalog);
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onClickReturn);
            }
         
            _m_wCanPutInTreasureInfoWnd?.discard();
            _m_wCanPutInTreasureInfoWnd = null;
            
            _m_wSelectedTreasureInfoWnd?.discard();
            _m_wSelectedTreasureInfoWnd = null;
            
            if (_m_wTreasureContainerWnd != null)
            {
                _m_wTreasureContainerWnd.onSelectTreasure -= _onSelectTreasure;
                _m_wTreasureContainerWnd.discard();
                _m_wTreasureContainerWnd = null;
            }

            _m_rLabRefObj = null;
            _m_iCanPutInTreasureInfo = null;
            _m_lShowTreasureList?.Clear();
            _m_lShowTreasureList = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wCanPutInTreasureInfoWnd?.hideWnd();
            _m_wSelectedTreasureInfoWnd?.hideWnd();
            _m_wTreasureContainerWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCanPutInTreasureInfoWnd?.resetWnd();
            _m_wSelectedTreasureInfoWnd?.resetWnd();
            _m_wTreasureContainerWnd?.resetWnd();
        }

        public void setData(TreasureHuntLabRefObj _labRefObj, TreasureHuntGotTreasureInfo _canPutInTreasureInfo, long _selectedTreasureId)
        {
            _m_rLabRefObj = _labRefObj;
            if(_m_lCanChgLabRefObjList == null)
                _m_lCanChgLabRefObjList = new List<TreasureHuntLabRefObj>();
            _m_lCanChgLabRefObjList.Clear();
            foreach (var labRefObj in GRefdataCoreMgr.instance.treasureHuntLabRefCore.refList)
            {
                if(labRefObj != null && labRefObj.isUnlock())
                    _m_lCanChgLabRefObjList.Add(labRefObj);
            }
            // _m_lCanChgLabRefObjList按照labRefObj.id升序排列
            _m_lCanChgLabRefObjList.Sort((_a, _b) =>
            {
                if (_b == null) return -1;
                if (_a == null) return 1;
                return _a.id.CompareTo(_b.id);
            });
            
            _m_iCanPutInTreasureInfo = _canPutInTreasureInfo;
            _m_lSelectedTreasureId = _selectedTreasureId;

            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口内容
        /// </summary>
        private void _refreshWnd()
        {
            if(wnd == null || _m_rLabRefObj == null || !isShow)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtLabName, TextTranslate.instance.getLanguage(_m_rLabRefObj.name));

            _refreshCanPutInTreasure();
            _refreshTreasureList();
            _refreshSelectedTreasure();
            
            _refreshPreNextLabBtn();
        }

        /// <summary>
        /// 刷新可放入的奇物信息
        /// </summary>
        private void _refreshCanPutInTreasure()
        {
            int canPutInTreasureCount = 0;
            if (_m_iCanPutInTreasureInfo == null)
            {
                canPutInTreasureCount = TreasureHuntUtil.getCanPutInLabTreasureCount(out _m_iCanPutInTreasureInfo);
            }
            else
            {
                canPutInTreasureCount = TreasureHuntUtil.getCanPutInLabTreasureCount(out TreasureHuntGotTreasureInfo _treasureInfo);
            }

            if (wnd != null)
                ALUGUICommon.setLabelTxt(wnd.txtCanPutInTreasureNum, canPutInTreasureCount.ToString());
            
            if (_m_iCanPutInTreasureInfo != null)
            {
                _m_wCanPutInTreasureInfoWnd?.showWnd();
                _m_wCanPutInTreasureInfoWnd?.setData(_m_iCanPutInTreasureInfo);
                
                if(wnd != null)
                    ALUGUICommon.setGameObjEnable(wnd.hasCanPutInTreasureShow, true);
            }
            else
            {
                _m_wCanPutInTreasureInfoWnd?.hideWnd();
                
                if(wnd != null)
                    ALUGUICommon.setGameObjEnable(wnd.hasCanPutInTreasureShow, false);
            }
        }
        
        /// <summary>
        /// 点击放入按钮
        /// </summary>
        private void _onClickPutIn(GameObject _go)
        {
            if (_m_rLabRefObj == null || !_m_rLabRefObj.isUnlock())
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_labLockCannotPutInTip_str, _m_rLabRefObj?.name));
                return;
            }

            if (_m_iCanPutInTreasureInfo == null)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.treasureHunt_noTreasureCanPutInLabTip_none);
                return;
            }

            NPPlayer.instance.treasureHuntComponent.reqTreasureHuntTreasureSkillActive(_m_iCanPutInTreasureInfo.treasureId,
                (_isSucc, _msg) =>
                {
                    if(!_isSucc || _msg == null)
                        return;

                    TreasureHuntGotTreasureInfo treasureInfo = _m_iCanPutInTreasureInfo;
                    if (_m_rLabRefObj != null && treasureInfo != null && treasureInfo.treasureRefObj != null
                        && treasureInfo.treasureRefObj.related_lab_id != _m_rLabRefObj.id)
                    {
                        GNodeTreasureHuntLab.addNodeBySelectedTreasureId(treasureInfo.treasureId);
                    }
                    else
                    {
                        _m_iCanPutInTreasureInfo = null;//放入成功后, 置为空
                        _refreshCanPutInTreasure();//刷新可放入奇物
                        _refreshTreasureList();
                        _refreshSelectedTreasure();
                    }
                    
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndTreasureHuntLabTreasurePutIn.instance, () =>
                    {
                        GGUIWndTreasureHuntLabTreasurePutIn.instance.setData(_m_rLabRefObj, treasureInfo);
                        GGUIWndTreasureHuntLabTreasurePutIn.instance.showWnd();
                    }, UINodeTagConst.C_TREASURE_HUNT_LAB_TREASURE_PUT_IN);
                });
        }

        /// <summary>
        /// 获取显示的奇物信息
        /// </summary>
        /// <param name="_treasureId"></param>
        /// <returns></returns>
        private _ITreasureHuntTreasureInfo _getShowTreasureInfo(long _treasureId)
        {
            return _m_lShowTreasureList?.Find((_item) =>
            {
                return _item != null && _item.treasureId == _treasureId;
            }) ?? null;
        }
        
        /// <summary>
        /// 刷新选中的奇物信息
        /// </summary>
        private void _refreshSelectedTreasure()
        {
            _ITreasureHuntTreasureInfo treasureInfo = _getShowTreasureInfo(_m_lSelectedTreasureId);
            _refreshSelectedTreasure(treasureInfo);
        }
        
        /// <summary>
        /// 刷新选中的奇物信息
        /// </summary>
        private void _refreshSelectedTreasure(_ITreasureHuntTreasureInfo _treasureInfo)
        {
            if (_treasureInfo != null)
            {
                _m_wSelectedTreasureInfoWnd?.showWnd();
                _m_wSelectedTreasureInfoWnd?.setData(_treasureInfo);
            }
            else
            {
                _m_wSelectedTreasureInfoWnd?.hideWnd();
            }
            
            WinMsg.SendMsg(WinMsgType.ON_TREASURE_HUNT_LAB_REFRESH_SELECTED_TREASURE, _treasureInfo);
        }
        
        /// <summary>
        /// 刷新显示奇物列表
        /// </summary>
        private void _refreshTreasureList()
        {
            if (_m_lShowTreasureList == null)
                _m_lShowTreasureList = new List<_ITreasureHuntTreasureInfo>();
            _m_lShowTreasureList.Clear();
            // 获取本实验室下的所有可展示奇物
            if (_m_rLabRefObj != null)
            {
                NPPlayer.instance.treasureHuntComponent.dealAllGotTreasure((_gotTreasureInfo) => 
                {
                    if (_gotTreasureInfo != null && _gotTreasureInfo.treasureRefObj != null && 
                        _gotTreasureInfo.treasureRefObj.related_lab_id == _m_rLabRefObj.id &&  
                        _gotTreasureInfo.treasureState == ETreasureHuntTreasureState.GOT_ACTIVATED)
                        _m_lShowTreasureList.Add(_gotTreasureInfo);
                });
            }
            
            // 品质 高->低, id 低->高 排序
            _m_lShowTreasureList.Sort((_a, _b) =>
            {
                if (_b == null || _b.treasureRefObj == null)
                    return -1;
                if (_a == null || _a.treasureRefObj == null)
                    return 1;
                if (ReferenceEquals(_a, _b))
                    return 0;

                int qualityCompare = _a.treasureRefObj.quality.CompareTo(_b.treasureRefObj.quality);
                if (qualityCompare != 0)
                    return -qualityCompare;
                
                return _a.treasureRefObj.id.CompareTo(_b.treasureRefObj.id);
            });
            
            // 显示列表更新后, 要检查是否需要更新选中物体列表, 若当前选中的物体不在显示列表中, 则需要更新选中物体
            // 更新逻辑为: 优先显示有产出可领取的奇物, 若没有, 则显示第一个奇物
            _ITreasureHuntTreasureInfo selectedTreasureInfo = _getShowTreasureInfo(_m_lSelectedTreasureId);
            if (selectedTreasureInfo == null)
            {
                foreach (var treasureInfo in _m_lShowTreasureList)
                {
                    if(treasureInfo == null)
                        continue;

                    TreasureHuntTreasureOutputInfo outputInfo = NPPlayer.instance.treasureHuntComponent.getTreasureOutputInfo(treasureInfo.treasureId);
                    if (outputInfo != null && outputInfo.canDraw)
                        selectedTreasureInfo = treasureInfo;
                }
                
                if (selectedTreasureInfo == null)
                    selectedTreasureInfo = _m_lShowTreasureList?.SafeGet(0);

                _m_lSelectedTreasureId = selectedTreasureInfo?.treasureId ?? 0;
                _refreshSelectedTreasure(selectedTreasureInfo);
            }

            if (_m_wTreasureContainerWnd != null)
            {
                _m_wTreasureContainerWnd.showWnd();
                _m_wTreasureContainerWnd.setData(_m_lShowTreasureList, selectedTreasureInfo);
            }
        }

        /// <summary>
        /// 刷新前/后实验室按钮显示
        /// </summary>
        private void _refreshPreNextLabBtn()
        {
            if(wnd == null)
                return;
            
            // 可切换的实验室列表为空或数量不足2个, 则不显示前后切换按钮
            if (_m_rLabRefObj == null || _m_lCanChgLabRefObjList == null || _m_lCanChgLabRefObjList.Count <= 1)
            {
                ALUGUICommon.setGameObjEnable(wnd.btnPreLab, false);
                ALUGUICommon.setGameObjEnable(wnd.btnNextLab, false);

                return;
            }
            
            int curLabIndex = _m_lCanChgLabRefObjList.FindIndex((_item) =>
            {
                return _item == _m_rLabRefObj;
            });

            // 找不到的情况, 则不显示前后切换按钮
            if (curLabIndex < 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.btnPreLab, false);
                ALUGUICommon.setGameObjEnable(wnd.btnNextLab, false);
                return;
            }
            
            // 非第一个实验室, 则显示前一个实验室按钮
            ALUGUICommon.setGameObjEnable(wnd.btnPreLab, curLabIndex > 0);
            // 非最后一个实验室, 则显示后一个实验室按钮
            ALUGUICommon.setGameObjEnable(wnd.btnNextLab, curLabIndex < _m_lCanChgLabRefObjList.Count - 1);

            bool needShowPreBtnRed = false;// 是否需要显示前一个实验室按钮红点
            bool needShowNextBtnRed = false;// 是否需要显示后一个实验室按钮红点
            foreach (var treasureOutputInfo in NPPlayer.instance.treasureHuntComponent.treasureOutputInfoList)
            {
                // 奇物数据有问题 或 不可领取, 则跳过
                if(treasureOutputInfo == null || treasureOutputInfo.treasureRefObj == null || !treasureOutputInfo.canDraw)
                    continue;
                
                if(treasureOutputInfo.treasureRefObj.related_lab_id < _m_rLabRefObj.id)
                    needShowPreBtnRed = true;
                else if(treasureOutputInfo.treasureRefObj.related_lab_id > _m_rLabRefObj.id)
                    needShowNextBtnRed = true;
                
                if(needShowNextBtnRed && needShowPreBtnRed)
                    break;
            }
            
            ALUGUICommon.setGameObjEnable(wnd.preLabBtnRedTip, needShowPreBtnRed);
            ALUGUICommon.setGameObjEnable(wnd.nextLabBtnRedTip, needShowNextBtnRed);
        }
        
        /// <summary>
        /// 当选中奇物时的回调
        /// </summary>
        /// <param name="_treasureInfo">选中的奇物信息</param>
        private void _onSelectTreasure(_ITreasureHuntTreasureInfo _treasureInfo)
        {
            _m_lSelectedTreasureId = _treasureInfo?.treasureId ?? 0;
            _refreshSelectedTreasure(_treasureInfo);
        }

        /// <summary>
        /// 点击切换实验室按钮
        /// </summary>
        private void _onClickChgLab(GameObject _go)
        {
            GGUIWndTreasureHuntLabSelect.instance.setData(_m_rLabRefObj);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndTreasureHuntLabSelect.instance, () =>
            {
                GGUIWndTreasureHuntLabSelect.instance.showWnd();
            }, UINodeTagConst.C_TREASURE_HUNT_SELECT_LAB);
        }

        /// <summary>
        /// 当点击上一个实验室按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickPreLab(GameObject _go)
        {
            if (_m_rLabRefObj == null || _m_lCanChgLabRefObjList == null || _m_lCanChgLabRefObjList.Count <= 1)
                return;

            int curLabIndex = _m_lCanChgLabRefObjList.FindIndex((_item) =>
            {
                return _item == _m_rLabRefObj;
            });

            TreasureHuntLabRefObj preLabRefObj = _m_lCanChgLabRefObjList.SafeGet(curLabIndex - 1);
            if (preLabRefObj != null)
                GNodeTreasureHuntLab.addNode(preLabRefObj);
        }
        
        /// <summary>
        /// 点击下一个实验室按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickNextLab(GameObject _go)
        {
            if (_m_rLabRefObj == null || _m_lCanChgLabRefObjList == null || _m_lCanChgLabRefObjList.Count <= 1)
                return;

            int curLabIndex = _m_lCanChgLabRefObjList.FindIndex((_item) =>
            {
                return _item == _m_rLabRefObj;
            });

            TreasureHuntLabRefObj nextLabRefObj = _m_lCanChgLabRefObjList.SafeGet(curLabIndex + 1);
            if (nextLabRefObj != null)
                GNodeTreasureHuntLab.addNode(nextLabRefObj);
        }
        
        /// <summary>
        /// 点击图鉴按钮
        /// </summary>
        private void _onClickCatalog(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndTreasureHuntCatalogMain.instance, UINodeTagConst.C_TREASURE_HUNT_CATALOG_MAIN, 0);
        }

        /// <summary>
        /// 点击返回按钮
        /// </summary>
        private void _onClickReturn(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_LAB);
        }
    }
}