using ALPackage;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// New标签缓存，value：是否已读
    /// </summary>
    public class NewTagSaver : _AALBasicSettingInfo
    {
        public const char keyValueSplit = ':';//key value的分隔符
        public const char itemSplit = '|';//字典中每一项的分隔符

        // 第一个string是存储key，第二个是存储是否已读
        private Dictionary<string, bool> _m_dNewTagDic = new Dictionary<string, bool>();


        public NewTagSaver(long _accountCID)
            : base(string.Format("{0}_new_tag_saver", _accountCID))
        {

        }

        protected override void _initSettingStr(string _infoStr)
        {
            try
            {
                // 使用ItemSplit拆分出所有键值对字符串
                string[] keyValueListStr = _infoStr.Split(new[] { itemSplit }, StringSplitOptions.RemoveEmptyEntries);
                if (keyValueListStr == null || keyValueListStr.Length <= 0)
                {
                    Debug.LogWarning("[NPNewTagSaver]setting中没有非强制红点Tag信息");
                    return;
                }

                // 变量每一个键值对字符串
                for (int i = 0; i < keyValueListStr.Length; i++)
                {
                    // 拆分键和值
                    string[] keyValueStr = keyValueListStr[i].Split(new[] { keyValueSplit }, StringSplitOptions.RemoveEmptyEntries);
                    // 若拆分的长度小于4，数据错误
                    if (keyValueStr == null || keyValueStr.Length < 2)
                    {
                        Debug.LogError($"[NPNewTagSaver]setting中NewTag:{keyValueStr}信息错误");
                        continue;
                    }

                    bool isRead;
                    // 进行类型转换
                    if (!bool.TryParse(keyValueStr[1], out isRead))
                    {
                        Debug.LogError($"[NPNewTagSaver]setting中NewTag:{keyValueStr}信息错误");
                        continue;
                    }

                    _m_dNewTagDic[keyValueStr[0]] = isRead;
                }
            }
            catch (Exception _ex)
            {
                Debug.LogError($"[AccountSetting.NPNewTagSaver] load error \n" + _ex.ToString());
            }
        }

        protected override string _makeSettingStr()
        {
            if (_m_dNewTagDic == null)
                return String.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, bool> pair in _m_dNewTagDic)
            {
                sb.Append(pair.Key);
                sb.Append(keyValueSplit);// 分割键和值
                sb.Append(pair.Value);
                sb.Append(itemSplit);// 分割每个键值对
            }
            return sb.ToString();
        }
    }
}
