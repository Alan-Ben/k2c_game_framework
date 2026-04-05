using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 学院主界面
    /// </summary>
    public class GGUIWndSchool : _ANPGGUIBasicResBarWnd<GGUIMonoSchool>
    {
        [NotNull] public static GGUIWndSchool instance { get { return _g_instance ??= new GGUIWndSchool(); } }
        private static GGUIWndSchool _g_instance;

        public GGUIWndSchool() : base(EALUIWndLayer.NORMAL)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoSchool.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoSchool.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        public override bool needDiscardOnSwitch { get { return true; } }


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
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Child.C_MAIN_SCHOOL_NODE);
        }
    }
}