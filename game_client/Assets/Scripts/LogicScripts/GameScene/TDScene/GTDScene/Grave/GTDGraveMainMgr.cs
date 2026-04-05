using System;
using System.Collections.Generic;
using ALPackage;
using Common.GraveObj;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 杰出者大厅主界面TD场景管理器
    /// </summary>
    public class GTDGraveMainMgr
    {
        private GTDGraveMainSceneMono _m_sceneMono;

        private List<GTDGraveMainFollowView> _m_lViewList;
        private List<GGUISubWndGraveMainItem> _m_graveShowItems ;

        private bool _m_bIsInit;
        private bool _m_bInitDone;
        private Action _m_aOnInitDone;

        public long _m_graveMainId = 1;

        public GTDGraveMainMgr()
        {
            _m_bIsInit = false;
        }

        public void init([NotNull] GTDGraveMainSceneMono _mono, long _graveMainId,List<Common.GraveObj.GraveObj_NewInfo> _newInfoList,  Action _onInitDoneAction = null)
        {
            if (_m_bInitDone)//先判断是否初始化完成, 若已经完成, 直接调用回调
            {
                _onInitDoneAction?.Invoke();
                return;
            }
            _m_graveMainId = _graveMainId;
            
            regOnInitDoneDelegate(_onInitDoneAction);

            if (_m_bIsInit)//判断是否已经调用过init, 若已经调用过, 只需要注册回调
                return;

            _m_bIsInit = true;
            
            _m_sceneMono = _mono;      
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(()=>
            {
                _updateGraveMain();
                _onInitDone();
            });
            
            if (_m_lViewList == null)
                _m_lViewList = new List<GTDGraveMainFollowView>();
            
            _m_graveShowItems = new List<GGUISubWndGraveMainItem>();
            if (_m_sceneMono == null )
                return;
            if (_m_sceneMono.itemList != null)
                foreach (GTDGraveMainFollowMono item in _m_sceneMono.itemList)
                {
                    if (item != null && item.itemMono != null)
                    {
                        GraveTypeRefObj graveTypeRef = GRefdataCoreMgr.instance.graveTypeRefCore.getRef(item.graveTypeId);
                        if (graveTypeRef != null)
                        {
                            GGUISubWndGraveMainItem graveShowItem = new GGUISubWndGraveMainItem(graveTypeRef, item.itemMono);
                            _m_graveShowItems.Add(graveShowItem);
                        }
                    }
                }
            

            if (_m_sceneMono.itemList != null)
            {
                if (_newInfoList != null)
                {
                    foreach (GGUISubWndGraveMainItem graveShowItem in _m_graveShowItems)
                    {
                        if(graveShowItem == null || graveShowItem.wnd == null) continue;
                        GraveTypeRefObj graveTypeRef = GRefdataCoreMgr.instance.graveTypeRefCore.getRef(graveShowItem.typeId);

                        long cid = 0;
                        long titleId = 0;
                        if (graveTypeRef != null && graveTypeRef.player_title_id_list != null)
                        {
                            foreach (var newInfo in _newInfoList)
                            {
                                if(newInfo == null) continue;
                                if (graveTypeRef.player_title_id_list.Contains(newInfo.getTitleId()))
                                {
                                    cid = newInfo.getCid();
                                    titleId = newInfo.getTitleId();
                                    break;
                                }
                            }
                        }

                        graveShowItem.setInfo(cid, titleId);
                    }
                }
                
            }  
            stepCounter.addDoneStepCount();
            
        }

        public void discard()
        {
            if (!_m_bIsInit)
                return;

            _m_bIsInit = false;
            _m_bInitDone = false;
            
            _m_sceneMono = null;

            if (_m_lViewList != null)
            {
                foreach (var view in _m_lViewList)
                {
                    view?.discard();
                }
                _m_lViewList.Clear();
            }
            _m_lViewList = null;
            
            if (_m_graveShowItems != null)
            {
                foreach (GGUISubWndGraveMainItem item in _m_graveShowItems)
                {
                    if (item != null) 
                        item.discard();
                }
                _m_graveShowItems = null;
            }
            
        }

        public void changeGraveMain(long _graveMainId)
        {
            _m_graveMainId = _graveMainId;
            _updateGraveMain();
        }

        private void _updateGraveMain()
        {
            GraveMainRefObj graveMainRef = GRefdataCoreMgr.instance.graveMainRefCore.getRef(_m_graveMainId);
            if(graveMainRef == null || graveMainRef.type_id_list == null)
                return;
            foreach (GGUISubWndGraveMainItem graveShowItem in _m_graveShowItems)
            {
                if(graveShowItem == null)
                    continue;
                bool isShow = graveMainRef.type_id_list.Contains(graveShowItem.typeId);
                if(isShow)
                    graveShowItem.showWnd();
                else
                    graveShowItem.hideWnd();
            }
            if (_m_sceneMono != null && _m_sceneMono.hallShowItems != null)
            {
                foreach (GraveHallShow item in _m_sceneMono.hallShowItems)
                {
                    if (item == null) continue;
                    ALUGUICommon.setGameObjEnable(item.showGos, item.hallId == _m_graveMainId);
                }
            }
        }

        public void regOnInitDoneDelegate(Action _action)
        {
            if(_action == null)
                return;

            if (_m_bInitDone)
            {
                _action();
                return;                
            }

            _m_aOnInitDone += _action;
        }

        private void _onInitDone()
        {
            if(!_m_bIsInit)//初始化完成后, 先判断_m_bIsInit是否为false, 为false则表示还未调用过init方法 或 已经调用了discard方法进行了销毁, 这两种情况都不需要后续操作
                return;

            _m_bInitDone = true;
            
            Action action = _m_aOnInitDone;
            _m_aOnInitDone = null;
            
            action?.Invoke();
        }
    }
}