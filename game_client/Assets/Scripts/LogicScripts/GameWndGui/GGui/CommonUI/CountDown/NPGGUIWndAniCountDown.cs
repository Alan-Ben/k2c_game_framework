using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 通用带特殊动画倒计时
    /// </summary>
    public class NPGGUIWndAniCountDown : _ATNPBasicSimpleUISubWnd<NPGGUIMonoAniCountDown>
    {
        //倒计时时长（毫秒）
        private long _m_lCDTimeMs;
        //倒计时的key
        private string _m_sKeyStr;
        //倒计时结束回调
        private Action _m_aDoneAction;
        //获取倒计时秒数的方法
        private Func<long> _m_fGetCDMsFunc;
        //上个秒数
        private long _m_lLastSec;
        //每秒任务控制对象
        private ALCommonEnableTaskController _m_tcTickTaskController;

        public NPGGUIWndAniCountDown(NPGGUIMonoAniCountDown _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {
            _stopTask();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _stopTask();
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_cdTimeMs">倒计时时长毫秒</param>
        /// <param name="_doneAction">倒计时结束回调</param>
        /// <param name="_keyStr">显示key,传空会默认使用mono配置的key,非空则优先使用该key</param>
        public void setInfo(long _cdTimeMs, Action _doneAction = null, string _keyStr = "")
        {
            _m_lCDTimeMs = _cdTimeMs;
            _m_sKeyStr = _keyStr;
            _m_fGetCDMsFunc = null;
            _m_aDoneAction = _doneAction;
            _m_lLastSec = 0;

            _startTask();
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_getCDMsFunc">获取倒计时毫秒数的方法</param>
        /// <param name="_doneAction">倒计时结束回调</param>
        /// <param name="_keyStr">显示key,传空会默认使用mono配置的key,非空则优先使用该key</param>
        public void setInfo(Func<long> _getCDMsFunc = null, Action _doneAction = null, string _keyStr = "")
        {
            _m_lCDTimeMs = 0;
            _m_sKeyStr = _keyStr;
            _m_fGetCDMsFunc = _getCDMsFunc;
            _m_aDoneAction = _doneAction;
            _m_lLastSec = 0;

            _startTask();
        }

        /// <summary>
        /// 恢复倒计时
        /// </summary>
        public void resumeCD()
        {
            _startTask();
        }

        /// <summary>
        /// 暂停倒计时
        /// </summary>
        public void pauseCD()
        {
            _stopTask();
        }

        //开启倒计时任务
        private void _startTask()
        {
            _stopTask();
            _m_tcTickTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_refreshWnd, 1f);
        }

        //关闭倒计时任务
        private void _stopTask()
        {
            _m_tcTickTaskController.setDisable();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            //如果计算倒计时的方法是外面传的使用外面方法的数值
            if (_m_fGetCDMsFunc != null)
                _m_lCDTimeMs = _m_fGetCDMsFunc();

            long cdSec = TimeUtil.msToSecCeiling(_m_lCDTimeMs);
            if (_m_lLastSec == 0)
                _m_lLastSec = cdSec;

            if (cdSec < 0)
            {
                _stopTask();
                _m_aDoneAction?.Invoke();
                return;
            }

            //设置翻译,优先使用传进来的key,没有则使用mono配置的key
            string strCdSec = $"{cdSec}";
            if (!string.IsNullOrEmpty(_m_sKeyStr))
                strCdSec = TextTranslate.instance.getLanguage(_m_sKeyStr, cdSec);
            else if(!string.IsNullOrEmpty(wnd.secTransKey))
                strCdSec = TextTranslate.instance.getLanguage(wnd.secTransKey, cdSec);

            //设置倒计时
            ALUGUICommon.setLabelTxt(wnd.txtNormalCD, strCdSec);
            ALUGUICommon.setLabelTxt(wnd.txtSpecialCD, strCdSec);

            //设置显隐
            bool isShowSpecial = cdSec <= wnd.specialShowMiniSec;
            ALUGUICommon.setGameObjEnable(wnd.goSpecHideList, !isShowSpecial);
            ALUGUICommon.setGameObjEnable(wnd.goSpecShowList, isShowSpecial);

            //播放特殊动画
            if (isShowSpecial && _m_lLastSec != cdSec && wnd.specialAnimation != null && !string.IsNullOrEmpty(wnd.specialAnimationName))
                wnd.specialAnimation.ForcePlay(wnd.specialAnimationName);

            _m_lLastSec = cdSec;

            if(_m_fGetCDMsFunc == null)
                _m_lCDTimeMs -= 1000;
        }
    }
}
