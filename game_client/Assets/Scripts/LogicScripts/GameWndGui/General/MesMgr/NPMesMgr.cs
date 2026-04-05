using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 提示信息管理对象
    /// </summary>
    public class NPMesMgr
    {
        private static NPMesMgr _g_instance = new NPMesMgr();
        public static NPMesMgr instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPMesMgr();

                return _g_instance;
            }
        }

        private Queue<_ANPMesDealer> _m_qDealerQueue;//信息队列
        private _ANPMesDealer _m_dCurMes;//当前信息
        private bool _m_bIsShowing;//是否在提示信息显示过程中

        protected NPMesMgr()
        {
            _m_qDealerQueue = new Queue<_ANPMesDealer>();
            _m_dCurMes = null;
            _m_bIsShowing = false;
        }

        public void showOneComfirmBtnTranslateMes(string _mesTranslateKey, Action _clickDelegate, bool _needTransBk = true)
        {
            NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(_mesTranslateKey), TextTranslate.instance.getLanguage(TransKeyConst.confirm), _clickDelegate, _needTransBk);
        }
        public void showOneBtnMes(string _mes, string _btnTxt, Action _clickDelegate,bool _needTransBk = true,string _titleKey = TransKeyConst.sys_tip_title_none)
        {
            addDealer(new NPMesDealer_OneBtn(_mes, _btnTxt, _clickDelegate, _needTransBk, _titleKey));
        }
        public void showTwoBtnMes(string _mes, string _leftBtnTxt, Action _clickLeftBtnDelegate, string _rightBtnTxt, Action _clickRightBtnDelegate, bool _needTransBk = true, string _titleKey = TransKeyConst.sys_tip_title_none)
        {
            addDealer(new NPMesDealer_TwoBtn(_mes, _leftBtnTxt, _clickLeftBtnDelegate, _rightBtnTxt, _clickRightBtnDelegate, null, _needTransBk, _titleKey));
        }
        public void showTwoBtnMes(string _mes, string _leftBtnTxt, Action _clickLeftBtnDelegate, string _rightBtnTxt, Action _clickRightBtnDelegate, Action _cancelDelegate, bool _needTransBk = true, string _titleKey = TransKeyConst.sys_tip_title_none)
        {
            addDealer(new NPMesDealer_TwoBtn(_mes, _leftBtnTxt, _clickLeftBtnDelegate, _rightBtnTxt, _clickRightBtnDelegate, _cancelDelegate, _needTransBk, _titleKey));
        }

        /// <summary>
        /// 显示带勾选框的二次确认弹窗
        /// </summary>
        /// <param name="_onConfirm"></param>
        /// <param name="_txtTitle"></param>
        /// <param name="_txtContent"></param>
        /// <param name="_txtToggleDesc">勾选框描述，#1_no_longer_tips_today_none 今日不再提醒/#1_no_longer_tips_any_more_none 不再提醒</param>
        public void showOneBtnTogMes(Action<bool> _onConfirm, string _txtTitle, string _txtContent, string _txtToggleDesc = TransKeyConst.no_longer_tips_today_none, bool _needTransBk = true)
        {
            showOneBtnTogMes(TransKeyConst.confirm, _onConfirm, _txtTitle, _txtContent, _txtToggleDesc, _needTransBk);
        }

        /// <summary>
        /// 显示带勾选框的二次确认弹窗
        /// </summary>
        /// <param name="_txtBtn"></param>
        /// <param name="_onClickBtn"></param>
        /// <param name="_txtTitle"></param>
        /// <param name="_txtContent"></param>
        /// <param name="_txtToggleDesc"></param>
        /// <param name="_needTransBk"></param>
        public void showOneBtnTogMes(
            string _txtBtn,
            Action<bool> _onClickBtn,
            string _txtTitle,
            string _txtContent,
            string _txtToggleDesc,
            bool _needTransBk = true)
        {
            addDealer(new NPMesDealer_OneBtn_Toggle(_txtBtn, _onClickBtn, _txtTitle, _txtContent, _txtToggleDesc, _needTransBk));
        }

        /// <summary>
        /// 显示带勾选框的二次确认弹窗
        /// </summary>
        /// <param name="_onConfirm"></param>
        /// <param name="_txtTitle"></param>
        /// <param name="_txtContent"></param>
        /// <param name="_txtToggleDesc">勾选框描述，#1_no_longer_tips_today_none 今日不再提醒/#1_no_longer_tips_any_more_none 不再提醒</param>
        public void showTwoBtnTogMes(Action<bool> _onConfirm, Action<bool> _onCancel, string _txtTitle, string _txtContent, string _txtToggleDesc = TransKeyConst.no_longer_tips_today_none, bool _needTransBk = true, Action _aClickTransBk = null)
        {
            showTwoBtnTogMes(TransKeyConst.cancel, _onCancel, TransKeyConst.confirm, _onConfirm, _txtTitle, _txtContent, _txtToggleDesc, _needTransBk, _aClickTransBk);
        }

        /// <summary>
        /// 显示带勾选框的二次确认弹窗
        /// </summary>
        /// <param name="_txtLeftBtn"></param>
        /// <param name="_onClickLeftBtn"></param>
        /// <param name="_txtRightBtn"></param>
        /// <param name="_onClickRightBtn"></param>
        /// <param name="_txtTitle"></param>
        /// <param name="_txtContent"></param>
        /// <param name="_txtToggleDesc"></param>
        public void showTwoBtnTogMes(
            string _txtLeftBtn,
            Action<bool> _onClickLeftBtn,
            string _txtRightBtn,
            Action<bool> _onClickRightBtn,
            string _txtTitle,
            string _txtContent,
            string _txtToggleDesc, 
            bool _needTransBk = true,
            Action _aClickTransBk = null)
        {
            addDealer(new NPMesDealer_TwoBtn_Toggle(_txtLeftBtn, _onClickLeftBtn, _txtRightBtn, _onClickRightBtn, _txtTitle, _txtContent, _txtToggleDesc, _needTransBk, _aClickTransBk));
        }

        /// <summary>
        /// 显示带勾选框的今日不再提醒二次确认弹窗，已经确认过则直接执行回调
        /// </summary>
        /// <param name="_onConfirm"></param>
        /// <param name="_warningType"></param>
        /// <param name="_txtTitle"></param>
        /// <param name="_txtContent"></param>
        public void showWarningTipMes(Action _onConfirm, Action _onCancel, ENPWarningType _warningType, string _txtTitle, string _txtContent)
        {
            bool needConfirm = AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(_warningType);
            if (needConfirm)
            {
                showTwoBtnTogMes(
                    (_toggle) =>
                    {
                        _onConfirm?.Invoke();
                        if (_toggle)
                        {
                            AccountSettingMgr.instance.warningTipSaver.setTodayIgnoreWarningTip(_warningType);
                        }
                    },
                    (_toggle) =>
                    {
                        _onCancel?.Invoke();
                    },
                    _txtTitle,
                    _txtContent);
            }
            else
            {
                _onConfirm?.Invoke();
            }
        }

        /// <summary>
        /// 消耗确认弹窗
        /// </summary>
        /// <param name="_costItem"></param>
        /// <param name="_onConfirm"></param>
        /// <param name="_txtTitle"></param>
        /// <param name="_txtContent"></param>
        /// <param name="_needTransBk"></param>
        public void showCostItemMes(
            NPCommonCostItem _costItem,
            Action _onConfirm,
            Action _onCancel,
            string _txtTitle,
            string _txtContent,
            bool _needTransBk = true)
        {
            addDealer(new NPMesDealer_Cost(_costItem, _onConfirm, _onCancel, _txtTitle, _txtContent, _needTransBk));
        }

        /// <summary>
        /// 带勾选框的消耗确认弹窗
        /// </summary>
        /// <param name="_costItem"></param>
        /// <param name="_onConfirm"></param>
        /// <param name="_txtTitle"></param>
        /// <param name="_txtContent"></param>
        /// <param name="_txtToggleDesc"></param>
        /// <param name="_needTransBk"></param>
        public void showCostItemTogMes(
            NPCommonCostItem _costItem, 
            Action<bool> _onConfirm, 
            Action _onCancel, 
            string _txtTitle, 
            string _txtContent, 
            string _txtToggleDesc = TransKeyConst.no_longer_tips_today_none,
            bool _needTransBk = true)
        {
            addDealer(new NPMesDealer_Cost_Toggle(_costItem, _onConfirm, _onCancel, _txtTitle, _txtContent, _txtToggleDesc, _needTransBk));
        }

        /// <summary>
        /// 添加一个信息处理对象
        /// </summary>
        /// <param name="_dealer"></param>
        public void addDealer(_ANPMesDealer _dealer)
        {
            _m_qDealerQueue.Enqueue(_dealer);
            _tryPopAndShowMes();
        }

        /// <summary>
        /// 尝试显示新的信息
        /// </summary>
        private void _tryPopAndShowMes()
        {
            if (_m_bIsShowing)
                return;

            _ANPMesDealer dealer = null;
            while (_m_qDealerQueue.Count > 0 && dealer == null)
            {
                dealer = _m_qDealerQueue.Dequeue();
            }
            if (dealer != null)
            {
                _m_bIsShowing = true;
                _m_dCurMes = dealer;
                _m_dCurMes.showMes();
            }
        }

        /// <summary>
        /// 设置显示信息已完成
        /// </summary>
        public void setMesShowDone()
        {
            _m_bIsShowing = false;
            _m_dCurMes = null;
            _tryPopAndShowMes();
        }

        /// <summary>
        /// 清空所有信息提示
        /// </summary>
        public void reset()
        {
            _m_bIsShowing = false;

            //清空队列
            _m_qDealerQueue?.Clear();

            //释放当前显示信息
            _m_dCurMes?.discard();
            _m_dCurMes = null;
        }
    }
}
