using System;
using ALPackage;
using System.Text;
using UnityEngine;
using LitJson;
using System.Collections.Generic;
using Common;
using CommonEnum;
using JetBrains.Annotations;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


namespace GOE
{
    [Serializable]
    public class ConsortMomentNeedCommentData
    {
        public long momentInstanceId;
        public long playerCommentTimeTag;

        // 无参构造函数，用于JSON反序列化
        public ConsortMomentNeedCommentData()
        {
        }

        public ConsortMomentNeedCommentData(long _momentInstanceId,
        long _playerCommentTimeTag)
        {
            momentInstanceId = _momentInstanceId;
            playerCommentTimeTag = _playerCommentTimeTag;
        }
    }

    [Serializable]
    public class ConsortMomentConsortAICommentData
    {
        public long momentInstanceId;
        public ConsortChatCommentData consortCommentData;

        // 无参构造函数，用于JSON反序列化
        public ConsortMomentConsortAICommentData()
        {
        }
        
        public ConsortMomentConsortAICommentData(long _momentInstanceId, ConsortChatCommentData _consortCommentData)
        {
            momentInstanceId = _momentInstanceId;
            consortCommentData = _consortCommentData;
        }
    }

    /// <summary>
    /// 朋友圈记录的本地保存
    /// </summary>
    public class ConsortMomentSaverMgr : _AALBasicSettingInfo
    {
        // 朋友圈细节记录
        [NotNull] private readonly Dictionary<long, ConsortMomentSaver> _m_momentSaverDic;
        [NotNull] private readonly Dictionary<long, int> _m_consortMonentsCountDic;
        [NotNull] private readonly Dictionary<long, int> _m_bgGroupMomentsCountDic;

        [NotNull] private List<long> _m_momentInstanceIdList;// 已经保存的朋友圈实例ID列表
        [NotNull] private List<ConsortMomentConsortAICommentData> _m_pendingConsortAIComment; // 待展示的妃子AI评论列表（玩家评论后AI回复）
        [NotNull] private List<ConsortMomentConsortAICommentData> _m_unreadConsortAIComment; // 未读的妃子AI评论列表
        private bool _m_hasUnRead = false;// 有未读朋友圈
        
        private const char _m_fieldSplit = '&';
        private const char _m_dataSplit = '|';
        private readonly long _m_accountCID;

        public int momentCount => _m_momentInstanceIdList.Count;
        public int unReadConsortAICommentCount => _m_unreadConsortAIComment.Count;
        
        public ConsortMomentSaverMgr(long _accountCID)
            : base($"{_accountCID}_cache_account_consort_moments_history_list")
        {
            _m_momentInstanceIdList = new List<long>();
            _m_momentSaverDic = new Dictionary<long, ConsortMomentSaver>();
            _m_consortMonentsCountDic = new Dictionary<long, int>();
            _m_bgGroupMomentsCountDic = new Dictionary<long, int>();
            _m_pendingConsortAIComment = new List<ConsortMomentConsortAICommentData>();
            _m_unreadConsortAIComment = new List<ConsortMomentConsortAICommentData>();
            _m_accountCID = _accountCID;
        }
        
        /*************
        * 构建需要保存的字符串
         * 
        **/
        protected override string _makeSettingStr()
        {
            // 删除超过最大朋友圈数量的记录
            _checkOverMaxMoments();
            
            StringBuilder sb = new StringBuilder();
            int listCount = 0;

            foreach (long momentId in _m_momentInstanceIdList)
            {
                listCount++;
                sb.Append(momentId);
                if (listCount != _m_momentInstanceIdList.Count)
                    sb.Append(_m_dataSplit);
            }
            sb.Append(_m_fieldSplit);

            sb.Append(_m_hasUnRead);
            sb.Append(_m_fieldSplit);

            // 保存待展示的妃子AI评论列表 (使用JSON格式)
            string pendingCommentJsonStr = "";
            try
            {
                if (_m_pendingConsortAIComment.Count > 0)
                {
                    pendingCommentJsonStr = JsonMapper.ToJson(_m_pendingConsortAIComment);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"序列化待展示AI评论列表失败: {e.Message}");
            }
            sb.Append(pendingCommentJsonStr);
            sb.Append(_m_fieldSplit);

            // 保存朋友圈计数字典
            listCount = 0;
            foreach (var kvp in _m_consortMonentsCountDic)
            {
                listCount++;
                sb.Append(kvp.Key).Append(':').Append(kvp.Value);
                if (listCount != _m_consortMonentsCountDic.Count)
                    sb.Append(_m_dataSplit);
            }
            sb.Append(_m_fieldSplit);

            // 保存背景组计数字典
            listCount = 0;
            foreach (var kvp in _m_bgGroupMomentsCountDic)
            {
                listCount++;
                sb.Append(kvp.Key).Append(':').Append(kvp.Value);
                if (listCount != _m_bgGroupMomentsCountDic.Count)
                    sb.Append(_m_dataSplit);
            }
            sb.Append(_m_fieldSplit);

            // 保存未读妃子AI评论列表 (使用JSON格式)
            string jsonStr = "";
            try
            {
                if (_m_unreadConsortAIComment.Count > 0)
                {
                    jsonStr = JsonMapper.ToJson(_m_unreadConsortAIComment);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"序列化未读AI评论列表失败: {e.Message}");
            }
            sb.Append(jsonStr);
            
            return sb.ToString();
        }

