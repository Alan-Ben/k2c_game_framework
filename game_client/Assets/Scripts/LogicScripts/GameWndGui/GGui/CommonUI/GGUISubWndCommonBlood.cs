using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndCommonBlood : _ATALBasicUISubWnd<GGUIMonoCommonBlood>, _IShakable
    {
        public long curValue { get { return _m_lCurBlood; } }
        public long maxValue { get { return _m_lMaxBlood; } }

        public GGUISubWndCommonBlood(GGUIMonoCommonBlood _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {            
        }

        protected override void _onHideWnd()
        {
            _m_iShakeTaskSerialize = ALSerializeOpMgr.next();
            _m_iFadeTaskSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.transShakeRoot != null)
                _m_vShakeOriginPos = wnd.transShakeRoot.anchoredPosition;
        }

        public int taskSerialize { get { return _m_iShakeTaskSerialize; } }
        public Vector3 shakeOriginPos { get { return _m_vShakeOriginPos; } }
        public RectTransform shakeRoot { get { return wnd?.transShakeRoot; } }
        // 震动任务的序列号
        private int _m_iShakeTaskSerialize;
        private int _m_iFadeTaskSerialize;
        // 整栋的原点坐标
        private Vector3 _m_vShakeOriginPos;
                
        private long _m_lCurBlood;
        private long _m_lMaxBlood;


        /// <summary>
        /// 直接设置血量
        /// </summary>
        public void setBloodData(long _current, long _max)
        {
            if (wnd == null)
                return;

            // 停下动画
            _m_iShakeTaskSerialize = ALSerializeOpMgr.next();
            _m_iFadeTaskSerialize = ALSerializeOpMgr.next();

            // 设置真正的当前血量
            _m_lCurBlood = _current;
            _m_lMaxBlood = _max;

            // 设置血条显示数据
            wnd.updateBloodBarShow(_m_lCurBlood, _m_lMaxBlood, 0, 0);
            _setTextShowData(_m_lCurBlood, _m_lMaxBlood);
        }


        /// <summary>
        /// 伤害
        /// </summary>
        public void damage(long _value, bool _clearLastFade = true)
        {
            if (wnd == null)
                return;

            damage(_value, wnd.defaultShakeLevel, _clearLastFade);
        }


        /// <summary>
        /// 伤害
        /// </summary>
        public void damage(long _value, int _shakeLevel, bool _clearLastFade = true)
        {
            if (wnd == null)
                return;

            if (wnd.animDamage != null)
                wnd.animDamage.Play(wnd.damageAnimName);

            ShakeData shakeData = wnd.safeGetShakeData(_shakeLevel);
            if (shakeData != null)
                new ShakeTask(this, shakeData.fShakeIntensity, shakeData.fShakeDuration, true).deal();

            double dValue = _value;
            // 受伤扣血
            _m_lCurBlood -= _value;
            // 如果死透了，要把血量拉到0
            if (_m_lCurBlood < 0)
            {
                double outPart = 0 - _m_lCurBlood;
                _m_lCurBlood = 0;
                dValue -= outPart; // 把溢出值扣掉
            }
            // 表现扣血要加上之前还没表现完的部分
            if (!_clearLastFade) dValue += wnd.bloodFadeDamageShow;
            // 设置表现数据
            wnd.updateBloodBarShow(_m_lCurBlood, _m_lMaxBlood, dValue, 0);
            _setTextShowData(_m_lCurBlood, _m_lMaxBlood);

            // 开启渐变任务
            _m_iFadeTaskSerialize = ALSerializeOpMgr.next();
            ALMonoTaskMgr.instance.addMonoTask(new BloodFadeTask(this), wnd.fTimeBeforeFade);
        }


        /// <summary>
        /// 治愈
        /// </summary>
        public void cure(long _value, bool _clearLastFade = true)
        {
            if (wnd == null)
                return;

            if (wnd.animCure != null)
                wnd.animCure.Play(wnd.cureAnimName);

            double dValue = _value;
            // 治愈回血
            _m_lCurBlood += _value;
            // 如果超出了血量上限，做特殊的回调处理
            if (_m_lCurBlood > _m_lMaxBlood)
            {
                double outPart = _m_lMaxBlood - _m_lCurBlood;
                _m_lCurBlood = _m_lMaxBlood;
                dValue -= outPart; // 把溢出值扣掉
            }
            // 表现回血要加上之前还没表现完的部分
            if (!_clearLastFade) dValue += wnd.bloodFadeCureShow;
            // 设置表现数据
            wnd.updateBloodBarShow(_m_lCurBlood - dValue, _m_lMaxBlood, 0, dValue);
            _setTextShowData(_m_lCurBlood, _m_lMaxBlood);

            // 开启渐变任务
            _m_iFadeTaskSerialize = ALSerializeOpMgr.next();
            ALMonoTaskMgr.instance.addMonoTask(new BloodFadeTask(this), wnd.fTimeBeforeFade);
        }

        // 设置文字表现
        private void _setTextShowData(long _current, long _max)
        {
            string showText = wnd.bShowLargeNum ? _current.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT) : _current.ToString(); ;

            if (wnd.bShowMaxBlood)
                showText = TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, showText, wnd.bShowLargeNum ? _max.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT) : _max.ToString());

            ALUGUICommon.setLabelTxt(wnd.txtBloodValue, showText);
        }

        // 渐变任务，把DamageFade条逐渐减为0，把CureFade条逐渐转换为实际的血条
        private class BloodFadeTask : _IALBaseMonoTask
        {
            private GGUISubWndCommonBlood _m_iInstance;
            private int _m_iTaskSerialize;
            private double _m_fFadeSpeed; // 每秒钟扣多少血

            public BloodFadeTask(GGUISubWndCommonBlood _instance)
            {
                _m_iInstance = _instance;
                _m_iTaskSerialize = _instance._m_iFadeTaskSerialize;
                _m_fFadeSpeed = _instance.wnd.fFadeSpeed * _instance.wnd.bloodMaxShow;
            }

            public void deal()
            {
                if (_m_iInstance._m_iFadeTaskSerialize != _m_iTaskSerialize)
                    return;

                if (_m_fFadeSpeed <= 0)
                    return;

                double fadeOffset = _m_fFadeSpeed * Time.unscaledDeltaTime;
                double damage = _m_iInstance.wnd.bloodFadeDamageShow - fadeOffset;
                if (damage < 0) damage = 0;

                double blood = _m_iInstance.wnd.bloodCurShow + Math.Min(_m_iInstance.wnd.bloodFadeCureShow, fadeOffset);
                double cure = _m_iInstance.wnd.bloodFadeCureShow - fadeOffset;
                if (cure < 0) cure = 0;

                _m_iInstance.wnd.updateBloodBarShow(blood, _m_iInstance.wnd.bloodMaxShow, damage, cure);

                ALMonoTaskMgr.instance.addNextFrameTask(this);
            }
        }
    }
}
