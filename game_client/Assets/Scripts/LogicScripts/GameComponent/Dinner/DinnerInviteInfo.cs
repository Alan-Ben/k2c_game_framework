namespace GOE
{
    public class DinnerInviteInfo
    {
        private long _m_playerCid;
        public long cid =>_m_playerCid;
        //
        // public int joinMyNum;
        

        public DinnerInviteInfo(long _cid)
        {
            _m_playerCid = _cid;
        }
    }
}