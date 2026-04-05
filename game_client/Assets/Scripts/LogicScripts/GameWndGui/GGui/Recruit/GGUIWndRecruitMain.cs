using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndRecruitMain : _ANPGGUIBasicResBarWnd<GGUIMonoRecruitMain>
    {
        private static GGUIWndRecruitMain _g_instance;
        public static GGUIWndRecruitMain instance { get { return _g_instance ??= new GGUIWndRecruitMain(); } }

        public GGUIWndRecruitMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoRecruitMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoRecruitMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            }
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        /// <summary>
        /// 返回按钮点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_RECRUIT_MAIN);
        }
    }
}