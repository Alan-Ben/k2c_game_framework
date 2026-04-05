using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

#if AL_SQLITE
using SQLite4Unity3d;
#endif

using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace ALPackage
{
#if AL_SQLITE
    /// <summary>
    /// 使用对应的结构体数据队列，用SQLite格式导出到指定位置数据库文件的导出处理函数
    /// </summary>
    public class ALSQLiteExportFunction
    {
        public static void exportDB<T>(List<T> _dataList, string _dbPath, string _tableName, bool _createNewDBFile = false)
        {
            //获取完整路径，并根据文件夹进行创建操作
            string folderPath = _dbPath.Substring(0, _dbPath.LastIndexOf('/') + 1);

            //检查文件夹是否有效
            ALBasicExportFunction.checkFolder(folderPath);

            //构建数据库文件路径
            string dbPath = string.Format(@"{0}/Resources/Refdata/__DLExport/{1}", Application.dataPath, _dbPath);

            //判断文件是否存在，根据带入参数判断是否删除旧文件
            if (_createNewDBFile && File.Exists(dbPath))
                File.Delete(dbPath);

            //创建连接对象
            SQLiteConnection ds = new SQLiteConnection(dbPath);
            try
            {
                //尝试丢弃自动创建的数据表，保证不会因为旧数据处理到一半而出现错误数据
                ds.DropTable<T>();
                //再删除需要创建的目标表，保证数据清除
                string del = string.Format("DROP TABLE if EXISTS \"{0}\"", _tableName);
                var cmdDel = ds.CreateCommand(del);
                cmdDel.ExecuteNonQuery();

                //使用Sqlite自带的创建代码进行数据表创建和插入，减少无谓代码
                ds.CreateTable<T>();

                //把数据插入自动创建的表，使用Sqlite库的系统函数
                ds.InsertAll(_dataList);

                //将Sqlite自动创建的表重命名为新表，用重命名的方式直接更改即可
                //先通过底层接口获取对应创建表的相关映射数据
                TableMapping map = ds.GetMapping(typeof(T));
                string query = string.Format("ALTER TABLE {0} RENAME TO {1}", map.TableName, _tableName);
                var cmd = ds.CreateCommand(query);

                cmd.ExecuteNonQuery();
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
                //VACUUM可以保证存储数据和索引用最大限度的连续方式存储在文件中
                ds.Execute("VACUUM");
                ds.Close();
            }
        }

        /*****************
         * 根据带入的对象以及导出路径将对象保存到对应路径，并返回最新的对象路径
         * 默认带入的路径格式为xxx/xxxx
         **/
        public static string importDB(string _dbPath)
        {
            string fullAssetPath = "Assets/Resources/Refdata/__DLExport/" + _dbPath;
            //导入资源
            AssetDatabase.ImportAsset(fullAssetPath);

            //返回导出对象的asset路径
            return fullAssetPath;
        }
        /*****************
         * 导出对应对象，并设置对应对象的assetbundle路径信息
         **/
        public static void setDBExportPath(string _dbPath, string _assetBundlePath)
        {
            string realAssetPath = importDB(_dbPath);

            //设置对应的assetbundle路径
            ALAssetBundleCommon.setFileAssetBundleName(realAssetPath, _assetBundlePath);
        }
        public static void setDBExportPath(string _dbPath, string _assetBundlePath, string _varianName)
        {
            string realAssetPath = importDB(_dbPath);

            //设置对应的assetbundle路径
            ALAssetBundleCommon.setFileAssetBundleName(realAssetPath, _assetBundlePath.Replace("." + _varianName, ""), _varianName);
        }
    }
#endif
}

