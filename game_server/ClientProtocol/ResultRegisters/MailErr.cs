using GOE;
namespace Common
{
/****
 邮件错误
 ****/
    class MailErr
    {
        static MailErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(130001, "邮件不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(130002, "没有可领取的物品"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(130003, "邮件物品已经领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(130004, "已达收藏邮件的上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(130005, "有物品的邮件不能删除"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(130006, "收藏邮件不能删除"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(130007, "必读未读的邮件不能删除"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(130008, "必读未读完的邮件不能删除"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(130009, "邮件不能领取"));
        }
    }
}
