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
    public partial class NPPlayerIconBgkComponent : _ANPBasicPlayerComponent
    {

        private List<PlayerIconBgkItem> _m_lPlayerIconBgkList;

        // 红点管理器
        [NotNull] private RedTipDealer _m_redTipDealer;

        //构造函数
        public NPPlayerIconBgkComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_lPlayerIconBgkList = new List<PlayerIconBgkItem>();
            _m_redTipDealer = new RedTipDealer(this);

        }

        public List<PlayerIconBgkItem> playerIconBgkList { get { return _m_lPlayerIconBgkList; } }

        //属性
        public override bool isMustInit { get { return true; } }
        //组件类型
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.PLAYER_ICON_BGK; } }
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
            ALLog.Error("NPPlayerIconBgkComponent init Fail!!!");
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

        public void getIconBgkList(List<PlayerIconBgkItem> _list)
        {
            if (null == _list)
                return;

            _list.AddRange(_m_lPlayerIconBgkList);
        }

        public PlayerIconBgkItem getIconBgk(long _id)
        {
            PlayerIconBgkItem temp = null;
            for (int i = 0; i < _m_lPlayerIconBgkList.Count; i++)
            {
                temp = _m_lPlayerIconBgkList[i];
                if (null == temp || null == temp.iconBgkRef)
                    continue;
                if (temp.iconBgkRef.id == _id)
                    return temp;
            }

            return null;
        }
        // 更新头像框
        public void chgItem(GS2GC.p021_PlayerInfo.GS2GC_021_012_PlayerIconBgkChg _info)
        {
            PlayerIconBgkItem item = getIconBgk(_info.getId());
            if (null != item)
            {
                item.setExpiredTimeTagS(_info.getExpireTimeTagS());
            }
            else
            {
                ALLog.Error($"can not find icon bgk info for id: {_info.getId()}");
            }

            WinMsg.SendMsg(WinMsgType.PLAYER_INFO_ICON_BGK_CHG);
        }
        //新增头像框
        public void addItem(GS2GC.p021_PlayerInfo.GS2GC_021_011_PlayerIconBgkAdd _info)
        {
            //判断是否有对应数据，有则需要报错
            PlayerIconBgkItem item = getIconBgk(_info.getId());
            if (null != item)
            {
                //更新时间
                item.setExpiredTimeTagS(_info.getExpireTimeTagS());
                ALLog.Error($"add icon bgk multi {_info.getId()}");
                return;
            }

            //创建新对象插入
            item = new PlayerIconBgkItem(_info);
            if (null != item.iconBgkRef)
                _m_lPlayerIconBgkList.Add(item);

            if (item.isNew)
                _m_redTipDealer.onAddIconBgk(item.refId);

            WinMsg.SendMsg(WinMsgType.PLAYER_INFO_ICON_BGK_CHG);

        }
        //删除头像框
        public void removeItem(long _id)
        {
            PlayerIconBgkItem tmpItem = null;
            for (int i = 0; i < _m_lPlayerIconBgkList.Count; i++)
            {
                tmpItem = _m_lPlayerIconBgkList[i];
                if (null == tmpItem)
                    continue;

                if (tmpItem.iconBgkRef.id == _id)
                {
                    _m_lPlayerIconBgkList.RemoveAt(i);
                    _m_redTipDealer.readIconBgk(tmpItem.refId);
                    break;
                }
            }

            WinMsg.SendMsg(WinMsgType.PLAYER_INFO_ICON_BGK_CHG);
        }
        public void clear()
        {
            _m_lPlayerIconBgkList.Clear();
            _m_redTipDealer.clear();

        }

        #region 红点

        /// <summary>
        /// iconBgk 是否需要展示红点
        /// </summary>
        /// <param name="_id"></param>
        public bool redTipIconBgkNeedShow(long _id)
        {
            return _m_redTipDealer.iconBgkNeedShow(_id);
        }

        /// <summary>
        /// 新增iconBgk
        /// </summary>
        /// <param name="_id"></param>
        public void redTipOnAddIconBgk(long _id)
        {
            _m_redTipDealer.onAddIconBgk(_id);
        }

        /// <summary>
        /// 设置iconBgk已读
        /// </summary>
        /// <param name="_id"></param>
        public void redTipReadIconBgk(long _id)
        {
            _m_redTipDealer.readIconBgk(_id);
        }

        /// <summary>
        /// 设置所有iconBgk已读
        /// </summary>
        public void redTipReadAllIconBgk()
        {
            _m_redTipDealer.readAllIconBgk();
        }

        #endregion

        #region S2C
        public void retList(List<Common.NpPlayerInfoObj.PlayerInfo_IconBgk> _list)
        {
            if (_list == null)
            {
                //即便加载失败,也要设置完成进入游戏
                setInitDone();
                return;
            }

            //清空数据集
            _m_lPlayerIconBgkList.Clear();

            for (int i = 0, count = _list.Count; i < count; i++)
            {
                PlayerIconBgkItem item = new PlayerIconBgkItem(_list[i]);
                if ( null == item.iconBgkRef)
                    continue;
                //加入队列
                _m_lPlayerIconBgkList.Add(item);
                if (item.isNew)
                    _m_redTipDealer.onAddIconBgk(item.refId);
            }
            //初始化完成，设置状态
            setInitDone();
        }

        #endregion

        #region C2S
        public void reqList()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_022_ReqPlayerIconBgkList());
        }
        //告知服务器已查看
        public void reqIsViewed(long _id)
        {
            NPGSClientListener.sendMsgByLog(NPGSWriter_021_PlayerInfoOp.make_010_ReqViewPlayerIconBgk(_id));
        }

        //设置头像框
        public void reqSetIconBgk(long _iconBgkId, Action _callBack)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_007_ReqSetIconBgk(_iconBgkId),
               new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_007_SetIconBgkRes>((info) =>
               {
                   if (null != _callBack)
                       _callBack();
               }));
        }
        #endregion

    }
}
