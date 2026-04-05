using LitJson;

namespace GOE
{
    //每个配表的补丁接口
    public interface _ICommonHotRefPatchDealer
    {
        //获取表名
        public string getTableName { get; }
        
        //打补丁
        public bool patchHotRefData(JsonData _data);
    }
}