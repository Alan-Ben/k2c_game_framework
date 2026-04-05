using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 入口解锁描述
    /// </summary>
    public class EntryUnlockDescPointView
    {
        //入口点的跟随实例
        private GGUICommonFollowTarget _m_followInstance;
        //入口点跟随的信息controller
        private GGUIEntryUnlockDescController _m_followInfoController;
        //入口点配置
        private EntryPointRefObj _m_curEntryPointRef;

        public EntryUnlockDescPointView()
        {
        }

        //初始化
        public void init(Transform _followParent, EntryPointRefObj _entryPointRef)
        {
            if (null == _followParent)
                return;

            _m_curEntryPointRef = _entryPointRef;
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
            if (_m_curEntryPointRef == null)
                return;

            if (null == _m_followInfoController || _m_followInfoController.uiResId != _m_curEntryPointRef.lock_desc_ui_res_id)
            {
                _discardInfoController();
                _m_followInfoController = new GGUIEntryUnlockDescController(_m_curEntryPointRef.lock_desc_ui_res_id);
                GGUIWndHomeEntryFollow.instance.addController(_m_followInstance, _m_followInfoController);
            }
            _m_followInfoController.setShowEntrance(_m_curEntryPointRef);
        }
        
        //销毁跟随信息
        private void _discardInfoController()
        {
            if (null != _m_followInfoController)
            {
                if(_m_followInfoController.itemMono != null)
                    _m_followInfoController.discard();
                else
                    _m_followInfoController.regItemWndLoadDoneDelegate(_m_followInfoController.discard);
            }
            _m_followInfoController = null;
        }
    }
}