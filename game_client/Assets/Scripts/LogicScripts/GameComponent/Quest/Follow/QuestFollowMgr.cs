using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 追踪任务管理器
    /// </summary>
    public class QuestFollowMgr
    {
        private static QuestFollowMgr _m_instance;
        [NotNull]
        public static QuestFollowMgr instance
        {
            get
            {
                if (_m_instance == null)
                    _m_instance = new QuestFollowMgr();
                return _m_instance;
            }
        }

        private _IFollowableQuest _m_curFollow;//当前追踪目标
        private bool _m_bIsInited;//是否已经初始化
        private bool _m_bNeedCheck;//是否需要检查

        private QuestFollowMgr()
        {
            _m_bIsInited = false;
            _m_bNeedCheck = false;
        }

        /// <summary> 当前追踪目标 </summary>
        public _IFollowableQuest curFollow { get { return _m_curFollow; } }

        /// <summary>
        /// 初始化
        /// </summary>
        public void init()
        {
            if (_m_bIsInited)
                return;

            _m_bIsInited = true;

            //读取缓存，并检查当前有效性
            _IFollowableQuest curFollow = _getFollowableQuest(AccountSettingMgr.instance.accountSetting.followingQuestType, AccountSettingMgr.instance.accountSetting.followingQuestId);
            if (curFollow == null || !curFollow.enableFollow)
                setFollowQuest(_tryGetAutoFollowQuest());
            else
                setFollowQuest(curFollow);
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void clear()
        {
            if (!_m_bIsInited)
                return;

            _m_bIsInited = false;

            _m_curFollow = null;
        }

        /// <summary>
        /// 设置追踪目标
        /// </summary>
        /// <param name="_quest"></param>
        public void setFollowQuest(_IFollowableQuest _quest, bool _needShowBarSfx = false)
        {
            if (_quest != null && !_quest.enableFollow)
            {
                Debug.LogWarning($"【QuestFollowMgr.setFollowQuest Warning】尝试追踪了不可追踪的任务，ENPFollowQuestType:{_quest.questType}，id:{_quest.id}");
                return;
            }

            _m_curFollow = _quest;
            if (_m_curFollow != null)
                AccountSettingMgr.instance.accountSetting.setFollowingQuest(_m_curFollow.questType, _m_curFollow.id);
            else
                AccountSettingMgr.instance.accountSetting.clearFollowingQuest();

            WinMsg.SendMsg(WinMsgType.QUEST_FOLLOW, _m_curFollow);

            //是否展示任务栏特效
            if(_m_curFollow != null && _needShowBarSfx)
                WinMsg.SendMsg(WinMsgType.ON_ACCEPT_NEW_QUEST_SHOW);
        }

        /// <summary>
        /// 判断是否当前追踪目标
        /// </summary>
        /// <param name="_quest"></param>
        /// <returns></returns>
        public bool getIsCurFollow(_IFollowableQuest _quest)
        {
            if (_quest == null || _m_curFollow == null)
                return false;

            return _m_curFollow.questType == _quest.questType && _m_curFollow.id == _quest.id;
        }

        /// <summary>
        /// 检查当前追踪有效性，并尝试自动追踪
        /// </summary>
        public void checkAndAutoFollow()
        {
            if (!_m_bIsInited)
                return;

            //防止同一帧检查多次
            if (_m_bNeedCheck)
                return;

            _m_bNeedCheck = true;

            ALCommonActionMonoTask.addNextFrameTask(_checkAndAutoFollow);
        }

        /// <summary>
        /// 检查当前追踪有效性，并尝试自动追踪
        /// </summary>
        private void _checkAndAutoFollow()
        {
            _m_bNeedCheck = false;

            if (!_m_bIsInited)
                return;

            //当前任务已经不允许追踪了，置为空
            if (_m_curFollow != null && !_m_curFollow.enableFollow)
            {
                _m_curFollow = null;
            }

            //当前追踪为空，尝试自动追踪
            if (_m_curFollow == null)
            {
                setFollowQuest(_tryGetAutoFollowQuest());
            }
        }

        /// <summary>
        /// 获取可追踪的任务
        /// </summary>
        /// <param name="_followType"></param>
        /// <param name="_id"></param>
        /// <returns></returns>
        private _IFollowableQuest _getFollowableQuest(ENPFollowQuestType _followType, long _id)
        {
            switch (_followType)
            {
                //主线
                case ENPFollowQuestType.MAIN:
                //心愿任务
                case ENPFollowQuestType.WISH_QUEST:
                    return NPPlayer.instance.questComp.questItemMgr.getQuestItem(_id);

            }

            return null;
        }

        /// <summary>
        /// 尝试获取自动追踪的任务，主线 >
        /// </summary>
        /// <returns></returns>
        private _IFollowableQuest _tryGetAutoFollowQuest()
        {
            //主线
            _IFollowableQuest quest = NPPlayer.instance.questComp.questItemMgr.getAutoFollowQuest();
            return quest;
        }
    }
}