        /**************
        * 读取保存的字符串
        **/
        protected override void _initSettingStr(string _infoStr)
        {
            if (string.IsNullOrEmpty(_infoStr))
                return;

            _m_momentInstanceIdList.Clear();
            _m_pendingConsortAIComment.Clear();
            _m_consortMonentsCountDic.Clear();
            _m_bgGroupMomentsCountDic.Clear();
            _m_unreadConsortAIComment.Clear();
            
            try
            {
                string[] fieldStrs = _infoStr.Split(_m_fieldSplit);
                if (fieldStrs.Length > 0)
                {
                    string[] dataStrs = fieldStrs[0].Split(_m_dataSplit);
                    foreach (string value in dataStrs)
                    {
                        if(string.IsNullOrEmpty(value))
                            continue;
                        long momentId = ALCommon.ParseLong(value);
                        if(momentId == 0)
                            continue;
                        _m_momentInstanceIdList.Add(momentId);
                    }
                }

                if (fieldStrs.Length > 1)
                {
                    bool.TryParse(fieldStrs[1], out _m_hasUnRead);
                }

                // 加载待展示的妃子AI评论列表 (使用JSON格式)
                if (fieldStrs.Length > 2 && !string.IsNullOrEmpty(fieldStrs[2]))
                {
                    try
                    {
                        // 设置LitJson的类型转换，允许int到long的自动转换
                        JsonMapper.RegisterImporter<int, long>((input) => input);
                        
                        List<ConsortMomentConsortAICommentData> loadedList = JsonMapper.ToObject<List<ConsortMomentConsortAICommentData>>(fieldStrs[2]);
                        if (loadedList != null)
                        {
                            _m_pendingConsortAIComment.AddRange(loadedList);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"解析待展示AI评论列表失败: {ex.Message}, JSON内容: {fieldStrs[2]}");
                    }
                }

                // 加载朋友圈计数字典
                if (fieldStrs.Length > 3 && !string.IsNullOrEmpty(fieldStrs[3]))
                {
                    string[] dataStrs = fieldStrs[3].Split(_m_dataSplit);
                    foreach (string value in dataStrs)
                    {
                        string[] keyValuePair = value.Split(':');
                        if (keyValuePair.Length == 2)
                        {
                            long consortId = ALCommon.ParseLong(keyValuePair[0]);
                            int count = ALCommon.ParseInt(keyValuePair[1]);
                            if (consortId != 0)
                            {
                                _m_consortMonentsCountDic[consortId] = count;
                            }
                        }
                    }
                }

                // 加载背景组计数字典
                if (fieldStrs.Length > 4 && !string.IsNullOrEmpty(fieldStrs[4]))
                {
                    string[] dataStrs = fieldStrs[4].Split(_m_dataSplit);
                    foreach (string value in dataStrs)
                    {
                        string[] keyValuePair = value.Split(':');
                        if (keyValuePair.Length == 2)
                        {
                            long bgGroupId = ALCommon.ParseLong(keyValuePair[0]);
                            int count = ALCommon.ParseInt(keyValuePair[1]);
                            if (bgGroupId != 0)
                            {
                                _m_bgGroupMomentsCountDic[bgGroupId] = count;
                            }
                        }
                    }
                }

                // 加载未读妃子AI评论列表 (使用JSON格式)
                if (fieldStrs.Length > 5 && !string.IsNullOrEmpty(fieldStrs[5]))
                {
                    try
                    {
                        // 设置LitJson的类型转换，允许int到long的自动转换
                        JsonMapper.RegisterImporter<int, long>((input) => input);
                        
                        List<ConsortMomentConsortAICommentData> loadedList = JsonMapper.ToObject<List<ConsortMomentConsortAICommentData>>(fieldStrs[5]);
                        if (loadedList != null)
                        {
                            _m_unreadConsortAIComment.AddRange(loadedList);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"解析未读AI评论列表失败: {ex.Message}, JSON内容: {fieldStrs[5]}");
                    }
                }

                // 更新红点状态
                _updateRedTip();

            }
            catch (Exception e)
            {
                Debug.LogError("HistorySaver init has Exception.   _infoStr:   [" + _infoStr + "]\t\tException:   " + e);
                _m_momentInstanceIdList.Clear();
                _m_pendingConsortAIComment.Clear();
                _m_consortMonentsCountDic.Clear();
                _m_bgGroupMomentsCountDic.Clear();
                _m_unreadConsortAIComment.Clear();
            }

        }
        
