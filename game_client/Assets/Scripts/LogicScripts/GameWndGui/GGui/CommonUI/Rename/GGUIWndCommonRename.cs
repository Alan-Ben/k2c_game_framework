using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using NPEnum;

namespace GOE
{
    //通用改名弹窗
    public class GGUIWndCommonRename : _ANPGGUIBasicWnd<GGUIMonoCommonRename>
    {
        //消耗item
        private NPGGUIWndCommonItem _m_costItemWnd;

        //消耗列表
        private List<NPCommonCostItem> _m_costItemList;

        //记录之前的名字
        private string _m_beforeName;

        //记录消耗是否足够
        private bool _m_isCostEnough;

        //名称个数范围
        private WCGIntRange _m_nameRange;

        //确认回调
        private Action<string> _m_confirmAction;

        //获取随机名称
        private Func<string> _m_getRandomNameFunc;

        private readonly long _m_uiPathId;

        public GGUIWndCommonRename(long _uiPathId): base(EALUIWndLayer.ADDITION)
        {
            _m_uiPathId = _uiPathId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_uiPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_uiPathId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        #region override
        protected override void _onWndInitDone()
        {
            _m_isCostEnough = false;

            if (null != wnd.costItemMono)
                _m_costItemWnd = new NPGGUIWndCommonItem(wnd.costItemMono);

            if (null != wnd.nameInputField)
                wnd.nameInputField.onValueChanged.AddListener(_onValueChange);

            ALUGUICommon.combineBtnClick(wnd.confirmBtn, _confirmBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _closeBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.randomBtn, _randomBtnDidClick);

        }
        protected override void _onShowWnd()
        {
        }
        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (null != _m_costItemWnd)
                _m_costItemWnd.discard();
            _m_costItemWnd = null;

            _m_isCostEnough = false;

            if (null == wnd)
                return;

            if (null != wnd.nameInputField)
                wnd.nameInputField.onValueChanged.RemoveListener(_onValueChange);

            ALUGUICommon.uncombineBtnClick(wnd.confirmBtn, _confirmBtnDidClick);
            ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _closeBtnDidClick);
            ALUGUICommon.uncombineBtnClick(wnd.randomBtn, _randomBtnDidClick);
        }
        #endregion

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_beforeName"></param>
        /// <param name="_confirmAction"></param>
        /// <param name="_costList"></param>
        public void setData(string _beforeName, List<NPCommonCostItem> _costList, WCGIntRange _nameRange, Func<string> _getRandomNameFunc, Action<string> _confirmAction)
        {
            _m_beforeName = _beforeName;
            _m_confirmAction = _confirmAction;
            _m_costItemList = _costList;
            _m_nameRange = _nameRange;
            _m_getRandomNameFunc = _getRandomNameFunc;

            _setName(_m_beforeName);
            _refresh();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void _refresh()
        {
            bool noCost = null == _m_costItemList || _m_costItemList.Count == 0;
            ALUGUICommon.setGameObjEnable(wnd.costShowGoList, !noCost);
            ALUGUICommon.setGameObjEnable(wnd.noCostShowGoList, noCost);
            if (noCost)
            {
                _m_isCostEnough = true;
                return;
            }

            //需要消耗的数据
            if (null != _m_costItemWnd && null != _m_costItemList && _m_costItemList.Count > 0)
            {
                NPCommonCostItem costItemTmp = null;
                for (int i = 0; i < _m_costItemList.Count; i++)
                {
                    costItemTmp = _m_costItemList[i];
                    if (null == costItemTmp)
                        continue;

                    //如果够则展示
                    if (GCommon.isItemEnough(costItemTmp,false))
                    {
                        _m_costItemWnd.setItem(costItemTmp);
                        _m_isCostEnough = true;
                        break;
                    }

                    //如果都不够则展示最后一个
                    if (i == _m_costItemList.Count - 1)
                        _m_costItemWnd.setItem(costItemTmp);
                }
            }

            _refreshName();
        }

        /// <summary>
        /// 名字变动
        /// </summary>
        private void _onValueChange(string args)
        {
            _refreshName();
        }

        private void _refreshName()
        {
            if (wnd == null)
                return;

            bool isCanUse = _checkCanUse();
            GGameCommonInfo.grayImage(wnd.grayImgList, !isCanUse);

            string nameStr = wnd.nameInputField.text;
            int strLength = CharacterDetermineMgr.instance.getUnicodeStringLength(nameStr);
            ALUGUICommon.setLabelTxt(wnd.inputCountTxt, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, strLength, _m_nameRange?.max ?? 0));
            ALUGUICommon.setGameObjEnable(wnd.goInputShowList, !string.IsNullOrEmpty(nameStr));
            ALUGUICommon.setGameObjEnable(wnd.goInputHideList, string.IsNullOrEmpty(nameStr));
        }

        /// <summary>
        /// 判断是否可以消耗
        /// </summary>
        private bool _checkCanUse(bool showTip = false)
        {
            if (null == wnd || null == wnd.nameInputField)
                return false;
            
            bool isCanUse = GCommon.checkNameCanUse(wnd.nameInputField.text, _m_nameRange, showTip, wnd.nameNotInRangeTipKey);
            if (!isCanUse)
                return false;

            //消耗不足
            if (!_m_isCostEnough)
            {
                if (showTip)
                    GCommon.dealItemNotEnough(_m_costItemWnd.itemData.getItemType(), _m_costItemWnd.itemData.subId);
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// 设置随机名称
        /// </summary>
        private void _setName(string _name = null)
        {
            if (wnd == null || wnd.nameInputField == null)
                return;

            if (string.IsNullOrEmpty(_name) && null != _m_getRandomNameFunc)
                _name = _m_getRandomNameFunc();
            
            ALUGUICommon.setLabelTxt(wnd.nameInputField, _name);
        }

        /// <summary>
        /// 点击随机按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _randomBtnDidClick(GameObject _go)
        {
            _setName();
        }

        /// <summary>
        /// 点击使用按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _confirmBtnDidClick(GameObject _go)
        {
            bool canUse = _checkCanUse(true);

            if (!canUse)
                return;

            if (null != _m_confirmAction)
            {
                _m_confirmAction(wnd.nameInputField.text);
            }
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _closeBtnDidClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_ADD_PLAYERINFO_RENAME_NODE);
        }
    }
}
