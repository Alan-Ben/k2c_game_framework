using ALPackage;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星建筑未读变化本地保存
    /// 记录有未读信息的建筑ID
    /// </summary>
    public class UnreadMarsBuildingChangeSaver : _AALBasicSettingInfo
    {
        public const char itemSplit = '|';//每个ID的分隔符

        /// <summary>
        /// 未读建筑ID变化事件
        /// </summary>
        public event Action onUnreadMarsBuildingChanged;

        [NotNull] private HashSet<long> _m_unreadBuildingIds = new HashSet<long>();

        public UnreadMarsBuildingChangeSaver(long _accountCID)
            : base($"{_accountCID}_cache_account_unread_mars_building_change_saver")
        {
        }

        /// <summary>
        /// 构建需要保存的字符串
        /// </summary>
        protected override string _makeSettingStr()
        {
            if (_m_unreadBuildingIds == null || _m_unreadBuildingIds.Count == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (long buildingId in _m_unreadBuildingIds)
            {
                sb.Append(buildingId);
                sb.Append(itemSplit);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 读取保存的字符串
        /// </summary>
        protected override void _initSettingStr(string _infoStr)
        {
            try
            {
                if (string.IsNullOrEmpty(_infoStr))
                    return;

                // 使用itemSplit拆分出所有建筑ID字符串
                string[] idListStr = _infoStr.Split(new[] { itemSplit }, StringSplitOptions.RemoveEmptyEntries);
                if (idListStr == null || idListStr.Length <= 0)
                {
                    Debug.LogWarning("[UnreadMarsBuildingChangeSaver]setting中没有未读建筑ID信息");
                    return;
                }

                // 遍历每一个ID字符串
                for (int i = 0; i < idListStr.Length; i++)
                {
                    long buildingId;
                    // 进行类型转换
                    if (!long.TryParse(idListStr[i], out buildingId))
                    {
                        Debug.LogError($"[UnreadMarsBuildingChangeSaver]setting中建筑ID:{idListStr[i]}信息错误");
                        continue;
                    }

                    _m_unreadBuildingIds.Add(buildingId);
                }
            }
            catch (Exception _ex)
            {
                Debug.LogError($"[UnreadMarsBuildingChangeSaver] load error \n" + _ex.ToString());
            }
        }

        /// <summary>
        /// 添加未读建筑ID
        /// </summary>
        public void addUnreadBuildingId(long _buildingId)
        {
            if (_m_unreadBuildingIds.Add(_buildingId))
            {
                saveSetting();
                onUnreadMarsBuildingChanged?.Invoke();
            }
        }

        /// <summary>
        /// 移除未读建筑ID
        /// </summary>
        public void removeUnreadBuildingId(long _buildingId)
        {
            if (_m_unreadBuildingIds.Remove(_buildingId))
            {
                saveSetting();
                onUnreadMarsBuildingChanged?.Invoke();
            }
        }

        /// <summary>
        /// 检查建筑ID是否未读
        /// </summary>
        public bool isUnread(long _buildingId)
        {
            return _m_unreadBuildingIds.Contains(_buildingId);
        }

        /// <summary>
        /// 获取所有未读建筑ID列表
        /// </summary>
        public HashSet<long> getUnreadBuildingIds()
        {
            return _m_unreadBuildingIds;
        }

        /// <summary>
        /// 清空所有未读建筑ID
        /// </summary>
        public void clearAll()
        {
            if (_m_unreadBuildingIds.Count == 0)
                return;

            _m_unreadBuildingIds.Clear();
            saveSetting();
            onUnreadMarsBuildingChanged?.Invoke();
        }
    }
}
