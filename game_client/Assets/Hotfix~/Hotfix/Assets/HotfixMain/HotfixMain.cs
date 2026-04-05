using ALBasicProtocolPack;
using ALPackage;
using GOE;
using UnityEngine;

namespace Hotfix
{
    public partial class HotfixMain
    {
        /// <summary>
        /// 初始化入口
        /// </summary>
        public static void init()
        {
            setSCVersion();

            HotfixRefCommonParseDealer.createCache(5, 10);
            _initProtocolDealer();

            //注册热更配表dealer
            _initPatchTableDealer();

            //注册热更活动对象
            _initPatchActivity();
            
            Debug.Log($"Init Hotfix dll suc! version：{HotfixSCVersion.instance.clientShowId}");
        }

        public static void test()
        {
            Debug.Log($"dasdadad1231231dasdasda");
            
            // 使用自定义node打开窗口
            // QueueMgr.instance.AddNode(new GQueueDemoNode("NPGGuiDemoWnd"));
            
            // 使用通用node打开窗口
            // NPQueueMgr.instance.addNode_InGame_SingleWnd(NPGGuiDemoWnd.instance, NPGGuiDemoWnd.instance.showWnd, "NPGGuiDemoWnd");
            // NPQueueMgr.instance.addNode_InGame_SingleWnd(NPGGUIDemoContainerMain.instance, NPGGUIDemoContainerMain.instance.showWnd, "NPGGUIDemoContainerMain");
            // NPQueueMgr.instance.addNode_InGame_SingleWnd(NPGGUIWndDemoGridMain.instance, NPGGUIWndDemoGridMain.instance.showWnd, "NPGGUIWndDemoGridMain");
            
            //用notice的方式加
            // NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_Demo());
        }
        
        public static void setSCVersion()
        {
            ALHotfixMgr_ILRuntime_Global.instance.setVersion(HotfixSCVersion.instance.main, HotfixSCVersion.instance.sub, HotfixSCVersion.instance.patch, HotfixSCVersion.instance.build);
        }
    }
}