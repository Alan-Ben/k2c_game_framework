
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NPCommon;
using ALPackage;
using System.Text;


namespace GOE
{
    //单个单位产出数据对象
    public class NPPlayerBuffInfo
    {
        private NPPlayerBuffComponent _m_compPlayerBuffComponent;

        private long _m_lBuffId;
        private int _m_iLayer;
        private long _m_fStartMs;
        private long _m_fEndMs;//当改值<=0时表示永久
        private NPPlayerBuffRefObj _m_refPlayerBuffRef;

        public long buffId { get { return _m_lBuffId; } }
        public int layer { get { return _m_iLayer; } }
        public NPPlayerBuffRefObj refObj { get { return _m_refPlayerBuffRef; } }
        public long startTimeMs { get { return _m_fStartMs; } }

        //计算buf剩余时间用于显示(-1：永久 其他：剩余毫秒数)
        public long curLeftTimeMS
        {
            get
            {
                if(_m_fEndMs <= 0)
                    return -1;

                long timeLeft = _m_fEndMs - FpsAndPingMgr.instance.serverTimeTag;
                if(timeLeft <= 0)
                    return 0;

                return timeLeft;
            }
        }

        /// <summary>
        /// 已激活天数
        /// </summary>
        public long hadActiveDay
        {
            get
            {
                if (_m_fStartMs <= 0)
                    return 0;

                long nowMs = FpsAndPingMgr.instance.serverTimeTag;

                //开始时间晚于有效结束时间，说明尚未开始或配置异常
                if (nowMs < _m_fStartMs)
                    return 0;

                //转为服务器时区下的日期（Date 取去除时间部分）
                DateTime startDate = TimeUtil.FromUTCMilliseconds(_m_fStartMs).Date;
                DateTime endDate = TimeUtil.FromUTCMilliseconds(nowMs).Date;

                //天数差（含开始当天，故+1）
                long days = (long)(endDate - startDate).TotalDays + 1;
                if (days < 0)
                    return 0;

                return days;
            }
        }

        public NPPlayerBuffInfo(NPPlayerBuffComponent _comp, NPCommon_PlayerBuffInfo _info, NPPlayerBuffRefObj _ref)
        {
            _m_compPlayerBuffComponent = _comp;

            _m_lBuffId = _info.getBuffId();
            _m_iLayer = _info.getLayer();
            _m_fStartMs = _info.getStartMs();
            _m_fEndMs = _info.getEndMs();

            _m_refPlayerBuffRef = _ref;
        }

        //更新buff数据
        public void update(NPCommon_PlayerBuffInfo _info)
        {
            //设置新的
            _m_iLayer = _info.getLayer();
            _m_fStartMs = _info.getStartMs();
            _m_fEndMs = _info.getEndMs();

            //Debug.LogError("update : " + _m_lBuffId + " > " + _m_fLeftMs + " ^ " + curLeftTimeS);
        }

        //检查buff是否失效
        public bool hasExpired()
        {
            //Debug.LogError("tick : " + _m_lBuffId + " > " + _m_fLeftMs + " ^ " + curLeftTimeS);
            if (_m_refPlayerBuffRef == null)//若buff配表不存在算buff已过期
                return true;
            
            // 当(层数<=0 且 层数为0需要移除) || (有结束时间 且 已过期)
            return (_m_iLayer <= 0 && _m_refPlayerBuffRef.layer_empty_remove) || 
                   (_m_fEndMs > 0 && _m_fEndMs - FpsAndPingMgr.instance.serverTimeTag < 0);
        }
        
        //获取指定ID的Buff剩余时间
        public string getLeftTimeStr()
        {
            //永久buff
            if(-1 == curLeftTimeMS)
                return TextTranslate.instance.getLanguage(TransKeyConst.timeForever);

            return TimeUtil.millisecondsToTime_Two(curLeftTimeMS);
        }
    }
}

