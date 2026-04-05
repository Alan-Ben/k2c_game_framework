using System;
using ALPackage;

namespace GOE
{
    public class GNodeHero : _AGNodeMainSub
    {
        public GNodeHero()
            : base(EMainFunctionTabType.HERO, UINodeTagConst.C_HERO_MAIN)
        {
        }
        

        public override bool isOnlyUINode => true;

        public override void onEnterQueue()
        {
        }
        public override void onClose()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                //重置一下列表位置
                GGUIWndHeroMain.instance.resetListPos();
            });
        }

        /// <summary>
        /// 当进入节点时做的操作
        /// </summary>
        protected override void _doEnterNode(Action _triggerEnterDone)
        {
            GUISceneMain.instance.showMainScene(GMainGUIAddSceneHeroMain.instance, ()=>
            {
                WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.HERO_MAIN_WND_ENTER_DONE);
                
                _triggerEnterDone?.Invoke();
            });
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        protected override void _doQuitNode()
        {
        }
    }
}
