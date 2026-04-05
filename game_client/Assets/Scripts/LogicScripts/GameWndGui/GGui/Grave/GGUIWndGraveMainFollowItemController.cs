using System;
using ALPackage;

namespace GOE
{
    // 杰出者大厅主界面 跟随控制器
    public class GGUIGraveMainFollowItemController : _ATALGGUICommonFollowItemController<GGUIMonoGraveMainFollowItem, GGUIWndGraveMainFollowItem>
    {
        private readonly GResPathIndex _m_index;
        private int _m_graveTypeId;
        private long _m_playerCid = 0;
        private long _m_titleId = 0;
        public GGUIGraveMainFollowItemController(int _graveTypeId,  long _cid, long _titleId, int _id):base()
        {            
            _m_graveTypeId = _graveTypeId;
            _m_playerCid = _cid;
            _m_titleId = _titleId;
            _m_index = new GResPathIndex(_id);
        }
        public override _AALBasicLoadResIndexInfo followItemIndex { get => _m_index; }
        protected override GGUIWndGraveMainFollowItem _createItemWnd(GGUIMonoGraveMainFollowItem _wndMono)
        {
            return new GGUIWndGraveMainFollowItem(_m_graveTypeId, _m_playerCid, _m_titleId, _wndMono);
        }
        
        public void dealItemWnd(Action<GGUIWndGraveMainFollowItem> _action)
        {
            if(_action == null)
                return;
            
            regItemWndLoadDoneDelegate(() =>
            {
                _action(wnd);
            });
        }
    }
}