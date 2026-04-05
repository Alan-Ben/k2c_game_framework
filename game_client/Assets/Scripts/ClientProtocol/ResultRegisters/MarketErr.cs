using GOE;
namespace Common
{
/****
 集市相关错误
 ****/
    class MarketErr
    {
        static MarketErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(200001, "集市尚未解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(200002, "集市已经满级"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(200003, "集市经营次数不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(200004, "集市CD未回复"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(200005, "集市一键经营未解锁"));
        }
    }
}
