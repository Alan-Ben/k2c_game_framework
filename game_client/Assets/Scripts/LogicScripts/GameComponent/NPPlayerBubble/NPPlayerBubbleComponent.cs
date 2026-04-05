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
    //玩家气泡框管理器
    public partial class NPPlayerBubbleComponent : _ANPBasicPlayerComponent
    {
        //玩家拥有的气泡框列表
        private List<NPPlayerBubbleItem> _m_lPlayerBubbleList;

        // 红点管理器
        [NotNull] private RedTipDealer _m_redTipDealer;

        //构造函数
        public NPPlayerBubbleComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_lPlayerBubbleList = new List<NPPlayerBubbleItem>();
            _m_redTipDealer = new RedTipDealer(this);

        }

        //玩家拥有的气泡框列表
        public List<NPPlayerBubbleItem> playerBubbleList { get { return _m_lPlayerBubbleList; } }

        //属性
        public override bool isMustInit { get { return true; } }
        //组件类型
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.PLAYER_BUBBLE; } }
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
            //请求玩家气泡框列表
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
            ALLog.Error("NPPlayerBubbleComponent init Fail!!!");
        }
        //释放资源函数
        protected override void _discard()
        {
            clear();
        }
        public override void onAllCompInited()
        {
            base.onAllCompInited();
        }

        public void getBubbleList(List<NPPlayerBubbleItem> _list)
        {
            if (null == _list)
                return;

            _list.AddRange(_m_lPlayerBubbleList);
        }

        /// <summary>
        /// 查找玩家对应气泡框的拥有信息
        /// 无则返回空
        /// </summary>
        public NPPlayerBubbleItem getBubble(long _bubbleId)
        {
            NPPlayerBubbleItem temp = null;
            for (int i = 0; i < _m_lPlayerBubbleList.Count; i++)
            {
                temp = _m_lPlayerBubbleList[i];
                if (null == temp || null == temp.baseData)
                    continue;
                if (temp.refObj.id == _bubbleId)
                    return temp;
            }

            return null;
        }
        // 更新气泡框
        public void chgItem(GS2GC.p021_PlayerInfo.GS2GC_021_017_PlayerBubbleChg _info)
        {
            NPPlayerBubbleItem item = getBubble(_info.getId());
            if (null != item)
            {
                item.setExpiredTimeTagS(_info.getExpireTimeTagS());
            }
            else
            {
                ALLog.Error($"can not find bubble info for id: {_info.getId()}");
            }

            WinMsg.SendMsg(WinMsgType.PLAYER_INFO_BUBBLE_CHG);

        }
        //新增 or 更新  气泡框
        public void addItem(GS2GC.p021_PlayerInfo.GS2GC_021_016_PlayerBubbleAdd _info)
        {
            //判断是否有对应数据，有则需要报错
            NPPlayerBubbleItem item = getBubble(_info.getId());
            if (null != item)
            {
                //更新时间
                item.setExpiredTimeTagS(_info.getExpireTimeTagS());
                return;
            }

            //创建新对象插入
            item = new NPPlayerBubbleItem(_info);
            if (null != item.refObj)
                _m_lPlayerBubbleList.Add(item);

            if (item.isNew)
                _m_redTipDealer.onAddBubble(item.refId);

            WinMsg.SendMsg(WinMsgType.PLAYER_INFO_BUBBLE_CHG);

        }
        //删除气泡框
        public void removeItem(long _id)
        {
            NPPlayerBubbleItem tmpItem = null;
            for (int i = 0; i < _m_lPlayerBubbleList.Count; i++)
            {
                tmpItem = _m_lPlayerBubbleList[i];
                if (null == tmpItem)
                    continue;

                if (tmpItem.refObj.id == _id)
                {
                    _m_lPlayerBubbleList.RemoveAt(i);
                    _m_redTipDealer.readBubble(tmpItem.refId);
                    break;
                }
            }

            WinMsg.SendMsg(WinMsgType.PLAYER_INFO_BUBBLE_CHG);

        }

        public void clear()
        {
            _m_lPlayerBubbleList.Clear();
            _m_redTipDealer.clear();

        }


        #region 红点

        /// <summary>
        /// bubble 是否需要展示红点
        /// </summary>
        /// <param name="_id"></param>
        public bool redTipBubbleNeedShow(long _id)
        {
            return _m_redTipDealer.bubbleNeedShow(_id);
        }

        /// <summary>
        /// 新增bubble
        /// </summary>
        /// <param name="_id"></param>
        public void redTipOnAddBubble(long _id)
        {
            _m_redTipDealer.onAddBubble(_id);
        }

        /// <summary>
        /// 设置bubble已读
        /// </summary>
        /// <param name="_id"></param>
        public void redTipReadBubble(long _id)
        {
            _m_redTipDealer.readBubble(_id);
        }

        /// <summary>
        /// 设置所有bubble已读
        /// </summary>
        public void redTipReadAllBubble()
        {
            _m_redTipDealer.readAllBubble();
        }

        #endregion

        #region S2C

        //初始化玩家气泡框
        public void retList(List<Common.NpPlayerInfoObj.PlayerInfo_Bubble> _list)
        {
            if (_list == null)
            {
                //即便加载失败,也要设置完成进入游戏
                setInitDone();
                return;
            }
            _m_lPlayerBubbleList.Clear();
            for (int i = 0, count = _list.Count; i < count; i++)
            {
                NPPlayerBubbleItem temp = new NPPlayerBubbleItem(_list[i]);
                if (null == temp || null == temp.refObj)
                    continue;
                _m_lPlayerBubbleList.Add(temp);
                if (temp.isNew)
                    _m_redTipDealer.onAddBubble(temp.refId);
            }
            //初始化完成，设置状态
            setInitDone();
        }

        #endregion

        #region C2S
        //请求玩家气泡框列表
        public void reqList()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_023_ReqPlayerBubbleList());
        }
        //告知服务器已查看
        public void reqIsViewed(long _id)
        {
            NPGSClientListener.sendMsgByLog(NPGSWriter_021_PlayerInfoOp.make_015_ReqViewPlayerBubble(_id));
        }


        //设置气泡框
        public void reqSetBubble(long _bubbleId, Action _callBack = null)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_009_ReqSetBubble(_bubbleId),
               new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_009_SetBubbleRes>((info) =>
               {
                   if (null != _callBack)
                       _callBack();
               }));
        }
        #endregion
    }
}
