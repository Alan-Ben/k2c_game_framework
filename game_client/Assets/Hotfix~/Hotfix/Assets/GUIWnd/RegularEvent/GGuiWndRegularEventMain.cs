using ALPackage;
using Common.ActivityEnum;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 万能活动主界面，支持换皮
    /// </summary>
    public class GGuiWndRegularEventMain : _AHotfixBaseSubPrefabWnd<GGUIMonoRegularEventMain>
    {
        //活动id
        private long _m_lActivityId;
        
        protected override string _monoAssetPath { get { return GCommon.getActivityPrefabSkinAssetPath(_m_lActivityId,HotfixUINodeTagConst.REGULAR_EVENT_MAIN); } }
        protected override string _monoObjName { get { return GCommon.getActivityPrefabSkinObjName(_m_lActivityId, HotfixUINodeTagConst.REGULAR_EVENT_MAIN); } }

        public GGuiWndRegularEventMain(long _activityId, Transform _parent) : base(_parent)
        {
            _m_lActivityId =_activityId;
        }

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
            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnShop, _onClickShop);
        }

        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            ALUGUICommon.combineBtnClick(hotfixWnd.btnStart,_onClickStart);
            ALUGUICommon.combineBtnClick(hotfixWnd.btnShop, _onClickShop);
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
        }

        //处理使用道具
        private void _dealUseItem(RegularEventShopItemRefObj _shopItemRef)
        {
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_m_lActivityId);
            if (activityInfo == null || !activityInfo.isPlaying)
            {
                //活动已结束
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.common_activity_alreadyEnd_none);
                return;
            }

            GGuiWndRegularEventOperation operationWnd = new GGuiWndRegularEventOperation(_m_lActivityId);
            operationWnd.setShopItem(_shopItemRef);
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(operationWnd, HotfixUINodeTagConst.REGULAR_EVENT_OPERATION, 0);
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
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(new GGuiWndRegularEventOperation(_m_lActivityId), HotfixUINodeTagConst.REGULAR_EVENT_OPERATION, 0);
        }

        //点击商店按钮
        private void _onClickShop(GameObject _go)
        {
            GGuiWndRegularEventShop shopWnd = new GGuiWndRegularEventShop(_m_lActivityId);
            shopWnd.onUseItem += _dealUseItem;
            QueueMgr.instance.addNode_InGame_SingleWnd(shopWnd, shopWnd.showWnd, HotfixUINodeTagConst.REGULAR_EVENT_SHOP);
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