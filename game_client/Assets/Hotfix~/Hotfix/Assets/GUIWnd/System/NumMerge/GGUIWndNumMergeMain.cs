using ALPackage;
using Common.ActivityEnum;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 2048 活动页面
    /// </summary>
    public class GGUIWndNumMergeMain : _AHotfixBaseSubPrefabWnd<GGUIMonoNumMergeMain>
    {
        //活动id
        private readonly long _m_lActivityId;
        

        public GGUIWndNumMergeMain(long _activityId, Transform _parent) 
            : base(_parent)
        {
            _m_lActivityId = _activityId;
        }
        

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(8600); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(8600); } }
        
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            _refreshWnd();
            _checkActivity();
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (hotfixWnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnStart, _onClickStart);
            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnHandbook, _onClickHandbook);
        }
        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            ALUGUICommon.combineBtnClick(hotfixWnd.btnStart, _onClickStart);
            ALUGUICommon.combineBtnClick(hotfixWnd.btnHandbook, _onClickHandbook);
        }
        

        //刷新窗口
        private void _refreshWnd()
        {
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_m_lActivityId);
            if (hotfixWnd == null || activityInfo == null)
                return;

            ALUGUICommon.setGameObjEnable(hotfixWnd.goRunningShowList, activityInfo.isPlaying);
            ALUGUICommon.setGameObjEnable(hotfixWnd.goEndShowList, !activityInfo.isPlaying);
            GGameCommonInfo.grayImage(hotfixWnd.goGrayList, !activityInfo.isPlaying);
        }
        //检查活动是否还在进行中
        private void _checkActivity()
        {
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_m_lActivityId);

            //活动不存在或者不在活动期，弹窗提示关闭窗口
            if (activityInfo == null || (!activityInfo.isEnable))
            {
                NPMesMgr.instance.showOneBtnMes(
                    TextTranslate.instance.getLanguage(TransKeyConst.common_activity_alreadyEnd_none),
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                    () =>
                    {
                        //活动结束，退出所有活动界面
                        QueueMgr.instance.QuitUntilCanStop(_node =>
                        {
                            return _node != null && _node.nodeTag == UINodeTagConst.C_ACTIVITY_CENTER_WND;
                        });
                    });
            }
            else
            {
                // 判断是否需要显示第一次进入游戏对话
                if (hotfixWnd != null && HotfixAccountSettingMgr.instance.hotfixAccountSetting.checkNeedShowFirstEnterNumMergeActivityDialog(activityInfo.startTimeMs))
                {
                    GCommon.enterDialogueNode(hotfixWnd.firstEnterShowDialogId, null, false);
                }
            }
        }

        #region 点击事件

        //点击开始按钮
        private void _onClickStart(GameObject _go)
        {
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_m_lActivityId);
            if(activityInfo == null || !activityInfo.isPlaying)
            {
                //活动已结束
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.common_activity_alreadyEnd_none);
                return;
            }

            //活动进行中，打开二级操作界面
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndNumMergeGame.instance, HotfixUINodeTagConst.NUMMERGE_GAME, 0);
        }

        //点击图鉴按钮
        private void _onClickHandbook(GameObject _go)
        {
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_m_lActivityId);
            if(activityInfo == null || !activityInfo.isPlaying)
            {
                //活动已结束
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.common_activity_alreadyEnd_none);
                return;
            }

            //打开图鉴界面
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndNumMergeHandbook.instance, GGUIWndNumMergeHandbook.instance.showWnd, HotfixUINodeTagConst.NUMMERGE_HANDBOOK);
        }

        #endregion

        #region 消息事件

        //活动状态变更
        private void _onActivityStateChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 4)
                return;

            long activityId = (long)_objects[0];
            if (activityId != _m_lActivityId)
                return;

            _checkActivity();
            _refreshWnd();
        }

        #endregion
    }
}
