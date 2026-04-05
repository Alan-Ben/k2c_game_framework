using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using GS2GC.p004_PlayerOp;
using NPEnum;

namespace GOE
{
    //创角界面
    public class GGUIWndCreatePlayerPrefab : _ANPGGUIBasicWnd<GGUIMonoCreatePlayerPrefab>
    {
        private static GGUIWndCreatePlayerPrefab _g_instance;
        public static GGUIWndCreatePlayerPrefab instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndCreatePlayerPrefab();
                return _g_instance;
            }
        }
        
        private PlayerCreatPlayerPrefabRefObj _m_curSelectedPrefabRefObj;
        
        //消耗item
        private NPGGUIWndCommonShowCase _m_commonShowCase;
        //容器
        private GGUIWndCreatePlayerPrefabContainer _m_containerWnd;


        public GGUIWndCreatePlayerPrefab(): base(EALUIWndLayer.NORMAL)
        {
            
        }

        protected override string _monoAssetPath { get { return GGUIMonoCreatePlayerPrefab.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoCreatePlayerPrefab.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            if(null != wnd.iconContainer)
            {
                _m_containerWnd = new GGUIWndCreatePlayerPrefabContainer(wnd.iconContainer);
                _m_containerWnd.onSelectItemChg += _onClickItemWnd;
            }

            if (wnd.showCaseMono != null) 
                _m_commonShowCase = new NPGGUIWndCommonShowCase(wnd.showCaseMono);

            ALUGUICommon.combineBtnClick(wnd.selectBtn, _confirmBtnDidClick);

        }
        
        protected override void _onShowWnd()
        {
            //关闭回退
            QueueMgr.instance.CloseRollBack(NodeESC_Const.C_QUEUE_ESC_CREATE_PLAYER_PREFAB);
            List<PlayerCreatPlayerPrefabRefObj> prefabRefObjList = new List<PlayerCreatPlayerPrefabRefObj>();
            prefabRefObjList.AddRange(GRefdataCoreMgr.instance.playerCreatPlayerPrefabRefCore.refList);
            
            if (null != _m_containerWnd)
            {
                _m_containerWnd.setInfoList(prefabRefObjList);
                _m_containerWnd.setIndexSelected(0);
                _m_containerWnd.showWnd();
            }
            
        }
        
        protected override void _onHideWnd()
        {
            //恢复回退
            QueueMgr.instance.OpenRollBack(NodeESC_Const.C_QUEUE_ESC_CREATE_PLAYER_PREFAB);
            if (_m_commonShowCase != null) 
                _m_commonShowCase.hideWnd();

            if (_m_containerWnd != null) 
                _m_containerWnd.hideWnd();

            _m_curSelectedPrefabRefObj = null;
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (null != _m_containerWnd)
                _m_containerWnd.discard();
            _m_containerWnd = null;

            if (_m_commonShowCase != null) 
                _m_commonShowCase.discard();
            _m_commonShowCase = null;

            _m_curSelectedPrefabRefObj = null;
            
            if (null == wnd)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.selectBtn, _confirmBtnDidClick);
        }
    
        /// <summary>
        /// 点击使用按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _confirmBtnDidClick(GameObject _go)
        {
            if(null == _m_curSelectedPrefabRefObj)
                return;

            NPGSClientListener.sendRequestByLog(
                NPGSWriter_004_PlayerOp.make_006_ReqSetPrefab(_m_curSelectedPrefabRefObj.id)
                , new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_006_RetSetPrefab>(
                    (_res) =>
                    {
                        //发送选择预设成功
                        WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.SELECT_PREFAB_SCU);

                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_OPEN_SET_PREFAB_WND);
                    }
                ));
        }

        private void _onClickItemWnd(PlayerCreatPlayerPrefabRefObj _prefabRef)
        {
            if (null == _prefabRef)
                return;

            _m_curSelectedPrefabRefObj = _prefabRef;
            
            PlayerSkinRefObj skinRef = GRefdataCoreMgr.instance.playerSkinRefCore.getRef(_prefabRef.skin_id);
            if (skinRef == null)
                return;

            if (_m_commonShowCase != null)
            {
                _m_commonShowCase.showWnd(new ShowCaseCommonResUnitInfoObj(skinRef.td_show));
            }
        }
    }
}
