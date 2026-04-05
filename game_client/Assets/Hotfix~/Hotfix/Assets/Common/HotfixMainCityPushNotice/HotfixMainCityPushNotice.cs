using System;
using ALPackage;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 热更主城推送弹窗
    /// </summary>
    public class HotfixMainCityPushNotice
    {
        public static void addMainCityPushNotice(long _addNoticeSerialize, bool _isLogin, Action _addDone)
        {
            ALProcess popCityAndRoom = ALProcess.CreateProcess("HotfixMainCityPushNotice.addMainCityPushNotice");
            
            // 热更工程添加主城推送弹窗示例
            // popCityAndRoom.addDelegateProcess((_delegateComplete) =>
            // {
            //     MainCityPushNoticeMgr.instance.addMainCityPushNotice(_addNoticeSerialize, _isLogin, _delegateComplete, (isLogin, done) =>
            //     {
            //         if (isLogin)//只有登录时需要展示
            //         {
            //             NPUINoticeMgr.instance.addDealer(new NoticeDealer_ActivityMergeShow(EMainCityPushNoticeTriggerType.LOGIN));
            //             done?.Invoke();
            //         }
            //         else
            //         {
            //             done?.Invoke();
            //         }
            //     });
            // });

            ///////////////////////需要添加notice在上面添加/////////////////////////
            popCityAndRoom.addProcess(() =>
            {
                _addDone?.Invoke();
            });
            
            popCityAndRoom.deal();
        }
    }
}