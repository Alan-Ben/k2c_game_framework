using GOE;
namespace Common
{
/****
 SS服务器活动排期错误
 ****/
    class SSActivityScheduleErr
    {
        static SSActivityScheduleErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(500001, "解析内容错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(500002, "无效分区ID"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(500003, "不是最新数据"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(500004, "生成文件失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(500005, "下载文件失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(500006, "排期不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(500007, "版本号过低"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(500008, "排期未激活"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(500009, "分组列表不匹配"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(500010, "分组列表重复"));
        }
    }
}
