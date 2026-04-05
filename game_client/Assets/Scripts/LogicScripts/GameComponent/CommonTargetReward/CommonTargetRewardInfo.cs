namespace GOE
{
    public class CommonTargetRewardInfo
    {
        private CommonTargetRewardRefObj _m_targetRewardRefObj;
        //服务端计数
        private long _m_serverCount;
        //是否领奖
        private bool _m_hadDraw;
        
        public long id { get { return null == _m_targetRewardRefObj ? 0 : _m_targetRewardRefObj.id; } }
        public CommonTargetRewardRefObj targetRewardRefObj { get { return _m_targetRewardRefObj; } }

        public CommonTargetRewardInfo(CommonTargetRewardRefObj _refObj)
        {
            if(null == _refObj)
                return;
            
            _m_targetRewardRefObj = _refObj;
            _m_serverCount = 0;
            _m_hadDraw = false;
        }
        
        public CommonTargetRewardInfo(Common.CommonFuncObj.CommonFunc_TargetReward _info)
        {
            if(null == _info)
                return;
            
            _m_targetRewardRefObj = GRefdataCoreMgr.instance.commonTargetRewardRefCore.getRef(_info.getId());
            _m_serverCount = _info.getValue();
            _m_hadDraw = _info.getHadDraw();
        }

        public void update(Common.CommonFuncObj.CommonFunc_TargetReward _info)
        {
            if(null == _info)
                return;
            
            _m_targetRewardRefObj = GRefdataCoreMgr.instance.commonTargetRewardRefCore.getRef(_info.getId());
            _m_serverCount = _info.getValue();
            _m_hadDraw = _info.getHadDraw();
        }
        
        //获取当前数值
        public long getCurCount()
        {
            if (null == _m_targetRewardRefObj || null == _m_targetRewardRefObj.process_cur_count)
                return _m_serverCount;

            return _m_targetRewardRefObj.process_cur_count.CalculateVariableResult(null) + _m_serverCount;
        }
        
        //获取目标数值
        public long getTargetCount()
        {
            if (null == _m_targetRewardRefObj)
                return 0;

            return _m_targetRewardRefObj.process_count;
        }
        
        //获取获取状态
        public ECommonRewardType getRewardType()
        {
            if (null == _m_targetRewardRefObj)
                return ECommonRewardType.NONE;

            if (_m_hadDraw)
                return ECommonRewardType.HAS_GET_REWARD;

            if (getCurCount() >= _m_targetRewardRefObj.process_count)
                return ECommonRewardType.CAN_GET_REWARD;

            return ECommonRewardType.NOT_GET_REWARD;
        }

        //是否满足显示条件
        public bool isShowConditionMet()
        {
            if (null == _m_targetRewardRefObj || null == _m_targetRewardRefObj.show_condition)
                return true;

            return _m_targetRewardRefObj.show_condition.IsEnable(null);
        }

        //获取描述
        public string getDesc()
        {
            if (null == _m_targetRewardRefObj)
                return string.Empty;

            return TextTranslate.instance.getLanguage(_m_targetRewardRefObj.target_txt, getCurCount(), getTargetCount());
        }
    }
}