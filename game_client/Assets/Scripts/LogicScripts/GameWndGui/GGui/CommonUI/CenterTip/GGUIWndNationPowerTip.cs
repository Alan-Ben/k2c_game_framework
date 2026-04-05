using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 国力变化上浮提示窗体
    /// </summary>
    public class GGUIWndNationPowerTip : _ANPGGUIBasicWnd<GGUIMonoNationPowerTip>
    {
        private static GGUIWndNationPowerTip _g_instance = new GGUIWndNationPowerTip();
        public static GGUIWndNationPowerTip instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndNationPowerTip();
                return _g_instance;
            }
        }

        private long _m_lOpSerialize;//操作序列号
        private Queue<long> _m_insAudioIdQueue;//音效资源实例id队列
        private long _m_lCurAddValue;//当前增加值
        private long _m_lTargetAddValue;//目标增加值

        protected GGUIWndNationPowerTip() : base(EALUIWndLayer.NOTICE)
        {

        }


        protected override string _monoAssetPath { get { return GGUIMonoNationPowerTip.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoNationPowerTip.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_lOpSerialize++;
        }

        protected override void _onHideWnd()
        {
            _m_lOpSerialize++;
            _m_lCurAddValue = 0;
            _m_lTargetAddValue = 0;

            while (_m_insAudioIdQueue != null && _m_insAudioIdQueue.Count > 0)
            {
                long stopInstanceId = _m_insAudioIdQueue.Dequeue();
                PlayAudioMgr.instance.stopClip(stopInstanceId);
            }
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_oriValue"></param>
        /// <param name="_curValue"></param>
        public void setInfo(long _oriValue, long _curValue)
        {
            if (wnd == null)
                return;

            if (_m_insAudioIdQueue == null)
                _m_insAudioIdQueue = new Queue<long>();

            _m_lOpSerialize++;
            long serialize = _m_lOpSerialize;
            long addValue = _curValue - _oriValue;

            //播放新音乐
            long insAudioId = PlayAudioMgr.instance.playClip(wnd.audioResId);
            _m_insAudioIdQueue.Enqueue(insAudioId);
            //停止超出数量的音乐
            while (_m_insAudioIdQueue.Count > wnd.maxAudioCount)
            {
                long stopInstanceId = _m_insAudioIdQueue.Dequeue();
                PlayAudioMgr.instance.stopClip(stopInstanceId);
            }

            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goAddShowList, addValue > 0);
            ALUGUICommon.setGameObjEnable(wnd.goReduceShowList, addValue <= 0);

            string key = string.IsNullOrEmpty(wnd.valueKey) ? TransKeyConst.common_value : wnd.valueKey;
            ALUGUICommon.setLabelTxt(wnd.txtNationPower, TextTranslate.instance.getLanguage(key, _curValue.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));

            if (wnd.anim != null) 
                wnd.anim.ForcePlay(wnd.numChgAnimName);

            //展示变化
            _showValueLerp(_m_lCurAddValue, _m_lTargetAddValue + addValue);

            //定时隐藏
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (serialize != _m_lOpSerialize)
                    return;

                hideWnd();
            }, wnd.disableTime);
        }

        //一段时间内，值从开始到结束的变化过程
        private void _showValueLerp(long _start, long _end)
        {
            if (null == wnd || _start == _end)
                return;

            //先设置初始值
            if (_m_lCurAddValue == 0)
                ALUGUICommon.setLabelTxt(wnd.txtAddNum, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_lCurAddValue));
            else
                ALUGUICommon.setLabelTxt(wnd.txtReduceNum, _m_lCurAddValue.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD));

            //设置目标值
            _m_lTargetAddValue = _end;

            NPMonoTaskLerpStartEndValueByTime.startLerpTask(() => { return wnd == null || !isShow; }
                , _m_lOpSerialize
                , _start
                , _end
                , Math.Abs(wnd.value_lerp_time) < 0.01 ? 1.0f : wnd.value_lerp_time
                , value =>
                {
                    //设置增加值
                    _m_lCurAddValue = value;
                    if (value > 0)
                        ALUGUICommon.setLabelTxt(wnd.txtAddNum, value.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD));
                    else
                        ALUGUICommon.setLabelTxt(wnd.txtReduceNum, value.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD));
                }
                , () => { return _m_lOpSerialize; }
                , null);
        }
    }
}
