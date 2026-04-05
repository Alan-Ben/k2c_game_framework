using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会举办item
    /// </summary>
    public class GGUIWndDinnerCreateItem_Props : _AGGUIWndDinnerCreateItemBase
    {
        private string _m_sAssetPath;
        private string _m_sObjName;
        private GGUIMonoDinnerCreateItem_Props _m_realWndMono;
        private DinnerCreateItemShowInfo _m_itemInfo;
        private NPGGUIWndCommonItemContainer _m_costItemContainer;//消耗列表
        private NPGGuiWndTexture _m_banner;

        public GGUIWndDinnerCreateItem_Props(string _assetPath, string _objName, Transform _parent) : base(_parent)
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
            _m_banner?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_costItemContainer?.discard();
            _m_costItemContainer = null;
            
            _m_banner?.discard();
            _m_banner = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            _m_realWndMono = wnd as GGUIMonoDinnerCreateItem_Props;
            if(null == _m_realWndMono)
                return;
            ALUGUICommon.combineBtnClick(_m_realWndMono.btnCreate, _clickCreate);
            if (null != _m_realWndMono.costItemContainer)
                _m_costItemContainer = new NPGGUIWndCommonItemContainer(_m_realWndMono.costItemContainer);

            if (null != _m_realWndMono.texBanner)
                _m_banner = new NPGGuiWndTexture(_m_realWndMono.texBanner);
        }

        public override void setInfo(DinnerCreateItemShowInfo _info)
        {
            _m_itemInfo = _info;
            if(null == _m_itemInfo)
                return;
            if (null == _m_realWndMono)
                return;
            if(_m_itemInfo.dinnerTypeRef == null)
                return;

            ALUGUICommon.setLabelTxt(_m_realWndMono.txtName, TextTranslate.instance.getLanguage(_m_itemInfo.dinnerTypeRef.name));
            ALUGUICommon.setLabelTxt(_m_realWndMono.txtSeatCount, TextTranslate.instance.getLanguage(TransKeyConst.dinner_create_seat_count, _m_itemInfo.dinnerTypeRef.default_seat_num));
            ALUGUICommon.setLabelTxt(_m_realWndMono.txtDesc, TextTranslate.instance.getLanguage(_m_itemInfo.dinnerTypeRef.desc));
            
            _m_banner?.showWnd();
            _m_banner?.setTexture(_m_itemInfo.dinnerTypeRef.banner);
            
            //消耗不足或者cd中，置灰
            bool isGary = !GCommon.isItemEnough(_m_itemInfo.dinnerTypeRef.open_cost, false);
            GGameCommonInfo.grayImage(_m_realWndMono.goListGray,isGary);
            
            _m_costItemContainer?.showWnd();
            _m_costItemContainer?.showItemList(_m_itemInfo.dinnerTypeRef.open_cost);
        }

        public override void setCreatDinner()
        {
            _clickCreate(null);
        }

        /// <summary>
        /// 点击举办
        /// </summary>
        /// <param name="obj"></param>
        private void _clickCreate(GameObject obj)
        {
            if(_m_itemInfo == null)
                return;
            
            if (NPPlayer.instance.dinnerComp.dinnerInstanceId > 0)
                 return;
        
            if(!GCommon.isItemEnough(_m_itemInfo.dinnerTypeRef.open_cost,true))
                 return;
            NPPlayer.instance.dinnerComp.reqStartDinner(_m_itemInfo.dinnerTypeRef.dinner_id, (_info) =>
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
