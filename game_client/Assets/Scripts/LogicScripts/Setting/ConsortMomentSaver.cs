using System;
using ALPackage;
using System.Text;
using UnityEngine;
using LitJson;
using System.Collections.Generic;
using System.Linq;
using CommonEnum;
using JetBrains.Annotations;
using Random = UnityEngine.Random;


namespace GOE
{
    [Serializable]
    public class ConsortChatCommentData
    {
        public long commentTimeTag;
        public long senderId;
        public string content;
        public long replyTarget;

        // 用于json序列化
        public ConsortChatCommentData()
        {
            
        }

        public ConsortChatCommentData(long _commentTimeTag, long _senderId, string _content, long _replyTarget)
        {
            commentTimeTag = _commentTimeTag;
            senderId = _senderId;
            content = _content;
            replyTarget = _replyTarget;
        }
        
        private static string getName(long _id)
        {
            if (_id == NPPlayer.instance.playerInfo.CID)
            {
                return TextTranslate.instance.getLanguage(TransKeyConst.consort_chat_moment_player_high_light,
                    NPPlayer.instance.playerInfo.PlayerName);
            }
            else
            {
                return NPPlayer.instance.consortComp.getConsortInfo(_id)?.consortTransName;
            }
        }
    
        public string commentSenderName()
        {
            return getName(senderId);
        }

        public string getContent()
        {
            if (replyTarget <= 0)
                return content;
            return TextTranslate.instance.getLanguage(TransKeyConst.consort_chat_moment_reply,
                getName(replyTarget), content);
        }
    }
    [Serializable]
    public class MomentImagePositionData
    {
        public float normalizeScale = 0.5f;
        public float normalizePosX = 0.5f;
        public float normalizePosY = 0.5f;
        public MomentImagePositionData()
        {
            normalizeScale = Random.value;
            normalizePosX = Random.value;
            normalizePosY = Random.value;
        }
    }
    [Serializable]
    public class ConsortMomentImageData
    {
        public EConsortChatShotType shotType = EConsortChatShotType.MidShot;
        public long bgImgId;
        public long actorImgId;
        public MomentImagePositionData bgPos;
        public MomentImagePositionData actorPos;

        public ConsortMomentImageData(EConsortChatShotType _shotType, long _bgImgId, long _actorImgId)
        {
            shotType = _shotType;
            bgImgId = _bgImgId;
            actorImgId = _actorImgId;
            bgPos = new MomentImagePositionData();
            actorPos = new MomentImagePositionData();
        }
    }
    //存储的消息结构
    [Serializable]
    public class ConsortMomentSaverData
    {
        public long momentInstanceId; // 朋友圈唯一id，同时也是朋友圈的创建时间戳
        public long consortId;
        public long bgGroupId;
        public bool hasGetContent;
        public string content;
        public bool hadComment = false;
        [NotNull]public List<long> likeConsortList = new List<long>();
        [NotNull]public List<ConsortChatCommentData> commentList = new List<ConsortChatCommentData>();
        public List<ConsortMomentImageData> imageList = new List<ConsortMomentImageData>();
        public long playerCommentTimeTag;
        [NotNull]public List<long> commentConsortList = new List<long>();

        public ConsortMomentSaverData(long _momentInstanceId)
        {
            momentInstanceId = _momentInstanceId;
            hadComment = false;
            hasGetContent = false;
        }

        internal void setData(long _consortId, long _bgGroupId, List<ConsortMomentImageData> _imageList)
        {
            consortId = _consortId;
            bgGroupId = _bgGroupId;
            if (imageList != null && _imageList != null) 
                imageList.AddRange(_imageList);
        }

        internal void updateContent(string _content)
        {
            content = _content;
            hasGetContent = true;
        }

        /// <summary>
        /// 更改点赞状态
        /// </summary>
        /// <param name="_id"></param>
        internal void changeMomentLike(long _id)
        {
            if (likeConsortList.Contains(_id))
                likeConsortList.Remove(_id);
            else
                likeConsortList.Add(_id);
        }
        

        internal bool isPlayerLike()
        {
            return likeConsortList.Contains(NPPlayer.instance.playerInfo.CID);
        }

        internal bool isComment()
        {
            return hadComment;
        }
        internal void addConsortMoment(ConsortChatCommentData _comment)
        {
            commentList.Add(_comment);
        }

        internal void addPlayerComment(ConsortChatCommentData _comment)
        {
            commentList.Add(_comment);
            hadComment = true;
            playerCommentTimeTag = _comment.commentTimeTag;
        }
        internal void addConsortComment(long _consortId)
        {
            commentConsortList.Add(_consortId);
        }
    }
    /// <summary>
    /// 单条朋友圈本地保存
    /// </summary>
    public class ConsortMomentSaver : _AALBasicSettingInfo
    {
        [JetBrains.Annotations.NotNull] private ConsortMomentSaverData _m_momentData;
        private readonly long _m_accountCID;
        private readonly long _m_momentInstanceId;
        
