using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 猫咪气泡入口点
    /// </summary>
    public class GTDCatBubblePointView
    {
        //入口点的跟随实例
        private GGUICommonFollowTarget _m_followInstance;
        //入口点跟随的信息controller
        private GGUICatBubbleFollowItemController _m_followInfoController;

        public GTDCatBubblePointView(Transform _followParent)
        {
            if(null == _followParent)
                return;

            _m_followInstance = new GGUICommonFollowTarget(_followParent, Vector3.zero);
        }

        //初始化
        public void init()
        {
            //向UI展示窗口注册本对象
            GGUIWndHomeEntryFollow.instance.regInstance(_m_followInstance);
        }

        //销毁
        public void discard()
        {
            GGUIWndHomeEntryFollow.instance.removeInstance(_m_followInstance);
            
            _discardInfoController();
        }

        //刷新显示
        public void refreshShow()
        {
            _showInfoController();
        }
        
        //隐藏显示
        public void hide()
        {
            _discardInfoController();
        }
        
        //显示跟随信息
        private void _showInfoController()
        {
            if (null == _m_followInfoController)
            {
                _m_followInfoController = new GGUICatBubbleFollowItemController();
                GGUIWndHomeEntryFollow.instance.addController(_m_followInstance, _m_followInfoController);
            }
            _m_followInfoController.setShowEntrance();
        }
        
        //销毁跟随信息
        private void _discardInfoController()
        {
            if (null != _m_followInfoController)
            {
                _m_followInfoController.discard();
            }
            _m_followInfoController = null;
        }
    }
}