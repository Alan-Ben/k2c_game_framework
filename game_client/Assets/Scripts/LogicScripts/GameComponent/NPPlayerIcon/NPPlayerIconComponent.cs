using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ALPackage;
using NPCommon;
using System.Text;
using GS2GC.p004_PlayerOp;
using JetBrains.Annotations;

namespace GOE
{
    //玩家头像管理器
    public partial class NPPlayerIconComponent : _ANPBasicPlayerComponent
    {
        //玩家拥有的头像列表
        private List<NPPlayerIconItem> _m_lPlayerIconList;

        // 红点管理器
        [NotNull] private RedTipDealer _m_redTipDealer;

        //构造函数
        public NPPlayerIconComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_lPlayerIconList = new List<NPPlayerIconItem>();
            _m_redTipDealer = new RedTipDealer(this);

        }

        //属性
        public override bool isMustInit { get { return true; } }
        //组件类型
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.PLAYER_ICON; } }
        //依赖的组件
        public override ENPPlayerCompType[] dependCompList { get { return null; } }


        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            //请求玩家头像列表
            reqList();

            _m_redTipDealer.init();

        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {

        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("NPPlayerIconComponent init Fail!!!");
        }
        //释放资源函数
        protected override void _discard()
        {
            _clear();
        }
        public override void onAllCompInited()
        {
            base.onAllCompInited();
        }

        /// <summary>
        ///是否拥有头像
        /// </summary>
        public bool isContainIcon(long _id)
        {
            NPPlayerIconItem tmpItem = null;
            for (int i = 0; i < _m_lPlayerIconList.Count; i++)
            {
                tmpItem = _m_lPlayerIconList[i];
                if (null == tmpItem)
                    continue;

                if (tmpItem.playerIconRef.id == _id)
                    return true;
            }

            return false;
        }

        public void getIconList(List<NPPlayerIconItem> _list)
        {
            if (null == _list)
                return;

            _list.AddRange(_m_lPlayerIconList);
        }

        public NPPlayerIconItem getIcon(long _id)
        {
            NPPlayerIconItem tmpItem = null;
            for (int i = 0; i < _m_lPlayerIconList.Count; i++)
            {
                tmpItem = _m_lPlayerIconList[i];
                if (null == tmpItem || null == tmpItem.playerIconRef)
                    continue;

                if (tmpItem.playerIconRef.id == _id)
                    return tmpItem;
            }
            return null;
        }

        private void _clear()
        {
            _m_lPlayerIconList.Clear();
            _m_redTipDealer.clear();

        }

        #region 红点

        /// <summary>
        /// icon 是否需要展示红点
        /// </summary>
        /// <param name="_id"></param>
        public bool redTipIconNeedShow(long _id)
        {
            return _m_redTipDealer.iconNeedShow(_id);
        }

        /// <summary>
        /// 新增icon
        /// </summary>
        /// <param name="_id"></param>
        public void redTipOnAddIcon(long _id)
        {
            _m_redTipDealer.onAddIcon(_id);
        }

        /// <summary>
        /// 设置已读
        /// </summary>
        /// <param name="_id"></param>
        public void redTipReadIcon(long _id)
        {
            _m_redTipDealer.readIcon(_id);
        }

        /// <summary>
        /// 设置所有已读
        /// </summary>
        public void redTipReadAllIcon()
        {
            _m_redTipDealer.readAllIcon();
        }

        #endregion

        // 更新头像
        public void chgItem(GS2GC.p021_PlayerInfo.GS2GC_021_007_PlayerIconChg _info)
        {
            NPPlayerIconItem item = getIcon(_info.getId());
            if (null != item)
            {
                item.setExpiredTimeTagS(_info.getExpireTimeTagS());
            }
            else
            {
                ALLog.Error($"can not find icon info for id: {_info.getId()}");
            }

            WinMsg.SendMsg(WinMsgType.PLAYER_INFO_ICON_CHG);

        }

        //新增头像
        public void addItem(GS2GC.p021_PlayerInfo.GS2GC_021_006_PlayerIconAdd _info)
        {
            //判断是否有对应数据，有则需要报错
            NPPlayerIconItem item = getIcon(_info.getId());
            if (null != item)
            {
                //更新时间
                item.setExpiredTimeTagS(_info.getExpireTimeTagS());
                ALLog.Error($"add icon multi {_info.getId()}");
                return;
            }

            item = new NPPlayerIconItem(_info);
            if (null != item.playerIconRef)
                _m_lPlayerIconList.Add(item);
            if (item.isNew)
                _m_redTipDealer.onAddIcon(item.refId);
            WinMsg.SendMsg(WinMsgType.PLAYER_INFO_ICON_CHG);
        }

        //删除头像
        public void removeItem(long _id)
        {
            NPPlayerIconItem tmpItem = null;
            for (int i = 0; i < _m_lPlayerIconList.Count; i++)
            {
                tmpItem = _m_lPlayerIconList[i];
                if (null == tmpItem)
                    continue;

                if (tmpItem.playerIconRef.id == _id)
                {
                    _m_lPlayerIconList.RemoveAt(i);
                    _m_redTipDealer.readIcon(tmpItem.refId);
                    break;
                }
            }

            WinMsg.SendMsg(WinMsgType.PLAYER_INFO_ICON_CHG);
        }

        #region S2C
        //初始化玩家头像
        public void retList(List<Common.NpPlayerInfoObj.PlayerInfo_Icon> _list)
        {
            if (_list == null)
            {
                //即便加载失败,也要设置完成进入游戏
                setInitDone();
                return;
            }

            _m_lPlayerIconList.Clear();

            for (int i = 0, count = _list.Count; i < count; i++)
            {
                NPPlayerIconItem temp = new NPPlayerIconItem(_list[i]);
                if (null == temp.playerIconRef)
                    continue;

                _m_lPlayerIconList.Add(temp);
                if (temp.isNew)
                    _m_redTipDealer.onAddIcon(temp.refId);
            }

            //初始化完成，设置状态
            setInitDone();
        }

        #endregion

        #region C2S
        //请求玩家头像列表
        public void reqList()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_021_ReqPlayerIconList());
        }
        //告知服务器查看过该头像
        public void reqIsViewed(long _id)
        {
            NPGSClientListener.sendMsgByLog(NPGSWriter_021_PlayerInfoOp.make_005_ReqViewPlayerIcon(_id));
        }

        //设置头像
        public void reqSetIcon(long _iconId, Action _callBack)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_005_ReqSetIcon(_iconId),
               new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_005_SetIconRes>((info) =>
               {
                   if (null != _callBack)
                       _callBack();
               }));
        }
        #endregion
    }
}
