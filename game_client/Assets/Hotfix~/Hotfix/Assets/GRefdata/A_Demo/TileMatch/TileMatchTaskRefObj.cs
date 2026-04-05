using System.Collections.Generic;

namespace Hotfix
{
    public class TileMatchTaskRefObj : _AHotfixBaseRefObj
    {
        public override long _refId { get { return id; } }

        public long id;//任务id
        public long gain_score;//获得的积分
        public int step_limit;//步数要求
        public List<int> chess_pieces_num;//任务要求(棋子数量)
        
        protected override void _parseFromString(string _line)
        {
            id = getLong("id");
            gain_score = getLong("gain_score");
            step_limit = getInt("step_limit");
            chess_pieces_num = getList<int>("chess_pieces_num");
        }
        
        public static string assetPath { get { return "refdata/hotfix_refdata.unity3d"; } }
        public static string objName { get { return "tilematch_task"; } }
    }
}