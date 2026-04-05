using CommonEnum;
using System;
using System.Collections.Generic;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        private void _initChild()
        {
            // 初始化一些辅助数据，抄服务端的
            childQualityCore.dealAllRef(_qualityRef =>
            {
                if (_qualityRef == null)
                    return;

                _qualityRef.addBonusStepList = new List<int>();
                
                //最大等级
                int maxLvl = _qualityRef.getMaxLvl();
                //计算需要增加基础收益的等级列表
                double perLvl = 1.0f * maxLvl * npGeneral.child_cal_unit_per / 10000f;
                if(perLvl <= 0 || perLvl >= maxLvl)
                    return;
                
                int curCount = 1;
                int curLvl = (int) Math.Ceiling(perLvl * curCount);
                while(curLvl < maxLvl)
                {
                    curLvl = (int) Math.Ceiling(perLvl * curCount);
                    _qualityRef.addBonusStepList.Add(curLvl);
    				
                    curCount++;
                }
                
            });
        }

        /// <summary>
        /// 根据学徒性别和配音类型获取可用的配音id列表
        /// </summary>
        /// <param name="_sex"></param>
        /// <param name="_voiceType"></param>
        /// <returns></returns>
        public List<long> getChildVoiceIdList(EChildSexType _sex, EChildVoiceType _voiceType)
        {
            List<long> voiceIdList = new List<long>();
            childVoiceGroupRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.sex == _sex && _ref.voice_type == _voiceType && _ref.voice_id_list != null &&
                    (_ref.unlock_condition == null || !_ref.unlock_condition.hasCondition || _ref.unlock_condition.IsEnable(null)))
                    voiceIdList.AddRange(_ref.voice_id_list);
            });
            return voiceIdList;
        }

        /// <summary>
        /// 根据学徒性别和配音类型获取可用的配音id列表
        /// </summary>
        /// <param name="_sex"></param>
        /// <param name="_voiceType"></param>
        /// <returns></returns>
        public void getChildVoiceIdList(EChildSexType _sex, EChildVoiceType _voiceType, List<long> _idList)
        {
            if(_idList == null)
                return;
            _idList.Clear();
            childVoiceGroupRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.sex == _sex && _ref.voice_type == _voiceType && _ref.voice_id_list != null &&
                    (_ref.unlock_condition == null || !_ref.unlock_condition.hasCondition || _ref.unlock_condition.IsEnable(null)))
                    _idList.AddRange(_ref.voice_id_list);
            });
        }
    }
}