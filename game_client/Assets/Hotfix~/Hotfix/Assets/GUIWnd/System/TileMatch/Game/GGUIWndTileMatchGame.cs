using ALPackage;
using Common.ActivityEnum;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 三消游戏页面
    /// </summary>
    public class GGUIWndTileMatchGame : _AHotfixBasicUIResBarWnd<GGUIMonoTileMatchGame>
    {
        private static GGUIWndTileMatchGame _g_instance = null;
        public static GGUIWndTileMatchGame instance { get { return _g_instance ??= new GGUIWndTileMatchGame(); } }

        private GGUIWndTileMatchGamePlay _m_wGamePlay;//游戏玩法子窗口
        
        public GGUIWndTileMatchGame() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(6101); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(6101); } }
        
        protected override void _onWndInitDoneHotfix()
        {
            if(hotfixWnd == null)
                return;

            if (hotfixWnd.monoGamePlay != null)
                _m_wGamePlay = new GGUIWndTileMatchGamePlay(hotfixWnd.monoGamePlay);
            
            ALUGUICommon.combineBtnClick(hotfixWnd.btnReturn, _onReturnBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (hotfixWnd != null)
            {
                ALUGUICommon.uncombineBtnClick(hotfixWnd.btnReturn, _onReturnBtnClick);
            }
            
            if(_m_wGamePlay != null)
                _m_wGamePlay.discard();
            _m_wGamePlay = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_wGamePlay?.showWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);

            _m_wGamePlay?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wGamePlay?.resetWnd();
        }

        /// <summary>
        /// 返回按钮被点击
        /// </summary>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(HotfixUINodeTagConst.TILEMATCH_GAME);
        }
        
        #region 消息事件

        //活动状态变更
        private void _onActivityStateChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 4)
                return;

            long activityId = (long)_objects[0];
            if (activityId != HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj.tilematch_activity_id)
                return;

            EActivityState nowActivityState = (EActivityState)_objects[3];
            if (nowActivityState != EActivityState.PLAYING)//若活动不处于进行中状态
            {
                NPMesMgr.instance.showOneBtnMes(
                    TextTranslate.instance.getLanguage(TransKeyConst.common_activity_alreadyEnd_none),
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                    () =>
                    {
                        //活动结束，退出所有活动界面
                        QueueMgr.instance.QuitUntilCanStop(_node =>
                        {
                            return _node != null && _node.nodeTag == HotfixUINodeTagConst.TILEMATCH_GAME;
                        }, true);
                    });
            }
        }

        #endregion
    }
}