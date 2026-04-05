using System;
using ALPackage;
using Common.DinnerEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndDinnerPermitGet : _ATALBasicUIWnd<GGUIMonoDinnerPermitGet>
    {
        private static GGUIWndDinnerPermitGet _g_instance = new GGUIWndDinnerPermitGet();

        public static GGUIWndDinnerPermitGet instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndDinnerPermitGet();
                return _g_instance;
            }
        }

        public GGUIWndDinnerPermitGet() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoDinnerPermitGet.assetPath; }
        protected override string _monoObjName { get => GGUIMonoDinnerPermitGet.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        private DinnerPermit _m_permitInfo;
        private GGUIWndConsortIconItem _m_consortCardItem;
        private Action _m_onClose;

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
            _m_consortCardItem?.discard();
            _m_consortCardItem = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnGo, _clickGo);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnGo, _clickGo);

            if (wnd.consortCardItem != null)
                _m_consortCardItem = new GGUIWndConsortIconItem(wnd.consortCardItem);
            
        }

        public void setInfo(DinnerPermit _permit, Action _onClose)
        {
            if(null == _permit)
                return;
            _m_permitInfo = _permit;
            _m_onClose = _onClose;
            _refreshWnd();
        }


        private void _refreshWnd()
        {
            if(null == _m_permitInfo || wnd == null || !isShow)
                return;
            DinnerPermitRefObj permitRef = GRefdataCoreMgr.instance.dinnerPermitRefCore.getRef((long)_m_permitInfo.type);
            if (permitRef != null)
            {
                GDinnerTypeRefObj dinnerTypeRef = GRefdataCoreMgr.instance.dinnerTypeRefCore.getRef(permitRef.dinner_id);

                if (dinnerTypeRef != null)
                {
                    ALUGUICommon.setLabelTxt(wnd.txtSeatCount,
                        TextTranslate.instance.getLanguage(TransKeyConst.dinner_create_seat_count,
                            dinnerTypeRef.default_seat_num));
                    ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(dinnerTypeRef.name));
                    GConsortRefObj consortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_permitInfo.typeId);
                    ALUGUICommon.setLabelTxt(wnd.txtDesc,
                        TextTranslate.instance.getLanguage(dinnerTypeRef.desc, consortRefObj?.transName));

                    bool isFamily = permitRef.permit_type == EDinnerPermitType.FAMILY;
                    if (isFamily) 
                    {
                        _m_consortCardItem?.showWnd();
                        _m_consortCardItem?.setInfo(new ConsortInfo(consortRefObj), 0);
                    }
                    else
                    {
                        _m_consortCardItem?.hideWnd();
                    }
                    ALUGUICommon.setGameObjEnable(wnd.familyPermitShowGos, isFamily);
                    ALUGUICommon.setGameObjEnable(wnd.familyPermitHideGos, !isFamily);
                }
            }

            _refreshPermitLifeTime();
        }

        /// <summary>
        /// 刷新凭证使用倒计时
        /// </summary>
        private void _refreshPermitLifeTime()
        {
            if (null == wnd || !isShow)
                return;
            long elapsedTs = _m_permitInfo.expiredTs - FpsAndPingMgr.instance.serverTimeTagS;
            if (elapsedTs > 0)
            {
                ALUGUICommon.setLabelTxt(wnd.txtLifeTime,
                    TextTranslate.instance.getLanguage(TransKeyConst.dinner_permit_cd,TimeUtil.millisecondsToTime_hms(elapsedTs * 1000)));
            }
            else
            {
                GGUIWndDinnerCreate.instance.refreshWnd();
            }

            ALCommonTaskController.CommonActionAddMonoTask(_refreshPermitLifeTime, 1f);
        }
    
        /// <summary>
        /// 点击前往
        /// </summary>
        /// <param name="obj"></param>
        private void _clickGo(GameObject obj)
        {
            _m_onClose?.Invoke();
            
            QueueMgr.instance.AddNode(new GNodeDinnerEntry());
            
            if (!NPPlayer.instance.dinnerComp.hasDinnerOpen)
            {
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndDinnerCreate.instance,
                    GGUIWndDinnerCreate.instance.showWnd, UINodeTagConst.C_DINNER_CREATE);
            }
            else
            {
                NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(TransKeyConst.dinner_permit_get_go_to_create_tip));
            }
        }


     
    }
}