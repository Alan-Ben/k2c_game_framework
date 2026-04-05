using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using System.Collections.Generic;

namespace GOE
{
    public partial class SevenDayGoalsData
    {
        public class RedTipDealerNew
        {
            [NotNull] private readonly SevenDayGoalsData _m_data;
            //红点字典
            [NotNull] private Dictionary<string, CommonForceRedTipNode> _m_dMyNode = new Dictionary<string, CommonForceRedTipNode>(); // 为了根据自定义的索引取到对应节点的字典
            private static bool _m_bNeedSendNextFrame = false;

            public RedTipDealerNew([NotNull] SevenDayGoalsData _data)
            {
                _m_data = _data;
            }
            
            /// <summary>
            /// 初始化
            /// </summary>
            public void init()
            {
                clear();

                for (int i = 0; i < GRefdataCoreMgr.instance.getSevenDayGoalsMaxDay(); i++)
                {
                    _addNode(i+1, ESevenDayGoalsRedTipType.NEW);
                    _addNode(i+1, ESevenDayGoalsRedTipType.TASK);
                    _addNode(i+1, ESevenDayGoalsRedTipType.GIFT);
                }

                refreshAll();
            }

            /// <summary>
            /// 清除数据
            /// </summary>
            public void clear()
            {
                refreshAll();
                _m_dMyNode.Clear();
                _m_bNeedSendNextFrame = false;
            }

            /// <summary>
            /// 刷新全部
            /// </summary>
            public void refreshAll()
            {
                refrshAllNewDayRedTip();
                refrshAllTaskRedTip();
                refrshAllGiftRedTip();
                refreshStepRedTip();
                _sendRedTipChgMsg();
            }

            /// <summary>
            /// 刷新新解锁天数红点
            /// </summary>
            public void refrshAllNewDayRedTip()
            {
                if (NPPlayer.instance.playerInfo == null)
                    return;

                // 获取当前解锁天数
                long serverNowDay = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.SERVER_START_DAYS);
                for (int i = 0; i < GRefdataCoreMgr.instance.getSevenDayGoalsMaxDay(); i++)
                {
                    bool isNewDay = serverNowDay >= (i+1) && 
                                    _m_data.activityInfo != null && 
                                    _m_data.activityInfo.isPlaying && 
                                    AccountSettingMgr.instance.accountSetting.getSevenDayGoalDayIsNew(_m_data.activityInfo.instanceId, i + 1);
                    CommonForceRedTipNode node = _getNode(i + 1, ESevenDayGoalsRedTipType.NEW);
                    node?.setCount(isNewDay ? 1 : 0);
                }

                _sendRedTipChgMsg();
            }

            /// <summary>
            /// 刷新所有任务红点
            /// </summary>
            public void refrshAllTaskRedTip()
            {
                for (int i = 0; i < _m_data._m_taskInfoList.Count; i++)
                {
                    int day = i + 1;
                    refreshTaskRedTipByDay(day);
                }
            }

            /// <summary>
            /// 根据任务ID刷新任务红点
            /// </summary>
            /// <param name="_taskId"></param>
            public void refreshTaskRedTipByTaskId(long _taskId)
            {
                if (!_m_data._m_taskInfoDic.TryGetValue(_taskId, out SevenDayGoalsTaskInfo taskInfo) || taskInfo == null)
                    return;

                int day = taskInfo.refObj.day;
                refreshTaskRedTipByDay(day);
            }

            /// <summary>
            /// 根据天数刷新任务红点
            /// </summary>
            /// <param name="_day"></param>
            public void refreshTaskRedTipByDay(int _day)
            {
                if (_day > _m_data._m_taskInfoList.Count || _day <= 0)
                    return;

                List<SevenDayGoalsTaskInfo> taskInfoList = _m_data._m_taskInfoList[_day - 1];

                long redTipCount = 0;
                for (int j = 0; j < taskInfoList.Count; j++)
                {
                    if (taskInfoList[j] == null)
                        continue;

                    bool needRed = _m_data.canTaskGetReward(taskInfoList[j].taskId) && _m_data._m_activityInfo != null && _m_data._m_activityInfo.isPlaying;
                    if (needRed)
                        redTipCount++;
                }
                CommonForceRedTipNode node = _getNode(_day, ESevenDayGoalsRedTipType.TASK);
                node?.setCount(redTipCount);

                _sendRedTipChgMsg();
            }

            /// <summary>
            /// 刷新所有礼包红点
            /// </summary>
            public void refrshAllGiftRedTip()
            {
                if (NPPlayer.instance.playerInfo == null)
                    return;

                // 获取当前解锁天数
                long serverNowDay = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.SERVER_START_DAYS);
                for (int i = 0; i < GRefdataCoreMgr.instance.getSevenDayGoalsMaxDay(); i++)
                {
                    bool unRead = serverNowDay >= (i + 1) &&
                                    _m_data.activityInfo != null &&
                                    _m_data.activityInfo.isPlaying &&
                                    !AccountSettingMgr.instance.accountSetting.getSevenDayGoalDayGiftIsRead(_m_data.activityInfo.instanceId, i + 1);
                    CommonForceRedTipNode node = _getNode(i + 1, ESevenDayGoalsRedTipType.GIFT);
                    node?.setCount(unRead ? 1 : 0);
                }

