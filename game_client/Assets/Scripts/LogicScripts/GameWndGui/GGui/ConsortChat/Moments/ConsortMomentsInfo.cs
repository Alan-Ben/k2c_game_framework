using System;
using System.Collections.Generic;

namespace GOE
{
    public class ConsortMomentsInfo : _AConsortChatInfo
    {
        public ConsortMomentsInfo() 
        {
            
        }

        /// <summary>
        /// 添加妃子朋友圈
        /// </summary>
        /// <param name="_consortIdList">已添加好友妃子列表</param>
        public void addMoment(long _momentInstanceId, List<long> _consortIdList)
        {
            ConsortMomentSaver saver = AccountSettingMgr.instance.consortMomentSaverMgr.getAndSaveNewMoments(_momentInstanceId, _consortIdList);
        
            if (saver != null)
            {
                // 添加妃子点赞
                int randomLikeCount = GRefdataCoreMgr.instance.npGeneral.consort_chat_moment_consort_like_random_count.getRandomValue();
                List<long> consortList = NPPlayer.instance.consortChatComp.getChatConsortList();
                for (int i = 0; i < randomLikeCount; i++)
                {
                    if (consortList == null || consortList.Count <= 0) break;
                    long consort = consortList.GetRandomItemAndRemove();
                    saver.changeMomentConsortLike(consort);
                }
            }
        }

        public void updateMomentContent(long _momentInstanceId, string _content, int _errorCode)
        {
            if (_errorCode != 0)
                _content = TextTranslate.instance.getLanguage(GRefdataCoreMgr.instance.npGeneral.consort_chat_ai_moment_content_error_list.GetRandomItem());
            ConsortMomentSaver saver = AccountSettingMgr.instance.consortMomentSaverMgr.onMomentContentAdd(_momentInstanceId, _content);
            ConsortChatMsgMomentInfo msg = new ConsortChatMsgMomentInfo(saver);
            receiveMsg(msg);

            AccountSettingMgr.instance.consortMomentSaverMgr.addRandomConsortComment(_momentInstanceId);
        }

        public void addConsortLike(long _momentInstanceId, long _consortId)
        {
            ConsortMomentSaver momentSaver = AccountSettingMgr.instance.consortMomentSaverMgr.getMomentSaver(_momentInstanceId);
            if (momentSaver != null) momentSaver.changeMomentConsortLike(_consortId);
        }

        /// <summary>
        /// 添加玩家评论
        /// </summary>
        /// <param name="_momentInstanceId"></param>
        /// <param name="_content"></param>
        public void addPlayerComment(long _momentInstanceId, string _content)
        {
            AccountSettingMgr.instance.consortMomentSaverMgr.addPlayerComment(_momentInstanceId, _content);
          
        }

        public int getMomentsCount()
        {
            return AccountSettingMgr.instance.consortMomentSaverMgr.momentCount;
        }
        /// <summary>
        /// 增加妃子评论
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_momentInstanceId"></param>
        /// <param name="_content"></param>
        /// <param name="_errorCode"></param>
        public void addConsortAIComment(long _momentInstanceId, long _consortId, string _content, int _errorCode)
        {
            AccountSettingMgr.instance.consortMomentSaverMgr.addConsortAIComment(_momentInstanceId, _consortId, _content, _errorCode);
        }
        protected override void _doLoadOp(Action<List<_AConsortChatMsgInfo>> _action)
        {
            _action?.Invoke(null);
        }
        
        public ConsortChatMsgMomentInfo getMomentMsg(long _momentInstanceId)
        {
            ConsortMomentSaver saver = AccountSettingMgr.instance.consortMomentSaverMgr.getMomentSaver(_momentInstanceId);
            if (saver == null)
                return null;
            
            return new ConsortChatMsgMomentInfo(saver);
        }
        
        /// <inheritdoc/>
        protected override void _getHistoryList(long _msgId, int _msgCount, Action<List<_AConsortChatMsgInfo>> _action)
        {
            if (_action == null)
                return;
            
            AccountSettingMgr.instance.consortMomentSaverMgr.getMomentsListBefore(_msgId, _msgCount, dataList =>
            {
                if (null == dataList)
                    return;

                List<_AConsortChatMsgInfo> infoList = new List<_AConsortChatMsgInfo>();
                if (dataList.Count > 0)
                {
                    for (int i = 0; i < dataList.Count; i++)
                    {
                        ConsortMomentSaver temp = dataList[i];
                        if (temp == null || temp.momentData == null)
                            continue;
                        ConsortChatMsgMomentInfo msg = new ConsortChatMsgMomentInfo(temp);
                        infoList.Add(msg);
                    }
                }
                
                _action(infoList);
            });
        }

    }
}