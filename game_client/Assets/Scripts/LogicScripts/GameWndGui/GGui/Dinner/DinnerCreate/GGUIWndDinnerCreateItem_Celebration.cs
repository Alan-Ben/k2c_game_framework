using ALPackage;
using Common.DinnerEnum;
using Common.DinnerObj;
using UnityEngine;

namespace GOE
{
    public class GGUIWndDinnerCreateItem_Celebration : _AGGUIWndDinnerCreateItemBase
    {
        private string _m_sAssetPath;
        private string _m_sObjName;
        GGUIMonoDinnerCreateItem_Celebration _m_realWndMono;
        private DinnerCreateItemShowInfo _m_itemInfo;
        private DinnerPermit _m_permitInfo; // 开宴凭证信息 
        public GGUIWndDinnerCreateItem_Celebration(string _assetPath, string _objName, Transform _parent) : base(_parent)
        {
            _m_sAssetPath = _assetPath;
            _m_sObjName = _objName;
        }

        protected override string _monoAssetPath => _m_sAssetPath;

        protected override string _monoObjName => _m_sObjName;

        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            _m_realWndMono = wnd as GGUIMonoDinnerCreateItem_Celebration;
            if(null == _m_realWndMono)
                return;
            ALUGUICommon.combineBtnClick(_m_realWndMono.btnCreate, _clickCreate);
        }

        public override void setInfo(DinnerCreateItemShowInfo _info)
        {          
            _m_itemInfo = _info;
            if(null == _m_itemInfo)
                return;
            if (null == _m_realWndMono)
                return;
            _m_permitInfo = _m_itemInfo.permit;
            _refreshWnd();
        }

        public override void setCreatDinner()
        {
            _clickCreate(null);
        }

        private void _refreshWnd()
        {
            DinnerPermitRefObj permitRef = GRefdataCoreMgr.instance.dinnerPermitRefCore.getRef((long)_m_permitInfo.type);
            
            ALUGUICommon.setLabelTxt(_m_realWndMono.txtSeatCount, TextTranslate.instance.getLanguage(TransKeyConst.dinner_create_seat_count, _m_itemInfo.dinnerTypeRef.default_seat_num));
            ALUGUICommon.setLabelTxt(_m_realWndMono.txtName, TextTranslate.instance.getLanguage(_m_itemInfo.dinnerTypeRef.name));
            if(_m_permitInfo.type == EDinnerPermitType.TOWER_CELE)
                ALUGUICommon.setLabelTxt(_m_realWndMono.txtDesc, TextTranslate.instance.getLanguage(permitRef.permit_desc, _m_permitInfo.typeId));
            else
                ALUGUICommon.setLabelTxt(_m_realWndMono.txtDesc, TextTranslate.instance.getLanguage(permitRef.permit_desc));
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
                ALUGUICommon.setLabelTxt(_m_realWndMono.txtLifeTime,
                    TextTranslate.instance.getLanguage(TransKeyConst.dinner_permit_cd,TimeUtil.millisecondsToTime_hms(elapsedTs * 1000)));
            }
            else
            {
                GGUIWndDinnerCreate.instance.refreshWnd();
            }

            ALCommonTaskController.CommonActionAddMonoTask(_refreshPermitLifeTime, 1f);
        }
        
        /// <summary>
        /// 点击举办
        /// </summary>
        /// <param name="obj"></param>
        private void _clickCreate(GameObject obj)
        {
            if (NPPlayer.instance.dinnerComp.hasDinnerOpen)
                return;
            if (NPPlayer.instance.dinnerComp.dinnerInstanceId > 0)
                return;
            NPPlayer.instance.dinnerComp.reqStartDinnerByPermit(_m_itemInfo.permit.id, (_info) =>
            {
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DINNER_CREATE);
                GDinnerInfo dinnerInfo = new GDinnerInfo(_info.getInfo(), 0, false, false);
                GCommon.enterDinner(dinnerInfo, null , null, () =>
                {
                    NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_DinnerCreateSucc(dinnerInfo));
                });
            });
        }
    }
}