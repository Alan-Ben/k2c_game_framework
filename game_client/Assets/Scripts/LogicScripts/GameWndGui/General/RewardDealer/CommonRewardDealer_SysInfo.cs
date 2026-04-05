using System;
using System.Collections.Generic;
using ALPackage;
using NPCommon;
using NPEnum;

namespace GOE
{
    public partial class CommonRewardDealer
    {
        private static void _dealGainSysInfo(List<NPCommon_ItemInfo> _itemList, Action _onFinish)
        {
            if (null == _itemList || _itemList.Count <= 0)
            {
                if (_onFinish != null) 
                    _onFinish();
                return;
            }
            
            
            ALProcess alProcess = ALProcess.CreateProcess();
            
            foreach (NPCommon_ItemInfo commonItemInfo in _itemList)
            {
                //========判断是否是商店好评触发道具========
                NPCommonItem triggerStoreReviews = GRefdataCoreMgr.instance.npGeneral.store_reviews_trigger_common_item;
                if (triggerStoreReviews != null)
                {
                    if ((ENPItemType)commonItemInfo.getItemType() == triggerStoreReviews.itemType && commonItemInfo.getSubId() == triggerStoreReviews.itemId)
                    {
                        //判断是否可以展示商店评价界面
                        if (!canShowStoreReviews())
                        {
                            //发送埋点-触发商店好评，但是已评价过或在CD中，不打开窗口
                            GCommon.sendStepReport(TraceConst.TRIGGER_STORE_REVIEWS_NOT_OPEN_WND);
                            continue;
                        }

                        alProcess.addDelegateProcess((_done) =>
                        {
                            //发送埋点-触发商店好评
                            GCommon.sendStepReport(TraceConst.TRIGGER_STORE_REVIEWS);
                            //先记录弹窗打开时间加24小时为cd开始时间
                            GameSetting.instance.setStoreReviewsCdStartTimeMs(FpsAndPingMgr.instance.serverTimeTag + (24 * 3600 * 1000));
                            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndStoreReviewsMain.instance, () =>
                            {
                                GGUIWndStoreReviewsMain.instance.setInfo(_done);
                                GGUIWndStoreReviewsMain.instance.showWnd();
                            }, UINodeTagConst.C_STORE_REVIEWS_MAIN);
                        });
                        
                        continue;
                    }
                }
            }
            alProcess.addProcess(_onFinish);
            alProcess.deal();
        }
        
        /// <summary>
        /// 是否可以展示商店评价界面
        /// </summary>
        /// <returns></returns>
        public static bool canShowStoreReviews()
        {
            //是否已经评价过
            if (GameSetting.instance.getIsFinishStoreReviews() || NPPlayer.instance.playerInfoComp.clientDataRemarkInfo.isFinishStoreReviews())
                return false;

            //是否还在CD中
            long lastTriggerTimeMs = GameSetting.instance.getStoreReviewsCdStartTimeMs();
            if (lastTriggerTimeMs != 0 && (FpsAndPingMgr.instance.serverTimeTag - lastTriggerTimeMs) < GRefdataCoreMgr.instance.npGeneral.store_reviews_trigger_cd_time_hour * 3600 * 1000)
                return false;

            return true;
        }
    }
}