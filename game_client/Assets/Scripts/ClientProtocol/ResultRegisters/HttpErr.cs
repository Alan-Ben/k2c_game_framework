using GOE;
namespace Common
{
/****
 后台相关报错
 ****/
    class HttpErr
    {
        static HttpErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(270001, "HTTP请求响应失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(270002, "HTTP请求平台返回错误码"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(270003, "HTTP请求平台返回数据"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(270004, "HTTP请求平台返回数据解析错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(270005, "找不到公告"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(270006, "公告奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(270007, "找不到问卷奖励"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(270008, "问卷活动不存在（玩家不可见）"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(270009, "问卷奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(270010, "问卷奖励重复添加（玩家不可见）"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(270011, "通知角色游戏事件类型找不到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(270012, "问卷奖励列表解析失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(270013, "问卷活动重复"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(270014, "问卷活动已关闭"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(270015, "平台消息处理错误"));
        }
    }
}
