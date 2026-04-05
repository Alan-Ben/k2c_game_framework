using System;
using System.Collections.Generic;
using System.Text;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class GConsortComponent
    {
        /// <summary>
        /// 妃子系统红点存储类，用于存储红点的变化和已读时间
        /// </summary>
        public class ConsortRedTipSaver : _AALBasicSettingInfo
        {
            [NotNull] private Dictionary<long, List<long>> _m_lHasReadUnlockBusinessSKillIdListDic = new Dictionary<long, List<long>>();//已读的解锁经营技能ID列表字典
            [NotNull] public Dictionary<long, long> _m_dConsortCharmPointChgTimeDic = new Dictionary<long, long>();//妃子加护点变化时间字典
            [NotNull] public Dictionary<long, long> _m_dConsortBlessSkillRedReadTimeDic = new Dictionary<long, long>();//妃子加护技能红点已读时间字典
            
            // 不同字段分隔符
            private const char _k_fieldSplit = ';';
            // 字典中数据项分隔符：
            private const char _k_dicDataSplit = '|';
            // 键值分隔符：
            private const char _k_keyValueSplit = ':';
            // 列表分隔符：
            private const char _k_listSplit = ',';

            public ConsortRedTipSaver() : base($"{NPPlayer.instance.playerInfo.CID}_Consort_Red_Tip")
            {
            }

            public ConsortRedTipSaver(long _userId) : base($"{_userId}_Consort_Red_Tip")
            {
            }

            protected override string _makeSettingStr()
            {
                StringBuilder sb = new StringBuilder();
                
                // 序列化_m_lHasReadUnlockBusinessSKillIdListDic
                // 序列化格式:
                // consortId:skillId1,skillId2,skillId3|consortId2:skillId1,skillId2
                if (_m_lHasReadUnlockBusinessSKillIdListDic.Count > 0)
                {
                    foreach (KeyValuePair<long, List<long>> kv in _m_lHasReadUnlockBusinessSKillIdListDic)
                    {
                        // 空列表不写入
                        if (kv.Value == null || kv.Value.Count == 0)
                            continue;

                        sb.Append(kv.Key);//写入妃子id
                        sb.Append(_k_keyValueSplit);// 写入键值分隔符_k_keyValueSplit:
                        foreach (var id in kv.Value)
                        {
                            sb.Append(id);//写入技能id
                            sb.Append(_k_listSplit);//写入列表分隔符_k_listSplit,
                        }
                        // 写入字典分隔符_k_dicDataSplit|
                        sb.Append(_k_dicDataSplit);
                    }
                }
                // 写入字段分隔符_k_fieldSplit;
                sb.Append(_k_fieldSplit);

                // 序列化_m_dConsortCharmPointChgTimeDic
                // 序列化格式: consortId:chgTime|consortId2:chgTime2
                if (_m_dConsortCharmPointChgTimeDic.Count > 0)
                {
                    foreach (KeyValuePair<long, long> kv in _m_dConsortCharmPointChgTimeDic)
                    {
                        sb.Append(kv.Key);
                        sb.Append(_k_keyValueSplit);
                        sb.Append(kv.Value);
                        sb.Append(_k_dicDataSplit);
                    }
                }
                sb.Append(_k_fieldSplit);

                // 序列化_m_dConsortBlessSkillRedReadTimeDic
                // 序列化格式: consortId:readTime|consortId2:readTime2
                if (_m_dConsortBlessSkillRedReadTimeDic.Count > 0)
                {
                    foreach (KeyValuePair<long, long> kv in _m_dConsortBlessSkillRedReadTimeDic)
                    {
                        sb.Append(kv.Key);
                        sb.Append(_k_keyValueSplit);
                        sb.Append(kv.Value);
                        sb.Append(_k_dicDataSplit);
                    }
                }
                sb.Append(_k_fieldSplit);

                return sb.ToString();
            }

            protected override void _initSettingStr(string _infoStr)
            {
                if (string.IsNullOrEmpty(_infoStr))
                    return;

                try
                {
                    // 先使用_k_fieldSplit分隔出不同字段, 不需要移除空项
                    string[] fieldArr = _infoStr.Split(_k_fieldSplit);
                    if(fieldArr == null)
                        return;

                    // 反序列化_m_lHasReadUnlockBusinessSKillIdListDic
                    _m_lHasReadUnlockBusinessSKillIdListDic.Clear();
                    if (fieldArr.Length > 0 && !string.IsNullOrEmpty(fieldArr[0]))
                    {
                        // 反序列化格式:
                        // consortId:(_k_keyValueSplit)skillId1,(_k_listSplit)skillId2,skillId3 |(_k_dicDataSplit) consortId2:skillId1,skillId2
                        string[] dicDataStrArr = fieldArr[0].Split(_k_dicDataSplit, StringSplitOptions.RemoveEmptyEntries);
                        if (dicDataStrArr != null && dicDataStrArr.Length > 0)
                        {
                            foreach (string dicDataStr in dicDataStrArr)
                            {
                                if (string.IsNullOrEmpty(dicDataStr))
                                    continue;
                                string[] keyValueStrArr = dicDataStr.Split(_k_keyValueSplit, StringSplitOptions.RemoveEmptyEntries);
                                if (keyValueStrArr == null || keyValueStrArr.Length != 2 || 
                                    string.IsNullOrEmpty(keyValueStrArr[0]) || string.IsNullOrEmpty(keyValueStrArr[1]))
                                    continue;
                                if (!long.TryParse(keyValueStrArr[0], out long consortId))
                                    continue;
                                string[] skillIdlistStrArr = keyValueStrArr[1].Split(_k_listSplit, StringSplitOptions.RemoveEmptyEntries);
                                if (skillIdlistStrArr == null || skillIdlistStrArr.Length <= 0)
                                    continue;
                                List<long> skillIdList = new List<long>();
                                _m_lHasReadUnlockBusinessSKillIdListDic.Add(consortId, skillIdList);
                                foreach (string skillIdStr in skillIdlistStrArr)
                                {
                                    if (long.TryParse(skillIdStr, out long skillId))
                                    {
                                        skillIdList.Add(skillId);
                                    }
                                }
                            }
                        }
                    }

                    // 反序列化_m_dConsortCharmPointChgTimeDic
                    _m_dConsortCharmPointChgTimeDic.Clear();
                    if (fieldArr.Length > 1 && !string.IsNullOrEmpty(fieldArr[1]))
                    {
                        string[] dicDataStrArr = fieldArr[1].Split(_k_dicDataSplit, StringSplitOptions.RemoveEmptyEntries);
                        if (dicDataStrArr != null && dicDataStrArr.Length > 0)
                        {
                            foreach (string dicDataStr in dicDataStrArr)
                            {
                                if (string.IsNullOrEmpty(dicDataStr))
                                    continue;
                                string[] keyValueStrArr = dicDataStr.Split(_k_keyValueSplit, StringSplitOptions.RemoveEmptyEntries);
                                if (keyValueStrArr == null || keyValueStrArr.Length != 2 ||
                                    string.IsNullOrEmpty(keyValueStrArr[0]) || string.IsNullOrEmpty(keyValueStrArr[1]))
                                    continue;
                                if (!long.TryParse(keyValueStrArr[0], out long consortId))
                                    continue;
                                if (!long.TryParse(keyValueStrArr[1], out long chgTime))
                                    continue;
                                _m_dConsortCharmPointChgTimeDic[consortId] = chgTime;
                            }
                        }
                    }

                    // 反序列化_m_dConsortBlessSkillRedReadTimeDic
                    _m_dConsortBlessSkillRedReadTimeDic.Clear();
                    if (fieldArr.Length > 2 && !string.IsNullOrEmpty(fieldArr[2]))
                    {
                        string[] dicDataStrArr = fieldArr[2].Split(_k_dicDataSplit, StringSplitOptions.RemoveEmptyEntries);
                        if (dicDataStrArr != null && dicDataStrArr.Length > 0)
                        {
                            foreach (string dicDataStr in dicDataStrArr)
                            {
                                if (string.IsNullOrEmpty(dicDataStr))
                                    continue;
                                string[] keyValueStrArr = dicDataStr.Split(_k_keyValueSplit, StringSplitOptions.RemoveEmptyEntries);
                                if (keyValueStrArr == null || keyValueStrArr.Length != 2 ||
                                    string.IsNullOrEmpty(keyValueStrArr[0]) || string.IsNullOrEmpty(keyValueStrArr[1]))
                                    continue;
                                if (!long.TryParse(keyValueStrArr[0], out long consortId))
                                    continue;
                                if (!long.TryParse(keyValueStrArr[1], out long readTime))
                                    continue;
                                _m_dConsortBlessSkillRedReadTimeDic[consortId] = readTime;
                            }
                        }
                    }
                    // 继续其他数据反序列化
                }
                catch (Exception e)
                {
                    Debug.LogError($"ConsortRedTipSaver init exception. _infoStr:[{_infoStr}] Exception:{e}");
                    _m_lHasReadUnlockBusinessSKillIdListDic.Clear();
                }
            }

            #region 经营技能

            /// <summary>
            /// 判断妃子对应经营技能是否已读
            /// </summary>
            /// <param name="_consortId">妃子ID</param>
            /// <param name="_skillId">技能ID</param>
            public bool hasReadUnlockBusinessSkill(long _consortId, long _skillId)
            {
                // 引用类型使用前空判断
                if (!_m_lHasReadUnlockBusinessSKillIdListDic.TryGetValue(_consortId, out List<long> list) || list == null)
                    return false;
                return list.Contains(_skillId);
            }

            /// <summary>
            /// 标记技能已读（若之前未读）
            /// </summary>
            public void setReadUnlockBusinessSkill(long _consortId, long _skillId)
            {
                if (!_m_lHasReadUnlockBusinessSKillIdListDic.TryGetValue(_consortId, out List<long> list) || list == null)
                {
                    list = new List<long>();
                    _m_lHasReadUnlockBusinessSKillIdListDic[_consortId] = list;
                }

                if (!list.Contains(_skillId))
                {
                    list.Add(_skillId);
                    saveSetting();
                }
            }

            /// <summary>
            /// 批量标记技能已读
            /// </summary>
            public void setReadUnlockBusinessSkillList(long _consortId, IEnumerable<long> _skillIdEnumerable)
            {
                if (_skillIdEnumerable == null)
                    return;

                if (!_m_lHasReadUnlockBusinessSKillIdListDic.TryGetValue(_consortId, out List<long> list) || list == null)
                {
                    list = new List<long>();
                    _m_lHasReadUnlockBusinessSKillIdListDic[_consortId] = list;
                }

                bool hasNew = false;
                foreach (long sid in _skillIdEnumerable)
                {
                    if (!list.Contains(sid))
                    {
                        list.Add(sid);
                        hasNew = true;
                    }
                }

                if (hasNew)
                    saveSetting();
            }

            /// <summary>
            /// 获取妃子已读的经营技能ID列表（只读副本）
            /// </summary>
            public List<long> getReadUnlockBusinessSkillIdList(long _consortId)
            {
                if (!_m_lHasReadUnlockBusinessSKillIdListDic.TryGetValue(_consortId, out List<long> list) || list == null)
                    return new List<long>();
                
                return new List<long>(list); // 返回副本避免外部修改内部集合
            }

            #endregion
            
            #region 加护点变化时间
            
            /// <summary>
            /// 设置妃子加护点变化时间
            /// </summary>
            /// <param name="_consortId"></param>
            /// <param name="_time"></param>
            public void setConsortCharmPointChgTime(long _consortId, long _time)
            {
                _m_dConsortCharmPointChgTimeDic[_consortId] = _time;
                saveSetting();
            }
            
            /// <summary>
            /// 获取妃子加护点变化时间
            /// </summary>
            /// <param name="_consortId"></param>
            /// <returns></returns>
            public long getConsortCharmPointChgTime(long _consortId)
            {
                if (_m_dConsortCharmPointChgTimeDic.TryGetValue(_consortId, out long time))
                {
                    return time;
                }

                return 0;
            }
            
            #endregion

            #region 加护技能红点已读时间
            
            /// <summary>
            /// 设置妃子加护技能红点已读时间
            /// </summary>
            /// <param name="_consortId"></param>
            /// <param name="_time"></param>
            public void setConsortBlessSkillRedReadTime(long _consortId, long _time)
            {
                _m_dConsortBlessSkillRedReadTimeDic[_consortId] = _time;
                saveSetting();
            }
            
            /// <summary>
            /// 获取妃子加护技能红点已读时间
            /// </summary>
            /// <param name="_consortId"></param>
            /// <returns></returns>
            public long getConsortBlessSkillRedReadTime(long _consortId)
            {
                if (_m_dConsortBlessSkillRedReadTimeDic.TryGetValue(_consortId, out long time))
                {
                    return time;
                }

                return 0;
            }

            #endregion
        }
    }
}