using System.Collections.Generic;
using System.Text;
using IlRuntimeLitJson;
using UnityEngine;

namespace Hotfix
{
    public class TileMatchUtil
    {
        public static readonly Vector2Int inVaildLogicPos = new Vector2Int(-1, -1);//无效的逻辑坐标

        public static int row  = 7;//行数
        public static int column = 6;//列数
        
        static TileMatchUtil()
        {
            if (HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj != null &&
                HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj.tilematch_map_size != Vector2Int.zero)
            {
                row = HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj.tilematch_map_size.x;
                column = HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj.tilematch_map_size.y;
            }
        }
        
        public static Vector2Int indexToVector2(int _index)
        {
            //所在行,所在列
            return new Vector2Int(_index % column,_index / column);
        }

        public static int vector2ToIndex(Vector2Int _v2)
        {
            return _v2.x + _v2.y * column;
        }

        /// <summary>
        /// 获取_pos2对于_pos1的方向
        /// </summary>
        /// <returns></returns>
        public static ETileMatchDirection getPosDirection(Vector2Int _pos1, Vector2Int _pos2)
        {
            if (_pos1.x == _pos2.x)
            {
                if (_pos1.y - _pos2.y > 0)
                    return ETileMatchDirection.Down;
                else if (_pos1.y - _pos2.y < 0)
                    return ETileMatchDirection.Up;
            }
            else if(_pos1.y == _pos2.y)
            {
                if (_pos1.x - _pos2.x > 0)
                    return ETileMatchDirection.Left;
                else if (_pos1.x - _pos2.x < 0)
                    return ETileMatchDirection.Right;
            }
            return ETileMatchDirection.None;
        }

        #region 协议输出

        public static string getTileMatch_CombineRemoveStr(Hotfix.Common.TileMatchObj.TileMatch_CombineRemove _serverData)
        {
            if (_serverData == null)
                return string.Empty;
            
            return $"{{" +
                   $"mainTriggerBlockIndex:{_serverData.getMainTriggerBlockIndex()}, " +
                   $"subTriggerBlockIndex:{_serverData.getSubTriggerBlockIndex()}, " +
                   $"removeBlockIndexList:{JsonMapper.ToJson(_serverData.getRemoveBlockIndexList())}, " +
                   $"}}";
        }
        
        public static string getTileMatch_CompositeStr(Hotfix.Common.TileMatchObj.TileMatch_Composite _serverData)
        {
            if (_serverData == null)
                return string.Empty;
            
            return $"{{" +
                   $"removeBlockIndexList:{JsonMapper.ToJson(_serverData.getRemoveBlockIndexList())}, " +
                   $"genBlockList:{getServerDataListStr(_serverData.getGenBlockList())}, " +
                   $"}}";
        }

        public static string getTileMatch_DropStr(Hotfix.Common.TileMatchObj.TileMatch_Drop _serverData)
        {
            if (_serverData == null)
                return string.Empty;
            
            return $"{{" +
                   $"posChgList:{getServerDataListStr(_serverData.getPosChgList())}, " +
                   $"genBlockList:{getServerDataListStr(_serverData.getGenBlockList())}, " +
                   $"}}";
        }
        
        public static string getTileMatch_RainbowTransStr(Hotfix.Common.TileMatchObj.TileMatch_RainbowTrans _serverData)
        {
            if (_serverData == null)
                return string.Empty;
            
            return $"{{" +
                   $"triggerBlockIndex:{_serverData.getTriggerBlockIndex()}, " +
                   $"newBlockId:{_serverData.getNewBlockId()}, " +
                   $"beTransIndexList:{JsonMapper.ToJson(_serverData.getBeTransIndexList())}, " +
                   $"}}";
        }

        public static string getTileMatch_RemoveStr(Hotfix.Common.TileMatchObj.TileMatch_Remove _serverData)
        {
            if (_serverData == null)
                return string.Empty;
            
            return $"{{" +
                   $"triggerBlockIndex:{_serverData.getTriggerBlockIndex()}, " +
                   $"removeBlockIndexList:{JsonMapper.ToJson(_serverData.getRemoveBlockIndexList())}, " +
                   $"}}";
        }
        
        public static string getServerDataListStr<T>(List<T> _serverDataList) where T : ALBasicProtocolPack._IALProtocolStructure
        {
            if (_serverDataList == null || _serverDataList.Count <= 0)
                return "[]";
            
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0, count = _serverDataList.Count; i < count; i++)
            {
                if (i != 0)
                    sb.Append(", ");
                sb.Append($"{_serverDataList[i]}");
            }
            sb.Append("]");

            return sb.ToString();
        }

        #endregion
    }
}