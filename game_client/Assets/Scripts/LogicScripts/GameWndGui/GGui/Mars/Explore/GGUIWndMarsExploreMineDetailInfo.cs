using ALPackage;
using Common.MarsObj;
using GS2GC.p004_PlayerOp;
using NPCommon;
using System;
using SQLite4Unity3d;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星探索矿点详情界面
    /// 展示矿点的详细信息，包括采集进度、占领者信息等
    /// 数据由外部传入
    /// </summary>
    public class GGUIWndMarsExploreMineDetailInfo : _ANPGGUIBasicWnd<GGUIMonoMarsExploreMineDetailInfo>
    {
        private static GGUIWndMarsExploreMineDetailInfo _g_instance;
        [NotNull] public static GGUIWndMarsExploreMineDetailInfo instance { get { return _g_instance ??= new GGUIWndMarsExploreMineDetailInfo(); } }
        
        // 矿点动态数据
        private Mars_MineDynamic _m_mineInfo;
        // 矿点配置数据
        private MarsExploreMineRefObj _m_rMineRef;
        // 显示序列号，用于判断回调是否有效
        private long _m_lShowSerialize;

        // 子窗口
        private NPGGuiWndTexture _m_bannerWnd;
        private NPGGuiWndTexture _m_iconWnd;
        private NPGGUIWndPlayerIcon _m_playerHeadIconWnd;
        private NPGGUIWndCommonItem _m_resourceIconWnd;
        
        // tick任务控制器
        private ALCommonEnableTaskController _m_tickTask;

        public GGUIWndMarsExploreMineDetailInfo() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsExploreMineDetailInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsExploreMineDetailInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_bannerWnd?.showWnd();
            _m_iconWnd?.showWnd();
            _m_playerHeadIconWnd?.showWnd();
            _m_resourceIconWnd?.showWnd();

            // 刷新界面
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            // 刷新序列号
            _m_lShowSerialize = ALSerializeOpMgr.next();

            // 关闭tick任务
            _m_tickTask.setDisable();

            _m_bannerWnd?.hideWnd();
            _m_iconWnd?.hideWnd();
            _m_playerHeadIconWnd?.hideWnd();
            _m_resourceIconWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_bannerWnd?.discardTexture();
            _m_iconWnd?.discardTexture();
            _m_playerHeadIconWnd?.resetWnd();
            _m_resourceIconWnd?.resetWnd();
        }

        protected override void _onDiscard()
        {
            // 关闭tick任务
            _m_tickTask.setDisable();

            // 销毁子窗口
            _m_bannerWnd?.discard();
            _m_bannerWnd = null;

            _m_iconWnd?.discard();
            _m_iconWnd = null;

            _m_playerHeadIconWnd?.discard();
            _m_playerHeadIconWnd = null;

            _m_resourceIconWnd?.discard();
            _m_resourceIconWnd = null;

            if (wnd == null)
                return;

            // 解绑按钮点击
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 绑定按钮点击
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);

            // 初始化子窗口
            if (wnd.imgBanner != null)
                _m_bannerWnd = new NPGGuiWndTexture(wnd.imgBanner);

            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.playerHeadIcon != null)
                _m_playerHeadIconWnd = new NPGGUIWndPlayerIcon(wnd.playerHeadIcon);

            if (wnd.monoResourceIcon != null)
                _m_resourceIconWnd = new NPGGUIWndCommonItem(wnd.monoResourceIcon);
        }

        /// <summary>
        /// 刷新界面，外部传入矿点数据
        /// </summary>
        /// <param name="_mineDynamic">矿点动态数据</param>
        /// <param name="_mineRef">矿点配置数据</param>
        public void refreshWnd(Mars_MineDynamic _mineDynamic, MarsExploreMineRefObj _mineRef)
        {
            _m_mineInfo = _mineDynamic;
            _m_rMineRef = _mineRef;
            // 更新序列号
            _m_lShowSerialize = ALSerializeOpMgr.next();

            _refreshWnd();
        }

        /// <summary>
        /// 内部刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            if (_m_mineInfo == null || _m_rMineRef == null)
                return;

            // 设置横幅图片
            _m_bannerWnd?.setTexture(_m_rMineRef.banner);
            // 设置图标
            _m_iconWnd?.setTexture(_m_rMineRef.icon);

            // 设置名称
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_rMineRef.name));
            // 设置等级
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_rMineRef.mine_lvl));
            // 设置描述
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_rMineRef.desc));

            // 设置资源图标
            _m_resourceIconWnd?.setItem(_m_rMineRef.res_type.toCommonItemData());

            // 设置采集速度
            long oriCollectSpeed = _m_rMineRef.mars_mine_collects_speed * 60 * 60;
            long addedCollectSpeed = _m_mineInfo.getCollectSpeed() * 60 * 60 - oriCollectSpeed;
            ALUGUICommon.setLabelTxt(wnd.txtCollectSpeed,
                TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineCollectSpeed_num_num
                    , oriCollectSpeed.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), addedCollectSpeed.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));

            ALUGUICommon.setLabelTxt(wnd.txtTeamOrder, _m_mineInfo.getOccupiedTeamId());
            
            // 设置队伍带兵量
            ALUGUICommon.setLabelTxt(wnd.txtTroopNum,
                TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineTeamTroopNum_num, 
                    _m_mineInfo.getTroopNum().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));

            // 设置队伍实力
            ALUGUICommon.setLabelTxt(wnd.txtTeamPower, _m_mineInfo.getTeamPower().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            
            // 刷新占领者信息
            _refreshOccupyInfo();

            // 先强制刷新一次
            _tick();
            // 开启tick任务
            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);
        }

        /// <summary>
        /// 刷新占领者信息
        /// </summary>
        private void _refreshOccupyInfo()
        {
            if (wnd == null || !_m_bIsShow || _m_mineInfo == null)
                return;

            long occupiedCid = _m_mineInfo.getOccupiedCid();

            // 如果没有占领者，清空玩家名称
            if (occupiedCid == 0)
            {
                ALUGUICommon.setLabelTxt(wnd.txtPlayerName, string.Empty);
                _m_playerHeadIconWnd?.discard();
                return;
            }

            // 记录当前序列号
            long tmpSerialize = _m_lShowSerialize;

            // 请求玩家信息
            _reqPlayerInfo(tmpSerialize, _playerInfo =>
            {
                // 序列号不一致则不处理
                if (_m_lShowSerialize != tmpSerialize)
                    return;

                if (_playerInfo == null || wnd == null)
                    return;

                // 判断是否是自己、公会成员或敌人
                bool isSelf = occupiedCid == NPPlayer.instance.playerInfo.CID;
                bool isGuildMember = NPPlayer.instance.guildComp.guildInfo != null && _playerInfo.getGuildId() == NPPlayer.instance.guildComp.guildInfo.guildId;

                // 设置玩家名称，并添加颜色
                string guildPlayerName = string.Empty;
                if(string.IsNullOrEmpty(_playerInfo.getGuildSimpleName()))
                    guildPlayerName = _playerInfo.getPlayerName();
                else
                    guildPlayerName = TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineOccupantName, 
                    _playerInfo.getGuildSimpleName(), _playerInfo.getPlayerName());
                
                Color showColor;
                if (isSelf)
                    showColor = wnd.selfNameColor;
                else if (isGuildMember)
                    showColor = wnd.guildMemberNameColor;
                else
                    showColor = wnd.enemyNameColor;

                string coloredName = GCommon.addColorForRichText(guildPlayerName, showColor);
                ALUGUICommon.setLabelTxt(wnd.txtPlayerName, coloredName);

                // 设置玩家头像
                _m_playerHeadIconWnd?.setPlayerInfo(new NPCommonSimplePlayerInfo(_playerInfo));
            });
        }

        /// <summary>
        /// 请求玩家信息
        /// </summary>
        /// <param name="_serialize">序列号</param>
        /// <param name="_complete">回调</param>
        private void _reqPlayerInfo(long _serialize, Action<PlayerInfo_IconShow> _complete)
        {
            if (_m_mineInfo == null)
            {
                _complete?.Invoke(null);
                return;
            }

            // 如果没有占领者
            if (_m_mineInfo.getOccupiedCid() == 0)
            {
                _complete?.Invoke(null);
                return;
            }

            // 如果是自己
            if (_m_mineInfo.getOccupiedCid() == NPPlayer.instance.playerInfo.CID)
            {
                PlayerInfo_IconShow selfInfo = NPPlayer.instance.playerInfo.getPlayerBriefInfo();
                _complete?.Invoke(selfInfo);
                return;
            }

            // 请求其他玩家信息
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_011_ReqSomeOnePlayerBriefInfo(_m_mineInfo.getOccupiedCid()),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>((_isSuc, _msg) =>
                {
                    // 序列号不一致则不处理
                    if (_m_lShowSerialize != _serialize)
                        return;

                    if (!_isSuc)
                    {
                        _complete?.Invoke(null);
                        return;
                    }

                    PlayerInfo_IconShow briefInfo = _msg?.getPlayerBrief();
                    _complete?.Invoke(briefInfo);
                }));
        }

        /// <summary>
        /// 每帧tick刷新处理
        /// </summary>
        private void _tick()
        {
            if (wnd == null || !_m_bIsShow || _m_mineInfo == null || _m_rMineRef == null)
                return;

            // 计算剩余资源数量
            long remainNum = _getRemainNum();
            // 计算已采集资源数量
            long hasCollectNum = _m_rMineRef.res_num - remainNum;

            // 设置剩余采集资源数量
            ALUGUICommon.setLabelTxt(wnd.txtRemainResourceNum,
                TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineRemainResource_num, 
                    remainNum.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));

            // 设置已采集资源数量
            ALUGUICommon.setLabelTxt(wnd.txtHasCollectResourceNum,
                TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineHasCollectResource_num, 
                    hasCollectNum.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));

            // 设置剩余采集时间
            long remainTime = _calculateRemainCollectTimeSec();
            ALUGUICommon.setLabelTxt(wnd.txtRemainCollectTime, TimeUtil.millisecondsToTime_DayHourOrHMS(remainTime));
        }

        /// <summary>
        /// 计算剩余采集时间（毫秒）
        /// </summary>
        private long _calculateRemainCollectTimeSec()
        {
            if (_m_mineInfo == null)
                return 0;

            if (_m_mineInfo.getCollectSpeed() <= 0)
                return 0;

            return Mathf.FloorToInt(_getRemainNum() * 1000f / _m_mineInfo.getCollectSpeed());
        }

        /// <summary>
        /// 获取剩余资源数量
        /// </summary>
        private long _getRemainNum()
        {
            if (_m_mineInfo == null)
                return 0;

            // 如果没有采集速度，直接返回剩余数量
            if (_m_mineInfo.getCollectSpeed() <= 0)
                return _m_mineInfo.getRemainNum();

            // 根据采集速度和时间计算实时剩余数量
            long remain = Mathf.FloorToInt(_m_mineInfo.getRemainNum() - 
                (FpsAndPingMgr.instance.serverTimeTag - _m_mineInfo.getOccupiedMs()) * _m_mineInfo.getCollectSpeed() / 1000f);
            
            // 确保不为负数
            return Math.Max(0, remain);
        }


        /// <summary>
        /// 关闭按钮点击
        /// </summary>
        private void _onClickClose(GameObject _go)
        {
            // 关闭窗口
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_MINE_DETAIL_INFO);
        }
    }
}
