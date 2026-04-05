using NPEnum;

namespace GOE
{
    //分享相关
    public partial class GRefdataCoreMgr
    {
        /// <summary>
        /// 初始化分享表相关信息
        /// </summary>
        private void _initShareRefCore()
        {
            NPSOCommonBoxRefObj boxRef = null;
            for (int i = 0; i < shareMap.refList.Count; i++)
            {
                boxRef = commonBoxMap.getRef(shareMap.refList[i].id);
                if (boxRef != null)
                    shareMap.refList[i].shareItemTypeRefObj = boxRef;
            }
        }

        /// <summary>
        /// 根据分享表id获取分享表相关的通用宝箱数据
        /// </summary>
        /// <param name="_shareRefId"></param>
        /// <returns></returns>
        public NPSOCommonBoxRefObj getCommonBoxRefByShareId(long _shareRefId)
        {
            NPSOCommonBoxRefObj boxRef = null;
            if (getShareRefSubDateType(_shareRefId) == ENPShareItemType.BOX)
                boxRef = (NPSOCommonBoxRefObj)shareMap.getRef(_shareRefId).shareItemTypeRefObj;
            return boxRef;
        }

        /// <summary>
        /// 根据分享表id获取子表类型
        /// </summary>
        /// <param name="_shareRefId"></param>
        /// <returns></returns>
        public ENPShareItemType getShareRefSubDateType(long _shareRefId)
        {
            NPSOShareRefObj shareRef = shareMap.getRef(_shareRefId);
            if (shareRef != null && shareRef.shareItemTypeRefObj != null)
                return shareRef.shareItemTypeRefObj.shareItemType;

            return ENPShareItemType.NONE;
        }
    }
}