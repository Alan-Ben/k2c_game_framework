using ALPackage;

namespace GOE
{
    /// <summary>
    /// 入口解锁描述跟随入口item控制器
    /// </summary>
    public class GGUIEntryUnlockDescController : _ATALGGUICommonFollowItemController<GGUIMonoEntryUnlockDesc, GGUIWndEntryUnlockDesc>
    {
        private int _m_iUIResId;
        private readonly GResPathIndex _m_index;

        public GGUIEntryUnlockDescController(int _uiResId):base()
        {
            _m_iUIResId = _uiResId;
            _m_index = new GResPathIndex(_uiResId);
        }

        public int uiResId { get { return _m_iUIResId; } }
        public override _AALBasicLoadResIndexInfo followItemIndex { get => _m_index; }
        protected override GGUIWndEntryUnlockDesc _createItemWnd(GGUIMonoEntryUnlockDesc _wndMono)
        {
            return new GGUIWndEntryUnlockDesc(_wndMono);
        }

        /// <summary>
        /// 释放创建出来的对象
        /// </summary>
        /// <param name="_item"></param>
        protected override void _discardItem(_IALGGUIWndCommonFollowItem _item)
        {
            if (null != _item)
            {
                GGUIWndEntryUnlockDesc targetItem = (GGUIWndEntryUnlockDesc) _item;
                targetItem.hideWnd(targetItem.discard);
            }
        }

        /// <summary>
        /// 展示入口
        /// </summary>
        public void setShowEntrance(EntryPointRefObj _refObj)
        {
            regItemWndLoadDoneDelegate(() =>
            {
                wnd?.showWnd();
                wnd?.setInfo(_refObj);
            });
        }
    }
}