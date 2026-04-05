using ALPackage;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 公会协作属性据点完成弹窗
    /// </summary>
    public class GGUIWndGuildCooperateAttrPosFinish : _ANPGGUIBasicWnd<GGUIMonoGuildCooperateAttrPosFinish>
    {
        private static GGUIWndGuildCooperateAttrPosFinish _g_instance;
        public static GGUIWndGuildCooperateAttrPosFinish instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndGuildCooperateAttrPosFinish();
                return _g_instance;
            }
        }

        // 奖励据点图标
        private NPGGuiWndTexture _m_wPosIcon;

        public GGUIWndGuildCooperateAttrPosFinish() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildCooperateAttrPosFinish.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGuildCooperateAttrPosFinish.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wPosIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wPosIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wPosIcon?.discard();
            _m_wPosIcon = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);

            //同时关闭属性据点详情界面
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_COOPERATE_ATTR_POINT_DETAIL);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgPosIcon != null)
                _m_wPosIcon = new NPGGuiWndTexture(wnd.imgPosIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_attrType"></param>
        public void setInfo(ESpecAttrType _attrType, GuildCooperateAreaPosRefObj _areaPosRef)
        {
            if (wnd == null)
                return;

            BasicAttrRefObj attrRefObj = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)_attrType);
            if(attrRefObj == null)
                return;

            _m_wPosIcon?.showWnd();
            _m_wPosIcon?.setTexture(_areaPosRef?.icon);
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_COOPERATE_ATTR_POINT_FINISH);
        }
    }
}