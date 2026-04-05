using System;
using System.Collections.Generic;
using ALPackage;
using NPCommon;
using NPEnum;

namespace GOE
{
    public partial class CommonRewardDealer
    {
        private static void _dealGainConsort(List<NPCommon_ItemInfo> _itemList, Action _onDealDone)
        {
            if (_itemList == null || _itemList.Count <= 0)
            {
                _onDealDone?.Invoke();
                return;
            }

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(_itemList.Count);
            stepCounter.regAllDoneDelegate(_onDealDone);
            
            foreach (var commonItem in _itemList)
            {
                if (commonItem == null || commonItem.getItemType() != (int) ENPItemType.CONSORT)
                {
                    stepCounter.addDoneStepCount();
                    continue;
                }
                
                dealShowGainConsort(commonItem.getSubId(), stepCounter.addDoneStepCount);
            }
        }

        /// <summary>
        /// 展示获取妃子
        /// </summary>
        /// <param name="_consortId"></param>
        public static void dealShowGainConsort(long _consortId, Action _doneDelegate = null)
        {
            GGottenConsortInfo gottenConsortInfo = NPPlayer.instance.consortComp.getConsortInfo(_consortId);
            if (gottenConsortInfo == null || gottenConsortInfo.consortRefObj == null)
            {
                _doneDelegate?.Invoke();
                return;
            }
            AccountSettingMgr.instance.accountSetting.addAlreadyShowGainConsort(_consortId);
            
            ALProcess alProcess = ALProcess.CreateProcess();
            
            //获得前播放一段对话
            if(gottenConsortInfo.consortRefObj.get_consort_dlg_list != null)
            {
                foreach (long dialogId in gottenConsortInfo.consortRefObj.get_consort_dlg_list)
                {
                    alProcess.addDelegateProcess((_done) =>
                    {
                        GCommon.enterDialogueNode(dialogId, _done);
                    });
                }    
            }
                
            //获得妃子展示
            alProcess.addDelegateProcess((_done) =>
            {
                QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndConsortGet.instance, UINodeTagConst.C_CONSORT_GET, null, () =>
                {
                    GGUIWndConsortGet.instance.setInfo(gottenConsortInfo, _done);
                }, 0);
            });
            alProcess.addDelegateProcess((_done) =>
            {
                NPPlayer.instance.dinnerComp.showConsortPermitGetNotice(_consortId, _done);   
            });
            alProcess.addProcess(() =>
            {
                //触发引导trigger
                WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.GAIN_CONSORT_SHOW_FINISH);
            });
            alProcess.addProcess(_doneDelegate);
            alProcess.deal();
        }
    }
}