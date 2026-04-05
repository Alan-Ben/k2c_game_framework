using ALBasicProtocolPack;

namespace GOE
{
    public abstract class NPSubDealer<T> : _AALBasicProtocolSubBasicOrderDealer<T> where T : _IALProtocolStructure
    {
        protected abstract void _dealProtocolByLog(_IALProtocolDealer _dealer, T _msg);

        protected virtual bool needPrintProtocol => true;
        
        protected override void _dealProtocol(_IALProtocolDealer _dealer, T _msg)
        {
            if(Game.instance.mainCamera.gameSetting.printProtocol && needPrintProtocol)
            {
                if (_msg.GetFullPackBufSize() <= Game.instance.mainCamera.gameSetting.protocolPrintMinSize)
                {
                    GCommon.NetRecv(string.Format("<color=red>S -> C: {0} ; </color> {1}", _msg.GetType().Name, GCommon.GetInfoPropertys(_msg)));
                }
                else
                {
                    GCommon.NetWaring($"协议{_msg.GetType().Name}过大，大小：{_msg.GetFullPackBufSize() / 1024}kb。不打印，注意检查");
                }
            }
            
            
#if UNITY_EDITOR
            if (null != _msg && _msg.GetFullPackBufSize() > Game.instance.mainCamera.gameSetting.protocolErrorMinSize)
            {
                Debug.LogError($"协议大小超过20k: {_msg.GetType().Name}，大小：{_msg.GetFullPackBufSize()}");
            }
#endif
            
            _dealProtocolByLog(_dealer, _msg);
        }
    }
}
