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
    public class PlayerFriendApplyItemData
    {
        //玩家cid
        private long _m_cid;

        //玩家信息
        private NPCommonSimplePlayerInfo _m_playerInfo;

        public PlayerFriendApplyItemData(long _cid, PlayerInfo_CommonShow _info)
        {
            _m_cid = _cid;
            _m_playerInfo = new NPCommonSimplePlayerInfo(_info);
        }
        public long cid { get { return _m_cid; } }

        public NPCommonSimplePlayerInfo playerInfo { get { return _m_playerInfo; } }
    }

    // 好友申请数据结构
    public class PlayerFriendApplyItem : _AReqBaseItem<PlayerFriendApplyItemData>
    {
        //玩家cid
        private long _m_cid;

        //时间戳
        private long _m_applyTimeS;

        //构造函数
        public PlayerFriendApplyItem(Friend_ApplyInfo _info)
        {
            if (null == _info)
                return;

            _m_cid = _info.getApplyCid();
            _m_applyTimeS = _info.getApplyTimeS();
        }

        public long applyTimeS { get { return _m_applyTimeS; } }

        public long cid { get { return _m_cid; } }

        //是否在申请有效期内
        public bool isAvaiable { get { return FpsAndPingMgr.instance.serverTimeTagS <= _m_applyTimeS + GRefdataCoreMgr.instance.npGeneral.friend_apply_avaiable_secs; } }

        //请求详细信息
        protected override void _onReqData(Action<PlayerFriendApplyItemData> _doneAction)
        {
            GCommon.reqPlayerInfoSer(_m_cid, (info) =>
            {
                if (null == info)
                    return;

                PlayerFriendApplyItemData itemData = new PlayerFriendApplyItemData(_m_cid,info.getSomeOneShowInfo());

                if (_doneAction != null)
                    _doneAction(itemData);
            });
        }

        //更新申请时间戳
        public void updateApplyTimeS(long _applyTimeS)
        {
            _m_applyTimeS = _applyTimeS;
        }
    }
}