        private void _checkOverMaxMoments()
        {
            // 如果超过了最大朋友圈数量，则删除最早的一个
            int overCount = _m_momentInstanceIdList.Count - 100;
            if (overCount > 0)
            {
                for (int i = 0; i < overCount; i++)
                {
                    long removeMomentId = _m_momentInstanceIdList[0];
                    _m_momentInstanceIdList.RemoveAt(0);
                    
                    // 获取要删除的朋友圈数据并更新计数字典
                    if (_m_momentSaverDic.TryGetValue(removeMomentId, out ConsortMomentSaver removeSaver) && 
                        removeSaver.momentData != null)
                    {
                        long consortId = removeSaver.consortId;
                        // 更新朋友圈计数
                        if (_m_consortMonentsCountDic.ContainsKey(consortId))
                        {
                            _m_consortMonentsCountDic[consortId] = Math.Max(0, _m_consortMonentsCountDic[consortId] - 1);
                        }
                        
                        // 更新背景组计数 - 直接使用momentData中的bgGroupId
                        long bgGroupId = removeSaver.momentData.bgGroupId;
                        if (bgGroupId != 0 && _m_bgGroupMomentsCountDic.ContainsKey(bgGroupId))
                        {
                            _m_bgGroupMomentsCountDic[bgGroupId] = Math.Max(0, _m_bgGroupMomentsCountDic[bgGroupId] - 1);
                        }
                    }
                    
                    _m_momentSaverDic.Remove(removeMomentId);
                    
                    // 同时从待展示AI评论列表中移除
                    _m_pendingConsortAIComment.RemoveAll(data => data == null || data.momentInstanceId == removeMomentId);
                    
                    // 同时从未读AI评论列表中移除
                    _m_unreadConsortAIComment.RemoveAll(data => data == null || data.momentInstanceId == removeMomentId);
                    
                    ConsortMomentSaver saver = new ConsortMomentSaver(NPPlayer.instance.playerInfo.CID, removeMomentId);
                    saver.delete();
                }               
            }
        }

        private void _updateRedTip()
        {
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_CONSORT_CHAT_MOMENT_PAGE, _m_hasUnRead || _m_unreadConsortAIComment.Count > 0 ? 1 : 0);
        }

