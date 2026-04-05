using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 加成配置混合读取容器
    /// </summary>
    public class UnionBonus
    {
        private List<_IBonusReader> _m_bonusList;

        public UnionBonus()
        {
            this._m_bonusList = new List<_IBonusReader>();
        }

        //region get&&set

        public List<_IBonusReader> getBonusReaderList()
        {
            return _m_bonusList;
        }

        /// <summary>
        /// 获取加成属性值
        /// </summary>
        public long getValue(EBonusPropertyType _propertyType, JudgeUnionBonusPart[] _judgeParts = null)
        {
            long result = 0;
            foreach (_IBonusReader bonus in _m_bonusList)
            {
                EBonusFilterType filterType = bonus.getFilterType();
                PlayerBonusPropertyModifier propertyModifier = bonus.getBonusPropertyModifier();
                long value = propertyModifier.getPropValue(_propertyType);
                
                if (filterType == EBonusFilterType.NONE)
                    result += value;
                else if (_judgeParts != null)
                {
                    foreach (JudgeUnionBonusPart part in _judgeParts)
                    {
                        if (part.filterType == filterType && part.id == bonus.getBonusId())
                        {
                            result += value;
                            break;
                        }
                    }
                }
            }

            return result;
        }
        public long getValueAllJudgeParts(EBonusPropertyType _propertyType)
        {
            long result = 0;
            foreach (_IBonusReader bonus in _m_bonusList)
            {
                PlayerBonusPropertyModifier propertyModifier = bonus.getBonusPropertyModifier();
                long value = propertyModifier.getPropValue(_propertyType);
                result += value;
            }

            return result;
        }

        //endregion
        public bool parseFromString(String sValue)
        {
            if (sValue == null || sValue.Trim().Length <= 0)
                return true;

            readStr(sValue);

            return true;
        }

        /**
         * 读取字符串
         * 加成范围|加成类型枚举|属性枚举：数量；属性枚举：数量#加成范围|加成类型枚举|属性枚举：数量；属性枚举：数量
         * 加成范围： EPropBonusType 加成过滤类型
         * @param _sValue 配置字符串
         */
        private void readStr(String _sValue)
        {
            ALStringReader strR = new ALStringReader(_sValue);
            //按 ‘#’ 分割
            String s = strR.readItem('#');
            while (s != null)
            {
                ALStringReader insideStrR = new ALStringReader(s);
                //按'|'分割，获取bonusReader的类型
                String bonusTypeStr = insideStrR.readItem('|');
                if (bonusTypeStr == null)
                {
                    s = strR.readItem('#');
                    continue;
                }

                if (!Enum.TryParse(bonusTypeStr.Trim(), true, out EBonusFilterType bonusType))
                {
                    //next
                    s = strR.readItem('#');
                    continue;
                }

                //获取对应的Reader
                _IBonusReader bonusReader = BonusReaderMgr.instance.createNew(bonusType);
                //将‘|’后的内容交由对应类型的Bonus自己解析
                bonusReader.ParseFromString(insideStrR.readItem('#'));

                //非空则加入队列
                if (!bonusReader.getBonusPropertyModifier().isEmpty())
                    _m_bonusList.Add(bonusReader);

                //next
                s = strR.readItem('#');
            }
        }
    }
}