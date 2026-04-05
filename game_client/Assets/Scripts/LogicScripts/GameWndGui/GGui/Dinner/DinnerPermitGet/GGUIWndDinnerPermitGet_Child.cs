using System;
using ALPackage;
using Common.DinnerEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会凭证获得界面（子嗣庆功宴）
    /// </summary>
    public class GGUIWndDinnerPermitGet_Child : _ATALBasicUIWnd<GGUIMonoDinnerPermitGet_Child>
    {
        private static GGUIWndDinnerPermitGet_Child _g_instance = new GGUIWndDinnerPermitGet_Child();

        public static GGUIWndDinnerPermitGet_Child instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndDinnerPermitGet_Child();
                return _g_instance;
            }
        }

        public GGUIWndDinnerPermitGet_Child() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoDinnerPermitGet_Child.assetPath; }
        protected override string _monoObjName { get => GGUIMonoDinnerPermitGet_Child.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        private DinnerPermit _m_permitInfo;
        private GGUISubWndChildInfo _m_childCardItem;
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
            _m_childCardItem?.discard();
            _m_childCardItem = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnGo, _clickGo);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd) return;
            ALUGUICommon.combineBtnClick(wnd.btnGo, _clickGo);

            if (wnd.childCardItem != null)
                _m_childCardItem = new GGUISubWndChildInfo(wnd.childCardItem);
        }

        public void setInfo(DinnerPermit _permit, Action _onClose)
        {
            if (null == _permit) return;
            _m_permitInfo = _permit;
            _m_onClose = _onClose;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == _m_permitInfo) return;
            DinnerPermitRefObj permitRef = GRefdataCoreMgr.instance.dinnerPermitRefCore.getRef((long)_m_permitInfo.type);
            if (permitRef != null)
            {
                GDinnerTypeRefObj dinnerTypeRef = GRefdataCoreMgr.instance.dinnerTypeRefCore.getRef(permitRef.dinner_id);
                if (dinnerTypeRef != null)
                {
                    ALUGUICommon.setLabelTxt(wnd.txtSeatCount,
                        TextTranslate.instance.getLanguage(TransKeyConst.dinner_create_seat_count, dinnerTypeRef.default_seat_num));
                    ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(dinnerTypeRef.name));
                    _m_childCardItem?.hideWnd();
                    NPPlayer.instance.childComp.getAdultChildInfoById(_m_permitInfo.typeId, (_childInfo) =>
                    {
                        if (wnd == null) return;
                        ALUGUICommon.setLabelTxt(wnd.txtDesc,
                            TextTranslate.instance.getLanguage(dinnerTypeRef.desc, _childInfo?.name));
                        if (_childInfo != null)
                        {
                            _m_childCardItem?.showWnd();
                            _m_childCardItem?.refreshWnd(_childInfo);
                        }
                    });
                }
            }
            _refreshPermitLifeTime();
        }

        private void _refreshPermitLifeTime()
        {
            if (null == wnd || !isShow) return;
            long elapsedTs = _m_permitInfo.expiredTs - FpsAndPingMgr.instance.serverTimeTagS;
            if (elapsedTs > 0)
            {
                ALUGUICommon.setLabelTxt(wnd.txtLifeTime,
                    TextTranslate.instance.getLanguage(TransKeyConst.dinner_permit_cd, TimeUtil.millisecondsToTime_hms(elapsedTs * 1000)));
            }
            else
            {
                GGUIWndDinnerCreate.instance.refreshWnd();
            }
            ALCommonTaskController.CommonActionAddMonoTask(_refreshPermitLifeTime, 1f);
        }

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
