using GOE;
namespace Common
{
/****
 排行榜错误
 ****/
    class RankErr
    {
        static RankErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(100001, "跨服分组实例不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(100002, "排行对象不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(100003, "排行榜操作失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(100004, "跨服排行榜服务器承载失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(100005, "常驻排行榜未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(100006, "常驻排行榜找不到点赞对象"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(100007, "常驻排行榜点赞次数不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(100008, "常驻排行榜一键点赞功能未解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(100009, "该常驻排行榜不支持点赞"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(100010, "常驻排行榜没有可点赞目标"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(100011, "排行item数据不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(100012, "未参加排行"));
        }
    }
}
