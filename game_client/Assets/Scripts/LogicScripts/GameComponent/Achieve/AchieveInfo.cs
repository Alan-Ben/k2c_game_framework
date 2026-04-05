using System.Collections.Generic;
using ALPackage;
using Common.AchieveObj;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class AchieveInfo
    {
        private long _m_achieveId;//成就id
        private AchieveRefObj _m_achieveRefObj;//成就配表数据
        private List<AchieveStepInfo> _m_stepInfoList;//步骤列表
        private AchieveStepInfo _m_curStepInfo;//当前步骤
        private HashSet<int> _m_iHasDrawStepList;//已经获取奖励步骤列表
        private long _m_curCounter;//当前计数
        private bool _m_isLastFull = false;//上次刷新是否已满
        private bool _m_bIsInitLastFullTag = false;//是否初始化了LastFull标记



        /// <summary>
        /// 成就id
        /// </summary>
        public long achieveId { get => _m_achieveId; }
        /// <summary>
        /// 成就配置数据
        /// </summary>
        public AchieveRefObj achieveRefObj { get => _m_achieveRefObj; }
        /// <summary>
        /// 步骤列表
        /// </summary>
        public List<AchieveStepInfo> stepInfoList { get => _m_stepInfoList; }
        /// <summary>
        /// 当前步骤
        /// </summary>
        public AchieveStepInfo curStepInfo { get => _m_curStepInfo; }
        /// <summary>
        /// 当前步骤id
        /// </summary>
        public int step { get => _m_curStepInfo == null ? 0 : _m_curStepInfo.step; }
        /// <summary>
        /// 当前步骤计数
        /// </summary>
        public long curStepCount { get => _m_curCounter + _m_achieveRefObj.process_cur_count.CalculateVariableResult(null); }
        /// <summary>
        /// 已经获取奖励步骤列表
        /// </summary>
        public HashSet<int> hasDrawStepList { get => _m_iHasDrawStepList; }
        /// <summary>
        /// 成就类型
        /// </summary>
        public EAchieveType achieveType { get => _m_achieveRefObj != null ? _m_achieveRefObj.achieve_type : EAchieveType.NONE; }


        public AchieveInfo(AchieveRefObj _achieveRef)
        {
            if (_achieveRef == null || null == _achieveRef.step_list)
            {
                Debug.LogError("【成就】初始化成就数据错误 _achieveRef = null");
                return;
            }

            //初始化成就配置数据
            _m_achieveRefObj = _achieveRef;
            _m_achieveId = _achieveRef.achieve_id;

            //初始化步骤列表
            _m_stepInfoList = new List<AchieveStepInfo>();
            AchieveStepRefObj tempStepRef = null;
            for (int i = 0; i < _m_achieveRefObj.step_list.Count; i++)
            {
                tempStepRef = _m_achieveRefObj.step_list[i]; 
                if(null == tempStepRef)
                    continue;
                _m_stepInfoList.Add(new AchieveStepInfo(this, tempStepRef));
            }

            //初始化计数
            _m_curCounter = 0;

            //初始化步骤，默认第一个步骤
            if (_m_stepInfoList.Count != 0)
                _m_curStepInfo = _m_stepInfoList[0];
            else
                _m_curStepInfo = null;

            //监听客户端目标数变动
            _addRegister();
        }

        /// <summary>
        /// 刷新服务端信息
        /// </summary>
        /// <param name="_info"></param>
        public void refreshData(Achieve_Info _info, bool isPopTip = true)
        {
            if (null == _info || _info.getAchieveId() != _m_achieveId)
                return;

            _m_curCounter = _info.getCounter();
            if(_m_iHasDrawStepList == null)
                _m_iHasDrawStepList = new HashSet<int>(_info.getHadDrawStepList());
            else
            {
                _m_iHasDrawStepList.Clear();
                for (int i = 0; i < _info.getHadDrawStepList().Count; i++)
                {
                    if (!_m_iHasDrawStepList.Contains(_info.getHadDrawStepList()[i]))
                        _m_iHasDrawStepList.Add(_info.getHadDrawStepList()[i]);
                }
            }
            _m_curStepInfo = _getCurStep();
            _setAchieveIsFull(isPopTip);
        }

        /// <summary>
        /// 初始化上次刷新是否已满标记，由于会引用到其他组件数据去判断，所以需要在所有组件都初始化完之后再初始化
        /// </summary>
        public void initIsLastFull()
        {
            if (_m_curStepInfo != null)
                _m_isLastFull = _m_curStepInfo.isFull();
            _m_bIsInitLastFullTag = true;
        }

        /// <summary>
        /// 是否成就步骤已经全部完成
        /// </summary>
        /// <returns></returns>
        public bool isAllDone()
        {
            if (_m_curStepInfo == null)
                return true;

            foreach (AchieveStepInfo achieveStepInfo in _m_stepInfoList)
            {
                if (null == achieveStepInfo)
                    continue;
                //只要有一个不是已领取，就不算全部完成
                if (achieveStepInfo.getRewardState() != ENPCommonGetStat.HAS_GET)
                    return false;

            }
            return true;
        }
        
        /// <summary>
        /// 销毁
        /// </summary>
        public void discard()
        {
            _removeRegister();
            _m_stepInfoList?.Clear();
        }

        /// <summary>
        /// 获取当前步骤领奖状态
        /// </summary>
        /// <returns></returns>
        public ENPCommonGetStat getCurStepRewardState()
        {
            if (_m_curStepInfo != null)
                return _m_curStepInfo.getRewardState();
            else
                return ENPCommonGetStat.CAN_NOT_GET;
        }
        

        /// <summary>
        /// 获取步骤领奖状态
        /// </summary>
        /// <returns></returns>
        public ENPCommonGetStat getStepRewardState(long _stepId)
        {
            AchieveStepInfo stepInfo = getStepInfo(_stepId);
            if (null != stepInfo)
                return stepInfo.getRewardState();
            return ENPCommonGetStat.CAN_NOT_GET;
        }

        /// <summary>
        /// 获取成就进度值，不同的计数类型会有不同的展示格式
        /// </summary> 
        /// <returns></returns>
        public string getAchieveProgressStr(long _stepId, Color _txtColor, long _txtSize, bool _chgAllStr = false)
        {
            if (_m_achieveRefObj == null || _m_curStepInfo == null || _m_curStepInfo.stepRefObj == null)
                return null;

            AchieveStepInfo achieveStepInfo = getStepInfo(_stepId);
            long curCount = curStepCount;
            long targetCount = achieveStepInfo != null ? achieveStepInfo.stepRefObj.process_count : 0;

            //获取对应格式进度字符串
            string curCountStr = GCommon.getValueFormatStr(_m_achieveRefObj.process_num_format, curCount);
            string targetCountStr = GCommon.getValueFormatStr(_m_achieveRefObj.process_num_format, targetCount);

            curCountStr = GCommon.addSizeForRichText(curCountStr, _txtSize);

            if (_chgAllStr)
                return GCommon.addColorForRichText(TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num,curCountStr, targetCountStr),_txtColor);

            return TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, GCommon.addColorForRichText(curCountStr, _txtColor), targetCountStr);
        }

        /// <summary>
        /// 监听客户端目标数变动
        /// </summary>
        private void _addRegister()
        {
            if (null == _m_achieveRefObj || null == _m_achieveRefObj.add_msg_type_list)
                return;

            WinMsgType temp = 0;
            for (int i = 0; i < _m_achieveRefObj.add_msg_type_list.Count; i++)
            {
                bool isParse = ALCommon.TryEnumParse(typeof(WinMsgType),_m_achieveRefObj.add_msg_type_list[i], out temp);
                if (!isParse || temp == WinMsgType.NONE)
                    continue;

                WinMsg.RegisterMsgAct(temp, _setAchieveIsFull);
            }
        }
        
        /// <summary>
        /// 移除监听
        /// </summary>
        private void _removeRegister()
        {
            //监听客户端目标数变动
            if (_m_achieveRefObj != null && null != _m_achieveRefObj.add_msg_type_list)
            {
                WinMsgType temp = 0;
                for (int i = 0; i < _m_achieveRefObj.add_msg_type_list.Count; i++)
                {
                    bool isParse = ALCommon.TryEnumParse(typeof(WinMsgType), _m_achieveRefObj.add_msg_type_list[i], out temp);
                    if (!isParse || temp == WinMsgType.NONE)
                        continue;

                    WinMsg.UnregisterMsgAct(temp, _setAchieveIsFull);
                }
            }
        }
        
        /// <summary>
        /// 设置客户端是否完成成就
        /// </summary>
        private void _setAchieveIsFull(bool isPopTip)
        {
            if (null == _m_achieveRefObj || _m_curStepInfo == null)
                return;

            bool isFullLast = _m_isLastFull;
            _m_isLastFull = _m_curStepInfo.isFull();
            AchieveTypeRefObj achieveTypeRefObj = GRefdataCoreMgr.instance.achieveTypeMap.getRef((long)_m_achieveRefObj.achieve_type);
            //设置红点
            if (!isFullLast && _m_isLastFull && null != achieveTypeRefObj && achieveTypeRefObj.red_tip_id > 0)
                RedTipMgr.instance.addCountByRefRedTipId(achieveTypeRefObj.red_tip_id, 1);

            //不发出消息或者系统未解锁就跳过
            if (!isPopTip || !GCommon.isFuncUnlock(ENPFunctionType.ACHIEVE))
                return;

            //当前成就达成后弹出达成提示
            if (!isFullLast && _m_isLastFull && _m_bIsInitLastFullTag)
            {
                AchieveTypeRefObj typeRefObj = GRefdataCoreMgr.instance.achieveTypeMap.getRef((long)_m_achieveRefObj.achieve_type);
                if(null == typeRefObj || typeRefObj.center_tip_id <= 0)
                    return;

                string txtString = TextTranslate.instance.getLanguage(TransKeyConst.common_strDotStr, _m_curStepInfo.step, _m_curStepInfo.stepRefObj.getName);
                NPGUIAddSceneCenterTip.instance.showIconTextTip(_m_achieveRefObj.icon, txtString, typeRefObj.center_tip_id);
            }
        }
        
        /// <summary>
        /// 设置客户端是否完成成就
        /// </summary>
        private void _setAchieveIsFull()
        {
            _setAchieveIsFull(true);
        }

        /// <summary>
        /// 获取当前的步骤
        /// </summary>
        /// <returns></returns>
        private AchieveStepInfo _getCurStep()
        {
            if (_m_stepInfoList == null || _m_stepInfoList.Count <= 0)
                return null;

            if (_m_iHasDrawStepList == null || _m_iHasDrawStepList.Count == 0)
                return _m_stepInfoList[0];

            for (int i = 0; i < _m_stepInfoList.Count; i++)
            {
                if (!_m_iHasDrawStepList.Contains(_m_stepInfoList[i].step))
                    return _m_stepInfoList[i];
            }

            //全部领取了设置当前步骤为最后一步
            return _m_stepInfoList[_m_stepInfoList.Count - 1];
        }

        /// <summary>
        /// 获取步骤信息
        /// </summary>
        /// <param name="_step"></param>
        /// <returns></returns>
        public AchieveStepInfo getStepInfo(long _step)
        {
            AchieveStepInfo stepInfo = null;
            for (int i = 0; i < _m_stepInfoList.Count; i++)
            {
                if (_m_stepInfoList[i] == null)
                    continue;

                stepInfo = _m_stepInfoList[i];
                if (stepInfo.step == _step)
                {
                    return stepInfo;
                }
            }

            return null;
        }
    }
}