        public int todayMomentCount()
        {
            int count = 0;
            DateTime dateTime1 = TimeUtil.FromUTCMilliseconds(FpsAndPingMgr.instance.serverTimeTag);
            DateTime todayDayTime = new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day, 0, 0, 0, DateTimeKind.Utc);
            long today = TimeUtil.dateTime2Milliseconds(todayDayTime);
            foreach (long momentInstanceId in _m_momentInstanceIdList)
            {
                if (momentInstanceId >= today)
                {
                    count++;
                }
            }
            return count;
        }

        public int targetMomentInstanceDayMomentCount(long _momentInstanceId)
        {
            int count = 0;
            DateTime dateTime1 = TimeUtil.FromUTCMilliseconds(_momentInstanceId);
            DateTime targetDayStartTime = new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day, 0, 0, 0, DateTimeKind.Utc);
            DateTime targetDayEndTime = targetDayStartTime.AddDays(1);
            long start = TimeUtil.dateTime2Milliseconds(targetDayStartTime);
            long end = TimeUtil.dateTime2Milliseconds(targetDayEndTime);
            foreach (long momentInstanceId in _m_momentInstanceIdList)
            {
                if (momentInstanceId >= start && momentInstanceId < end)
                {
                    count++;
                }
            }
            return count;
        }

        public void setRead()
        {
            _m_hasUnRead = false;
            _updateRedTip();
            saveSetting();
        }

        /// <summary>
        /// 添加玩家评论
        /// </summary>
        /// <param name="_momentInstanceId"></param>
        /// <param name="_content"></param>
        public void addPlayerComment(long _momentInstanceId, string _content)
        {
            var momentSaver = getMomentSaver(_momentInstanceId);
            if (momentSaver != null)
            {
                ConsortChatCommentData comment = new ConsortChatCommentData(FpsAndPingMgr.instance.serverTimeTag, NPPlayer.instance.playerInfo.CID, _content, 0);
                momentSaver.playerComment(comment);
                
                momentSaver.tempCommentConsortId = momentSaver.consortId;
                // 立即请求AI回复
                NPPlayer.instance.consortChatComp.reqConsortAiChatMoment(momentSaver.consortId, _momentInstanceId, momentSaver.getReqAICommentMsgList(), false);
            }
        }

        public void addRandomConsortComment(long _momentInstanceId)
        {
            List<long> consortList = NPPlayer.instance.consortChatComp.getChatConsortList();
            ConsortMomentSaver momentSaver = getMomentSaver(_momentInstanceId);
            if(momentSaver == null || consortList == null || consortList.Count <= 1)
                return;
            Random.State originalState = Random.state; // 保存当前状态
            Random.InitState((int)_momentInstanceId); // 使用固定种子
            int replyCount = GRefdataCoreMgr.instance.npGeneral.consort_chat_moment_consort_ai_reply_count.getRandomValue();
            int replyAfterCount = GRefdataCoreMgr.instance.npGeneral.consort_chat_moment_consort_ai_reply_after_player_count.getRandomValue();
            // 添加玩家评论后，回复数量增加
            if (momentSaver.isPlayerComment())
                replyCount += replyAfterCount;
            Random.state = originalState; // 恢复之前的随机状态
            consortList.Remove(momentSaver.consortId);
            List<long> hasCommentList = momentSaver.getCommentOtherConsortRecordList();

          
            if(hasCommentList == null || replyCount <= hasCommentList.Count)
                return;
            foreach (var consortId in hasCommentList)
            {
                consortList.Remove(consortId);
            }

            if (consortList.Count > 0)
            {
                long consortId = consortList.GetRandomItem();
                momentSaver.tempCommentConsortId = consortId;
                momentSaver.addConsortCommentRecord(consortId);
                
                List<Common_AiChatMessage> msgList = new List<Common_AiChatMessage>();
                msgList.Add(new Common_AiChatMessage(CommonEnum.EAiChatRoleType.USER, 
                    TextTranslate.instance.getLanguage(TransKeyConst.consort_chat_moment_ai_comment_prompt_str_str, momentSaver.momentData.content)));
                NPPlayer.instance.consortChatComp.reqConsortEvaluateReply(consortId, _momentInstanceId, msgList);
            }
        }

        /// <summary>
        /// 检查并展示待处理的AI评论回复
        /// </summary>
        public void checkCommentReply()
        {
            if(_m_pendingConsortAIComment.Count <= 0)
                return;

            bool hasAddComment = false;
            // 遍历所有待展示的AI评论，如果服务器时间大于评论时间，则添加评论
            for (var i = _m_pendingConsortAIComment.Count - 1; i >= 0; i--)
            {
                var commentData = _m_pendingConsortAIComment[i];
             
                if (commentData == null || commentData.consortCommentData == null)
                {
                    _m_pendingConsortAIComment.RemoveAt(i);
                    continue;
                }

                if (FpsAndPingMgr.instance.serverTimeTag > commentData.consortCommentData.commentTimeTag)
                {
                    _m_pendingConsortAIComment.RemoveAt(i);
                    if(commentData.consortCommentData.replyTarget == NPPlayer.instance.playerInfo.CID)
                        _m_unreadConsortAIComment.Add(commentData);
                    ConsortMomentSaver momentSaver = getMomentSaver(commentData.momentInstanceId);
                    if (momentSaver != null) 
                        momentSaver.consortComment(commentData.consortCommentData);
                    hasAddComment = true;
                }
            }

            if (hasAddComment)
            {
                // 更新红点和UI
                _updateRedTip();
                WinMsg.SendMsg(WinMsgType.ON_CONSORT_CHAT_MOMENTS_CHG);
            
                // 保存状态
                saveSetting();
            }
        }

        public ConsortMomentSaver getMomentSaver(long _momentInstanceId)
        {
            _m_momentSaverDic.TryGetValue(_momentInstanceId, out ConsortMomentSaver saver);

            if(saver == null)
            {
                saver = new ConsortMomentSaver(NPPlayer.instance.playerInfo.CID, _momentInstanceId);
                saver.init();
                _m_momentSaverDic.Add(_momentInstanceId, saver);
            }

            return saver;
        }
        
        public List<ConsortMomentConsortAICommentData> getUnreadConsortAICommentList()
        {
            List<ConsortMomentConsortAICommentData> list = new List<ConsortMomentConsortAICommentData>();
            foreach (var data in _m_unreadConsortAIComment)
            {
                list.Add(data);
            }
            
            _m_unreadConsortAIComment.Clear();
            _updateRedTip();
            WinMsg.SendMsg(WinMsgType.ON_CONSORT_CHAT_MOMENTS_CHG);
            return list;
        }


        public long lastMomentInstanceId()
        {
            if(_m_momentInstanceIdList.Count == 0)
                return 0;
            return _m_momentInstanceIdList.GetLast();
        }

        private int _getConsortMomentsCount(long _consortId)
        {
            if (_m_consortMonentsCountDic.TryGetValue(_consortId, out int count))
            {
                return count;
            }
            else
            {
                _m_consortMonentsCountDic[_consortId] = 0;
                return 0;
            }
        }

        private long _randomConsort(List<long> _consortIdList)
        {
            List<int> weights = new List<int>();

            int maxCount = 1;
            int totalCount = 0;
            foreach (var consortId in _consortIdList)
            {
                int momentsCount = _getConsortMomentsCount(consortId);
                weights.Add(momentsCount);
                if(maxCount < momentsCount)
                    maxCount = momentsCount;
                totalCount += momentsCount;
            }

            totalCount = maxCount * _consortIdList.Count - totalCount;
            
            // 如果totalCount为0（所有联盟出现次数相同），则平均随机选择
            if (totalCount <= 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, _consortIdList.Count);
                return _consortIdList[randomIndex];
            }
            
            int rand = UnityEngine.Random.Range(0, totalCount);
            int sum = 0;
            for (int i = 0; i < weights.Count; i++)
            {
                sum += (maxCount - weights[i]);
                if (rand < sum) return _consortIdList[i];
            }

            return 0;
        }
        private int _getBgMomentsCount(long _bgGroupId)
        {
            if (_m_bgGroupMomentsCountDic.TryGetValue(_bgGroupId, out int count))
            {
                return count;
            }
            else
            {
                _m_bgGroupMomentsCountDic[_bgGroupId] = 0;
                return 0;
            }
        }
        private long _randomBgGroup(List<long> _bgGroupIdList)
        {
            List<int> weights = new List<int>();
            int maxCount = 1;
            int totalCount = 0;
            foreach (var bgGroupId in _bgGroupIdList)
            {
                int momentsCount = _getBgMomentsCount(bgGroupId);
                weights.Add(momentsCount);
                if(maxCount < momentsCount)
                    maxCount = momentsCount;
                totalCount += momentsCount;
            }
            totalCount = maxCount * _bgGroupIdList.Count - totalCount;

            // 如果totalCount为0（所有背景组出现次数相同），则平均随机选择
            if (totalCount <= 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, _bgGroupIdList.Count);
                return _bgGroupIdList[randomIndex];
            }

            int rand = UnityEngine.Random.Range(0, totalCount);
            int sum = 0;
            for (int i = 0; i < weights.Count; i++)
            {
                sum += (maxCount - weights[i]);
                if (rand < sum) return _bgGroupIdList[i];
            }
            return 0;
        }
        
        
        /// <summary>
        /// 生成一个新的朋友圈
        /// </summary>
        /// <param name="_consortIdList"></param>
        /// <returns></returns>
        public ConsortMomentSaver getAndSaveNewMoments(long _momentInstanceId, List<long> _consortIdList)
        {
            if(_consortIdList == null || _consortIdList.Count == 0)
            {
                return null; // 没有家人ID，返回0
            }

            List<long> hasMomentsConsort = new List<long>();
            foreach (var consort in _consortIdList)
            {
                if (GRefdataCoreMgr.instance.getConsortMomentRefCount(consort) > 0)
                    hasMomentsConsort.Add(consort);
            }
            if(hasMomentsConsort.Count == 0)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("一个已添加好友的妃子朋友圈配置都没有，无法生成朋友圈");
#endif
                return null; // 没有家人ID，返回0
            }
            long consortId = _randomConsort(hasMomentsConsort);
            List<ConsortChatMomentsRefObj> momentRefList = GRefdataCoreMgr.instance.getConsortMomentRefList(consortId);
          
            ConsortChatMomentsRefObj refObj = momentRefList.GetRandomItem();
            
            long momentInstanceId = _momentInstanceId;
            if (refObj == null || momentInstanceId == 0 || consortId == 0)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError($"妃子朋友圈生成出错： {(refObj == null ? "ConsortChatMomentsRefObj未找到":"")},{(consortId == 0 ? $"没有随机到妃子":"")}");
