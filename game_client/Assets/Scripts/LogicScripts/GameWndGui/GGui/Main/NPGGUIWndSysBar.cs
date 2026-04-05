using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 战场TopBar
    /// </summary>
    public class NPGGUIWndSysBar : _ANPGGUIBasicWnd<NPGGUIMonoSysBar>
    {
        private static NPGGUIWndSysBar _g_instance = new NPGGUIWndSysBar();
        public static NPGGUIWndSysBar instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPGGUIWndSysBar();
                return _g_instance;
            }
        }

        //下一个添加分计数的时间
        private long _m_lNextAddMinTimeTag;
        private int _m_iShowMinute;
        private int _m_iShowHour;

        private float _m_iShowPower;
        private float _m_fShowPowerF;
        private NPGGUISystemPowerStepInfo _m_siCurStepInfo;

        protected NPGGUIWndSysBar()
            : base(EALUIWndLayer.NORMAL)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return NPGGUIMonoSysBar.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoSysBar.objName; } }

        /**************
        * 获取用于加载资源的管理对象
        **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        /******************
        * 显示窗口的事件函数
        **/
        protected override void _onShowWnd()
        {
            if(null == wnd)
                return;

            //比对服务器时间判断是否需要刷新
            DateTime serverNow = TimeUtil.FromUTCMilliseconds(FpsAndPingMgr.instance.serverTimeTag);

            _m_lNextAddMinTimeTag = FpsAndPingMgr.instance.serverTimeTag;
            //计算整分的时间
            _m_lNextAddMinTimeTag = _m_lNextAddMinTimeTag - (_m_lNextAddMinTimeTag % 60000) + 60000;

            _m_iShowHour = serverNow.Hour;
            _m_iShowMinute = serverNow.Minute;

            //刷新显示
            ALUGUICommon.setLabelTxt(wnd.txtSystemTime, _m_iShowHour.ToString("00") + ":" + _m_iShowMinute.ToString("00"));
        }

        /******************
        * 隐藏窗口的事件函数
        **/
        protected override void _onHideWnd()
        {
            _m_iShowMinute = 0;
            _m_iShowHour = 0;

            _m_iShowPower = 0;
            _m_fShowPowerF = 0f;
            _m_siCurStepInfo = null;
        }

        /******************
         * 重置窗口数据的事件函数
         //**/
        protected override void _onReset()
        {
        }

        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
        }

        /*************
        * 窗口初始化完成调用的函数
        * */
        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 刷新系统时间和电量信息
        /// </summary>
        public void updateSystemTimeAndPower()
        {
            if(wnd == null)
                return;

            if(FpsAndPingMgr.instance.serverTimeTag >= _m_lNextAddMinTimeTag)
            {
                //增加分
                _m_iShowMinute++;
                if(_m_iShowMinute > 60)
                {
                    _m_iShowMinute = _m_iShowMinute - 60;
                    _m_iShowHour++;
                    if(_m_iShowHour > 24)
                        _m_iShowHour -= 24;
                }
                //增加下一次刷新时间
                _m_lNextAddMinTimeTag += 60000;

                //刷新显示
                ALUGUICommon.setLabelTxt(wnd.txtSystemTime, _m_iShowHour.ToString("00") + ":" + _m_iShowMinute.ToString("00"));
            }

            //电量显示信息尚未配置，隐藏
            if(wnd.systemPowerStepList == null || wnd.systemPowerStepList.Count == 0)
            {
                ALUGUICommon.setGameObjDisable(wnd.GoSystemPower);
                return;
            }


            //#if UNITY_EDITOR
            //        //隐藏对象
            //        ALUGUICommon.setGameObjEnable(wnd.GoSystemPower, false);
            //#else
            //充电中标识
            ALUGUICommon.setUIObjScale(wnd.GoCharging, SystemInfo.batteryStatus == BatteryStatus.Charging ? 1f : 0f);

            //电池百分比为-1表示该平台无法获取电池信息，隐藏
            if(SystemInfo.batteryLevel == -1)
            {
                ALUGUICommon.setUIObjScale(wnd.GoSystemPower, 0f);
                return;
            }

            //电池电量百分比
            int powerPercent = (int)(100 * SystemInfo.batteryLevel);
            _m_fShowPowerF = SystemInfo.batteryLevel;

            if(powerPercent > 100)
            {
                powerPercent = 100;
                _m_fShowPowerF = 1f;
            }
            if(powerPercent < 1)
            {
                powerPercent = 1;
                _m_fShowPowerF = 0.01f;
            }

            //比对是否有电量变更
            if(powerPercent != _m_iShowPower)
            {
                //按阶段显示不同颜色和文字
                NPGGUISystemPowerStepInfo curPowerStepInfo = wnd.getStepInfo(powerPercent);
                if(curPowerStepInfo != null && curPowerStepInfo != _m_siCurStepInfo)
                {
                    _m_siCurStepInfo = curPowerStepInfo;

                    //设置颜色等状态
                    for(int i = 0; i < wnd.chgColorGraphicList.Count; i++)
                    {
                        ALUGUICommon.setUIObjColor(wnd.chgColorGraphicList[i], _m_siCurStepInfo.showColor);
                    }
                }

                //百分比文字
                if(null != wnd.txtSystemPowerPercent)
                    ALUGUICommon.setLabelTxt(wnd.txtSystemPowerPercent, powerPercent + "%");

                //百分比进度条
                ALUGUICommon.setSliderScale(wnd.ImgSystemPowerProcess, _m_fShowPowerF);
            }
            //#endif
        }
    }
}

