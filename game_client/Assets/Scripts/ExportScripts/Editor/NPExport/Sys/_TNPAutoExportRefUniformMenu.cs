using System;
using System.Collections.Generic;

using System.IO;
using UnityEngine;
using UnityEditor;
using ALPackage;
using Excel;
using NPEnum;
using SQLite4Unity3d;



namespace GOE
{
	//第一个类型是模板类, 第二个类型是表的集合类
	/// <summary>
	/// 需要走uniform的导出类
	/// </summary>
	/// <typeparam name="Tobj"></typeparam>
	/// <typeparam name="TMap"></typeparam>
	public class _TNPAutoExportRefUniformMenu<Tobj, TMap> : NPBasicAutoExportMenuItemTwoTemp<Tobj, UniformItemRefObjBase, Tobj, TMap> 
	//    where TTemp : _IALBasicRefObj, new()
	//    where TTempTwo : UniformItemRefObjBase, new()
	    where Tobj : _IALBasicRefObj, new() 
	    where TMap : _TALSOBasicRefSet<Tobj>, new()
	{
	    private string _m_sMenuStr;
	    private ENPItemType _m_enpItemType;
    
	    public _TNPAutoExportRefUniformMenu(ENPExportSettingEnum _exprotEnum, string _assetPath, string _objName, string _tag, string _menuText, ENPItemType _itemType, Func<string, string, bool> _judgeCanShowFunc)
	        : base(_exprotEnum, _assetPath, _objName, _tag, _judgeCanShowFunc)
	    {
	        _m_sMenuStr = _menuText;
	        _m_enpItemType = _itemType;
	    }

	    /****************
	     * 显示的菜单文字
	     **/
	    protected override string _menuText { get { return _m_sMenuStr; } }
    
	    public override List<Tobj> _exchangeTemplate(List<Tobj> _tempList, List<UniformItemRefObjBase> _tempTwoList)
	    {
	        //实际导出的asset数据列表
	        List<Tobj> result = new List<Tobj>();
        
	        if (_tempList == null || null == _tempTwoList)
	        {
	            Debug.LogError("_exchangeTemplate  => _tempList, __tempTwoList == null ");
	            return result;
	        }

	        if (_tempList.Count != _tempTwoList.Count)
	        {
	            Debug.LogError("refobj跟uniform的导出数据不一致，停止导出，请注意");
	            return result;
	        }
      
	        //refobj数据
	        result.AddRange(_tempList);
        
	        //导出到Uniform的数据
	        List<UniformItemObj> uniformItemList = new List<UniformItemObj>();
        
	        for (int i = 0; i < _tempTwoList.Count; i++)
	        {
	            if (null == _tempTwoList[i] || null == _tempList[i])
	                continue;
            
	            //加到uniform数据里
	            uniformItemList.Add(new UniformItemObj(_m_enpItemType.ToString() ,_tempList[i]._refId, _tempTwoList[i]));
	        }
        
	        //导出uniform
	        export(uniformItemList);

	        return result;
	    }
    
    
	    /// <summary>
	    /// 直接导出uniform数据库
	    /// </summary>
	    public static void export(List<UniformItemObj> _refObjList)
	    {
	        if (_refObjList == null || _refObjList.Count == 0)
	            return;

	        string dbPath = string.Format(@"{0}/Resources/Refdata/__DLExport/{1}", Application.dataPath, UniformItemAsset.AssetObjName);

	        Debug.Log("uniform DB Final PATH: " + dbPath);
	        SQLiteConnection ds = new SQLiteConnection(dbPath);
	        try
	        {
	            // 创建表, 执行sql语句 create table if not exists
	            ds.CreateTable<UniformItemObj>();

	            //使用replace 批量替换/插入数据
	            foreach (UniformItemObj refObj in _refObjList)
	            {
	                //类型一样且子表ID一样的, 删除掉，防止重复数据和冗余数据
	                string del = string.Format("delete from \"{0}\" where ItemType = \'{1}\' and SubId = \'{2}\'", UniformItemAsset.TableName, refObj.ItemType, refObj.SubId);
	                SQLiteCommand cmdDel = ds.CreateCommand(del);
	                cmdDel.ExecuteNonQuery();

	                //插入给定对象并检索其自动递增的主键(如果有),如果某个已有对象发生UNIQUE约束违例,则此函数将删除旧对象。
	                ds.InsertOrReplace(refObj);
	            }

	            //提交事务
	            ds.Commit();
	        }
	        catch (Exception ex)
	        {
	            ds.Rollback();
	            Debug.LogException(ex);
	        }
	        finally
	        {
	            ds.Execute("VACUUM");
	            //ds.Close();
	            ds.Dispose();
      
	        }
        
	        // 获取数据库中数据, 存储到uniform_review.txt中
	        ds = new SQLiteConnection(dbPath);
	        List<UniformItemObj> resultList = null;
	        try
	        {
	            // 读取全部数据
	            string sql = string.Format("select * from \"{0}\" order by \"Id\"", UniformItemAsset.TableName);
	            resultList = ds.Query<UniformItemObj>(sql);
	        }
	        catch (Exception ex)
	        {
	            Debug.LogException(ex);
	        }
	        finally
	        {
	            //ds.Close();
	            ds.Dispose();
	        }
#if UNITY_EDITOR
	        UniformToTxt.WriteToTxtFile(resultList);
#endif
        
	        //文件目录没有在Scripts\MGExport\Editor下
#if UNITY_EDITOR
	        string path = "Assets/Resources/Refdata/__DLExport/" + UniformItemAsset.AssetObjName;
	        //Debug.LogError("path:   " + path);
	        AssetDatabase.Refresh();
	        AssetImporter assetImporter = AssetImporter.GetAtPath(path);

	        if (null == assetImporter)
	        {
	            UnityEngine.Debug.LogError("get AssetImporter is null: " + path);
	            return;
	        }
	        //设置Bundle文件的名称
	        assetImporter.assetBundleName = UniformItemAsset.AssetPath.Replace(".unity3d", "");
	        //设置Bundle文件的扩展名
	        assetImporter.assetBundleVariant = "unity3d";
#endif

	    }
	}

}