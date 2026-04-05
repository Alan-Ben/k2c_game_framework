
using System;
using System.Collections.Generic;
using Common;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 跑马灯信息
    /// </summary>
    public class MarqueeInfo
    {
        //实例id
        private long _m_lDbId;
        //配表id
        private long _m_lRefId;
        //窗口展示位置
        private int _m_iShowPosId;
        //优先级（越大越优先）
        private int _m_lPriorityId;
        //循环播放时长秒（优先于次数）
        private int _m_lDurationSec;
        //循环播放次数
        private int _m_lDurationCount;
        //预制体ID
        private long _m_lUiResId;
        //是否可删除
        private bool _m_bCanDel;
        //内容
        private string _m_sContent;
        //显示条件
        private _NPPlayerConditionSerializeInfo _m_showCondition;
        //过期时间
        private long _m_lExpiredTimeMs;
        //参数列表
        private List<string> _m_lParamList;
        //解析完参数列表
        private List<string> _m_lRealParamList;
        //跑马灯配表数据
        private MarqueeRefObj _m_marqueeRef;
        //已展示次数
        private long _m_lCurShowTime;
        //记录的开始展示时间
        private long _m_lRecordStartShowTimeSec;
        //记录总的已展示时间
        private float _m_lRecordTotalShowTimeSec;
        //需要解析参数的标志
        private const string ANALYTIC_ITEMS_TAG = "##";


        /// <summary>
        /// 实例id
        /// </summary>
        public long instanceId { get { return _m_lDbId; } }
        /// <summary>
        /// 配表id
        /// </summary>
        public long refId { get { return _m_lRefId; } }
        /// <summary>
        /// 窗口展示位置
        /// </summary>
        public int showPosId { get { return _m_iShowPosId; } }
        /// <summary>
        /// 优先级
        /// </summary>
        public long priorityId { get { return _m_lPriorityId; } }
        /// <summary>
        /// 循环播放时长秒
        /// </summary>
        public long durationSec { get { return _m_lDurationSec; } }
        /// <summary>
        /// 过期时间
        /// </summary>
        public long expiredTimeMs { get { return _m_lExpiredTimeMs; } }
        /// <summary>
        /// 循环播放次数
        /// </summary>
        public long durationCount { get { return _m_lDurationCount; } }
        /// <summary>
        /// 预制体ID
        /// </summary>
        public long uiResId { get { return _m_lUiResId; } }
        /// <summary>
        /// 是否可删除
        /// </summary>
        public bool canDel { get { return _m_bCanDel; } }
        /// <summary>
        /// 跑马灯展示文本内容
        /// </summary>
        public string content
        {
            get
            {
                if (!string.IsNullOrEmpty(_m_sContent))
                {
                    if (_m_lRealParamList == null || _m_lRealParamList.Count == 0)
                        _m_lRealParamList = _analysisParam(_m_lParamList);

                    return TextTranslate.instance.getLanguage(_m_sContent, _m_lRealParamList);
                }
                else
                    return null;
            }
        }


        public MarqueeInfo(int _showPosId, Common_MarqueeInfo _info)
        {
            if(_info == null)
                return;

            _m_iShowPosId = _showPosId;
            _m_lDbId = _info.getDbId();
            _m_lRefId = _info.getRefId();
            _m_lExpiredTimeMs = _info.getExpiredTimeMs();
            _m_marqueeRef = GRefdataCoreMgr.instance.marqueeRefCore.getRef(_m_lRefId);

            //设置优先级（越大越优先）
            if (_info.getPriorityId() > 0)
                _m_lPriorityId = _info.getPriorityId();
            else
                _m_lPriorityId = _m_marqueeRef != null ? _m_marqueeRef.priority_id : 0;

            //设置循环播放时长秒（优先于次数）
            if (_info.getDurationSec() > 0)
                _m_lDurationSec = _info.getDurationSec();
            else
                _m_lDurationSec = _m_marqueeRef != null ? _m_marqueeRef.duration_sec : 0;

            //设置循环播放次数
            if (_info.getDurationCount() > 0)
                _m_lDurationCount = _info.getDurationCount();
            else
                _m_lDurationCount = _m_marqueeRef != null ? _m_marqueeRef.duration_count : 0;

            //设置预制体ID
            if (_info.getUiResId() > 0)
                _m_lUiResId = _info.getUiResId();
            else
                _m_lUiResId = _m_marqueeRef != null ? _m_marqueeRef.ui_res_id : 0;

            //设置内容
            if (!string.IsNullOrEmpty(_info.getContent()))
                _m_sContent = _info.getContent();
            else
                _m_sContent = _m_marqueeRef != null ? _m_marqueeRef.content : "";

            //设置展示条件
            if(!string.IsNullOrEmpty(_info.getShowCondition()))
                _m_showCondition = _NPPlayerConditionSerializeInfo.ReadFromString(_info.getShowCondition());
            else
                _m_showCondition = _m_marqueeRef != null ? _m_marqueeRef.show_condition : null;

            //设置是否可删除
            switch (_info.getCanDelType())
            {
                case EMarqueeCanDelType.READ_REF:
                    _m_bCanDel = _m_marqueeRef != null ? _m_marqueeRef.can_del : false;
                    break;
                case EMarqueeCanDelType.TRUE:
                    _m_bCanDel = true;
                    break;
                case EMarqueeCanDelType.FALSE:
                    _m_bCanDel = false;
                    break;
            }

            _m_lParamList = _info.getParamList();
            _m_lCurShowTime = 0;
            _m_lRecordStartShowTimeSec = 0;
        }

        /// <summary>
        /// 设置开始展示时间
        /// </summary>
        public void setStartShowTime()
        {
            _m_lRecordStartShowTimeSec = FpsAndPingMgr.instance.serverTimeTagS;
        }

        /// <summary>
        /// 设置已读
        /// </summary>
        /// <param name="_intervalTime">两条跑马灯展示间隔时间（在这个时间内第一条跑马灯还在展示中，需要加上这个时间）</param>
        public void setRead(float _intervalTime)
        {
            //记录展示次数
            _m_lCurShowTime++;
            //记录展示时间
            _m_lRecordTotalShowTimeSec += (FpsAndPingMgr.instance.serverTimeTagS - _m_lRecordStartShowTimeSec + _intervalTime);
        }

        /// <summary>
        /// 检查跑马灯是否有效
        /// </summary>
        public bool checkIsValid()
        {
            //检查参数是否正确
            if (_m_lUiResId <= 0 || string.IsNullOrEmpty(content) || (_m_lDurationSec <= 0 && _m_lDurationCount <= 0))
            {
                Debug.LogError($"【跑马灯】跑马灯参数有问题，instanceId:{_m_lDbId},refId:{_m_lRefId},UiResId:{_m_lUiResId},DurationSec:{_m_lDurationSec},DurationCount{_m_lDurationCount},content:{content} ");
                return false;
            }

            //先检查是否已读
            if (!NPPlayer.instance.marqueeComp.checkIsValidByRecord(_m_iShowPosId, priorityId, _m_lDbId))
                return false;

            //先检查条件是否满足
            if (_m_showCondition != null && !_m_showCondition.IsEnable(null))
                return false;

            //判断是否过期
            if (_m_lExpiredTimeMs > 0 && FpsAndPingMgr.instance.serverTimeTag >= _m_lExpiredTimeMs)
                return false;

            //判断循环播放时长是否达到
            if (_m_lDurationSec > 0)
                return _m_lRecordTotalShowTimeSec < _m_lDurationSec;

            //判断循环次数是否达到
            if (_m_lDurationCount > 0)
                return _m_lCurShowTime < _m_lDurationCount;

            //判断是否展示过了
            return _m_lCurShowTime < 1;
        }

        /// <summary>
        /// 解析参数
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        private List<string> _analysisParam(List<string> _params)
        {
            List<string> targetParamList = new List<string>();
            if (_params == null)
                return targetParamList;

            //根据每个参数类型解析成对应数据
            for (int i = 0; i < _params.Count; i++)
            {
                string paramStr = _params[i];
                string targetStr = null;
                if(string.IsNullOrEmpty(paramStr))
                    continue;

                try
                {
                    //****解析参数****
                    if (paramStr.StartsWith(ANALYTIC_ITEMS_TAG))
                    {
                        string[] afterSplit = paramStr.Split(new string[] { ANALYTIC_ITEMS_TAG }, StringSplitOptions.RemoveEmptyEntries);
                        if (afterSplit.Length >= 2 && !string.IsNullOrEmpty(afterSplit[0]))
                        {
                            switch (afterSplit[0])
                            {
                                //道具类型，格式：##COMMON-ITEM##CLOTHES_UNIT-250014000
                                case "COMMON-ITEM":
                                    if (!string.IsNullOrEmpty(afterSplit[1]))
                                    {
                                        NPCommonItem commonItem = NPCommonItem.readFromStr(afterSplit[1]);
                                        if (commonItem != null)
                                            targetStr = GCommon.getItemName(commonItem.itemType, commonItem.itemId);
                                        else
                                            targetStr = paramStr;
                                    }
                                    else
                                        targetStr = paramStr;
                                    break;
                                default:
                                    targetStr = paramStr;
                                    break;
                            }
                        }
                        else
                            targetStr = paramStr;
                    }
                    else
                        targetStr = paramStr;

                    targetParamList.Add(targetStr);
                }
                catch (Exception e)
                {
                    Debug.LogError($"跑马灯参数解析错误!!! refId:{_m_lRefId},参数：{paramStr}\n{e}");
                }
            }

            return targetParamList;
        }
    }
}