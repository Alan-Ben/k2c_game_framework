using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 入口引导手指
    /// </summary>
    public class EntryGuideHandPointView
    {
        //入口点的跟随实例
        private GGUICommonFollowTarget _m_followInstance;
        //入口点跟随的信息controller
        private GGUIEntryGuideHandController _m_followInfoController;
        //ui资源id
        private long _m_uiResId;

        public EntryGuideHandPointView()
        {
        }

        //初始化
        public void init(Transform _followParent, long _uiResId)
        {
            if (null == _followParent)
                return;

            _m_uiResId = _uiResId;

            _m_followInstance = new GGUICommonFollowTarget(_followParent, Vector3.zero);

            //向UI展示窗口注册本对象
            GGUIWndHomeEntryFollow.instance.regInstance(_m_followInstance);
        }

        //销毁
        public void discard()
        {
            GGUIWndHomeEntryFollow.instance.removeInstance(_m_followInstance);

            _m_followInstance = null;
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
                _discardInfoController();
                _m_followInfoController = new GGUIEntryGuideHandController((int)_m_uiResId);
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