
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public partial class PlayerChildComponent
    {
        public class RedTipDealer
        {
            [NotNull] private readonly PlayerChildComponent _m_comp;
            private _ARedTipNode _m_engageRequestNode;
            private _ARedTipNode _m_seatNode;
            [ItemNotNull, NotNull] private readonly List<AdultEngageRequestInfo> _m_unreadInfoList;
            
            
            public RedTipDealer([NotNull] PlayerChildComponent _comp)
            {
                _m_comp = _comp;
                _m_unreadInfoList = new List<AdultEngageRequestInfo>();
            }


            public void init()
            {
                // 刷新联姻红点
                _m_engageRequestNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_PLAYER_CHILD_ENGAGE_REQUEST);
                foreach (AdultEngageRequestInfo info in _m_comp._m_engageRequestInfoList)
                {
                    if (info.isEnable && !_m_comp.isRead(info))
                        _m_unreadInfoList.Add(info);
                }
                _m_engageRequestNode?.setCount(_m_unreadInfoList.Count);
                
                // 刷新席位红点
                _m_seatNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_PLAYER_CHILD_SEAT);
                _refreshSeatRed();
                
                // 添加事件，实现每次回到城建界面时刷新一次席位的红点，因为对于脑力值的刷新如果要做每帧检查，有些大材小用
                WinMsg.RegisterMsgAct(WinMsgType.ENTER_CITY, _refreshSeatRed);
            }
            public void clear()
            {
                WinMsg.UnregisterMsgAct(WinMsgType.ENTER_CITY, _refreshSeatRed);
                _m_engageRequestNode = null;
                _m_seatNode = null;
            }

            public void unreadEngageRequestAdd(AdultEngageRequestInfo _info)
            {
                if (_m_engageRequestNode == null)
                    return;
                
                _m_unreadInfoList.Add(_info);
                _m_engageRequestNode.setCount(_m_unreadInfoList.Count);
            }
            public void engageRequestRead(AdultEngageRequestInfo _info)
            {
                if (_m_engageRequestNode == null)
                    return;

                if (!_m_unreadInfoList.Remove(_info))
                    return;
                
                _m_engageRequestNode.setCount(_m_unreadInfoList.Count);
            }
            public void allEngageRequestRead()
            {
                if (_m_engageRequestNode == null)
                    return;

                _m_unreadInfoList.Clear();
                _m_engageRequestNode.setCount(_m_unreadInfoList.Count);
            }
            public void onChildAddOrRemove()
            {
                _refreshSeatRed();   
            }
            public void onChildNamed()
            {
                _refreshSeatRed();
            }
            public void onSeatChg()
            {
                _refreshSeatRed();
            }


            private void _refreshSeatRed()
            {
                if (_m_seatNode == null)
                    return;
                
                int redCount = 0;
                foreach (SeatInfo seatInfo in _m_comp._m_seatList)
                {
                    if (seatInfo.needShowRedTip())
                        redCount++;
                }
                
                _m_seatNode.setCount(redCount);
            }
        }
    }
}