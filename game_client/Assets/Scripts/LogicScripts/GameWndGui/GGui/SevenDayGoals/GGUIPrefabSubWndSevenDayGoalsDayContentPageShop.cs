using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndSevenDayGoalsDayContentPageShop : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoSevenDayGoalsDayContentPageShop>
    {
        private int _m_day;
        //钻石礼包列表
        private GGUIWndActivityCrystalGiftPackPageContainer _m_packContainer;
        //定时任务
        private ALCommonEnableTaskController _m_iCheckTask;
        
        
        public GGUIPrefabSubWndSevenDayGoalsDayContentPageShop(Transform _parent) 
            : base(_parent)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoSevenDayGoalsDayContentPageShop.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoSevenDayGoalsDayContentPageShop.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_packContainer?.showWnd();
            
            refreshWnd(true);
            
            WinMsg.RegisterMsg(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_REFRESH, _onGiftPackRefresh);
            WinMsg.RegisterMsg(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_BUY_RECORD_CHG, _onGiftPackBuyCountChg);

            _m_iCheckTask.setDisable();
            _m_iCheckTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_checkNeedRefresh, 1.0f);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_REFRESH, _onGiftPackRefresh);
            WinMsg.UnregisterMsg(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_BUY_RECORD_CHG, _onGiftPackBuyCountChg);
            
            _m_packContainer?.hideWnd();
            _m_iCheckTask.setDisable();
        }
        protected override void _onReset()
        {
            _m_packContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_packContainer?.discard();
            _m_packContainer = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoPackContainer != null)
                _m_packContainer = new GGUIWndActivityCrystalGiftPackPageContainer(wnd.monoPackContainer);
        }


        public void refreshWnd(int _day, bool _reset = false)
        {
            _m_day = _day;
            refreshWnd(_reset);
        }
        public void refreshWnd(bool _reset = false)
        {   
            if (wnd == null || !_m_bIsShow)
                return;
            
            List<ActivityCrystalGiftPackItemInfo> packList = getPackList();
            if (_m_packContainer != null)
            {
                packList.Sort(_sortList);
                _m_packContainer.showItemList(packList);
                if (_reset)
                    _m_packContainer.moveToTop();
            }
        }


        private List<ActivityCrystalGiftPackItemInfo> getPackList()
        {
            SevenDayGoalsGiftPackRefObj refObj = GRefdataCoreMgr.instance.sevenDayGoalsGiftPackRefCore.getRef(_m_day);
            if (refObj?.gift_pack_list == null)
                return new List<ActivityCrystalGiftPackItemInfo>(0);
            
            List<ActivityCrystalGiftPackItemInfo> packList = new List<ActivityCrystalGiftPackItemInfo>(refObj.gift_pack_list.Count);
            _ABaseActivityInfo activityInfo = NPPlayer.instance?.sevenDayGoalsComp?.data?.activityInfo;
            if (activityInfo?.crystalGiftPackInfo == null)
                return packList;
            
            foreach (long packId in refObj.gift_pack_list)
            {
                ActivityCrystalGiftPackItemInfo itemInfo = activityInfo.crystalGiftPackInfo.getCrystalGiftPackItemInfo(packId);
                if (itemInfo == null)
                    continue;
                
                packList.Add(itemInfo);
            }

            return packList;
        }
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
        private void _checkNeedRefresh()
        {
            _ABaseActivityInfo activityInfo = NPPlayer.instance?.sevenDayGoalsComp?.data?.activityInfo;
            if (activityInfo?.crystalGiftPackInfo == null)
                return;

            long serverTimeMs = FpsAndPingMgr.instance.serverTimeTag;

            //如果超过刷新时间，则请求刷新
            if (activityInfo.crystalGiftPackInfo.nextRefreshTimeMs > 0 && activityInfo.crystalGiftPackInfo.nextRefreshTimeMs <= serverTimeMs)
                NPPlayer.instance.commonActivityComp.reqRefreshActivityCrystalGiftPack(activityInfo.instanceId, activityInfo.crystalGiftPackInfo.giftPackGroupId);
        }
        //礼包刷新事件
        private void _onGiftPackRefresh(params object[] _objects)
        {
            _ABaseActivityInfo activityInfo = NPPlayer.instance?.sevenDayGoalsComp?.data?.activityInfo;
            if (_objects == null || _objects.Length == 0 || activityInfo == null)
                return;

            long activityInstanceId = (long)_objects[0];
            if (activityInfo.instanceId != activityInstanceId)
                return;

            //刷新窗口
            refreshWnd();
        }

        //礼包购买记录变更事件
        private void _onGiftPackBuyCountChg(params object[] _objects)
        {
            _ABaseActivityInfo activityInfo = NPPlayer.instance?.sevenDayGoalsComp?.data?.activityInfo;
            long activityInstanceId = (long)_objects[0];
            if (activityInfo == null || activityInfo.instanceId != activityInstanceId)
                return;

            //刷新窗口
            refreshWnd();
        }
    }
}