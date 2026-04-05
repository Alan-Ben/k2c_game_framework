using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsBuildQueueContainerItem : _ATALBasicUISubWnd<GGUIMonoMarsBuildQueueContainerItem>
    {
        private int _m_queueIndex;
        private new bool _m_bIsShow;


        public GGUISubWndMarsBuildQueueContainerItem(GGUIMonoMarsBuildQueueContainerItem _wnd)
            : base(_wnd)
        {
            _m_queueIndex = -1;
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_bIsShow = true;
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_bIsShow = false;
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnJump, _onClickJump);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnJump, _onClickJump);
        }


        public void refreshWnd(int _queueIndex)
        {
            _m_queueIndex = _queueIndex;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_queueIndex < 0)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtQueueName, TextTranslate.instance.getLanguage(TransKeyConst.mars_buildingQueueName_index, _m_queueIndex + 1));
            MarsBuildingInfo buildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoByQueueIndex(_m_queueIndex);
            if (buildingInfo != null)
            {
                switch (buildingInfo.state)
                {
                    case MarsBuildingInfo.StateType.Unbuilt:
                    case MarsBuildingInfo.StateType.Normal:
                        ALUGUICommon.setLabelTxt(wnd.txtUsingDesc, string.Empty);
                        break;
                    case MarsBuildingInfo.StateType.Constructing:
                        ALUGUICommon.setLabelTxt(wnd.txtUsingDesc, TextTranslate.instance.getLanguage(TransKeyConst.mars_buildingQueueConstructingDesc_name, buildingInfo.nameTranslated));
                        break;
                    case MarsBuildingInfo.StateType.Upgrading:
                        ALUGUICommon.setLabelTxt(wnd.txtUsingDesc, TextTranslate.instance.getLanguage(TransKeyConst.mars_buildingQueueUpgradingDesc_name, buildingInfo.nameTranslated));
                        break;
                }

                long remainingTime = buildingInfo.remainingBuildOrUpgradeTime;
                long totalTime = buildingInfo.buildOrUpgradeEndTime - buildingInfo.buildOrUpgradeStartTime;
                ALUGUICommon.setLabelTxt(wnd.txtRemainTime, TimeUtil.millisecondsToTime_DayHourOrHMS(remainingTime));
                if (wnd.sldRemainTime != null)
                {
                    wnd.sldRemainTime.minValue = 0;
                    wnd.sldRemainTime.maxValue = totalTime;
                    wnd.sldRemainTime.value = totalTime - remainingTime;
                }

                wnd.setIdleState(false);
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtRemainTime, string.Empty);
                if (wnd.sldRemainTime != null)
                {
                    wnd.sldRemainTime.minValue = 0;
                    wnd.sldRemainTime.maxValue = 1;
                    wnd.sldRemainTime.value = 0;
                }

                wnd.setIdleState(true);
            }

            NPPlayerBuffInfo buffInfo = NPPlayer.instance.playerBuffComp.lookup(GRefdataCoreMgr.instance.npGeneral.mars_temp_building_queue_gain_buff_item.subId);
            ALUGUICommon.setLabelTxt(wnd.txtTempUnlockRemainTime, TimeUtil.millisecondsToTime_DayHourOrHMS(buffInfo?.curLeftTimeMS ?? 0));
            
            int totalNum = (int)NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_BUILDING_QUEUE_NUM);
            int tempUnlockNum = buffInfo?.layer ?? 0;
            bool isLocked = _m_queueIndex >= totalNum - tempUnlockNum;
            bool isTempUnlock = isLocked && (_m_queueIndex < totalNum);
            wnd.setLockState(isLocked, isTempUnlock);
        }


        private void _onClickJump(GameObject _go)
        {
            if (_m_queueIndex < 0)
                return;
            
            MarsBuildingInfo buildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoByQueueIndex(_m_queueIndex);
            if (buildingInfo != null)
            {
                MarsUtil.jumpToBuildingUpgrade(buildingInfo);
                return;
            }
            
            int totalNum = (int)NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_BUILDING_QUEUE_NUM);
            bool isLocked = _m_queueIndex >= totalNum;
            if (isLocked)
                QueueMgr.instance.AddNode(new GNodeMarsBuildQueueBuy());
            else
            {
                MarsBuildingInfo upgradableBuildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getUpgradableBuildingInfo();
                if (upgradableBuildingInfo == null)
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_upgradableBuildingNotExistTip_none);
                    return;
                }
                
                MarsUtil.jumpToBuildingUpgrade(upgradableBuildingInfo);
            }
        }
    }
}
