using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 居民属性页面
    /// </summary>
    public class GGUIWndMarsResidentAttributePage : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoMarsResidentAttribute>
    {
        private NPCommonAssetPathInfo _m_iAssetPathInfo;
        
        // 属性进度条子窗口
        private GGUIWndMarsResidentAttributeSlider _m_wHealthSlider;
        private GGUIWndMarsResidentAttributeSlider _m_wHappinessSlider;
        private GGUIWndMarsResidentAttributeSlider _m_wOxygenSlider;
        private GGUIWndMarsResidentAttributeSlider _m_wSatietySlider;
        private GGUIWndMarsResidentAttributeSlider _m_wSleepSlider;
        private GGUIWndMarsResidentAttributeSlider _m_wComfortSlider;
        private GGUIWndMarsResidentAttributeSlider _m_wMoodSlider;
        
        public GGUIWndMarsResidentAttributePage(Transform _parent, NPCommonAssetPathInfo _assetPathInfo) : base(_parent)
        {
            _m_iAssetPathInfo = _assetPathInfo;
        }

        protected override string _monoAssetPath { get { return _m_iAssetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_iAssetPathInfo?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 创建各属性进度条子窗口
            if (wnd.monoHealthSlider != null)
                _m_wHealthSlider = new GGUIWndMarsResidentAttributeSlider(wnd.monoHealthSlider);
            
            if (wnd.monoHappinessSlider != null)
                _m_wHappinessSlider = new GGUIWndMarsResidentAttributeSlider(wnd.monoHappinessSlider);
            
            if (wnd.monoOxygenSlider != null)
                _m_wOxygenSlider = new GGUIWndMarsResidentAttributeSlider(wnd.monoOxygenSlider);
            
            if (wnd.monoSatietySlider != null)
                _m_wSatietySlider = new GGUIWndMarsResidentAttributeSlider(wnd.monoSatietySlider);
            
            if (wnd.monoSleepSlider != null)
                _m_wSleepSlider = new GGUIWndMarsResidentAttributeSlider(wnd.monoSleepSlider);
            
            if (wnd.monoComfortSlider != null)
                _m_wComfortSlider = new GGUIWndMarsResidentAttributeSlider(wnd.monoComfortSlider);
            
            if (wnd.monoMoodSlider != null)
                _m_wMoodSlider = new GGUIWndMarsResidentAttributeSlider(wnd.monoMoodSlider);
        }

        protected override void _onDiscard()
        {
            // 释放所有子窗口
            _m_wHealthSlider?.discard();
            _m_wHealthSlider = null;
            
            _m_wHappinessSlider?.discard();
            _m_wHappinessSlider = null;
            
            _m_wOxygenSlider?.discard();
            _m_wOxygenSlider = null;
            
            _m_wSatietySlider?.discard();
            _m_wSatietySlider = null;
            
            _m_wSleepSlider?.discard();
            _m_wSleepSlider = null;
            
            _m_wComfortSlider?.discard();
            _m_wComfortSlider = null;
            
            _m_wMoodSlider?.discard();
            _m_wMoodSlider = null;
        }

        protected override void _onShowWnd()
        {
            // 注册Mars组件属性变化事件
            if (NPPlayer.instance?.marsComp != null)
            {
                NPPlayer.instance.marsComp.onHealthValueChanged += _onHealthValueChanged;
                NPPlayer.instance.marsComp.onHappyValueChanged += _onHappyValueChanged;
                NPPlayer.instance.marsComp.onOxygenValueChanged += _onOxygenValueChanged;
                NPPlayer.instance.marsComp.onSatietyValueChanged += _onSatietyValueChanged;
                NPPlayer.instance.marsComp.onSleepValueChanged += _onSleepValueChanged;
                NPPlayer.instance.marsComp.onComfortValueChanged += _onComfortValueChanged;
                NPPlayer.instance.marsComp.onMoodValueChanged += _onMoodValueChanged;
            }

            // 刷新界面
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            // 注销Mars组件属性变化事件
            if (NPPlayer.instance?.marsComp != null)
            {
                NPPlayer.instance.marsComp.onHealthValueChanged -= _onHealthValueChanged;
                NPPlayer.instance.marsComp.onHappyValueChanged -= _onHappyValueChanged;
                NPPlayer.instance.marsComp.onOxygenValueChanged -= _onOxygenValueChanged;
                NPPlayer.instance.marsComp.onSatietyValueChanged -= _onSatietyValueChanged;
                NPPlayer.instance.marsComp.onSleepValueChanged -= _onSleepValueChanged;
                NPPlayer.instance.marsComp.onComfortValueChanged -= _onComfortValueChanged;
                NPPlayer.instance.marsComp.onMoodValueChanged -= _onMoodValueChanged;
            }

            // 隐藏所有子窗口
            _m_wHealthSlider?.hideWnd();
            _m_wHappinessSlider?.hideWnd();
            _m_wOxygenSlider?.hideWnd();
            _m_wSatietySlider?.hideWnd();
            _m_wSleepSlider?.hideWnd();
            _m_wComfortSlider?.hideWnd();
            _m_wMoodSlider?.hideWnd();
        }

        protected override void _onReset()
        {
            // 重置所有子窗口
            _m_wHealthSlider?.resetWnd();
            _m_wHappinessSlider?.resetWnd();
            _m_wOxygenSlider?.resetWnd();
            _m_wSatietySlider?.resetWnd();
            _m_wSleepSlider?.resetWnd();
            _m_wComfortSlider?.resetWnd();
            _m_wMoodSlider?.resetWnd();
        }


        /// <summary>
        /// 刷新界面
        /// </summary>
        public void refreshWnd()
        {
            if (NPPlayer.instance?.marsComp == null)
                return;

            // 刷新各属性进度条
            _refreshHealthSlider();
            _refreshHappinessSlider();
            _refreshOxygenSlider();
            _refreshSatietySlider();
            _refreshSleepSlider();
            _refreshComfortSlider();
            _refreshMoodSlider();
        }

        /// <summary>
        /// 刷新健康值进度条
        /// </summary>
        private void _refreshHealthSlider()
        {
            if (_m_wHealthSlider == null || NPPlayer.instance?.marsComp == null)
                return;

            long healthValue = NPPlayer.instance.marsComp.healthValue;
            long maxHealthValue = 100; // 最大值
            
            _m_wHealthSlider.showWnd();
            _m_wHealthSlider.setProgress(healthValue, maxHealthValue, EValueFormatType.NORMAL);
        }

        /// <summary>
        /// 刷新幸福值进度条
        /// </summary>
        private void _refreshHappinessSlider()
        {
            if (_m_wHappinessSlider == null || NPPlayer.instance?.marsComp == null)
                return;

            long happyValue = NPPlayer.instance.marsComp.happyValue;
            long maxHappyValue = 100; // 最大值
            
            _m_wHappinessSlider.showWnd();
            _m_wHappinessSlider.setProgress(happyValue, maxHappyValue, EValueFormatType.NORMAL);
        }

        /// <summary>
        /// 刷新氧气值进度条
        /// </summary>
        private void _refreshOxygenSlider()
        {
            if (_m_wOxygenSlider == null || NPPlayer.instance?.marsComp == null)
                return;

            long oxygenValue = NPPlayer.instance.marsComp.oxygenValue;
            long maxOxygenValue = 100; // 最大值
            
            _m_wOxygenSlider.showWnd();
            _m_wOxygenSlider.setProgress(oxygenValue, maxOxygenValue, EValueFormatType.NORMAL);
        }

        /// <summary>
        /// 刷新饱腹值进度条
        /// </summary>
        private void _refreshSatietySlider()
        {
            if (_m_wSatietySlider == null || NPPlayer.instance?.marsComp == null)
                return;

            long satietyValue = NPPlayer.instance.marsComp.satietyValue;
            long maxSatietyValue = 100; // 最大值
            
            _m_wSatietySlider.showWnd();
            _m_wSatietySlider.setProgress(satietyValue, maxSatietyValue, EValueFormatType.NORMAL);
        }

        /// <summary>
        /// 刷新睡眠值进度条
        /// </summary>
        private void _refreshSleepSlider()
        {
            if (_m_wSleepSlider == null || NPPlayer.instance?.marsComp == null)
                return;

            long sleepValue = NPPlayer.instance.marsComp.sleepValue;
            long maxSleepValue = 100; // 最大值
            
            _m_wSleepSlider.showWnd();
            _m_wSleepSlider.setProgress(sleepValue, maxSleepValue, EValueFormatType.NORMAL);
        }

        /// <summary>
        /// 刷新舒适值进度条
        /// </summary>
        private void _refreshComfortSlider()
        {
            if (_m_wComfortSlider == null || NPPlayer.instance?.marsComp == null)
                return;

            long comfortValue = NPPlayer.instance.marsComp.comfortValue;
            long maxComfortValue = 100; // 最大值
            
            _m_wComfortSlider.showWnd();
            _m_wComfortSlider.setProgress(comfortValue, maxComfortValue, EValueFormatType.NORMAL);
        }

        /// <summary>
        /// 刷新心情值进度条
        /// </summary>
        private void _refreshMoodSlider()
        {
            if (_m_wMoodSlider == null || NPPlayer.instance?.marsComp == null)
                return;

            long moodValue = NPPlayer.instance.marsComp.moodValue;
            long maxMoodValue = 100; // 最大值
            
            _m_wMoodSlider.showWnd();
            _m_wMoodSlider.setProgress(moodValue, maxMoodValue, EValueFormatType.NORMAL);
        }


        // 事件处理方法
        private void _onHealthValueChanged(long _value)
        {
            _refreshHealthSlider();
        }

        private void _onHappyValueChanged(long _value)
        {
            _refreshHappinessSlider();
        }

        private void _onOxygenValueChanged(long _value)
        {
            _refreshOxygenSlider();
        }

        private void _onSatietyValueChanged(long _value)
        {
            _refreshSatietySlider();
        }

        private void _onSleepValueChanged(long _value)
        {
            _refreshSleepSlider();
        }

        private void _onComfortValueChanged(long _value)
        {
            _refreshComfortSlider();
        }

        private void _onMoodValueChanged(long _value)
        {
            _refreshMoodSlider();
        }
    }
}