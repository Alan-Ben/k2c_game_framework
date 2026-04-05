using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnDishUnlockSuccess : _ATALBasicUIWnd<GGUIMonoInnDishUnlockSuccess>
    {
        [NotNull] public static GGUIWndInnDishUnlockSuccess instance { get { return _g_instance ??= new GGUIWndInnDishUnlockSuccess(); } }
        private static GGUIWndInnDishUnlockSuccess _g_instance;

        // 菜品信息
        private InnDishInfo _m_dishInfo;
        // 菜品图标
        private NPGGuiWndTexture _m_dishIconWnd;


        public GGUIWndInnDishUnlockSuccess()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoInnDishUnlockSuccess.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnDishUnlockSuccess.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_dishIconWnd?.showWnd();
            refreshWnd();
        }        
        protected override void _onHideWnd()
        {
            _m_dishIconWnd?.hideWnd();
        }        
        protected override void _onReset()
        {
            _m_dishIconWnd?.discardTexture();
        }        
        protected override void _onDiscard()
        {
            _m_dishInfo = null;
            
            _m_dishIconWnd?.discard();
            _m_dishIconWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgDishIcon != null)
                _m_dishIconWnd = new NPGGuiWndTexture(wnd.imgDishIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        /// <summary>
        /// 设置菜品信息并刷新窗口
        /// </summary>
        /// <param name="_dishInfo">菜品信息</param>
        public void refreshWnd(InnDishInfo _dishInfo)
        {
            _m_dishInfo = _dishInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_dishInfo == null)
                return;

            // 设置菜品名字
            ALUGUICommon.setLabelTxt(wnd.txtDishName, _m_dishInfo.nameTranslated);
            // 设置菜品描述
            ALUGUICommon.setLabelTxt(wnd.txtDishDesc, _m_dishInfo.descTranslated);
            // 设置菜品图标
            _m_dishIconWnd?.setTexture(_m_dishInfo.refObj.icon);
        }
        

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        /// <param name="_go">按钮对象</param>
        private void _onBtnCloseClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_DISH_UNLOCK_SUCCESS);
        }
    }
}