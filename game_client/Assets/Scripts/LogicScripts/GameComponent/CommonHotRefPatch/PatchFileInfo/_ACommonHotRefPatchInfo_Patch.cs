using System;
using System.IO;
using ALPackage;
using LitJson;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 补丁处理
    /// </summary>
    public abstract partial class _ACommonHotRefPatchInfo
    {
        //处理打补丁文件
        private bool _dealPatchFile()
        {
            try
            {
                //是否全部成功
                bool finalIsSuc = true;

                using (TextReader textReader = new StreamReader(patchFilePath))
                {
                    string allContent = textReader.ReadToEnd();
                    JsonData root = JsonMapper.ToObject(allContent);
                    //每张表单独循环更新
                    foreach (string tableName in root.Keys)
                    {
                        //服务器用的表忽略
                        if(CommonHotRefPatchDealerMgr.instance.isOnlyServerTable(tableName))
                            continue;
                        
                        _ICommonHotRefPatchDealer dealer = CommonHotRefPatchDealerMgr.instance.getPatchDealer(tableName);
                        if (null == dealer)
                        {
                            finalIsSuc = false;
                            //日志输出
                            UnityEngine.Debug.LogError($"配表补丁要补丁的表，程序内部没有注册Dealer，请检查，表名：{tableName}");
                            continue;
                        }
                        bool isSuc = dealer.patchHotRefData(root[tableName]);
                        
                        //如果一张表不成功，总的也失败
                        if (isSuc == false)
                        {
                            finalIsSuc = false;
                            //日志输出
                            UnityEngine.Debug.LogError($"配表补丁要补丁的表，程序内Dealer补丁失败，请检查，表名：{tableName}");
                        }
                    }
                }

                return finalIsSuc;
            }
            catch (Exception e)
            {
                //日志输出
                UnityEngine.Debug.LogError($"更新配表补丁错误: {patchFilePath}\n{e}");
                //删除对应补丁文件
                _deleteLocalFile();
                return false;
            }
        }
    }
}