using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 点击下一步触发器
    /// </summary>
    public class ClickNextStepTriggerMgr : _ANextStepTriggerMgr
    {
        [NotNull] public Dictionary<GameObject, List<NextStepTrigger>> _m_dBtnTriggerDic = new Dictionary<GameObject, List<NextStepTrigger>>();

        public ClickNextStepTriggerMgr(Action _onTrigger) : base(_onTrigger)
        {
        }
        
        public override ENextStepTriggerType triggerType { get { return ENextStepTriggerType.CLICK; } }
        protected override void _onStartMonitor()
        {
            foreach (var go in _m_dBtnTriggerDic.Keys)
            {
                ALUGUICommon.combineBtnClick(go, _onTriggerBtnClick);//注册点击事件
            }
        }

        protected override void _onReset()
        {
            foreach (var go in _m_dBtnTriggerDic.Keys)
            {
                ALUGUICommon.uncombineBtnClick(go, _onTriggerBtnClick);//反注册点击事件
            }
            
            _m_dBtnTriggerDic.Clear();
        }

        protected override bool _regNextStepTrigger(NextStepTrigger _trigger)
        {
            // 由于在基类已经判断过触发器类型, 所以这里不再判断
            
            if(_trigger.btnTrigger == null)
                return false;

            List<NextStepTrigger> triggerList = null;
            if (!_m_dBtnTriggerDic.TryGetValue(_trigger.btnTrigger, out triggerList) || triggerList == null)
            {
                triggerList = new List<NextStepTrigger>();
                _m_dBtnTriggerDic[_trigger.btnTrigger] = triggerList;
            }
            
            triggerList.Add(_trigger);

            return true;
        }

        public override bool allTriggerDone()
        {
            if (!_m_bIsStartMonitor)
                return false;

            return _m_dBtnTriggerDic.Count <= 0;
        }

        protected override void _dealTrigger(NextStepTrigger _trigger)
        {
            if (_trigger != null)
            {
                if (!string.IsNullOrEmpty(_trigger.triggerFunc))
                {
                    //触发后执行效果
                    _NPPlayerEffectSerializeInfo effectSerializeInfo = _NPPlayerEffectSerializeInfo.ReadFromString(_trigger.triggerFunc);
                    effectSerializeInfo?.dealEffect();
                }
            }
            
            // 这里重写基类dealTrigger方法, 去掉了_m_aOnTrigger调用, 因为改触发器触发以组为单位触发, 所以不需要单个触发器调用_m_aOnTrigger
        }

        /// <summary>
        /// 触发
        /// </summary>
        /// <param name="_btn"></param>
        private void _onTriggerBtnClick(GameObject _btn)
        {
            // 若触发器还未开始监听, 不执行
            if (!_m_bIsStartMonitor || _btn == null)
                return;

            List<NextStepTrigger> triggerList = null;
            if (!_m_dBtnTriggerDic.TryGetValue(_btn, out triggerList))
                return;

            // 反注销点击事件
            ALUGUICommon.uncombineBtnClick(_btn, _onTriggerBtnClick);
            _m_dBtnTriggerDic.Remove(_btn);//从字典中移除
            
            if (triggerList != null)
            {
                for (int i = triggerList.Count - 1; i >= 0; i--)
                {
                    NextStepTrigger trigger = triggerList[i];
                    _dealTrigger(trigger);
                }
                
                triggerList.Clear();
            }

            _m_aOnTrigger?.Invoke();
        }
    }
}