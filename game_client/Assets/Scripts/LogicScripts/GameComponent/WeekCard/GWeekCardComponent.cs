using System.Collections.Generic;
using System;
using ALPackage;
using Common.TravelObj;
using Common.WeekCardObj;
using CommonEnum;
using JetBrains.Annotations;
using GS2GC.p002_InitOp;
using GS2GC.p004_PlayerOp;
using GS2GC.p021_PlayerInfo;
using NPEnum;

namespace GOE
{
    // 周卡模块管理类
    public class GWeekCardComponent : _ANPBasicPlayerComponent
    {
        private int _m_expireTimeTagS;//周卡过期时间
        private bool _m_hadUseFreeTrial;//是否使用过免费试用
        private List<_AGWeekCardSettingInfoBase> _m_weekCardSettingInfoList;
        private EWeekCardNPCType _m_npcType;
        private long _m_npcId;

        public event Action onWeekCardInfoChg;
        
        //构造函数
        public GWeekCardComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_weekCardSettingInfoList = new List<_AGWeekCardSettingInfoBase>();
        }
        public int expireTimeTagS { get { return _m_expireTimeTagS; } }
        public bool hadUseFreeTrial { get { return _m_hadUseFreeTrial; } }
        public EWeekCardNPCType npcType { get { return _m_npcType; } }
        public long npcId { get { return _m_npcId; } }

        #region override
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.WEEK_CARD; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        #endregion