        public Action onMomentDataChange;
        public long tempCommentConsortId;

        public ConsortMomentSaverData momentData => _m_momentData;
        
        public List<ConsortChatCommentData> commentList => _m_momentData.commentList;
        public long momentInstanceId => _m_momentData.momentInstanceId;
        public long consortId => _m_momentData.consortId;
        public bool hasGetContent => _m_momentData.hasGetContent;

        public ConsortMomentSaver(long _accountCID, long _momentInstanceId)
            : base($"{_accountCID}_cache_account_consort_moments_history_{_momentInstanceId}")
        {
            _m_accountCID = _accountCID;
            _m_momentInstanceId = _momentInstanceId;
            _m_momentData = new ConsortMomentSaverData(_momentInstanceId);
        }


        /*************
        * 构建需要保存的字符串
         * 
        **/
        protected override string _makeSettingStr()
        {
            StringBuilder sb = new StringBuilder();
            int listCount = 0;

            sb.Append(JsonUtility.ToJson(_m_momentData));
            return sb.ToString();
        }

        /**************
        * 读取保存的字符串
        **/
        protected override void _initSettingStr(string _infoStr)
        {
            if (string.IsNullOrEmpty(_infoStr))
                return;

            try
            {
                _m_momentData = JsonUtility.FromJson<ConsortMomentSaverData>(_infoStr);
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError($"私聊相关本地保存反序列化失败:{_infoStr};/n Exception:{e}");
#endif
            }
        }

        /// <summary>
        /// 设置朋友圈数据
        /// </summary>
        /// <param name="_momentRefId"></param>
        /// <param name="_consortId"></param>
        public void setData(long _consortId, long _bgGroupId, List<ConsortMomentImageData> _imageList)
        {
            _m_momentData.setData(_consortId, _bgGroupId, _imageList);
            saveSetting();
        }

        public void updateContent(string _content)
        {
            _m_momentData.updateContent(_content);
            saveSetting();
        }
        
        /// <summary>
        /// 更改玩家点赞状态
        /// </summary>
        /// <param name="_id"></param>
        public void changeMomentPlayerLike()
        {
            _m_momentData.changeMomentLike(NPPlayer.instance.playerInfo.CID);
            onMomentDataChange?.Invoke();
            saveSetting();
        }
        
        /// <summary>
        /// 更改妃子点赞状态
        /// </summary>
        /// <param name="_consortId"></param>
        public void changeMomentConsortLike(long _consortId)
        {
            _m_momentData.changeMomentLike(_consortId);
            saveSetting();
        }
        
        /// <summary>
        /// 玩家是否评论过
        /// </summary>
        /// <returns></returns>
        public bool isPlayerComment()
        {
            return _m_momentData.hadComment;
        }
        /// <summary>
        /// 玩家是否点赞
        /// </summary>
        /// <returns></returns>
        public bool isPlayerLike()
        {
            return _m_momentData.isPlayerLike();
        }

        /// <summary>
        /// 是否有人点赞包括妃子和玩家
        /// </summary>
        /// <returns></returns>
        public bool isAnyLike()
        {
            return _m_momentData.likeConsortList.Count > 0;
        }
        public void consortComment(ConsortChatCommentData _comment)
        {
            _m_momentData.addConsortMoment(_comment);
            onMomentDataChange?.Invoke();
            saveSetting();
            
            // 有妃子评论后随机下一个AI评论
            AccountSettingMgr.instance.consortMomentSaverMgr.addRandomConsortComment(_m_momentInstanceId);
        }
        public void playerComment(ConsortChatCommentData _comment)
        {
            _m_momentData.addPlayerComment(_comment);
            onMomentDataChange?.Invoke();
            saveSetting();
        }

        /// <summary>
        /// 获取评论的其他妃子列表，数量可以用作其他妃子已评论次数判断
        /// </summary>
        /// <returns></returns>
        public List<long> getCommentOtherConsortRecordList()
        {
            return _m_momentData.commentConsortList;
        }

        public void addConsortCommentRecord(long _consortId)
        {
            _m_momentData.addConsortComment(_consortId);
        }

        public List<Common.Common_AiChatMessage> getReqAICommentMsgList()
        {
            List<Common.Common_AiChatMessage> msgList = new List<Common.Common_AiChatMessage>();
            
            Common.Common_AiChatMessage msg = new Common.Common_AiChatMessage(EAiChatRoleType.SYSTEM, _m_momentData.content);
            msgList.Add(msg);

            foreach (ConsortChatCommentData commentData in _m_momentData.commentList)
            {
                if(commentData == null) continue;
                msgList.Add(new Common.Common_AiChatMessage(
                    commentData.senderId == NPPlayer.instance.playerInfo.CID ? EAiChatRoleType.USER : EAiChatRoleType.SYSTEM,
                    commentData.content));
            }

            return msgList;
        }
    }
}