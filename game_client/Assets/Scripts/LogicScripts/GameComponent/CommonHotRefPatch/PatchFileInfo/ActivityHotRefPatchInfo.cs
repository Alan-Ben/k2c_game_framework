namespace GOE
{
    /// <summary>
    /// 活动的热更补丁配表，一个活动包含一堆表格文件
    /// </summary>
    public class ActivityHotRefPatchInfo :_ACommonHotRefPatchInfo
    {
        //活动实例ID
        private long _m_activityInstanceId;
        //活动ID
        private long _m_activityId;
        
        public ActivityHotRefPatchInfo(long _activityInstanceId, long _activityId, string _fileName, string _fileMd5, string _filePath) : base(_fileName, _fileMd5, _filePath)
        {
            _m_activityInstanceId = _activityInstanceId;
            _m_activityId = _activityId;
        }

        public long activityInstanceId
        {
            get { return _m_activityInstanceId; }
        }

        public long activityId
        {
            get { return _m_activityId; }
        }

        protected override bool _checkCanPatch()
        {
            //活动未开启不需要打补丁
            _ABaseActivityInfo aBaseActivityInfo = NPPlayer.instance.commonActivityComp.getActivityInfoByInstanceId(_m_activityInstanceId);
            if(aBaseActivityInfo == null || !aBaseActivityInfo.isEnable)
            {
                return false;
            }
            
            return true;
        }
    }
}