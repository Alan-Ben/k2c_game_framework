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
    //屏蔽的玩家数据信息
    public class PlayerShieldItemData
    {
        //玩家cid
        private long _m_cid;

        //玩家信息
        private NPCommonSimplePlayerInfo _m_playerInfo;

        public PlayerShieldItemData(long _cid, PlayerInfo_CommonShow _info)
        {
            _m_cid = _cid;
            _m_playerInfo = new NPCommonSimplePlayerInfo(_info);
        }
        public long cid { get { return _m_cid; } }

        public NPCommonSimplePlayerInfo playerInfo { get { return _m_playerInfo; } }

    }

    // 屏蔽的玩家数据结构
    public class PlayerShieldItem: _AReqBaseItem<PlayerShieldItemData>
    {
        //玩家cid
        private long _m_cid;

        public PlayerShieldItem(long _cid)
        {
            _m_cid = _cid;
        }

        public long cid { get { return _m_cid; } }
        //请求详细信息
        protected override void _onReqData(Action<PlayerShieldItemData> _doneAction)
        {
            GCommon.reqPlayerInfoSer(_m_cid, (info) =>
            {
                if (null == info)
                    return;

                PlayerShieldItemData itemData = new PlayerShieldItemData(_m_cid, info.getSomeOneShowInfo());

                if (_doneAction != null)
                    _doneAction(itemData);
            });
        }
    }
}
