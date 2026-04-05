using ALPackage;
using System.Text.RegularExpressions;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟创建界面
    /// </summary>
    public class GGUIWndGuildCreate : _ANPGGUIBasicWnd<GGUIMonoGuildCreate>
    {
        private static GGUIWndGuildCreate _g_instance = new GGUIWndGuildCreate();
        public static GGUIWndGuildCreate instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildCreate();
                return _g_instance;
            }
        }

        //创建联盟消耗
        private NPGGUIWndCommonItem _m_costItem;
        //旗帜图标
        private NPGGuiWndTexture _m_wFlagIcon;
        //当前选择的旗帜
        private GuildFlagRefObj _m_selectFlagRef;
        //是否允许其他玩家随机加入开关
        private NPGGUIWndCommonToggleEx _m_wToggle;
        //是初始化了输入
        private bool _m_isInitInput;
        //简称正则表达式字符串
        private const string PATTERN_STRING = "^[a-zA-Z0-9]+$";

        public GGUIWndGuildCreate() : base(EALUIWndLayer.ADDITION)
        {

        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildCreate.assetPath; } }

        protected override string _monoObjName { get { return GGUIMonoGuildCreate.objName; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            if (!_m_isInitInput)
            {
                _m_isInitInput = true;
                resetInputState();
            }

            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_costItem?.hideWnd();
            _m_wFlagIcon?.hideWnd();
            _m_wToggle?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_costItem?.resetWnd();
            _m_wFlagIcon?.discardTexture();
            _m_wToggle?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_selectFlagRef = null;
            _m_isInitInput = false;

            _m_costItem?.discard();
            _m_costItem = null;

            _m_wFlagIcon?.discard();
            _m_wFlagIcon = null;

            _m_wToggle?.discard();
            _m_wToggle = null;

            if(wnd.inputGuildName !=  null)
                wnd.inputGuildName.onValueChanged?.RemoveAllListeners();

            if(wnd.inputAbbreviation !=  null)
                wnd.inputAbbreviation.onValueChanged?.RemoveAllListeners();

            if(wnd.inputDeclaration !=  null)
                wnd.inputDeclaration.onValueChanged?.RemoveAllListeners();

            ALUGUICommon.uncombineBtnClick(wnd.btnChgFlag, _onClickChgFlag);
            ALUGUICommon.uncombineBtnClick(wnd.btnCreate, _onClickCreate);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_isInitInput = false;

            if (wnd.monoCostItem != null)
                _m_costItem = new NPGGUIWndCommonItem(wnd.monoCostItem);

            if (wnd.imgFlag != null)
                _m_wFlagIcon = new NPGGuiWndTexture(wnd.imgFlag);

            if (wnd.allowOthersJoinRandomlyToggle != null)
            {
                _m_wToggle = new NPGGUIWndCommonToggleEx(wnd.allowOthersJoinRandomlyToggle);
                _m_wToggle.clickDelegate += _onClickToggle;
            }

            if (wnd.inputGuildName != null)
                wnd.inputGuildName.onValueChanged?.AddListener(_onNameEdit);

            if(wnd.inputAbbreviation != null)
                wnd.inputAbbreviation.onValueChanged?.AddListener(_onAbbreviationEdit);

            if(wnd.inputDeclaration != null)
                wnd.inputDeclaration.onValueChanged?.AddListener(_onDeclarationEdit);

            ALUGUICommon.combineBtnClick(wnd.btnChgFlag, _onClickChgFlag);
            ALUGUICommon.combineBtnClick(wnd.btnCreate, _onClickCreate);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 重置输入状态
        /// </summary>
        public void resetInputState()
        {
            if (wnd == null)
                return;

            _m_wToggle?.setSelected(true);

            if (wnd.inputGuildName != null)
                wnd.inputGuildName.text = "";

            if (wnd.inputAbbreviation != null)
                wnd.inputAbbreviation.text = "";

            if (wnd.inputDeclaration != null)
                wnd.inputDeclaration.text = "";

            ALUGUICommon.setLabelTxt(wnd.txtInputNameTip, "");
            ALUGUICommon.setLabelTxt(wnd.txtInputAbbreviationTip, "");

            WCGIntRange nameLengthRange = GRefdataCoreMgr.instance.npGeneral.guild_name_length_limit;
            ALUGUICommon.setLabelTxt(wnd.txtInputNameCount, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, 0, nameLengthRange?.max));

            WCGIntRange declarationLengthRange = GRefdataCoreMgr.instance.npGeneral.guild_declaration_length_limit;
            ALUGUICommon.setLabelTxt(wnd.txtInputDeclarationCount, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, 0, declarationLengthRange?.max));

            WCGIntRange abbreviationLengthRange = GRefdataCoreMgr.instance.npGeneral.guild_simple_name_length_limit;
            ALUGUICommon.setLabelTxt(wnd.txtInputAbbreviationCount, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, 0, abbreviationLengthRange?.max));
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshCostItem();
            _refreshFlag(GRefdataCoreMgr.instance.guildFlagRefCore.refList?.GetRandomItem());
            _refreshCreateBtn();
        }

        //刷新创建联盟消耗
        private void _refreshCostItem()
        {
            if (_m_costItem != null)
            {
                _m_costItem.showWnd();
                _m_costItem.setItem(GRefdataCoreMgr.instance.npGeneral.guild_create_cost);
            }
        }

        //刷新旗帜
        private void _refreshFlag(GuildFlagRefObj _flagRefObj)
        {
            if (_flagRefObj == null)
                return;

            //设置当前选择的旗帜
            _m_selectFlagRef = _flagRefObj;

            if (_m_wFlagIcon != null)
            {
                _m_wFlagIcon.showWnd();
                _m_wFlagIcon.setTexture(_m_selectFlagRef.icon);
            }
        }

        //刷新按钮置灰状态
        private void _refreshCreateBtn()
        {
            if (wnd == null)
                return;

            bool canCreate = _checkCanCreate(false);
            if(canCreate)
                GGameCommonInfo.disgrayImage(wnd.grayList);
            else
                GGameCommonInfo.grayImage(wnd.grayList);
        }

        //检查是否可以创建联盟
        private bool _checkCanCreate(bool _popTip)
        {
            //检查名称
            string nameString = wnd.inputGuildName?.text;
            long nameLength = CharacterDetermineMgr.instance.getUnicodeStringLength(nameString);
            WCGIntRange nameLengthRange = GRefdataCoreMgr.instance.npGeneral.guild_name_length_limit;
            if (nameLength < nameLengthRange.min)
            {
                //太短
                if(_popTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputNameUnderLimit_none);
                return false;
            }
            else if (nameLength > nameLengthRange.max)
            {
                //太长
                if (_popTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputNameOverLimit_none);
                return false;
            }
            else if (CharacterDetermineMgr.instance.isPlayerNameIllegal(nameString))
            {
                //非法
                if (_popTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputNameIllegal_none);
                return false;
            }

            //检查简称
            string abbreviationString = wnd.inputAbbreviation?.text;
            long abbreviationLength = CharacterDetermineMgr.instance.getUnicodeStringLength(abbreviationString);
            WCGIntRange abbreviationLengthRange = GRefdataCoreMgr.instance.npGeneral.guild_simple_name_length_limit;
            if (!string.IsNullOrEmpty(abbreviationString) && !Regex.IsMatch(abbreviationString, PATTERN_STRING))
            {
                //非法
                if (_popTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputAbbreviationIllegal_none);
                return false;
            }
            else if (abbreviationLength < abbreviationLengthRange.min)
            {
                //太短
                if (_popTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputAbbreviationUnderLimit_none);
                return false;
            }
            else if (abbreviationLength > abbreviationLengthRange.max)
            {
                //太长
                if (_popTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputAbbreviationOverLimit_none);
                return false;
            }
            else if (CharacterDetermineMgr.instance.isPlayerNameIllegal(abbreviationString))
            {
                //非法
                if (_popTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputAbbreviationIllegal_none);
                return false;
            }

            //检查宣言
            string declarationString = wnd.inputDeclaration?.text;
            long declarationLength = CharacterDetermineMgr.instance.getUnicodeStringLength(declarationString);
            WCGIntRange declarationLengthRange = GRefdataCoreMgr.instance.npGeneral.guild_declaration_length_limit;
            if (declarationLength < declarationLengthRange.min)
            {
                //太短
                if (_popTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputDeclarationUnderLimit_none);
                return false;
            }
            else if (declarationLength > declarationLengthRange.max)
            {
                //太长
                if (_popTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputDeclarationOverLimit_none);
                return false;
            }
            else if (CharacterDetermineMgr.instance.isPlayerNameIllegal(declarationString))
            {
                //非法
                if (_popTip)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputDeclarationIllegal_none);
                return false;
            }

            //检查道具是否充足
            return GCommon.isItemEnough(GRefdataCoreMgr.instance.npGeneral.guild_create_cost, _popTip);
        }

        #region 编辑文本

        //名称编辑结束事件
        private void _onNameEdit(string _str)
        {
            if (wnd == null || wnd.inputGuildName == null)
                return;

            string nameString = wnd.inputGuildName.text;
            int strLength = CharacterDetermineMgr.instance.getUnicodeStringLength(nameString);
            WCGIntRange nameLengthRange = GRefdataCoreMgr.instance.npGeneral.guild_name_length_limit;
            if (nameLengthRange == null)
                return;

            //设置字数提示
            ALUGUICommon.setLabelTxt(wnd.txtInputNameCount, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, strLength, nameLengthRange.max));

            //设置文本是否合法提示
            if (strLength < nameLengthRange.min)
            {
                //名称过短
                ALUGUICommon.setLabelTxt(wnd.txtInputNameTip, TextTranslate.instance.getLanguage(TransKeyConst.guild_inputNameUnderLimit_none));
            }
            else if (strLength > nameLengthRange.max)
            {
                //名称过长
                ALUGUICommon.setLabelTxt(wnd.txtInputNameTip, TextTranslate.instance.getLanguage(TransKeyConst.guild_inputNameOverLimit_none));
            }
            else if(CharacterDetermineMgr.instance.isPlayerNameIllegal(nameString))
            {
                //名称包含特殊字符
                ALUGUICommon.setLabelTxt(wnd.txtInputNameTip, TextTranslate.instance.getLanguage(TransKeyConst.guild_inputNameIllegal_none));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtInputNameTip, "");
            }

            _refreshCreateBtn();
        }
        
        //简称编辑结束事件
        private void _onAbbreviationEdit(string _str)
        {
            if (wnd == null || wnd.inputAbbreviation == null)
                return;

            string abbreviationString = wnd.inputAbbreviation.text;
            int strLength = CharacterDetermineMgr.instance.getUnicodeStringLength(abbreviationString);
            WCGIntRange lengthRange = GRefdataCoreMgr.instance.npGeneral.guild_simple_name_length_limit;
            if (lengthRange == null)
                return;

            //设置字数提示
            ALUGUICommon.setLabelTxt(wnd.txtInputAbbreviationCount, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, strLength, lengthRange.max));

            //设置文本是否合法提示
            if (!string.IsNullOrEmpty(abbreviationString) && !Regex.IsMatch(abbreviationString, PATTERN_STRING))
            {
                //简称包含字母和数字外特殊字符
                ALUGUICommon.setLabelTxt(wnd.txtInputAbbreviationTip, TextTranslate.instance.getLanguage(TransKeyConst.guild_inputAbbreviationIllegal_none));
            }
            else if (strLength < lengthRange.min)
            {
                //简称过短
                ALUGUICommon.setLabelTxt(wnd.txtInputAbbreviationTip, TextTranslate.instance.getLanguage(TransKeyConst.guild_inputAbbreviationUnderLimit_none));
            }
            else if (strLength > lengthRange.max)
            {
                //简称过长
                ALUGUICommon.setLabelTxt(wnd.txtInputAbbreviationTip, TextTranslate.instance.getLanguage(TransKeyConst.guild_inputAbbreviationOverLimit_none));
            }
            else if(CharacterDetermineMgr.instance.isPlayerNameIllegal(abbreviationString))
            {
                //简称包含特殊字符
                ALUGUICommon.setLabelTxt(wnd.txtInputAbbreviationTip, TextTranslate.instance.getLanguage(TransKeyConst.guild_inputAbbreviationIllegal_none));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtInputAbbreviationTip, "");
            }

            _refreshCreateBtn();
        }

        //宣言编辑结束事件
        private void _onDeclarationEdit(string _str)
        {
            if (wnd == null || wnd.inputDeclaration == null)
                return;

            string declarationString = wnd.inputDeclaration.text;
            int strLength = CharacterDetermineMgr.instance.getUnicodeStringLength(declarationString);
            WCGIntRange lengthRange = GRefdataCoreMgr.instance.npGeneral.guild_declaration_length_limit;
            if (lengthRange == null)
                return;

            //设置字数提示
            ALUGUICommon.setLabelTxt(wnd.txtInputDeclarationCount, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, strLength, lengthRange.max));

            _refreshCreateBtn();
        }

        #endregion

        #region 点击事件

        //处理点击更换旗帜
        private void _onDealSelectFlag(GuildFlagRefObj _flagRefObj)
        {
            if (_flagRefObj == null)
                return;

            _refreshFlag(_flagRefObj);
        }

        //点击更换旗帜
        private void _onClickChgFlag(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildFlagSelect.instance, () =>
            {
                GGUIWndGuildFlagSelect.instance.showWnd();
                GGUIWndGuildFlagSelect.instance.setInfo(_m_selectFlagRef, false, _onDealSelectFlag);
            }, UINodeTagConst_Guild.C_GUILD_SELECT_FLAG);
        }

        //点击随机加入开关
        private void _onClickToggle(NPGGUIWndCommonToggleEx _commonToggle)
        {
            if(_m_wToggle != null)
                _m_wToggle.setSelected(!_m_wToggle.isOn);
        }

        //点击创建联盟
        private void _onClickCreate(GameObject _go)
        {
            if (wnd == null || _m_selectFlagRef == null || wnd.inputGuildName == null || wnd.inputAbbreviation == null || wnd.inputDeclaration == null || _m_wToggle == null)
                return;

            //检查是否可以创建
            if (!_checkCanCreate(true))
                return;

            //请求创建联盟
            NPPlayer.instance.guildComp.reqCreateGuild(
                _m_selectFlagRef.id, 
                wnd.inputGuildName.text,
                wnd.inputAbbreviation.text, 
                wnd.inputDeclaration.text,
                _m_wToggle.isOn,
                null);
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_CREATE);
        }

        #endregion
    }
}