        #region override 方法
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
            _reqWeekCardInitData();
        }

        protected override void _dealInit()
        {

        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            _refreshRed();
        }

        private void _refreshRed()
        {
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("GWeekCardComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            
        }

        #endregion

        /// <summary>
        /// 周卡当前状态
        /// </summary>
        /// <returns></returns>
        public EWeekCardStat getWeekCardStat()
        {
            if (!_m_hadUseFreeTrial)
                return EWeekCardStat.NONE;
            if (hasWeekCard())
                return EWeekCardStat.IN_WEEK_CARD;
            return EWeekCardStat.EXPIREED;
        }

        public bool hasWeekCard()
        {
            return NPPlayer.instance.weekCardComp.expireTimeTagS > FpsAndPingMgr.instance.serverTimeTagS;
        }
        
        public void getSettingInfoList(List<_AGWeekCardSettingInfoBase> _list)
        {
            if(_list == null)
                return;
            
            _list.AddRange(_m_weekCardSettingInfoList);
        }
        
        public _AGWeekCardSettingInfoBase getSettingInfo(EWeekCardSettleType _settingType)
        {
            foreach (_AGWeekCardSettingInfoBase settingInfoBase in _m_weekCardSettingInfoList)
            {
                if (settingInfoBase.settingType == _settingType)
                    return settingInfoBase;
            }

            return null;
        }

        /// <summary>
        /// 执政官形象
        /// </summary>
        /// <returns></returns>
        public NPGGoIndex getTDShowIndex()
        {
            if (_m_npcType == EWeekCardNPCType.HERO)
            {
                // HeroRefShowData heroInfo = new HeroRefShowData(_m_npcId);
                // return heroInfo.getTdShow();
            }

            if (_m_npcType == EWeekCardNPCType.CONSORT)
            {
                ConsortInfo consortInfo = new ConsortInfo(_m_npcId);
                return consortInfo.consortSkinShowInfo?.tdShow;
            }
            
            return null;
        }


        #region S2C
        public void retWeekCardInitData(GS2GC_002_037_RetWeekCardInit _msg)
        {
            if (null == _msg)
                return;
            _m_expireTimeTagS = _msg.getInfo().getExpireTimeTagS();
            _m_hadUseFreeTrial = _msg.getInfo().getHadUseFreeTrial();
            _m_npcType = _msg.getInfo().getNpcType();
            _m_npcId = _msg.getInfo().getNpcId();

            //如果没有设置过，就取默认形象
            if (_m_npcType == EWeekCardNPCType.NONE)
            {
                _m_npcType = GRefdataCoreMgr.instance.npGeneral.week_card_assign_default_td_show.enumValue;
                _m_npcId = GRefdataCoreMgr.instance.npGeneral.week_card_assign_default_td_show.longValue;
            }

            _m_weekCardSettingInfoList.Clear();
            foreach (WeekCard_SingleSettingInfo settingInfo in _msg.getSettingList())
            {
                switch (settingInfo.getType())
                {
                    case EWeekCardSettleType.COLLEGE_STUDY:
                        _m_weekCardSettingInfoList.Add(new GWeekCardSettingInfo_CollegeStudy(settingInfo));
                        break;
                    default:
                        _m_weekCardSettingInfoList.Add(new GWeekCardSettingInfo_DealPolicy(settingInfo));
                        break;
                }
            }
            
            setInitDone();
        }
        
        public void onWeekCardChg(GS2GC_004_063_OnWeekCardChg _msg)
        {
            if (null == _msg)
                return;
            
            _m_expireTimeTagS = _msg.getInfo().getExpireTimeTagS();
            _m_hadUseFreeTrial = _msg.getInfo().getHadUseFreeTrial();
            _m_npcType = _msg.getInfo().getNpcType();
            _m_npcId = _msg.getInfo().getNpcId();

            //如果是空的，就取默认形象
            if (_m_npcType == EWeekCardNPCType.NONE)
            {
                _m_npcType = GRefdataCoreMgr.instance.npGeneral.week_card_assign_default_td_show.enumValue;
                _m_npcId = GRefdataCoreMgr.instance.npGeneral.week_card_assign_default_td_show.longValue;
            }

            _refreshRed();
            onWeekCardInfoChg?.Invoke();
        }

        #endregion

        #region C2S

        /// <summary>
        /// 周卡组件初始化
        /// </summary>
        private void _reqWeekCardInitData()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_037_ReqWeekCardInit());
        }
        /// <summary>
        /// 请求周卡的结算信息
        /// </summary>
        public void reqWeekCardSettleInfo(Action<GS2GC_004_015_RetWeekCardSettleInfo> _succAction = null,Action _failAction = null)
        {

            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_015_ReqWeekCardSettleInfo(),
                new CommonRequestCallbackProtocolDealer<GS2GC_004_015_RetWeekCardSettleInfo>((info) =>
                {
                    if (null != _succAction)
                        _succAction(info);
                }, (_errCode) =>
                {
                    _failAction?.Invoke();
                }));
        }
        /// <summary>
        /// 请求修改周卡的设置
        /// </summary>
        public void reqWeekCardChgSetting(WeekCard_SingleSettingInfo _info,Action<GS2GC_004_016_RetWeekCardChgSetting> _backAction = null)
        {

            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_016_ReqWeekCardChgSetting(_info),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_016_RetWeekCardChgSetting>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }));
        }
        /// <summary>
        /// 请求激活周卡免费试用
        /// </summary>
        public void reqWeekCardActiveFreeTrial(Action<GS2GC_004_017_RetWeekCardActiveFreeTrial> _backAction = null)
        {

            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_017_ReqWeekCardActiveFreeTrial(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_017_RetWeekCardActiveFreeTrial>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }));
        }
        /// <summary>
        /// 请求修改周卡NPC
        /// </summary>
        public void reqWeekCardChgNPC(CommonEnum.EWeekCardNPCType _npcType
            , long _npcId, Action<GS2GC_004_018_RetWeekCardChgNPC> _backAction = null)
        {

            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_018_ReqWeekCardChgNPC(_npcType, _npcId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_018_RetWeekCardChgNPC>((info) =>
                {
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.week_card_chg_tdShow_succ));
                    if (null != _backAction)
                        _backAction(info);
                }));
        }

        #endregion

    }
}
