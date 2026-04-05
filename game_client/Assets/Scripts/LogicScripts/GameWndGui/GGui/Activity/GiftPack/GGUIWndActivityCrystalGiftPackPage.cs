using System.Collections.Generic;
using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用活动礼包界面钻石页面
    /// </summary>
    public class GGUIWndActivityCrystalGiftPackPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoActivityCrystalGiftPackPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //活动信息
        private _ABaseActivityInfo _m_activityInfo;
        //钻石礼包列表
        private GGUIWndActivityCrystalGiftPackPageContainer _m_wCrystalGiftPackContainer;
        //钻石礼包信息列表
        private List<ActivityCrystalGiftPackItemInfo> _m_lCrystalGiftPackList;
        //定时任务
        private ALCommonEnableTaskController _m_iCheckTask;

        public GGUIWndActivityCrystalGiftPackPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
            : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo?.asset_path;
            _m_sObjName = _assetPathInfo?.obj_name;
        }

        /**************
         * 窗口相关加载配置
         **/
        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_REFRESH, _onGiftPackRefresh);
            WinMsg.RegisterMsg(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_BUY_RECORD_CHG, _onGiftPackBuyCountChg);
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_REFRESH, _onGiftPackRefresh);
            WinMsg.UnregisterMsg(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_BUY_RECORD_CHG, _onGiftPackBuyCountChg);
            _m_wCrystalGiftPackContainer?.hideWnd();
            _m_iCheckTask.setDisable();
            _m_lCrystalGiftPackList?.Clear();
            _m_lCrystalGiftPackList = null;
        }
        
        protected override void _onReset()
        {
            _m_wCrystalGiftPackContainer?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wCrystalGiftPackContainer?.discard();
            _m_wCrystalGiftPackContainer = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.monoCrystalGiftPackContainer != null)
                _m_wCrystalGiftPackContainer = new GGUIWndActivityCrystalGiftPackPageContainer(wnd.monoCrystalGiftPackContainer);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(_ABaseActivityInfo _activityInfo)
        {
            _m_activityInfo = _activityInfo;
            _refreshWnd();

            //设置定时检查任务
            _m_iCheckTask.setDisable();
            _m_iCheckTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_checkNeedRefresh, 1.0f);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_activityInfo == null || _m_activityInfo.crystalGiftPackInfo == null)
                return;

            if (_m_lCrystalGiftPackList == null)
                _m_lCrystalGiftPackList = new List<ActivityCrystalGiftPackItemInfo>();
            _m_lCrystalGiftPackList.Clear();

            _m_activityInfo.crystalGiftPackInfo?.getCrystalGiftPackList(_m_lCrystalGiftPackList);

            //设置钻石礼包列表
            _m_lCrystalGiftPackList?.Sort(_sortList);
            _m_wCrystalGiftPackContainer?.showWnd();
            _m_wCrystalGiftPackContainer?.showItemList(_m_lCrystalGiftPackList);

            //刷新描述
            CrystalGiftPackGroupRefObj giftPackGroupRef = GRefdataCoreMgr.instance.crystalGiftPackGroupRefCore.getRef(_m_activityInfo.crystalGiftPackInfo.giftPackGroupId);
            ALUGUICommon.setLabelTxt(wnd.txtRefreshDesc, TextTranslate.instance.getLanguage(giftPackGroupRef?.refresh_desc));
        }

        //排序，未售罄>已售罄，排序id从小到大
        private int _sortList(ActivityCrystalGiftPackItemInfo _a, ActivityCrystalGiftPackItemInfo _b)
        {
            if (_a == null || _b == null)
                return 0;

            int comp = _a.isSellOut.CompareTo(_b.isSellOut);
            if (comp != 0)
                return comp;

            if (_a.crystalGiftPackRef == null || _b.crystalGiftPackRef == null)
                return 0;
            else
                return _a.crystalGiftPackRef.sort_id.CompareTo(_b.crystalGiftPackRef.sort_id);
        }

        //检查是否需要更新商店
        private void _checkNeedRefresh()
        {
            if (_m_activityInfo == null || _m_activityInfo.crystalGiftPackInfo == null)
                return;

            long serverTimeMs = FpsAndPingMgr.instance.serverTimeTag;

            //如果超过刷新时间，则请求刷新
            if (_m_activityInfo.crystalGiftPackInfo.nextRefreshTimeMs > 0 &&
                _m_activityInfo.crystalGiftPackInfo.nextRefreshTimeMs <= serverTimeMs)
            {
                _m_iCheckTask.setDisable();
                NPPlayer.instance.commonActivityComp.reqRefreshActivityCrystalGiftPack(_m_activityInfo.instanceId, _m_activityInfo.crystalGiftPackInfo.giftPackGroupId);
            }
        }

        #region 消息事件

        //礼包刷新事件
        private void _onGiftPackRefresh(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _m_activityInfo == null)
                return;

            long activityInstanceId = (long)_objects[0];
            if (_m_activityInfo == null || _m_activityInfo.instanceId != activityInstanceId)
                return;

            //刷新窗口
            _refreshWnd();
            //设置定时检查任务
            _m_iCheckTask.setDisable();
            _m_iCheckTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_checkNeedRefresh, 1.0f);
        }

        //礼包购买记录变更事件
        private void _onGiftPackBuyCountChg(params object[] _objects)
        {
            long activityInstanceId = (long)_objects[0];
            if (_m_activityInfo == null || _m_activityInfo.instanceId != activityInstanceId)
                return;

            //刷新窗口
            _refreshWnd();
        }

        #endregion
    }
}