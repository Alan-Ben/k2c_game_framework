using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NPEnum;

using NPCommon;

namespace GOE
{
    // 玩家等级提升展示数据
    public class NPPlayerLvUpShowInfo
    {
        private long _m_oldValue;
        private long _m_newValue;
        //属性类型
        private ENPPlayerPropertyType _m_type;

        //构造函数
        public NPPlayerLvUpShowInfo()
        {


        }
        public long oldValue { get { return _m_oldValue; } }
        public long newValue { get { return _m_newValue; } }
        public ENPPlayerPropertyType type { get { return _m_type; } }

        public void setValue(long _oldV , long _newV ,ENPPlayerPropertyType _type)
        {
            _m_oldValue = _oldV;
            _m_newValue = _newV;
            _m_type = _type;
        }
    }
}
