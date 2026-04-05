using GOE;
namespace Common
{
/****
 子嗣系统错误
 ****/
    class ChildErr
    {
        static ChildErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50001, "子嗣不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50002, "子嗣名称不符合要求"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50003, "子嗣名称已存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50004, "子嗣未命名"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50005, "子嗣训练位不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50006, "子嗣训练位未解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50007, "子嗣训练位没有脑力值"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50008, "子嗣已满级"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50009, "子嗣未满级"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50010, "成年未婚子嗣数量到达上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50011, "联姻请求不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50012, "子嗣状态不是空闲"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50013, "子嗣联姻失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50014, "子嗣状态不是指定联姻请求中"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50015, "子嗣联姻池不匹配"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50016, "不能与自身子嗣进行联姻"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50017, "联姻池子嗣不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50018, "子嗣不存在联姻池"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50019, "没有空闲位置"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50020, "随机获得子嗣失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(50021, "匹配子嗣需要超过最小值"));
        }
    }
}