                _sendRedTipChgMsg();
            }

            /// <summary>
            /// 刷新阶段奖励的红点
            /// </summary>
            public void refreshStepRedTip()
            {
                // 获取所有阶段奖励的列表
                long canGetStepRewardCount = 0;
                if (_m_data._m_activityInfo != null && _m_data._m_activityInfo.isPlaying && GRefdataCoreMgr.instance.sevenDayGoalsStepRewardRefCore.refList != null)
                {
                    foreach (SevenDayGoalsStepRewardRefObj stepReward in GRefdataCoreMgr.instance.sevenDayGoalsStepRewardRefCore.refList)
                    {
                        // 如果领过奖励了就跳过
                        if (stepReward == null || _m_data.isStepRewardHadDraw(stepReward.id))
                            continue;

                        // 分数达标就 + 1 计数
                        if (_m_data._m_score >= stepReward.need_score)
                            canGetStepRewardCount++;
                    }
                }
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_SEVEN_DAY_GOALS, canGetStepRewardCount);

                _sendRedTipChgMsg();
            }

            /// <summary>
            /// 获取红点是否显示
            /// </summary>
            /// <param name="_day"></param>
            /// <param name="_tag"></param>
            /// <returns></returns>
            public bool getRedTipCanShow(long _day, ESevenDayGoalsRedTipType _tag)
            {
                CommonForceRedTipNode node = _getNode(_day, _tag);
                return node != null && node.needShow();
            }

            /// <summary>
            /// 设置红点已读
            /// </summary>
            /// <param name="_day"></param>
            /// <param name="_tag"></param>
            public void setIsRead(long _day, ESevenDayGoalsRedTipType _tag)
            {
                if(_m_data._m_activityInfo == null)
                    return;

                switch (_tag)
                {
                    case ESevenDayGoalsRedTipType.NEW:
                        AccountSettingMgr.instance.accountSetting.setNewSevenDayGoalDayRead(_m_data._m_activityInfo.instanceId, _day);
                        CommonForceRedTipNode newNode = _getNode(_day, _tag);
                        newNode?.setCount(0);
                        break;
                    case ESevenDayGoalsRedTipType.GIFT:
                        AccountSettingMgr.instance.accountSetting.setSevenDayGoalDayGiftRead(_m_data._m_activityInfo.instanceId, _day);
                        CommonForceRedTipNode giftNode = _getNode(_day, _tag);
                        giftNode?.setCount(0);
                        break;
                }

                _sendRedTipChgMsg();
            }

            //添加红点
            private void _addNode(long _day, ESevenDayGoalsRedTipType _tag)
            {
                string nodeKey = _createRedKey(RedTipConst.RED_SEVEN_DAY_GOALS, _day, _tag);
                if (_m_dMyNode.ContainsKey(nodeKey))
                    return;

                CommonForceRedTipNode node = new CommonForceRedTipNode(nodeKey);
                _m_dMyNode[nodeKey] = node;
                RedTipMgr.instance.addRedTipNodeWithParent(node, RedTipConst.RED_SEVEN_DAY_GOALS);
            }

            //获取红点节点
            private CommonForceRedTipNode _getNode(long _day, ESevenDayGoalsRedTipType _tag)
            {
                string nodeKey = _createRedKey(RedTipConst.RED_SEVEN_DAY_GOALS, _day, _tag);
                if (_m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node))
                    return _node;
                return null;
            }

            //创建唯一key
            [NotNull]
            private string _createRedKey(long _key, long _day, ESevenDayGoalsRedTipType _tag)
            {
                return $"{_key}_{_day}_{_tag}";
            }

            #region 发送消息

            /// <summary>
            /// 这里会保证一帧只发送一个消息
            /// </summary>
            private void _sendRedTipChgMsg()
            {
                //如果下一帧已经要求发送则这里不做处理
                if (_m_bNeedSendNextFrame)
                    return;

                //注册下一帧处理代码
                ALCommonTaskController.CommonActionAddNextFrameTask(_dealSendByNextFrame);
                //设置状态变量
                _m_bNeedSendNextFrame = true;
            }
            private static void _dealSendByNextFrame()
            {
                if (!_m_bNeedSendNextFrame)
                    return;

                //发送消息
                WinMsg.SendMsg(WinMsgType.ON_SEVEN_DAY_GOAL_RED_TIP_CHG);
                //重置标记
                _m_bNeedSendNextFrame = false;
            }

            #endregion
        }
    }

    /// <summary>
    /// 红点类型
    /// </summary>
    public enum ESevenDayGoalsRedTipType
    {
        NEW,// 新解锁
        TASK,// 任务
        GIFT,// 礼包
    }
}