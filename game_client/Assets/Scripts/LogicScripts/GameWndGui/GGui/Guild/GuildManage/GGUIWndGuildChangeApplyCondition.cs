using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildEnum;
using Common.GuildObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟修改申请条件界面
    /// </summary>
    public class GGUIWndGuildChangeApplyCondition : _ANPGGUIBasicWnd<GGUIMonoGuildChangeApplyCondition>
    {
        private static GGUIWndGuildChangeApplyCondition _g_instance = new GGUIWndGuildChangeApplyCondition();
        public static GGUIWndGuildChangeApplyCondition instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildChangeApplyCondition();
                return _g_instance;
            }
        }

        //加入方式列表
        private List<EGuildJoinType> _m_lJoinTypeList;
        //玩家等级列表
        private List<PlayerLvlRefObj> _m_lPlayerLvlRefList;
        //加入方式选择下标
        private int _m_iTypeSelectIndex;
        //玩家等级选择下标
        private int _m_iLevelSelectIndex;

        public GGUIWndGuildChangeApplyCondition() : base(EALUIWndLayer.ADDITION)
        {

        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildChangeApplyCondition.assetPath; } }

        protected override string _monoObjName { get { return GGUIMonoGuildChangeApplyCondition.objName; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _setInfo();
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (wnd.inputNationPowerLimit != null)
                wnd.inputNationPowerLimit.onValueChanged.RemoveAllListeners();

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnSave, _onClickSave);
            ALUGUICommon.uncombineBtnClick(wnd.btnPreviousJoinType, _onClickPreviousJoinType);
            ALUGUICommon.uncombineBtnClick(wnd.btnNextJoinType, _onClickNextJoinType);
            ALUGUICommon.uncombineBtnClick(wnd.btnPreviousJoinLevel, _onClickPreviousJoinLevel);
            ALUGUICommon.uncombineBtnClick(wnd.btnNextJoinLevel, _onClickNextJoinLevel);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //初始化枚举列表
            _m_lJoinTypeList = new List<EGuildJoinType>();
            foreach (EGuildJoinType joinType in Enum.GetValues(typeof(EGuildJoinType)))
            {
                if(joinType != EGuildJoinType.NONE)
                    _m_lJoinTypeList.Add(joinType);
            }

            //初始化玩家等级列表
            _m_lPlayerLvlRefList = new List<PlayerLvlRefObj>();
            _m_lPlayerLvlRefList.AddRange(GRefdataCoreMgr.instance.playerLvlCore.refList);

            if(wnd.inputNationPowerLimit != null)
                wnd.inputNationPowerLimit.onValueChanged.AddListener(_onNationPowerEdit);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnSave, _onClickSave);
            ALUGUICommon.combineBtnClick(wnd.btnPreviousJoinType, _onClickPreviousJoinType);
            ALUGUICommon.combineBtnClick(wnd.btnNextJoinType, _onClickNextJoinType);
            ALUGUICommon.combineBtnClick(wnd.btnPreviousJoinLevel, _onClickPreviousJoinLevel);
            ALUGUICommon.combineBtnClick(wnd.btnNextJoinLevel, _onClickNextJoinLevel);
        }

        //刷新窗口
        private void _setInfo()
        {
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            //设置选择的加入方式
            for (int i = 0; i < _m_lJoinTypeList.Count; i++)
            {
                if (_m_lJoinTypeList[i] == guildInfo.joinType)
                {
                    _m_iTypeSelectIndex = i;
                    break;
                }
            }

            //设置等级限制
            _AGuildJoinLimitInfo joinLimitInfo = guildInfo.getJoinLimitInfoByType(EGuildJoinLimitType.LEVEL);
            if (joinLimitInfo != null)
            {
                for (int i = 0; i < _m_lPlayerLvlRefList.Count; i++)
                {
                    if (_m_lPlayerLvlRefList[i].lvl == joinLimitInfo.value)
                    {
                        _m_iLevelSelectIndex = i;
                        break;
                    }
                }
            }

            //设置国力
            _AGuildJoinLimitInfo nationPowerJoinLimitInfo = guildInfo.getJoinLimitInfoByType(EGuildJoinLimitType.NATION_POWER);
            long limitNationPower = 0;
            if (nationPowerJoinLimitInfo != null)
                limitNationPower = nationPowerJoinLimitInfo.value;
            if (wnd != null && wnd.inputNationPowerLimit != null && nationPowerJoinLimitInfo != null)
            {
                wnd.inputNationPowerLimit.text = limitNationPower.ToString();
            }
            ALUGUICommon.setGameObjEnable(wnd?.goNationPowerLimitHint, false);//单位没有扩大，所以先隐藏提示

            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshJoinType();
            _refreshJoinLevel();
        }

        //刷新加入方式
        private void _refreshJoinType()
        {
            if (wnd == null || _m_lJoinTypeList == null || _m_iTypeSelectIndex < 0 || _m_iTypeSelectIndex >= _m_lJoinTypeList.Count)
                return;

            ALUGUICommon.setGameObjEnable(wnd.btnPreviousJoinType, _m_iTypeSelectIndex != 0);
            ALUGUICommon.setGameObjEnable(wnd.btnNextJoinType, _m_iTypeSelectIndex != _m_lJoinTypeList.Count - 1);

            switch (_m_lJoinTypeList[_m_iTypeSelectIndex])
            {
                case EGuildJoinType.FREE_JOIN:
                    ALUGUICommon.setLabelTxt(wnd.txtJoinType,TextTranslate.instance.getLanguage(TransKeyConst.guild_joinLimit_freeJoin_none));
                    break;
                case EGuildJoinType.APPROVAL_JOIN:
                    ALUGUICommon.setLabelTxt(wnd.txtJoinType, TextTranslate.instance.getLanguage(TransKeyConst.guild_joinLimit_approvalJoin_none));
                    break;
                case EGuildJoinType.DECLINE_JOIN:
                    ALUGUICommon.setLabelTxt(wnd.txtJoinType, TextTranslate.instance.getLanguage(TransKeyConst.guild_joinLimit_declineJoin_none));
                    break;
            }
        }

        //刷新加入等级限制
        private void _refreshJoinLevel()
        {
            if (wnd == null || _m_lPlayerLvlRefList == null || _m_iLevelSelectIndex < 0 || _m_iLevelSelectIndex >= _m_lPlayerLvlRefList.Count)
                return;

            ALUGUICommon.setGameObjEnable(wnd.btnPreviousJoinLevel, _m_iLevelSelectIndex != 0);
            ALUGUICommon.setGameObjEnable(wnd.btnNextJoinLevel, _m_iLevelSelectIndex != _m_lPlayerLvlRefList.Count - 1);

            if (_m_lPlayerLvlRefList[_m_iLevelSelectIndex] != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtJoinLevel, _m_lPlayerLvlRefList[_m_iLevelSelectIndex].nameStr);
            }
        }

        //国力输入编辑事件
        private void _onNationPowerEdit(string _str)
        {
            // if (wnd == null)
            //     return;
            //
            // string valueString = wnd.inputNationPowerLimit.text;
            // long.TryParse(valueString, out long nationPower);
            // ALUGUICommon.setGameObjEnable(wnd.goNationPowerLimitHint, false);
        }

        #region 点击事件

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_APPLY_CONDITION);
        }

        //点击上一个加入方式
        private void _onClickPreviousJoinType(GameObject _go)
        {
            _m_iTypeSelectIndex--;
            if (_m_iTypeSelectIndex < 0)
                _m_iTypeSelectIndex = 0;

            _refreshJoinType();
        }

        //点击下一个加入方式
        private void _onClickNextJoinType(GameObject _go)
        {
            if (_m_lJoinTypeList == null)
                return;

            _m_iTypeSelectIndex++;
            if (_m_iTypeSelectIndex >= _m_lJoinTypeList.Count)
                _m_iTypeSelectIndex = _m_lJoinTypeList.Count - 1;

            _refreshJoinType();
        }

        //点击上一个加入等级
        private void _onClickPreviousJoinLevel(GameObject _go)
        {
            _m_iLevelSelectIndex--;
            if (_m_iLevelSelectIndex < 0)
                _m_iLevelSelectIndex = 0;

            _refreshJoinLevel();
        }

        //点击下一个加入等级
        private void _onClickNextJoinLevel(GameObject _go)
        {
            if (_m_lPlayerLvlRefList == null)
                return;

            _m_iLevelSelectIndex++;
            if (_m_iLevelSelectIndex >= _m_lPlayerLvlRefList.Count)
                _m_iLevelSelectIndex = _m_lPlayerLvlRefList.Count - 1;

            _refreshJoinLevel();
        }

        //点击保存
        private void _onClickSave(GameObject _go)
        {
            if (wnd == null || wnd.inputNationPowerLimit == null || _m_lJoinTypeList == null || _m_lPlayerLvlRefList == null)
                return;

            long.TryParse(wnd.inputNationPowerLimit.text, out long nationPower);

            EGuildJoinType joinType = _m_lJoinTypeList[_m_iTypeSelectIndex];
            List<Guild_JoinLimitInfo> joinLimitInfoList = new List<Guild_JoinLimitInfo>();
            joinLimitInfoList.Add(new Guild_JoinLimitInfo(EGuildJoinLimitType.LEVEL, _m_lPlayerLvlRefList[_m_iLevelSelectIndex].lvl));
            joinLimitInfoList.Add(new Guild_JoinLimitInfo(EGuildJoinLimitType.NATION_POWER, nationPower));

            //请求保存
            NPPlayer.instance.guildComp.reqSetGuildJoinType(joinType, joinLimitInfoList, () =>
            {
                //修改成功
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_chgInfoSuc_none);
            });
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_APPLY_CONDITION);
        }

        #endregion
    }
}