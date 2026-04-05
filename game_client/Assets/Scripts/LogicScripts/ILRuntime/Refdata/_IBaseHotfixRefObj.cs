using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的单个从string类型数据导入的refobj接口对象
    /// 用户热更工程配表继承，解析自身数据
    /// </summary>
    public interface _IBaseHotfixRefObj : _IALBasicRefObj
    {
        void parseFromString(string _line, string[] _titleArray, string _contextString);
    }
}