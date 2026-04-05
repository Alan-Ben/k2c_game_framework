namespace GOE
{
    public class Example
    {
        Go_CacheMgr _m_goCacheMgr = new Go_CacheMgr("example", 1, 3);

        public void testPop()
        {
            _m_goCacheMgr.popItem(new NPGAudioIndex(), (_actorIndex, _go) =>
            {
                
            });
        }

        public void testPushBack()
        {
            _m_goCacheMgr.pushBackItem(new NPGAudioIndex(), null);
        }
        
        public void discard()
        {
            _m_goCacheMgr.discardAll();
        }
    }
}