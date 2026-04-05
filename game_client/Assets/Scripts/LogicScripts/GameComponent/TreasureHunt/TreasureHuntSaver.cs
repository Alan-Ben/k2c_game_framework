using System;
using System.Text;
using ALPackage;

namespace GOE
{
    public partial class TreasureHuntComponent
    {
        public class TreasureHuntSaver : _AALBasicSettingInfo
        {
            private const char FieldSplitChar = '#';
            private const char CollectionItemSplitChar = ',';
            
            private long _m_lInAreaId; // 当前所在的太空区域id
            private ETreasureHuntGamePlayMode _m_eSelectedGamePlayMode; // 当前选择的游戏玩法模式
            
            public TreasureHuntSaver() : base($"{NPPlayer.instance.playerInfo.CID}_treasure_hunt_saver")
            {
                _m_lInAreaId = -1;
                _m_eSelectedGamePlayMode = ETreasureHuntGamePlayMode.NORMAL;
            }

            protected override string _makeSettingStr()
            {
                StringBuilder sb = new StringBuilder();
                
                // 序列化 _m_lInAreaId
                sb.Append(_m_lInAreaId);
                sb.Append(FieldSplitChar);
                
                // 序列化 _m_eSelectedGamePlayMode
                sb.Append(_m_eSelectedGamePlayMode.ToString());
                sb.Append(FieldSplitChar);

                return sb.ToString();
            }

            protected override void _initSettingStr(string _infoStr)
            {
                if(string.IsNullOrEmpty(_infoStr))
                    return;
                
                string[] fieldsArray = _infoStr.Split(FieldSplitChar);
                if(fieldsArray == null)
                    return;

                try
                {
                    // 解析 _m_lInAreaId 字段
                    _m_lInAreaId = ALCommon.ParseLong(fieldsArray[0]);
                    
                    // 解析 _m_eSelectedGamePlayMode 字段
                    ALCommon.TryEnumParse(typeof(ETreasureHuntGamePlayMode), fieldsArray[1], out _m_eSelectedGamePlayMode);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[TreasureHuntSaver _initSettingStr] fail infoStr: {_infoStr}, error: {e.Message}");
                }
            }

            #region _m_lInAreaId

            /// <summary>
            /// 设置当前所在的太空区域id
            /// </summary>
            /// <param name="_areaId"></param>
            public void setAreaId(long _areaId)
            {
                if(_m_lInAreaId == _areaId)
                    return;

                _m_lInAreaId = _areaId;
                saveSetting();
            }

            /// <summary>
            /// 获取当前所在的太空区域id
            /// </summary>
            /// <returns></returns>
            public long getAreaId()
            {
                return _m_lInAreaId;
            }

            #endregion

            #region _m_eSelectedGamePlayMode

            /// <summary>
            /// 设置当前选择的游戏玩法模式
            /// </summary>
            /// <param name="_mode"></param>
            public void setSelectedGamePlayMode(ETreasureHuntGamePlayMode _mode)
            {
                if(_m_eSelectedGamePlayMode == _mode)
                    return;

                _m_eSelectedGamePlayMode = _mode;
                saveSetting();
            }

            /// <summary>
            /// 获取当前选择的游戏玩法模式
            /// </summary>
            /// <param name="_mode"></param>
            public ETreasureHuntGamePlayMode getSelectedGamePlayMode()
            {
                return _m_eSelectedGamePlayMode;
            }
            
            #endregion
        }    
    }
}