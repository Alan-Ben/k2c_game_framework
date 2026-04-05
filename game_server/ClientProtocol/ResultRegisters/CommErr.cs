using GOE;
namespace Common
{
/****
 通用错误
 ****/
    class CommErr
    {
        static CommErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10001, "系统错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10002, "配表找不到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10003, "系统忙"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10004, "未知错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10005, "参数错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10006, "远程调用失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10007, "消耗失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10008, "物品不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10009, "文件读取错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10010, "玩家找不到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10011, "系统未开放"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10012, "价格配置错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10013, "当前不是跨服分组"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10014, "玩家缓存错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10015, "配表错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10017, "道具消耗计算错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10018, "数量超过限制"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10019, "不满足条件"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10020, "操作无效"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10021, "数据对象状态变化引发的通用错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10022, "数据对象错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10023, "通用协议处理错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10024, "CID格式错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10025, "US服务器找不到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10026, "数据已丢失"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10027, "处理超时"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10028, "签名校验失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10029, "参数数量错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10030, "系统暂未开放，请耐心等待"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(10031, "奖励物品未找到"));
        }
    }
}
