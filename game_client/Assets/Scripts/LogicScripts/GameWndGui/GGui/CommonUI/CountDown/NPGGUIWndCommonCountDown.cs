using ALPackage;
using System;

namespace GOE
{
    //通用窗口页签控件
    public class NPGGUIWndCommonCountDown : _ATNPBasicSimpleUISubWnd<NPGGUIMonoCommonCountDown>
    {
        //倒计时结束的时间戳
        private long _m_lFinishTimeS;
        //倒计时时长
        private long _m_countDownTimeS;
        //设置数据的序列号用于区分是否需要tick
        private long _m_lItemSerialize;
        //倒计时的key
        private string _m_keyStr;
        //倒计时结束回调
        private Action _m_doneAction;

        public NPGGUIWndCommonCountDown(NPGGUIMonoCommonCountDown _wnd) : base(_wnd)
        {
            initWnd();
        }

        public long itemSerialize { get { return _m_lItemSerialize; } }

        protected override void _onDiscard()
        {
            _stopTick();
        }

        protected override void _onHideWnd()
        {
            _stopTick();
        }

        protected override void _onReset()
        {
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_countDownTimeS">倒计时时长 秒</param>
        /// <param name="_keyStr">显示key</param>
        public void setInfo(long _countDownTimeS, string _keyStr,Action _doneAction = null)
        {
            _stopTick();
            _m_countDownTimeS = _countDownTimeS;
            _m_lFinishTimeS = _m_countDownTimeS + ALCommon.getNowTimeSec();
            _m_keyStr = _keyStr;
            _m_doneAction = _doneAction;

            _startTick();
        }

        public void setInfo(float _countDownFloatTimeS, string _keyStr, Action _doneAction = null)
        {
            setInfo((long)_countDownFloatTimeS + 1, _keyStr, _doneAction);
        }

        //更新倒计时
        public void setCountTimeS(long _countDownTimeS)
        {
            _m_countDownTimeS = _countDownTimeS;
            _m_lFinishTimeS = _m_countDownTimeS + ALCommon.getNowTimeSec();
        }

        public void tick1Sec()
        {
            _m_countDownTimeS = _m_lFinishTimeS - ALCommon.getNowTimeSec();
            if (null == _m_keyStr)
                _setTxt(TimeUtil.millisecondsToTime_hms(_m_countDownTimeS * 1000), _m_countDownTimeS);
            else
                _setTxt(TextTranslate.instance.getLanguage(_m_keyStr, TimeUtil.millisecondsToTime_Two(_m_countDownTimeS * 1000)), _m_countDownTimeS);


            if (_m_countDownTimeS <= 0)
            {
                _m_countDownTimeS = 0;
                ALUGUICommon.setGameObjEnable(wnd.stopTickShowGoList, true);
                ALUGUICommon.setGameObjEnable(wnd.stopTickHideGoList, false);

                _stopTick();

                if (null != _m_doneAction)
                    _m_doneAction();
            }
        }

        private void _startTick()
        {
            if (_m_countDownTimeS <= 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.stopTickShowGoList, true);
                ALUGUICommon.setGameObjEnable(wnd.stopTickHideGoList, false);
                return;
            }

            ALUGUICommon.setGameObjEnable(wnd.stopTickShowGoList, false);
            ALUGUICommon.setGameObjEnable(wnd.stopTickHideGoList, true);

            //开启任务进行刷新操作
            _m_lItemSerialize = ALSerializeOpMgr.next();
            NPGGUIWndCommonCountDownTickTask task = new NPGGUIWndCommonCountDownTickTask(this);
            //注册刷新
            ALMonoTaskMgr.instance.addMonoTask(task);
        }

        private void _stopTick()
        {
            _m_lItemSerialize = ALSerializeOpMgr.next();
        }

        private void _setTxt(string _txt, long _itemS)
        {
            ALUGUICommon.setLabelTxt(wnd.countDownTxt, _txt);
            ALUGUICommon.setLabelTxt(wnd.countDownTxtMeshPro, _txt);
            if (_itemS <= wnd.minTimeS)
            {
                ALUGUICommon.setUIObjFullColor(wnd.countDownTxt, wnd.chgColor);
                ALUGUICommon.setUIObjFullColor(wnd.countDownTxtMeshPro, wnd.chgColor);
            }
            else
            {
                ALUGUICommon.setUIObjFullColor(wnd.countDownTxt, wnd.normalColor);
                ALUGUICommon.setUIObjFullColor(wnd.countDownTxtMeshPro, wnd.normalColor);

            }
        }
    }

    /// <summary>
    /// 日常任务刷新倒计时
    /// </summary>
    public class NPGGUIWndCommonCountDownTickTask : _IALBaseMonoTask
    {
        //对象序列号
        private long _m_lItemSerialize;
        //窗口对象
        private NPGGUIWndCommonCountDown _m_wnd;

        public NPGGUIWndCommonCountDownTickTask(NPGGUIWndCommonCountDown _itemWnd)
        {
            _m_lItemSerialize = _itemWnd.itemSerialize;
            _m_wnd = _itemWnd;
        }

        /// <summary>
        /// 每秒处理操作
        /// </summary>
        public void deal()
        {
            if (null == _m_wnd || null == _m_wnd.wnd)
                return;

            if (_m_wnd.itemSerialize != _m_lItemSerialize)
                return;

            //处理tick
            _m_wnd.tick1Sec();

            ALMonoTaskMgr.instance.addMonoTask(this, 0.2f);
        }
    }
}
