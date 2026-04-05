using System;
using System.Collections.Generic;
using ALPackage;
using Common.NpPlayerInfoObj;
using GC2GS.p018_PlayerSkinOp;
using GS2GC.p002_InitOp;
using GS2GC.p018_PlayerSkinOp;
using JetBrains.Annotations;
using NPCommon;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 玩家称号管理器
    /// </summary>
    public partial class PlayerTitleComponent : _ANPBasicPlayerComponent
    {
        //获得的玩家固定、限时称号列表
        [NotNull] private List<PlayerTitleInfo> _m_lPlayerTitleInfoList;
        //组合称号前缀列表(包含已获得和未获得)
        [NotNull] private List<PlayerTitleComboPreInfo> _m_lComboPreList;
        //组合称号后缀列表(包含已获得和未获得)
        [NotNull] private List<PlayerTitleComboSfxInfo> _m_lComboSfxList;
        //组合称号底框列表(包含已获得和未获得)
        [NotNull] private List<PlayerTitleComboBgInfo> _m_lComboBgList;
        //当前穿戴的称号
        private PlayerInfo_CurTitle _m_curTitle;
        //是否展示给其他玩家看
        private bool _m_bIsShowOthers;
        // 红点管理器
        [NotNull] private RedTipDealer _m_redTipDealer;

        //构造函数
        public PlayerTitleComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_lPlayerTitleInfoList = new List<PlayerTitleInfo>();
            _m_lComboPreList = new List<PlayerTitleComboPreInfo>();
            _m_lComboSfxList = new List<PlayerTitleComboSfxInfo>();
            _m_lComboBgList = new List<PlayerTitleComboBgInfo>();
            _m_redTipDealer = new RedTipDealer(this);
        }

        /// <summary>
        /// 是否展示给其他玩家看
        /// </summary>
        public bool isShowOthers { get { return _m_bIsShowOthers; } }
        /// <summary>
        /// 当前穿戴的称号
        /// </summary>
        public PlayerInfo_CurTitle curTitle { get { return _m_curTitle; } }
        //属性
        public override bool isMustInit { get { return true; } }
        //组件类型
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.PLAYER_TITLE; } }
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
            //请求玩家称号列表
            reqPlayerTitleList();
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
            ALLog.Error("PlayerTitleComponent init Fail!!!");
        }

        public override void onAllCompInited()
        {
            for (int i = 0; i < _m_lComboPreList.Count; i++)
            {
                _m_lComboPreList[i]?.checkIsUnlock(false);
            }
            for (int i = 0; i < _m_lComboSfxList.Count; i++)
            {
                _m_lComboSfxList[i]?.checkIsUnlock(false);
            }
            for (int i = 0; i < _m_lComboBgList.Count; i++)
            {
                _m_lComboBgList[i]?.checkIsUnlock(false);
            }
        }

        //释放资源函数
        protected override void _discard()
        {
            clear();
        }

        public void clear()
        {
            _m_lPlayerTitleInfoList.Clear();
            for (int i = 0; i < _m_lComboPreList.Count; i++)
            {
                _m_lComboPreList[i]?.clear();
            }
            _m_lComboPreList.Clear();
            for (int i = 0; i < _m_lComboSfxList.Count; i++)
            {
                _m_lComboSfxList[i]?.clear();
            }
            _m_lComboSfxList.Clear();
            for (int i = 0; i < _m_lComboBgList.Count; i++)
            {
                _m_lComboBgList?.Clear();
            }
            _m_lComboBgList.Clear();
            _m_redTipDealer.clear();
        }

        /// <summary>
        /// 获取组合称号前缀列表
        /// </summary>
        /// <param name="_list"></param>
        public void getComboPreInfoList(List<PlayerTitleComboPreInfo> _list)
        {
            if (_list == null)
                return;

            _list.Clear();
            _list.AddRange(_m_lComboPreList);
        }

        /// <summary>
        /// 获取组合称号前缀列表
        /// </summary>
        /// <param name="_list"></param>
        public void getComboPreInfoList(List<_IPlayerTitleCombo> _list)
        {
            if (_list == null)
                return;

            _list.Clear();
            _list.AddRange(_m_lComboPreList);
        }

        /// <summary>
        /// 获取组合称号后缀列表
        /// </summary>
        /// <param name="_list"></param>
        public void getComboSfxInfoList(List<PlayerTitleComboSfxInfo> _list)
        {
            if (_list == null)
                return;

            _list.Clear();
            _list.AddRange(_m_lComboSfxList);
        }

        /// <summary>
        /// 获取组合称号后缀列表
        /// </summary>
        /// <param name="_list"></param>
        public void getComboSfxInfoList(List<_IPlayerTitleCombo> _list)
        {
            if (_list == null)
                return;

            _list.Clear();
            _list.AddRange(_m_lComboSfxList);
        }

        /// <summary>
        /// 获取组合称号底框列表
        /// </summary>
        /// <param name="_list"></param>
        public void getComboBgInfoList(List<PlayerTitleComboBgInfo> _list)
        {
            if (_list == null)
                return;

            _list.Clear();
            _list.AddRange(_m_lComboBgList);
        }

        /// <summary>
        /// 获取组合称号底框列表
        /// </summary>
        /// <param name="_list"></param>
        public void getComboBgInfoList(List<_IPlayerTitleCombo> _list)
        {
            if (_list == null)
                return;

            _list.Clear();
            _list.AddRange(_m_lComboBgList);
        }

        /// <summary>
        /// 获取固定、限时称号信息
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public PlayerTitleInfo getTitleInfo(long _id)
        {
            for (int i = 0; i < _m_lPlayerTitleInfoList.Count; i++)
            {
                if (_m_lPlayerTitleInfoList[i] != null && _m_lPlayerTitleInfoList[i].refId == _id)
                    return _m_lPlayerTitleInfoList[i];
            }

            return null;
        }

        /// <summary>
        /// 获取称号组合前缀信息
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public PlayerTitleComboPreInfo getTitleComboPreInfo(long _id)
        {
            for (int i = 0; i < _m_lComboPreList.Count; i++)
            {
                if (_m_lComboPreList[i] != null && _m_lComboPreList[i].id == _id)
                    return _m_lComboPreList[i];
            }

            return null;
        }

        /// <summary>
        /// 获取称号组合后缀信息
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public PlayerTitleComboSfxInfo getTitleComboSfxInfo(long _id)
        {
            for (int i = 0; i < _m_lComboSfxList.Count; i++)
            {
                if (_m_lComboSfxList[i] != null && _m_lComboSfxList[i].id == _id)
                    return _m_lComboSfxList[i];
            }

            return null;
        }

        /// <summary>
        /// 获取称号组合底框信息
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public PlayerTitleComboBgInfo getTitleComboBgInfo(long _id)
        {
            for (int i = 0; i < _m_lComboBgList.Count; i++)
            {
                if (_m_lComboBgList[i] != null && _m_lComboBgList[i].id == _id)
                    return _m_lComboBgList[i];
            }

            return null;
        }

        /// <summary>
        /// 获取当前穿戴的组合称号信息
        /// </summary>
        /// <returns></returns>
        public PlayerInfo_ComboTitle getCurWearComboTitleInfo()
        {
            if (_m_curTitle == null || _m_curTitle.getType() != ENPPlayerTitleType.COMBO)
                return null;

            PlayerInfo_ComboTitle comboTitle = new PlayerInfo_ComboTitle();
            comboTitle.readPackage(_m_curTitle.getInfo());
            return comboTitle;
        }

        /// <summary>
        /// 获取当前穿戴的固定、限时称号信息
        /// </summary>
        /// <returns></returns>
        public PlayerInfo_Title getCurWearCommonTitleInfo()
        {
            if (_m_curTitle == null || _m_curTitle.getType() != ENPPlayerTitleType.COMMON)
                return null;

            PlayerInfo_Title comTitle = new PlayerInfo_Title();
            comTitle.readPackage(_m_curTitle.getInfo());
            return comTitle;
        }

        /// <summary>
        /// 设置称号已读
        /// </summary>
        /// <param name="_id"></param>
        public void setReadTitleRedTip(long _id)
        {
            _m_redTipDealer.readTitle(_id);
        }

        /// <summary>
        /// 设置组合称号前缀已读
        /// </summary>
        /// <param name="_id"></param>
        public void setReadPrefixTitleRedTip(long _id)
        {
            _m_redTipDealer.readPrefix(_id);
        }

        /// <summary>
        /// 设置组合称号后缀已读
        /// </summary>
        /// <param name="_id"></param>
        public void setReadSuffixTitleRedTip(long _id)
        {
            _m_redTipDealer.readSuffix(_id);
        }

        /// <summary>
        /// 设置组合称号底框已读
        /// </summary>
        /// <param name="_id"></param>
        public void setReadBgTitleRedTip(long _id)
        {
            _m_redTipDealer.readBg(_id);
        }

        #region S2C

        /// <summary>
        /// 初始化玩家称号
        /// </summary>
        /// <param name="_msg"></param>
        public void retPlayerTitleList(GS2GC_002_020_RetPlayerTitle _msg)
        {
            if (_msg == null)
                return;

            //初始化已获得固定、限时称号列表
            _m_lPlayerTitleInfoList.Clear();
            if (_msg.getInfoList() != null)
            {
                for (int i = 0; i < _msg.getInfoList().Count; i++)
                {
                    PlayerTitleInfo titleInfo = new PlayerTitleInfo(_msg.getInfoList()[i]);
                    //如果是新的，添加红点
                    if (titleInfo.isNew)
                        _m_redTipDealer.onAddTitle(titleInfo.refId);
                    _m_lPlayerTitleInfoList.Add(titleInfo);
                }
            }

            //初始化已获得组合称号前缀列表
            _m_lComboPreList.Clear();
            List<PlayerInfo_ComboTitlePre> prefixInfoList = _msg.getComboPreList();
            GRefdataCoreMgr.instance.playerTitlePrefixRefCore.dealAllRef(_ref =>
            {
                PlayerTitleComboPreInfo preInfo = new PlayerTitleComboPreInfo(_ref);
                if (prefixInfoList != null)
                {
                    for (int i = 0; i < prefixInfoList.Count; i++)
                    {
                        if (prefixInfoList[i] != null && prefixInfoList[i].getPreId() == _ref.id)
                        {
                            preInfo.setIsViewed(prefixInfoList[i].getViewed());
                            preInfo.isUnlock = true;
                        }
                    }
                }
                //如果是新的，添加红点
                if (preInfo.isNew)
                    _m_redTipDealer.onAddPrefix(preInfo.id);
                _m_lComboPreList.Add(preInfo);
            });

            //初始化已获得组合称号后缀列表
            _m_lComboSfxList.Clear();
            List<PlayerInfo_ComboTitleSfx> suffixInfoList = _msg.getComboSfxList();
            GRefdataCoreMgr.instance.playerTitleSuffixRefCore.dealAllRef(_ref =>
            {
                PlayerTitleComboSfxInfo sufInfo = new PlayerTitleComboSfxInfo(_ref);
                if (suffixInfoList != null)
                {
                    for (int i = 0; i < suffixInfoList.Count; i++)
                    {
                        if (suffixInfoList[i] != null && suffixInfoList[i].getSfxId() == _ref.id)
                        {
                            sufInfo.setIsViewed(suffixInfoList[i].getViewed());
                            sufInfo.isUnlock = true;
                        }
                    }
                }
                //如果是新的，添加红点
                if (sufInfo.isNew)
                    _m_redTipDealer.onAddSuffix(sufInfo.id);
                _m_lComboSfxList.Add(sufInfo);
            });

            //初始化已获得组合称号底框列表
            _m_lComboBgList.Clear();
            List<PlayerInfo_ComboTitleBg> bgInfoList = _msg.getComboBgList();
            GRefdataCoreMgr.instance.playerTitleBgRefCore.dealAllRef(_ref =>
            {
                PlayerTitleComboBgInfo bgInfo = new PlayerTitleComboBgInfo(_ref);
                if (bgInfoList != null)
                {
                    for (int i = 0; i < bgInfoList.Count; i++)
                    {
                        if (bgInfoList[i] != null && bgInfoList[i].getBgId() == _ref.id)
                        {
                            bgInfo.setIsViewed(bgInfoList[i].getViewed());
                            bgInfo.isUnlock = true;
                        }
                    }
                }
                //如果是新的，添加红点
                if (bgInfo.isNew)
                    _m_redTipDealer.onAddBg(bgInfo.id);
                _m_lComboBgList.Add(bgInfo);
            });

            _m_curTitle = _msg.getCurInfo();
            _m_bIsShowOthers = _msg.getIsShow();

            //初始化完成
            setInitDone();
        }

        /// <summary>
        /// 普通称号数据变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onCommTitleChg(GS2GC_018_050_OnCommTitleChg _msg)
        {
            if (_msg == null || _msg.getInfo() == null)
                return;

            PlayerTitleInfo targetInfo = getTitleInfo(_msg.getInfo().getId());

            if (targetInfo == null)
            {
                targetInfo = new PlayerTitleInfo(_msg.getInfo());
                if(targetInfo.isNew)
                    _m_redTipDealer.onAddTitle(targetInfo.refId);
                _m_lPlayerTitleInfoList.Add(targetInfo);
            }
            else
                targetInfo.updateInfo(_msg.getInfo());
        }

        /// <summary>
        /// 组合称号前缀新增
        /// </summary>
        /// <param name="_msg"></param>
        public void onComboTitlePreChg(GS2GC_018_051_OnComboTitlePreChg _msg)
        {
            if (_msg == null || _msg.getPre() == null)
                return;

            PlayerTitleComboPreInfo targetInfo = getTitleComboPreInfo(_msg.getPre().getPreId());
            if (targetInfo != null)
            {
                targetInfo.setIsViewed(_msg.getPre().getViewed());
                targetInfo.isUnlock = true;

                //如果是新的，添加红点
                if (targetInfo.isNew)
                    _m_redTipDealer.onAddPrefix(targetInfo.id);
            }
        }

        /// <summary>
        /// 组合称号后缀新增
        /// </summary>
        /// <param name="_msg"></param>
        public void onComboTitleSfxChg(GS2GC_018_052_OnComboTitleSfxChg _msg)
        {
            if (_msg == null || _msg.getSfx() == null)
                return;

            PlayerTitleComboSfxInfo targetInfo = getTitleComboSfxInfo(_msg.getSfx().getSfxId());
            if (targetInfo != null)
            {
                targetInfo.setIsViewed(_msg.getSfx().getViewed());
                targetInfo.isUnlock = true;

                //如果是新的，添加红点
                if (targetInfo.isNew)
                    _m_redTipDealer.onAddSuffix(targetInfo.id);
            }
        }

        /// <summary>
        /// 组合称号底框新增
        /// </summary>
        /// <param name="_msg"></param>
        public void onComboTitleBgChg(GS2GC_018_053_OnComboTitleBgChg _msg)
        {
            if (_msg == null || _msg.getBg() == null)
                return;

            PlayerTitleComboBgInfo targetInfo = getTitleComboBgInfo(_msg.getBg().getBgId());
            if (targetInfo != null)
            {
                targetInfo.setIsViewed(_msg.getBg().getViewed());
                targetInfo.isUnlock = true;

                //如果是新的，添加红点
                if (targetInfo.isNew)
                    _m_redTipDealer.onAddBg(targetInfo.id);
            }
        }

        /// <summary>
        /// 当前穿戴称号变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onCurTitleChg(GS2GC_018_054_OnCurTitleChg _msg)
        {
            if (_msg == null)
                return;

            _m_curTitle = _msg.getCurInfo();
            WinMsg.SendMsg(WinMsgType.ON_CURRENT_TITLE_CHG, _m_curTitle);
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求玩家称号列表
        /// </summary>
        public void reqPlayerTitleList()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_020_ReqPlayerTitleList());
        }

        /// <summary>
        /// 请求穿戴普通称号
        /// </summary>
        /// <param name="_titleId"></param>
        /// <param name="_callback"></param>
        public void reqSetCommTitle(long _titleId, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_018_001_ReqSetCommTitle(_titleId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_018_001_RetSetCommTitle>((info) =>
                {
                    if (null != _callback)
                        _callback();
                }));
        }

        /// <summary>
        /// 请求穿戴组合称号
        /// </summary>
        /// <param name="_prefixId"></param>
        /// <param name="_suffixId"></param>
        /// <param name="_bgId"></param>
        /// <param name="_callback"></param>
        public void reqSetComboTitle(long _prefixId, long _suffixId, long _bgId, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_018_002_ReqSetComboTitle(_prefixId, _suffixId, _bgId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_018_002_RetSetComboTitle>((info) =>
                {
                    if (null != _callback)
                        _callback();
                }));
        }

        /// <summary>
        /// 请求设置称号是否可展示
        /// </summary>
        /// <param name="_isShow"></param>
        /// <param name="_callback"></param>
        public void reqSetTitleShow(bool _isShow, Action _callback = null)
        {
            _m_bIsShowOthers = _isShow;
            NPGSClientListener.sendRequestByLog(new GC2GS_018_003_ReqSetTitleShow(_isShow),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_018_003_RetSetTitleShow>((info) =>
                {
                    if (null != _callback)
                        _callback();
                }));
        }

        /// <summary>
        /// 请求解锁组合称号前缀
        /// </summary>
        /// <param name="_preId"></param>
        /// <param name="_callback"></param>
        public void reqUnlockComboTitlePre(long _preId, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_018_004_ReqUnlockComboTitlePre(_preId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_018_004_RetUnlockComboTitlePre>((info) =>
                {
                    if (null != _callback)
                        _callback();
                }));
        }

        /// <summary>
        /// 请求解锁组合称号后缀
        /// </summary>
        /// <param name="_sfxId"></param>
        /// <param name="_callback"></param>
        public void reqUnlockComboTitleSfx(long _sfxId, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_018_005_ReqUnlockComboTitleSfx(_sfxId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_018_005_RetUnlockComboTitleSfx>((info) =>
                {
                    if (null != _callback)
                        _callback();
                }));
        }

        /// <summary>
        /// 请求解锁组合称号底框
        /// </summary>
        /// <param name="_bgId"></param>
        /// <param name="_callback"></param>
        public void reqUnlockComboTitleBg(long _bgId, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_018_006_ReqUnlockComboTitleBg(_bgId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_018_006_RetUnlockComboTitleBg>((info) =>
                {
                    if (null != _callback)
                        _callback();
                }));
        }

        /// <summary>
        /// 设置组合称号前缀已查看
        /// </summary>
        /// <param name="_preId"></param>
        public void reqviewComboTitlePre(long _preId)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_018_007_ReqviewComboTitlePre(_preId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_018_007_RetviewComboTitlePre>(null));
        }

        /// <summary>
        /// 设置组合称号后缀已查看
        /// </summary>
        /// <param name="_sfxId"></param>
        public void reqViewComboTitleSfx(long _sfxId)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_018_008_ReqViewComboTitleSfx(_sfxId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_018_008_RetViewComboTitleSfx>(null));
        }

        /// <summary>
        /// 设置组合称号底框已查看
        /// </summary>
        /// <param name="_bgId"></param>
        public void reqViewComboTitleBg(long _bgId)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_018_009_ReqViewComboTitleBg(_bgId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_018_009_RetViewComboTitleBg>(null));
        }

        /// <summary>
        /// 设置称号已查看
        /// </summary>
        /// <param name="_titleId"></param>
        public void reqviewTitle(long _titleId)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_018_015_ReqviewTitle(_titleId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_018_015_RetviewTitle>(null));
        }

        #endregion
    }
}