#endif
                return null;
            }
            long bgGroup = _randomBgGroup(refObj.bg_img_group_id_list);
            long consortGroup = refObj.consort_img_group_id_list.GetRandomItem();
            
            ConsortMomentsBgGroupRefObj bgGroupRef = GRefdataCoreMgr.instance.consortMomentsBgGroupRefCore.getRef(bgGroup);
            ConsortMomentsConsortGroupRefObj consortGroupRef = GRefdataCoreMgr.instance.consortMomentsConsortGroupRefCore.getRef(consortGroup);
            if (bgGroupRef == null || consortGroupRef == null)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError($"妃子朋友圈配置不存在 ,{(bgGroupRef == null ? $"bgGroup:{bgGroup}未找到":"")}， {(consortGroupRef == null ? $"ConsortGroup:{consortGroup},未找到":"")}");
#endif
                return null;
            }
            if(!_m_momentInstanceIdList.Contains(momentInstanceId))
                _m_momentInstanceIdList.Add(momentInstanceId);
          
            ConsortMomentSaver momentSaver = getMomentSaver(momentInstanceId);
            int count = GRefdataCoreMgr.instance.npGeneral.consort_chat_moment_image_random_count.getRandomValue();
            List<ConsortMomentImageData> imageDataList = new List<ConsortMomentImageData>();

            List<EConsortChatShotType> shotTypeList = new List<EConsortChatShotType>();
            _refreshShotTypeList(ref shotTypeList);
            List<long> bgImgIdList = new List<long>();
            List<long> actorImgIdList = new List<long>();
            bgImgIdList.AddRange(bgGroupRef.bg_img_id_list);
            actorImgIdList.AddRange(consortGroupRef.consort_img_id_list);
            
            for (int i = 0; i < count; i++)
            {
                if(shotTypeList.Count <= 0)
                    _refreshShotTypeList(ref shotTypeList);
                var type = shotTypeList.GetRandomItemAndRemove();
                if(bgImgIdList.Count <=0)
                    bgImgIdList.AddRange(bgGroupRef.bg_img_id_list);
                if(actorImgIdList.Count <=0)
                    actorImgIdList.AddRange(consortGroupRef.consort_img_id_list);
                
                long actorId = actorImgIdList.GetRandomItemAndRemove();
                long bgId = bgImgIdList.GetRandomItemAndRemove();
                imageDataList.Add(new ConsortMomentImageData(type, bgId, actorId));
            }
            momentSaver?.setData(consortId, bgGroup, imageDataList);
            
            // 更新计数字典
            _m_consortMonentsCountDic[consortId] = _getConsortMomentsCount(consortId) + 1;
            _m_bgGroupMomentsCountDic[bgGroup] = _getBgMomentsCount(bgGroup) + 1;
            

            string bgPrompt = TextTranslate.instance.getLanguage(bgGroupRef.bg_prompt_key);
            string consortPrompt = TextTranslate.instance.getLanguage(consortGroupRef.consort_prompt_key);
            string msgContent = TextTranslate.instance.getLanguage(TransKeyConst.consort_chat_moment_content_ai_req_prompt_str_str, bgPrompt, consortPrompt);
            Common.Common_AiChatMessage msg = new Common.Common_AiChatMessage(EAiChatRoleType.USER, msgContent);
            
            List<Common.Common_AiChatMessage> msgList = new List<Common.Common_AiChatMessage>();
            msgList.Add(msg);
            NPPlayer.instance.consortChatComp.reqConsortAIMomentContent(consortId, momentInstanceId, msgList);
          
            saveSetting();
            return momentSaver;
        }

        public ConsortMomentSaver onMomentContentAdd(long _momentInstanceId, string _content)
        {
            ConsortMomentSaver momentSaver = getMomentSaver(_momentInstanceId);

            momentSaver?.updateContent(_content);
            // 标记有未读朋友圈
            _m_hasUnRead = true;
            _updateRedTip();
            saveSetting();
            return momentSaver;
        }

        public void addConsortAIComment(long _momentInstanceId, long _consortId, string _content, int _errorCode)
        {
            ConsortMomentSaver momentSaver = AccountSettingMgr.instance.consortMomentSaverMgr.getMomentSaver(_momentInstanceId);
           
            if(_errorCode != 0)
                _content = TextTranslate.instance.getLanguage(GRefdataCoreMgr.instance.npGeneral.consort_chat_ai_moment_error_reply_list.GetRandomItem());

            if (momentSaver != null)
            {
                long playerCommentTimeTag = momentSaver.momentData.playerCommentTimeTag;
                long consortReplyTimeTag = playerCommentTimeTag + GRefdataCoreMgr.instance.npGeneral.consort_chat_moment_reply_delay_random_time.getRandomValue() * 1000;
                ConsortChatCommentData comment = new ConsortChatCommentData(consortReplyTimeTag, _consortId, _content, momentSaver.isPlayerComment() ? NPPlayer.instance.playerInfo.CID : 0);

                // 判断是否可以显示ai回复
                if (FpsAndPingMgr.instance.serverTimeTag < consortReplyTimeTag)
                {
                    // 玩家评论后的AI回复，存储到待展示列表
                    _m_pendingConsortAIComment.Add(new ConsortMomentConsortAICommentData(_momentInstanceId, comment));
                }
                else
                {
                    momentSaver.consortComment(comment);

                    // 主动生成的AI评论，直接添加到未读列表
                    if(comment.replyTarget == NPPlayer.instance.playerInfo.CID)
                        _m_unreadConsortAIComment.Add(new ConsortMomentConsortAICommentData(_momentInstanceId, comment));
                    _updateRedTip();
                    WinMsg.SendMsg(WinMsgType.ON_CONSORT_CHAT_MOMENTS_CHG);
                }
                
                saveSetting();
            }
        }
        
        public void getMomentsListBefore(long _momentInstanceId, int _msgCount, Action<List<ConsortMomentSaver>> _action)
        {
            List<ConsortMomentSaver> countInfoList = new List<ConsortMomentSaver>();
            
            ConsortMomentSaver momentSaver;
            int count = 0;
            for (int i = _m_momentInstanceIdList.Count - 1; i >= 0; i--)
            {
                momentSaver = getMomentSaver(_m_momentInstanceIdList[i]);
                if (momentSaver == null || momentSaver.momentData == null || !momentSaver.momentData.hasGetContent)
                    continue;
                if(momentSaver.momentData.momentInstanceId >= _momentInstanceId && _momentInstanceId > -1)
                    continue;
                countInfoList.Insert(0, momentSaver);
                count++;
                if (count == _msgCount)
                    break;
            }
            
            _action?.Invoke(countInfoList);
        }

        private static void _refreshShotTypeList([NotNull]ref List<EConsortChatShotType> _shotTypeList)
        {
            _shotTypeList.Clear();
            _shotTypeList.Add(EConsortChatShotType.LongShot);
            _shotTypeList.Add(EConsortChatShotType.MidShot);
            _shotTypeList.Add(EConsortChatShotType.ShortShot);
            _shotTypeList.Add(EConsortChatShotType.EmptyShot);
        }
    }
}