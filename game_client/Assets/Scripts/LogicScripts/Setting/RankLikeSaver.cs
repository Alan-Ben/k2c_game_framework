using ALPackage;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOE
{
    public class RankLikeSaver : _AALBasicSettingInfo
    {
        // key:rankId  value:cidList
        public Dictionary<long, List<long>> _m_rankLikeDic;
        private const char _m_dataSplit = '|';
        private const char _m_keyValueSplit = ':';
        private const char _m_valueSplit = ',';
        public Action onLikeDataChg;//点赞数据变化

        public RankLikeSaver(long _accountCID)
            : base(string.Format("{0}_rank_like_tip", _accountCID))
        {
            _m_rankLikeDic = new Dictionary<long, List<long>>();
        }

        /*************
         * 构建需要保存的字符串
         **/
        protected override string _makeSettingStr()
        {
            StringBuilder sb = new StringBuilder();
            int forCount = 0;
            foreach (KeyValuePair<long, List<long>> kv in _m_rankLikeDic)
            {
                forCount++;
                sb.Append(kv.Key);
                sb.Append(_m_keyValueSplit);
                int listCount = 0;
                foreach (long listValue in kv.Value)
                {
                    listCount++;
                    sb.Append(listValue);
                    if (listCount != kv.Value.Count)
                        sb.Append(_m_valueSplit);
                }
                if (forCount != _m_rankLikeDic.Count)
                    sb.Append(_m_dataSplit);
            }
            return sb.ToString();
        }

        /**************
         * 读取保存的字符串
         **/
        protected override void _initSettingStr(string _infoStr) 
        {
            if (string.IsNullOrEmpty(_infoStr))
                return;

            _m_rankLikeDic.Clear();
            try
            {
                string[] dataStrs = _infoStr.Split(_m_dataSplit);
                for (int i = 0; i < dataStrs.Length; i++)
                {
                    string[] kvStrs = dataStrs[i].Split(_m_keyValueSplit);
                    long key = long.Parse(kvStrs[0]);
                    string[] valueStrs = kvStrs[1].Split(_m_valueSplit);
                    List<long> valueList = new List<long>();
                    foreach (string value in valueStrs)
                    {
                        valueList.Add(long.Parse(value));
                    }

                    _m_rankLikeDic.Add(key, valueList);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("NPRankLikeSaver init has Exception.   _infoStr:   [" + _infoStr + "]\t\tException:   " + e);
                _m_rankLikeDic.Clear();
            }
        }

        //添加点赞玩家cid
        public void addRankLikeCid(long _rankId, long _cid)
        {
            if (null == _m_rankLikeDic)
                return;

            List<long> cidList = null;
            _m_rankLikeDic.TryGetValue(_rankId, out cidList);

            if (null == cidList)
                cidList = new List<long>();
            if (!cidList.Contains(_cid))
                cidList.Add(_cid);

            if (!_m_rankLikeDic.ContainsKey(_rankId))
                _m_rankLikeDic.Add(_rankId, cidList);

            //获取当前时间戳
            int timeNow = TimeUtil.getTimeByYYYYMM(FpsAndPingMgr.instance.serverTimeTag);
            //设置当前点赞时间戳
            AccountSettingMgr.instance.accountSetting.setRankLikeTimeS(timeNow);

            //保存到本地
            saveSetting();
            onLikeDataChg?.Invoke();
        }

        //获取该玩家是否被点过赞
        public bool getRankCidIsLike(long _rankId, long _cid)
        {
            if ( null == _m_rankLikeDic)
                return false;

            //获取存储的时间戳
            int timeSave = AccountSettingMgr.instance.accountSetting.rankLikeTimeS;
            if (timeSave == 0)
                return false;

            //获取当前时间戳
            int timeNow = TimeUtil.getTimeByYYYYMM(FpsAndPingMgr.instance.serverTimeTag);
            //如果时间不一致清空点赞数据
            if (timeNow != timeSave)
            {
                _clearRankLikeData();
                return false;
            }

            List<long> cidList = null;
            _m_rankLikeDic.TryGetValue(_rankId, out cidList);
            if (null == cidList)
                return false;

            if (cidList.Contains(_cid))
                return true;

            return false;
        }

        /// <summary>
        /// 清空玩家点赞数据,获取的时候清理，不用触发变化回调
        /// </summary>
        private void _clearRankLikeData()
        {
            if (null == _m_rankLikeDic || _m_rankLikeDic.Count <= 0)
                return;

            _m_rankLikeDic.Clear();
        }
    }
}
