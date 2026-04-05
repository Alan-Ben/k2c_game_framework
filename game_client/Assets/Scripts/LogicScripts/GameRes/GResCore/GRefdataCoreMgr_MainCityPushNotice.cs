namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        /// <summary>
        /// 获取主城推送公告数据对象
        /// </summary>
        /// <param name="_noticeTag"></param>
        /// <returns></returns>
        public MainCityPushNoticeRefObj getMainCityPushNoticeRefObj(string _noticeTag)
        {
              return mainCityPushNoticeRefCore.refList.Find(_obj => _obj != null && _obj.notice_tag == _noticeTag);
        }
    }
}