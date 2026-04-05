using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NPEnum;
using ALPackage;
using Common.FriendObj;
using GS2GC.p004_PlayerOp;
using Common.NpPlayerInfoObj;

namespace GOE
{
    //联姻申请的数据信息
    public class PlayerFriendItemData
    {
        //玩家cid
        private long _m_cid;

        //玩家信息
        private NPCommonSimplePlayerInfo _m_playerInfo;

        public PlayerFriendItemData(long _cid, PlayerInfo_CommonShow _info)
        {
            _m_cid = _cid;
            _m_playerInfo = new NPCommonSimplePlayerInfo(_info);
        }
        public long cid { get { return _m_cid; } }

        public NPCommonSimplePlayerInfo playerInfo { get { return _m_playerInfo; } }

    }

    // 好友数据结构
    public class PlayerFriendItem: _AReqBaseItem<PlayerFriendItemData>
    {
        //玩家cid
        private long _m_cid;

        public PlayerFriendItem(Friend_Info _info)
        {
            if (null == _info)
                return;

            _m_cid = _info.getFcid();
        }

        public long cid { get { return _m_cid; } }
        //请求详细信息
        protected override void _onReqData(Action<PlayerFriendItemData> _doneAction)
        {
            GCommon.reqPlayerInfoSer(_m_cid, (info) =>
            {
                if (null == info)
                    return;

                PlayerFriendItemData itemData = new PlayerFriendItemData(_m_cid, info.getSomeOneShowInfo());

                if (_doneAction != null)
                    _doneAction(itemData);
            });
        }
    }
}
