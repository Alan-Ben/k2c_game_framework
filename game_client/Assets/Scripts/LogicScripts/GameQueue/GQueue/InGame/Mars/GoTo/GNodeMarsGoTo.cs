using System;

namespace GOE
{
    /// <summary>
    /// 前往火星node
    /// </summary>
    public class GNodeMarsGoTo : _AGNodeMainSub
    {
        public GNodeMarsGoTo() : base(EMainFunctionTabType.MARS, UINodeTagConst.C_MARS_GO_TO)
        {
        }
        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
        }
        
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
        }
        
        protected override void _doEnterNode(Action _triggerEnterDone)
        {
            GUISceneMain.instance.showMainScene(GMainGUIAddSceneMarsGoTo.instance, _triggerEnterDone);
        }

        protected override void _doQuitNode()
        {
           
        }

        /**********************
         * 在本节点不允许esc回退时执行的逻辑。
         **/
        public override void onCannotEscBack()
        {
        }
    }
}