using UnityEngine;
using System;
using System.Collections.Generic;

using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的中间提示信息
    /// </summary>
    public class NPGUIAddSceneCenterTip : _ABasicAdditionUIScene_NoChild
    {
        private static NPGUIAddSceneCenterTip _g_instance = new NPGUIAddSceneCenterTip();
        public static NPGUIAddSceneCenterTip instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGUIAddSceneCenterTip();

                return _g_instance;
            }
        }

        public NPGUIAddSceneCenterTip()
            : base()
        {
        }

        protected override void _onEnterScene()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(5);
            stepCounter.regAllDoneDelegate(setSceneInited);

            //开启窗口加载
            NPGGUIWndCenterTips.instance.load(stepCounter.addDoneStepCount);
            GGUIWndNationPowerTip.instance.load(stepCounter.addDoneStepCount);
            GGUIWndMarsPowerTip.instance.load(stepCounter.addDoneStepCount);
            // GGUIWndQuestCompleteTips.instance.load(stepCounter.addDoneStepCount);
            GGUIWndQuestEntryTip.instance.load(stepCounter.addDoneStepCount);
            GGUIWndRankRushRankingChangeTip.instance.load(stepCounter.addDoneStepCount);
        }

        protected override void _dealQuitScene()
        {
            //卸载窗口
            NPGGUIWndCenterTips.instance.discard();
            GGUIWndNationPowerTip.instance.discard();
            GGUIWndMarsPowerTip.instance.discard();
            // GGUIWndQuestCompleteTips.instance.discard();
            GGUIWndQuestEntryTip.instance.discard();
            GGUIWndRankRushRankingChangeTip.instance.discard();
        }

        protected override void _onSceneInited()
        {
            NPGGUIWndCenterTips.instance.showWnd();
        }

        /// <summary>
        /// 展示国力变化
        /// </summary>
        /// <param name="_oriValue"></param>
        /// <param name="_curValue"></param>
        public void showNationPower(long _oriValue, long _curValue)
        {
            GGUIWndNationPowerTip.instance.showWnd();
            GGUIWndNationPowerTip.instance.setInfo(_oriValue, _curValue);
        }

        /// <summary>
        /// 展示火星实力变化
        /// </summary>
        /// <param name="_oriValue"></param>
        /// <param name="_curValue"></param>
        public void showMarsPower(long _oriValue, long _curValue)
        {
            GGUIWndMarsPowerTip.instance.showWnd();
            GGUIWndMarsPowerTip.instance.setInfo(_oriValue, _curValue);
        }

        // /// <summary>
        // /// 展示任务完成提示界面
        // /// </summary>
        // /// <param name="_questName"></param>
        // public void showQuestCompleteTips(string _questName)
        // {
        //     GGUIWndQuestCompleteTips.instance.showWnd();
        //     GGUIWndQuestCompleteTips.instance.setInfo(_questName);
        // }

        /// <summary>
        /// 展示主线任务入口tip
        /// </summary>
        public void showQuestEntryTip()
        {
            GGUIWndQuestEntryTip.instance.showWnd();
        }

        /// <summary>
        /// 显示冲榜排名变化提示
        /// </summary>
        /// <param name="_rankrushId"></param>
        /// <param name="_lastRanking"></param>
        /// <param name="_curRanking"></param>
        /// <param name="_onHide"></param>
        public void showRankRushRankingChgTip(long _rankrushId, long _lastRanking, long _curRanking, Action _onHide)
        {
            GGUIWndRankRushRankingChangeTip.instance.showWnd();
            GGUIWndRankRushRankingChangeTip.instance.setInfo(_rankrushId, _lastRanking, _curRanking, _onHide);
        }

        /// <summary>
        /// 展示对应的信息
        /// </summary>
        /// <param name="_info"></param>
        public void showTransTextInfo(string _info)
        {
            showTextInfo(TextTranslate.instance.getLanguage(_info));
        }
        public void showTextInfo(string _info)
        {
            if (string.IsNullOrEmpty(_info))
                return;

            if (!isEntered)
            {
                UnityEngine.Debug.LogError("NPGUIAddSceneCenterTip 还未初始化！");
                return;
            }

            showTextSysTip(_info);
        }

        /// <summary>
        /// 展示对应的错误提示
        /// </summary>
        /// <param name="_errCode"></param>
        public void showErrorInfo(int _errCode)
        {
            if (0 == _errCode)
                return;

            showTextInfo(TextTranslate.instance.getLanguage(string.Format("#0_S2C_Error_{0}", _errCode)));
            //发送协议错误码埋点
            GCommon.sendStepReport(TraceConst.SERVER_ERROR.setMark($"{_errCode}-{TextTranslate.instance.getLanguage(string.Format("#0_S2C_Error_{0}", _errCode))}"));
#if UNITY_EDITOR
            UnityEngine.Debug.LogError($"0_S2C_Error_{_errCode}, {ProtocolErrorCodeResult.instance.getResultMsg(_errCode)}");
#endif
        }

        /// <summary>
        /// 根据tag移除为展示的tip
        /// </summary>
        public void clearQueueTipByTag(ETipDealerTagType _tag)
        {
            NPGGUIWndCenterTips.instance.clearQueueTipByTag(_tag);
        }

        /// <summary>
        /// 根据tag移除所有的tip
        /// </summary>
        public void clearAllTipByTag(ETipDealerTagType _tag)
        {
            NPGGUIWndCenterTips.instance.clearAllTipByTag(_tag);
        }

        #region 系统提示类型tip

        /// <summary>
        /// 显示 文本类型 系统提示，直接覆盖上一个
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_startPos"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showTextSysTip(string _text, Vector3 _startPos, long _tipId = NPConst.C_DEFAULT_TEXT_TIP_REF_ID, Action<NPGGUIWndTextTip> _onPop = null)
        {
            Vector2 screenPos = Game.instance.mainCamera.uiCamera.WorldToScreenPoint(_startPos);
            showTextSysTip(_text, _tipId, (_tipWnd) => { _tipWnd?.setTipPos(screenPos); _onPop?.Invoke(_tipWnd); });
        }

        /// <summary>
        /// 显示 文本类型 系统提示，直接覆盖上一个
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showTextSysTip(string _text, long _tipId = NPConst.C_DEFAULT_TEXT_TIP_REF_ID, Action<NPGGUIWndTextTip> _onPop = null)
        {
            showTextSysTip(new List<string>() { _text }, _tipId, _onPop);
        }

        /// <summary>
        /// 显示 文本+数量类型 系统提示，直接覆盖上一个
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_value"></param>
        /// <param name="_startPos"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showTextNumSysTip(string _text, float _value, Vector3 _startPos, long _tipId = NPConst.C_DEFAULT_TEXT_NUM_TIP_REF_ID, Action<NPGGUIWndTextTip> _onPop = null)
        {
            Vector2 screenPos = Game.instance.mainCamera.uiCamera.WorldToScreenPoint(_startPos);
            showTextNumSysTip(_text, _value, _tipId, (_tipWnd) => { _tipWnd?.setTipPos(screenPos); _onPop?.Invoke(_tipWnd); });
        }

        /// <summary>
        /// 显示 文本+数量类型 系统提示，直接覆盖上一个
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_value"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showTextNumSysTip(string _text, float _value, long _tipId = NPConst.C_DEFAULT_TEXT_NUM_TIP_REF_ID, Action<NPGGUIWndTextTip> _onPop = null)
        {
            string numStr = GCommon.getValueStr(_value);
            showTextSysTip(new List<string>() { _text, numStr }, _tipId, _onPop);
        }

        /// <summary>
        /// 显示 文本类型 系统提示，直接覆盖上一个
        /// </summary>
        /// <param name="_textList"></param>
        /// <param name="_startPos"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showTextSysTip(List<string> _textList, Vector3 _startPos, long _tipId = NPConst.C_DEFAULT_TEXT_TIP_REF_ID, Action<NPGGUIWndTextTip> _onPop = null)
        {
            Vector2 screenPos = Game.instance.mainCamera.uiCamera.WorldToScreenPoint(_startPos);
            showTextSysTip(_textList, _tipId, (_tipWnd) => { _tipWnd?.setTipPos(screenPos); _onPop?.Invoke(_tipWnd); });
        }

        /// <summary>
        /// 显示 文本类型 系统提示，直接覆盖上一个
        /// </summary>
        /// <param name="_textList"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showTextSysTip(List<string> _textList, long _tipId = NPConst.C_DEFAULT_TEXT_TIP_REF_ID, Action<NPGGUIWndTextTip> _onPop = null)
        {
            NPGGUIWndCenterTips.instance.showWnd();
            NPGGUIWndCenterTips.instance.showTextSysTip(_textList, _tipId, _onPop);
        }

        /// <summary>
        /// 显示 图片+文本类型 系统提示，直接覆盖上一个
        /// </summary>
        /// <param name="_icon"></param>
        /// <param name="_text"></param>
        /// <param name="_startPos"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showIconTextSysTip(NPGTextureIndex _icon, string _text, Vector3 _startPos, long _tipId = NPConst.C_DEFAULT_ICON_TEXT_TIP_REF_ID, Action<NPGGUIWndIconTextTip> _onPop = null)
        {
            Vector2 screenPos = Game.instance.mainCamera.uiCamera.WorldToScreenPoint(_startPos);
            showIconTextSysTip(_icon, _text, _tipId, (_tipWnd) => { _tipWnd?.setTipPos(screenPos); _onPop?.Invoke(_tipWnd); });
        }

        /// <summary>
        /// 显示 图片+文本类型 系统提示，直接覆盖上一个
        /// </summary>
        /// <param name="_icon"></param>
        /// <param name="_text"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showIconTextSysTip(NPGTextureIndex _icon, string _text, long _tipId = NPConst.C_DEFAULT_ICON_TEXT_TIP_REF_ID, Action<NPGGUIWndIconTextTip> _onPop = null)
        {
            NPGGUIWndCenterTips.instance.showWnd();
            NPGGUIWndCenterTips.instance.showIconTextSysTip(_icon, _text, _tipId, _onPop);
        }

        #endregion


        #region 队列类型tip

        /// <summary>
        /// 显示 文本类型 队列上浮提示
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_startPos"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showTextTip(string _text, Vector3 _startPos, long _tipId = NPConst.C_DEFAULT_TEXT_TIP_REF_ID, Action<NPGGUIWndTextTip> _onPop = null, ETipDealerTagType _tag = ETipDealerTagType.NONE)
        {
            Vector2 screenPos = Game.instance.mainCamera.uiCamera.WorldToScreenPoint(_startPos);
            showTextTip(_text, _tipId, (_tipWnd) => { _tipWnd?.setTipPos(screenPos); _onPop?.Invoke(_tipWnd); }, _tag);
        }

        /// <summary>
        /// 显示 文本类型 队列上浮提示
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showTextTip(string _text, long _tipId = NPConst.C_DEFAULT_TEXT_TIP_REF_ID, Action<NPGGUIWndTextTip> _onPop = null, ETipDealerTagType _tag = ETipDealerTagType.NONE)
        {
            showTextTip(new List<string>() { _text }, _tipId, _onPop, _tag);
        }

        /// <summary>
        /// 显示 文本+数量类型 队列上浮提示
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_value"></param>
        /// <param name="_startPos"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showTextNumTip(string _text, float _value, Vector3 _startPos, long _tipId = NPConst.C_DEFAULT_TEXT_NUM_TIP_REF_ID, Action<NPGGUIWndTextTip> _onPop = null, ETipDealerTagType _tag = ETipDealerTagType.NONE)
        {
            Vector2 screenPos = Game.instance.mainCamera.uiCamera.WorldToScreenPoint(_startPos);
            showTextNumTip(_text, _value, _tipId, (_tipWnd) => { _tipWnd?.setTipPos(screenPos); _onPop?.Invoke(_tipWnd); }, _tag);
        }

        /// <summary>
        /// 显示 文本+数量类型 队列上浮提示
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_value"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showTextNumTip(string _text, float _value, long _tipId = NPConst.C_DEFAULT_TEXT_NUM_TIP_REF_ID, Action<NPGGUIWndTextTip> _onPop = null, ETipDealerTagType _tag = ETipDealerTagType.NONE)
        {
            string numStr = GCommon.getValueStr(_value);
            showTextTip(new List<string>() { _text, numStr }, _tipId, _onPop, _tag);
        }

        /// <summary>
        /// 显示 文本类型 队列上浮提示
        /// </summary>
        /// <param name="_textList"></param>
        /// <param name="_startPos"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showTextTip(List<string> _textList, Vector3 _startPos, long _tipId = NPConst.C_DEFAULT_TEXT_TIP_REF_ID, Action<NPGGUIWndTextTip> _onPop = null, ETipDealerTagType _tag = ETipDealerTagType.NONE)
        {
            Vector2 screenPos = Game.instance.mainCamera.uiCamera.WorldToScreenPoint(_startPos);
            showTextTip(_textList, _tipId, (_tipWnd) => { _tipWnd?.setTipPos(screenPos); _onPop?.Invoke(_tipWnd); }, _tag);
        }

        /// <summary>
        /// 显示 文本类型 队列上浮提示
        /// </summary>
        /// <param name="_textList"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showTextTip(List<string> _textList, long _tipId = NPConst.C_DEFAULT_TEXT_TIP_REF_ID, Action<NPGGUIWndTextTip> _onPop = null, ETipDealerTagType _tag = ETipDealerTagType.NONE)
        {
            NPGGUIWndCenterTips.instance.showWnd();
            NPGGUIWndCenterTips.instance.showTextTip(_textList, _tipId, _onPop, _tag);
        }


        /// <summary>
        /// 清除提示
        /// </summary>
        public void clearAllTip()
        {
            NPGGUIWndCenterTips.instance.clearAllTip();
        }

        /// <summary>
        /// 显示 图片+文本类型 队列上浮提示
        /// </summary>
        /// <param name="_icon"></param>
        /// <param name="_text"></param>
        /// <param name="_startPos"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showIconTextTip(NPGTextureIndex _icon, string _text, Vector3 _startPos, long _tipId = NPConst.C_DEFAULT_ICON_TEXT_TIP_REF_ID, Action<NPGGUIWndIconTextTip> _onPop = null, ETipDealerTagType _tag = ETipDealerTagType.NONE)
        {
            Vector2 screenPos = Game.instance.mainCamera.uiCamera.WorldToScreenPoint(_startPos);
            showIconTextTip(_icon, _text, _tipId, (_tipWnd) => { _tipWnd?.setTipPos(screenPos); _onPop?.Invoke(_tipWnd); }, _tag);
        }

        /// <summary>
        /// 显示 图片+文本类型 队列上浮提示
        /// </summary>
        /// <param name="_icon"></param>
        /// <param name="_text"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showIconTextTip(NPGTextureIndex _icon, string _text, long _tipId = NPConst.C_DEFAULT_ICON_TEXT_TIP_REF_ID, Action<NPGGUIWndIconTextTip> _onPop = null , ETipDealerTagType _tag = ETipDealerTagType.NONE)
        {
            NPGGUIWndCenterTips.instance.showWnd();
            NPGGUIWndCenterTips.instance.showIconTextTip(_icon, new List<string>(){_text}, _tipId, _onPop, _tag);
        }
        
        /// <summary>
        /// 显示 图片+文本类型 队列上浮提示
        /// </summary>
        /// <param name="_icon"></param>
        /// <param name="_text"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showIconTextTextTip(NPGTextureIndex _icon, string _text, string _text2, long _tipId = NPConst.C_DEFAULT_ICON_TEXT_TEXT_TIP_REF_ID, Action<NPGGUIWndIconTextTip> _onPop = null , ETipDealerTagType _tag = ETipDealerTagType.NONE)
        {
            showIconTextTip(_icon, new List<string>(){_text, _text2}, _tipId, _onPop, _tag);
        }
        
        /// <summary>
        /// 显示 图片+文本类型 队列上浮提示
        /// </summary>
        /// <param name="_icon"></param>
        /// <param name="_text"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showIconTextTip(NPGTextureIndex _icon, List<string> _textList, long _tipId = NPConst.C_DEFAULT_ICON_TEXT_TIP_REF_ID, Action<NPGGUIWndIconTextTip> _onPop = null , ETipDealerTagType _tag = ETipDealerTagType.NONE)
        {
            NPGGUIWndCenterTips.instance.showWnd();
            NPGGUIWndCenterTips.instance.showIconTextTip(_icon, _textList, _tipId, _onPop, _tag);
        }

        /// <summary>
        /// 显示 Item+Text+Text 队列上浮提示
        /// </summary>
        /// <param name="_icon"></param>
        /// <param name="_text"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showItemTextTextTip(_IItem _item, string _textOne, string _textTow, long _tipId = NPConst.C_DEFAULT_ITEM_TEXT_TEXT_TIP_REF_ID, Action<NPGGUIWndItemTextTextTip> _onPop = null, ETipDealerTagType _tag = ETipDealerTagType.NONE)
        {
            NPGGUIWndCenterTips.instance.showWnd();
            NPGGUIWndCenterTips.instance.showItemTextTextTip(_item, _textOne, _textTow, _tipId, _onPop, _tag);
        }

        public void showEmptyTip(long _tipId, Action<NPGGUIWndEmptyTip> _onPop = null, ETipDealerTagType _tag = ETipDealerTagType.NONE)
        {
            NPGGUIWndCenterTips.instance.showWnd();
            NPGGUIWndCenterTips.instance.showEmptyTip(_tipId, _onPop, _tag);
        }
        
        #endregion
    }
}
