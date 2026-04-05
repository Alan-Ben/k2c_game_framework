using UnityEngine;

namespace GOE.MiniGame
{
    public class PuzzleGameMatchItemMatchSuccessState : _APuzzleGameMatchItemState<Vector3>
    {
        public PuzzleGameMatchItemMatchSuccessState(PuzzleGameMatchItemUnit _matchItemUnit) : base(_matchItemUnit)
        {
        }

        public override EPuzzleGameMatchItemState state { get { return EPuzzleGameMatchItemState.MATCH_SUCCESS; } }
        
        protected override void _onEnterSub(Vector3 _position)
        {
            _m_matchItemUnit.openOpMask();//打开操作屏蔽
            
            _m_matchItemUnit.setItemPosition(_position);//设置item位置
            _m_matchItemUnit.setItemShowState(EPuzzleGameMatchItemState.MATCH_SUCCESS);
        }
        
        protected override void _onExitSub()
        {
        }

        public override bool canEnterState(EPuzzleGameMatchItemState _newState)
        {
            return _newState is EPuzzleGameMatchItemState.NONE;
        }
    }
}