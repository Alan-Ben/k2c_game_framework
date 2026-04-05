using System;
using System.Text.RegularExpressions;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟修改名称界面
    /// </summary>
    public class GGUIWndGuildChangeName : _ANPGGUIBasicWnd<GGUIMonoGuildChangeName>
    {
        private static GGUIWndGuildChangeName _g_instance = new GGUIWndGuildChangeName();
        public static GGUIWndGuildChangeName instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildChangeName();
                return _g_instance;
            }
        }

        //确认回调<name,simpleName>
        private Action<string, string> _m_aOnConfirm;
        //消耗道具
        private NPGGUIWndCommonItem _m_wCostItem;
        //原先的名称
        private string _m_sOriName;
        //原先的简称
        private string _m_sOriSimpleName;
        //简称正则表达式字符串
        private const string PATTERN_STRING = "^[a-zA-Z0-9]+$";

        public GGUIWndGuildChangeName() : base(EALUIWndLayer.ADDITION)
        {

        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildChangeName.assetPath; } }

        protected override string _monoObjName { get { return GGUIMonoGuildChangeName.objName; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wCostItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCostItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wCostItem?.discard();
            _m_wCostItem = null;

            if (wnd.inputName != null)
                wnd.inputName.onValueChanged?.RemoveAllListeners();

            if (wnd.inputSimpleName != null)
                wnd.inputSimpleName.onValueChanged?.RemoveAllListeners();

            ALUGUICommon.uncombineBtnClick(wnd.btnChg, _onClickChg);
            ALUGUICommon.uncombineBtnClick(wnd.btnCancel, _onClickCancel);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCancel);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);

            if (wnd.inputName != null)
                wnd.inputName.onValueChanged?.AddListener(_onNameEdit);

            if(wnd.inputSimpleName != null)
                wnd.inputSimpleName.onValueChanged?.AddListener(_onSimpleNameEdit);

            ALUGUICommon.combineBtnClick(wnd.btnChg, _onClickChg);
            ALUGUICommon.combineBtnClick(wnd.btnCancel, _onClickCancel);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCancel);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_onConfirm"></param>
        public void setInfo(Action<string, string> _onConfirm)
        {
            _m_aOnConfirm = _onConfirm;

            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            _m_sOriName = guildInfo.name;
            _m_sOriSimpleName = guildInfo.simpleName;

            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            //设置输入
            if(wnd.inputName != null)
                wnd.inputName.text = guildInfo.name;
            if(wnd.inputSimpleName != null)
                wnd.inputSimpleName.text = guildInfo.simpleName;

            //设置消耗
            if (_m_wCostItem != null)
            {
                _m_wCostItem.showWnd();
                _m_wCostItem.setItem(GRefdataCoreMgr.instance.npGeneral.guild_change_name_cost);
            }
        }

        //名称编辑变化
        private void _onNameEdit(string _str)
        {
            if (wnd == null || wnd.inputName == null)
                return;

            string nameString = wnd.inputName.text;
            int strLength = CharacterDetermineMgr.instance.getUnicodeStringLength(nameString);
            WCGIntRange nameLengthRange = GRefdataCoreMgr.instance.npGeneral.guild_name_length_limit;
            if (nameLengthRange == null)
                return;

            //设置字数提示
            ALUGUICommon.setLabelTxt(wnd.txtNameCount, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, strLength, nameLengthRange.max));

            //设置文本是否合法提示
            if (strLength < nameLengthRange.min)
            {
                //名称过短
                ALUGUICommon.setLabelTxt(wnd.txtNameTip, TextTranslate.instance.getLanguage(TransKeyConst.guild_inputNameUnderLimit_none));
            }
            else if (strLength > nameLengthRange.max)
            {
                //名称过长
                ALUGUICommon.setLabelTxt(wnd.txtNameTip, TextTranslate.instance.getLanguage(TransKeyConst.guild_inputNameOverLimit_none));
            }
            else if (CharacterDetermineMgr.instance.isPlayerNameIllegal(nameString))
            {
                //名称包含特殊字符
                ALUGUICommon.setLabelTxt(wnd.txtNameTip, TextTranslate.instance.getLanguage(TransKeyConst.guild_inputNameIllegal_none));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtNameTip, "");
            }
        }

        //简称编辑变化
        private void _onSimpleNameEdit(string _str)
        {
            if (wnd == null || wnd.inputSimpleName == null)
                return;

            string abbreviationString = wnd.inputSimpleName.text;
            int strLength = CharacterDetermineMgr.instance.getUnicodeStringLength(abbreviationString);
            WCGIntRange lengthRange = GRefdataCoreMgr.instance.npGeneral.guild_simple_name_length_limit;
            if (lengthRange == null)
                return;

            //设置字数提示
            ALUGUICommon.setLabelTxt(wnd.txtSimpleNameCount, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, strLength, lengthRange.max));

            //设置文本是否合法提示
            if (!string.IsNullOrEmpty(abbreviationString) && !Regex.IsMatch(abbreviationString, PATTERN_STRING))
            {
                //简称包含字母和数字外特殊字符
                ALUGUICommon.setLabelTxt(wnd.txtSimpleNameTip, TextTranslate.instance.getLanguage(TransKeyConst.guild_inputAbbreviationIllegal_none));
            }
            else if (strLength < lengthRange.min)
            {
                //简称过短
                ALUGUICommon.setLabelTxt(wnd.txtSimpleNameTip, TextTranslate.instance.getLanguage(TransKeyConst.guild_inputAbbreviationUnderLimit_none));
            }
            else if (strLength > lengthRange.max)
            {
                //简称过长
                ALUGUICommon.setLabelTxt(wnd.txtSimpleNameTip, TextTranslate.instance.getLanguage(TransKeyConst.guild_inputAbbreviationOverLimit_none));
            }
            else if (CharacterDetermineMgr.instance.isPlayerNameIllegal(abbreviationString))
            {
                //简称包含特殊字符
                ALUGUICommon.setLabelTxt(wnd.txtSimpleNameTip, TextTranslate.instance.getLanguage(TransKeyConst.guild_inputAbbreviationIllegal_none));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtSimpleNameTip, "");
            }
        }

        //点击修改按钮
        private void _onClickChg(GameObject _go)
        {
            if (wnd == null)
                return;

            //检查名称
            string nameString = wnd.inputName?.text;
            long nameLength = CharacterDetermineMgr.instance.getUnicodeStringLength(nameString);
            WCGIntRange nameLengthRange = GRefdataCoreMgr.instance.npGeneral.guild_name_length_limit;
            if (nameLength < nameLengthRange.min)
            {
                //太短
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputNameUnderLimit_none);
                return;
            }
            else if (nameLength > nameLengthRange.max)
            {
                //太长
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputNameOverLimit_none);
                return;
            }
            else if (CharacterDetermineMgr.instance.isPlayerNameIllegal(nameString))
            {
                //非法
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputNameIllegal_none);
                return;
            }

            //检查简称
            string abbreviationString = wnd.inputSimpleName?.text;
            long abbreviationLength = CharacterDetermineMgr.instance.getUnicodeStringLength(abbreviationString);
            WCGIntRange abbreviationLengthRange = GRefdataCoreMgr.instance.npGeneral.guild_simple_name_length_limit;
            if (!string.IsNullOrEmpty(abbreviationString) && !Regex.IsMatch(abbreviationString, PATTERN_STRING))
            {
                //非法
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputAbbreviationIllegal_none);
                return;
            }
            else if (abbreviationLength < abbreviationLengthRange.min)
            {
                //太短
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputAbbreviationUnderLimit_none);
                return;
            }
            else if (abbreviationLength > abbreviationLengthRange.max)
            {
                //太长
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputAbbreviationOverLimit_none);
                return;
            }
            else if (CharacterDetermineMgr.instance.isPlayerNameIllegal(abbreviationString))
            {
                //非法
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputAbbreviationIllegal_none);
                return;
            }

            //判断名称是否变更
            if (_m_sOriName == nameString && _m_sOriSimpleName == abbreviationString)
            {
                //未修改
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_nameNotChg_none);
                return;
            }

            //判断消耗是否足够
            if (!GCommon.isItemEnough(GRefdataCoreMgr.instance.npGeneral.guild_change_name_cost, true))
                return;

            _m_aOnConfirm?.Invoke(wnd.inputName?.text, wnd.inputSimpleName?.text);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_CHANGE_NAME);
        }

        //点击取消按钮
        private void _onClickCancel(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_CHANGE_NAME);
        }
    }
}