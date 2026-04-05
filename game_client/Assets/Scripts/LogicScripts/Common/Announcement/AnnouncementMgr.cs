using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine.Pool;

namespace GOE
{
    /// <summary>
    /// 运营公告管理类
    /// </summary>
    public class AnnouncementMgr
    {
        private static AnnouncementMgr _g_instance;
        [NotNull]
        public static AnnouncementMgr instance
        {
            get
            {
                if (_g_instance == null)
                {
                    _g_instance = new AnnouncementMgr();
                }
                return _g_instance;
            }
        }

        private AnnouncementRemarkInfo _m_announcementRemarkInfo;//记录信息
        private bool _m_bIsAllInit;//是否已经全部初始化完
        private Action _m_aOnInitDone;//初始化完成回调
        private bool _m_bIsPopNotice;//这次登录是否展示过弹窗

        public AnnouncementMgr()
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void init()
        {
            if (!Game.instance.isUseCdn)
            {
                //重置红点
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_ANNOUNCEMENT, 0);
                return;
            }

            _m_bIsPopNotice = false;
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                _m_bIsAllInit = true;
                refreshRedTip();
                _m_aOnInitDone?.Invoke();
                _m_aOnInitDone = null;
                //刷新一下ui显示
                GCommon.reloadCustomLoadPrefab();
            });

            //初始化记录信息
            _m_announcementRemarkInfo = new AnnouncementRemarkInfo();
            _m_announcementRemarkInfo.sendRequest(stepCounter.addDoneStepCount);
            //注册初始化CDN
            CDNSetting_AnnouncementInfo.instance.regCDNInitDelegate(stepCounter.addDoneStepCount);
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        public void reset()
        {
            _m_announcementRemarkInfo = null;
            _m_bIsAllInit = false;
            _m_bIsPopNotice = false;
            _m_aOnInitDone = null;
        }

        /// <summary>
        /// 获取有效的运营公告列表
        /// </summary>
        /// <param name="_resultValidList"></param>
        public void getValidAnnouncementList(List<AnnouncementShowInfo> _resultValidList)
        {
            CDNSetting_AnnouncementInfo.instance.getValidAnnouncementList(_resultValidList);
        }

        /// <summary>
        /// 是否有有效的运营公告
        /// </summary>
        /// <returns></returns>
        public bool haveValidAnnouncement()
        {
            List<AnnouncementShowInfo> infoList = ListPool<AnnouncementShowInfo>.Get();//使用对象池，避免频繁new
            getValidAnnouncementList(infoList);

            bool haveAnnouncement = infoList.Count > 0;
            ListPool<AnnouncementShowInfo>.Release(infoList);
            return haveAnnouncement;
        }

        /// <summary>
        /// 注册初始化完成回调
        /// </summary>
        /// <param name="_onDone"></param>
        public void regInitDone(Action _onDone)
        {
            if (_m_bIsAllInit)
            {
                _onDone?.Invoke();
                return;
            }

            if (_m_aOnInitDone == null)
                _m_aOnInitDone = _onDone;
            else
                _m_aOnInitDone += _onDone;
        }

        /// <summary>
        /// 检查是否能展示弹窗
        /// </summary>
        public void checkCanShowNotice(EMainCityPushNoticeTriggerType _pushNoticeTriggerType, Action<bool> _addNoticeDone)
        {
            if (!Game.instance.isUseCdn)
            {
                _addNoticeDone?.Invoke(false);
                return;
            }

            //此次登录展示过就不再展示
            if (_m_bIsPopNotice)
            {
                _addNoticeDone?.Invoke(false);
                return;
            }

            regInitDone(() =>
            {
                //有未读公告时，每天默认登录时弹出一次，有新增运营公告时登录也直接弹出
                if (canShowNotice())
                {
                    _m_bIsPopNotice = true;
                    NPUINoticeMgr.instance.addDealer(new NoticeDealer_Announcement(_pushNoticeTriggerType));
                    _addNoticeDone?.Invoke(true);
                }
                else
                {
                    _addNoticeDone?.Invoke(false);
                }
            });
        }

        /// <summary>
        /// 是否可以展示运营公告
        /// </summary>
        /// <returns></returns>
        public bool canShowNotice()
        {
            if (!Game.instance.isUseCdn)
                return false;

            //此次登录是否展示过
            if (_m_bIsPopNotice)
                return false;

            //功能是否解锁
            if (!GCommon.isFuncUnlock(ENPFunctionType.ANNOUNCEMENT))
                return false;

            //是否是新的一天
            bool isNewDay = AccountSettingMgr.instance.dailyTagSaver.isNewDay(DailyTagConst.ANNOUNCEMENT);
            //是否有新的运营公告
            bool haveNew = _m_announcementRemarkInfo != null && _m_announcementRemarkInfo.haveNewAnnouncement();

            List<AnnouncementShowInfo> showInfoList = new List<AnnouncementShowInfo>();
            getValidAnnouncementList(showInfoList);
            //是否有未读公告
            bool haveUnread = false;
            for (int i = 0; i < showInfoList.Count; i++)
            {
                if (showInfoList[i] != null && !showInfoList[i].isRead())
                {
                    haveUnread = true;
                    break;
                }
            }

            //有未读公告时，每天默认登录时弹出一次，有新增运营公告时登录也直接弹出
            return ((isNewDay && haveUnread) || haveNew) && showInfoList.Count > 0;
        }

        /// <summary>
        /// 是否已读
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public bool isRead(long _id)
        {
            return _m_announcementRemarkInfo == null ? false : _m_announcementRemarkInfo.isRead(_id);
        }

        /// <summary>
        /// 设置已读
        /// </summary>
        /// <param name="_id"></param>
        public void setIsRead(long _id)
        {
            if (_m_announcementRemarkInfo == null)
                _m_announcementRemarkInfo = new AnnouncementRemarkInfo();

            _m_announcementRemarkInfo.addReadId(_id);
            //如果是新的已读信息，刷新一次红点
            refreshRedTip();
        }

        /// <summary>
        /// 设置运营公告已展示过
        /// </summary>
        public void setIsShow()
        {
            if (_m_announcementRemarkInfo == null)
                _m_announcementRemarkInfo = new AnnouncementRemarkInfo();

            _m_announcementRemarkInfo.setMaxId();
            AccountSettingMgr.instance.dailyTagSaver.setSaveToday(DailyTagConst.ANNOUNCEMENT);
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        public void refreshRedTip()
        {
            List<AnnouncementShowInfo> showInfoList = ListPool<AnnouncementShowInfo>.Get();//使用对象池，避免频繁new
            getValidAnnouncementList(showInfoList);
            long newCount = 0;
            for (int i = 0; i < showInfoList.Count; i++)
            {
                if (showInfoList[i] != null && !showInfoList[i].isRead())
                    newCount++;
            }

            ListPool<AnnouncementShowInfo>.Release(showInfoList);
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_ANNOUNCEMENT, newCount);
        }
    }
}
