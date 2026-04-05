using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 跑马灯记录队列展示位置信息
    /// </summary>
    public class MarqueeRecordShowInfo
    {
        //记录的位置信息字典
        [NotNull] private Dictionary<int, List<MarqueeRecordShowData>> _m_dRecordDic;
        //同种posId的跑马灯一起展示的队列，只展示最后的一个，前面的停止展示，用于两个窗口都打开并且配置了同一个posId，就只展示最上面那个（比如主城界面和聊天界面，聊天界面打开时不会关闭主城界面）
        [NotNull] private Dictionary<int, List<GGUICustomMonoMarquee>> _m_dSamePosMarqueeQueue;

        public MarqueeRecordShowInfo()
        {
            _m_dRecordDic = new Dictionary<int, List<MarqueeRecordShowData>>();
            _m_dSamePosMarqueeQueue = new Dictionary<int, List<GGUICustomMonoMarquee>>();
        }

        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_pos"></param>
        public void addInfo(MarqueeInfo _info, Vector2 _pos)
        {
            if (_info == null)
                return;

            MarqueeRecordShowData data = new MarqueeRecordShowData();
            data.info = _info;
            data.pos = _pos;
            if (!_m_dRecordDic.ContainsKey(_info.showPosId))
            {
                List<MarqueeRecordShowData> dataList = new List<MarqueeRecordShowData>();
                dataList.Add(data);
                _m_dRecordDic.Add(_info.showPosId, dataList);
            }
            else
            {
                List<MarqueeRecordShowData> dataList = _m_dRecordDic[_info.showPosId];
                dataList.Add(data);
            }
        }

        /// <summary>
        /// 移除记录
        /// </summary>
        /// <param name="_posId"></param>
        public void removeInfo(int _posId)
        {
            _m_dRecordDic.Remove(_posId);
        }

        /// <summary>
        /// 根据posId检查是否有记录
        /// </summary>
        /// <param name="_posId"></param>
        /// <returns></returns>
        public bool checkIsRecordPosId(int _posId)
        {
            return _m_dRecordDic.ContainsKey(_posId);
        }

        /// <summary>
        /// 获取记录列表
        /// </summary>
        /// <param name="_posId"></param>
        /// <returns></returns>
        public List<MarqueeRecordShowData> getInfoList(int _posId)
        {
            if (_m_dRecordDic.TryGetValue(_posId, out List<MarqueeRecordShowData> _dataList))
            {
                return _dataList;
            }

            return null;
        }

        /// <summary>
        /// 添加可见跑马灯预制体控制类到队列里
        /// </summary>
        /// <param name="_posId"></param>
        /// <param name="_marquee"></param>
        public void addMarqueeQueueRecord(List<int> _posIdList, GGUICustomMonoMarquee _marquee)
        {
            if (_posIdList == null)
                return;

            for (int i = 0; i < _posIdList.Count; i++)
            {
                if (_m_dSamePosMarqueeQueue.TryGetValue(_posIdList[i], out List<GGUICustomMonoMarquee> infoList))
                {
                    if (infoList.Contains(_marquee))
                        return;

                    infoList.Add(_marquee);

                    //如果当前位置的不止有一个正在展示，对上一个暂停并记录跑马灯位置，让新的跑马灯预制体从记录的位置继续展示
                    if (infoList.Count > 1)
                        infoList[infoList.Count - 2].pauseAndRecord();
                }
                else
                {
                    List<GGUICustomMonoMarquee> curInfo = new List<GGUICustomMonoMarquee>();
                    curInfo.Add(_marquee);
                    _m_dSamePosMarqueeQueue[_posIdList[i]] = curInfo;
                }
            }
        }

        /// <summary>
        /// 从可见跑马灯预制体控制队列里移除
        /// </summary>
        /// <param name="_posId"></param>
        /// <param name="_marquee"></param>
        public void removeMarqueeQueueRecord(List<int> _posIdList, GGUICustomMonoMarquee _marquee)
        {
            if (_posIdList == null)
                return;

            for (int i = 0; i < _posIdList.Count; i++)
            {
                if (_m_dSamePosMarqueeQueue.TryGetValue(_posIdList[i], out List<GGUICustomMonoMarquee> infoList))
                {
                    if (infoList.Contains(_marquee))
                    {
                        infoList.Remove(_marquee);
                        //如果当前位置还有跑马灯，设置继续展示
                        if (infoList.Count > 0)
                            infoList.GetLast().resumeShow();
                    }
                }
            }
        }

        /// <summary>
        /// 当前跑马灯UI是否在最上层
        /// </summary>
        /// <param name="_posId"></param>
        /// <param name="_marquee"></param>
        public bool isTopMarqueeUI(int _posId, GGUICustomMonoMarquee _marquee)
        {
            if (_m_dSamePosMarqueeQueue.TryGetValue(_posId, out List<GGUICustomMonoMarquee> infoList))
            {
                if (infoList != null && infoList.Count > 0)
                    return infoList.GetLast() == _marquee;
            }

            return true;
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void clear()
        {
            _m_dRecordDic.Clear();
            _m_dSamePosMarqueeQueue.Clear();
        }

        /// <summary>
        /// 记录结构体
        /// </summary>
        public struct MarqueeRecordShowData
        {
            public Vector2 pos;
            public MarqueeInfo info;
        }
    }
}