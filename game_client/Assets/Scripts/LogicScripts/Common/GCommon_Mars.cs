using System;
using NPEnum;

namespace GOE
{
    public static partial class GCommon
    {
        /// <summary>
        /// 处理进入火星界面
        /// </summary>
        public static void dealEnterMars(Action onComplete = null)
        {
            // 火星功能是否已解锁
            bool isUnlockMars = GCommon.isFuncUnlock(ENPFunctionType.MARS);
            if (!isUnlockMars)
            {
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsMissionPreview.instance, GGUIWndMarsMissionPreview.instance.showWnd, UINodeTagConst.C_MARS_MISSION_PREVIEW);
                return;
            }

            // 是否开始前往火星
            bool isStartGoToMars = NPPlayer.instance.marsComp.goToSubComponent.isStartGoToMars;
            // 是否已选择火星登录地点
            bool isSelectLandingArea = NPPlayer.instance.playerInfoComp.clientDataRemarkInfo.isSelectLandingMarsArea();
            // 是否已到达火星
            bool isArriveMars = NPPlayer.instance.marsComp.goToSubComponent.isArriveMars;


            if (!isStartGoToMars)
            {
                // 未出发前往火星
                QueueMgr.instance.AddNode(new GNodeMarsGoToStartConfirm());
            }
            else if (!isArriveMars)
            {
                // 前往火星途中
                QueueMgr.instance.AddNode(new GNodeMarsGoTo());// 前往火星中
            }
            else if (!isSelectLandingArea)
            {
                // 已到达火星未选择登陆地点
                QueueMgr.instance.AddNode(new GNodeMarsLandingSelect(() =>
                {
                    // 选择完登陆地点后，进入火星主界面
                    QueueMgr.instance.AddNode(new GNodeMars((_node) =>
                    {
                        if (onComplete != null) 
                            onComplete();
                    }));
                }));
            }
            else
            {
                // 已到达火星且已选择登陆地点
                QueueMgr.instance.AddNode(new GNodeMars((_node) =>
                {
                    if (onComplete != null) 
                        onComplete();
                }));
            }
        }
    }
}