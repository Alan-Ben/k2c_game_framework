
using System;
using System.Collections.Generic;
using System.Text;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用非强制节点的时间数据保存到setting中的类，通用的非强制红点是根据一个新消息的时间来和已储存的时间来比对，判断是不是需要重新激活未读
    /// </summary>
    public class RedTipTimeSaver : _AALBasicSettingInfo
    {
        public const char NodeDataSplit = ',';//节点ID和Tag的分隔符
        public const char ItemSplit = '|';//字典中每一项的分隔符

        // 第一个string是存储key，第二个是红点的数据
        [NotNull]private Dictionary<string, long> _m_dNodeIdTagDic = new Dictionary<string, long>();

        public RedTipTimeSaver()
            : base(string.Format("{0}_Red_Tip_Time", NPPlayer.instance.playerInfo.CID))
        {
        }

        public RedTipTimeSaver(long _userId)
            : base(string.Format("{0}_Red_Tip_Time", _userId))
        {
        }

        /// <summary>
        /// 保存红点的信息
        /// </summary>
        public void saveNodeData(string _saveKey, long _selfCount)
        {
            _m_dNodeIdTagDic[_saveKey] = _selfCount;
            saveSetting();
#if UNITY_EDITOR
            if(_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"[CommonNotForceRedTipTimeTagSaver]保存{_saveKey}: _selfCount-{_selfCount} 标记信息成功");
#endif
        }

        /// <summary>
        /// 根据红点的saveKey获取对应的数据
        /// </summary>
        public bool getNodeData(string _saveKey, out long _selfCount)
        {
            if (_m_dNodeIdTagDic.TryGetValue(_saveKey, out long _data))
            {
                _selfCount = _data;
#if UNITY_EDITOR
                if(_AALMonoMain.instance.showDebugOutput)
                    Debug.Log($"[NotForceRedTipSaver]读取{_saveKey}: selfCount-{_selfCount} 标记信息成功");
#endif
                return true;
            }

            _selfCount = 0;
            return false;
        }

        protected override string _makeSettingStr()
        {
            if (_m_dNodeIdTagDic == null)
                return String.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, long> pair in _m_dNodeIdTagDic)
            {
                sb.Append(pair.Key);
                sb.Append(NodeDataSplit);// 分割键和值
                sb.Append(pair.Value);
                sb.Append(ItemSplit);// 分割每个键值对
            }
            return sb.ToString();
        }

        protected override void _initSettingStr(string _infoStr)
        {
            // 使用ItemSplit拆分出所有键值对字符串
            string[] keyValueListStr = _infoStr.Split(new[] { ItemSplit }, StringSplitOptions.RemoveEmptyEntries);
            if (keyValueListStr == null || keyValueListStr.Length <= 0)
            {
                Debug.LogWarning("[CommonNotForceRedTipTimeTagSaver]setting中没有非强制红点Tag信息");
                return;
            }

            // 变量每一个键值对字符串
            for (int i = 0; i < keyValueListStr.Length; i++)
            {
                // 使用NodeIdTagSplit拆分键和值
                string[] keyValueStr = keyValueListStr[i].Split(new[] { NodeDataSplit }, StringSplitOptions.RemoveEmptyEntries);
                // 若拆分的长度小于4，数据错误
                if (keyValueStr == null || keyValueStr.Length < 2)
                {
                    Debug.LogError($"[CommonNotForceRedTipTimeTagSaver]setting中非强制红点Tag:{keyValueStr}信息错误");
                    continue;
                }

                long selfCount;
                // 进行类型转换
                if (!long.TryParse(keyValueStr[1], out selfCount))
                {
                    Debug.LogError($"[CommonNotForceRedTipTimeTagSaver]setting中非强制红点Tag:{keyValueStr}信息错误");
                    continue;
                }

                _m_dNodeIdTagDic[keyValueStr[0]] = selfCount;
            }
        }
    }
}