using System;

namespace GOE
{
    /// <summary>
    /// 到达火星着陆选择界面node
    /// </summary>
    public class GNodeMarsLandingSelect : _AGNodeMainSub
    {
        //选择着陆点后的回调
        private Action _m_aOnSelectLanding;

        public GNodeMarsLandingSelect(Action _onSelectLanding = null) : base(EMainFunctionTabType.MARS, UINodeTagConst.C_MARS_LANDING_SELECT)
        {
            _m_aOnSelectLanding = _onSelectLanding;
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
            //预先进入火星场景
            GTDSceneMain.instance.showMainScene(MainAdditionMarsTDScene.instance, null);
            //打开界面
            GUISceneMain.instance.showMainScene(GMainGUIAddSceneMarsLandingSelect.instance, ()=>
            {
                GMainGUIAddSceneMarsLandingSelect.instance.showScene();
                GMainGUIAddSceneMarsLandingSelect.instance.setInfo(_m_aOnSelectLanding);
                _triggerEnterDone?.Invoke();
            });
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