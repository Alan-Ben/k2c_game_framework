using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 自动导出的时候根据属性判断对应值是否可为空
    /// </summary>
    public class ALAutoExportVariableAttr : Attribute
    {
        /// <summary>
        /// 是否可为空
        /// </summary>
        public bool canBeEmpty;
        /// <summary>
        /// 如果数据为空是否继续读取
        /// </summary>
        public bool emptyKeepRead;
        /// <summary>
        /// 是否屏蔽不读取
        /// </summary>
        public bool ignoreReading;

        public ALAutoExportVariableAttr(bool _canBeEmpty = true, bool _emptyKeepRead = true, bool _ignoreReading = false)
        {
            canBeEmpty = _canBeEmpty;
            emptyKeepRead = _emptyKeepRead;
            ignoreReading = _ignoreReading;
        }
    }


#if AL_LUA
    [XLua.LuaCallCSharp]
#endif
    public enum ALGUIListLayoutStyle
    {
        HORIZONTAL,
        VERTICAL,
    }

#if AL_LUA
    [XLua.LuaCallCSharp]
#endif
    public partial class ALCommon
    {
        private static string g_sHexStrIdxString = "0123456789abcdef";
        private static System.Random g_rRandom = new System.Random();

        //替代的转化操作
        public static object EnumParse(Type enumType, string value, bool ignoreCase = true)
        {
            if(null == value || value.Length <= 0)
                return 0;

            //这里这样写是因为，枚举解析错误，这种错误比较严重，所以在Editor下就直接不捕获抛异常
#if !UNITY_EDITOR
            try
            {
#endif
            return Enum.Parse(enumType, value, ignoreCase);
#if !UNITY_EDITOR
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("Enum [" + enumType.ToString() + "] Parse err value: " + value);
                return 0;
            }
#endif
        }
        public static bool TryEnumParse<TEnum>(Type enumType, string value, out TEnum res, bool ignoreCase = true) where TEnum : struct
        {
            if(null == value || value.Length <= 0)
            {
                res = default(TEnum);
                return false;
            }

#if !UNITY_EDITOR
            try
            {
#endif
                return Enum.TryParse<TEnum>(value, ignoreCase, out res);
#if! UNITY_EDITOR
            }
            catch (Exception)
            {
                res = default(TEnum);
                UnityEngine.Debug.LogError("Enum [" + enumType.ToString() + "] Parse err value: " + value);
                return false;
            }
#endif
        }
        
        public static float ParseFloat(string _value)
        {
            try
            {
                return float.Parse(_value, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"[{_value}] ParseFloat err:{e}");
                return 0;
            }
        }
        
        public static bool TryParseFloat(string _value, out float _result)
        {
            return float.TryParse(_value,NumberStyles.Float,  CultureInfo.InvariantCulture, out _result);
        }

        public static int ParseInt(string _value)
        {
            try
            {
                return int.Parse(_value);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"[{_value}] ParseInt err:{e}");
                return 0;
            }
        }
        
        public static long ParseLong(string _value)
        {
            try
            {
                return long.Parse(_value);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"[{_value}] ParseLong err:{e}");
                return 0;
            }
        }

        /**********************
         * 获取当前的UCT时间 - 秒
         **/
        public static int getNowTimeSec()
        {
            return decimal.ToInt32(decimal.Divide(System.DateTime.UtcNow.Ticks - 621355968000000000, 10000000));
        }
        public static long getNowTimeMill()
        {
            return decimal.ToInt64(decimal.Divide(System.DateTime.UtcNow.Ticks - 621355968000000000, 10000));
        }

        //将对应时间结构体转化为整形表示
        public static int getTimeSec(DateTime _date)
        {
            return decimal.ToInt32(decimal.Divide(_date.Ticks - 621355968000000000, 10000000));
        }
        public static long getTimeMill(DateTime _date)
        {
            return decimal.ToInt64(decimal.Divide(_date.Ticks - 621355968000000000, 10000));
        }

        public static DateTime getDateTimeByTimeTag(int _timeSec)
        {
            long lTime = ((long)_timeSec * (long)10000000) + 621355968000000000;
            return new DateTime(lTime);
        }
        public static DateTime getDateTimeByTimeTag(long _timeMillSec)
        {
            long lTime = (_timeMillSec * 10000) + 621355968000000000;
            return new DateTime(lTime);
        }

        /**********************
         * 获取当前的时间标记，年月日时分秒yyyyMMddhhmmss
         **/
        public static long getNowTimeTagS()
        {
            System.DateTime nowTime = System.DateTime.Now;

            long tag = nowTime.Year;
            tag = tag * 100 + nowTime.Month;
            tag = tag * 100 + nowTime.Day;
            tag = tag * 100 + nowTime.Hour;
            tag = tag * 100 + nowTime.Minute;
            tag = tag * 100 + nowTime.Second;

            return tag;
        }

        /**************
         * 根据新的小时标记划分不同天，并返回日期的标记
         * @company Isg @author alzq.zhf
         * 2014年10月8日 下午3:07:14
         */
        public static int getDayTagByHourSplit(DateTime _dateTime, int _hour)
        {
            if (_dateTime.Hour < _hour)
                _dateTime.AddDays(-1);

            //拼凑数据
            int tag = _dateTime.Year;
            tag = (tag * 100) + _dateTime.Month;
            tag = (tag * 100) + _dateTime.Day;

            return tag;
        }

        /*****************
         * 设置某一对象的朝向
         **/
        public static void setTransformForward(Transform _transform, Vector3 _forward)
        {
            if (_forward.x == 0)
                _forward.x = 0.001f;
            if (_forward.z == 0)
                _forward.z = 0.001f;

            _transform.rotation = Quaternion.FromToRotation(Vector3.forward, _forward);
        }

        /************
         * 根据位置设置对应字节的对应位置值
         * 
         * @author alzq.z
         * @time   2016年5月18日 上午1:31:48
         */
        public static byte setByteTag(byte _srcValue, int _pos, bool _isTrue)
        {
            if (_isTrue)
                return (byte)(_srcValue | (0x01 << _pos));
            else
                return (byte)(_srcValue & ~(0x01 << _pos));
        }
        public static bool getByteTag(byte _srcValue, int _pos)
        {
            return ((_srcValue & (0x01 << _pos)) != 0);
        }

        /*****************
         * 将2个int转化为long对象
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 11:30:30 AM
         */
        public static long mergeInt(int _i1, int _i2)
        {
            long l1 = _i1;
            long l2 = _i2;
            return (l1 << 32) | l2;
        }


        /*****************
         * 将字节转化为int对象
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 11:30:30 AM
         */
        public static int byte2int(byte value) { return (int)value & 0xFF; }

        /**
         * 将字节数组转换成十六进制字符串
         * @param iDelta
         * @return
         */
        public static string getHexString(byte[] _buf)
        {
            if (null == _buf)
                return null;

            //逐个字节处理
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _buf.Length; i++)
            {
                byte b = _buf[i];

                //取前4位
                builder.Append(g_sHexStrIdxString[b >> 4]);
                //取后4位
                builder.Append(g_sHexStrIdxString[b & 0x0F]);
            }

            return builder.ToString();
        }

        /**********************
         * 获取随机的编码数组
         **/
        public static byte[] getRndHexBytes(int _len)
        {
            byte[] res = new byte[_len];

            for (int i = 0; i < _len; i++)
            {
                res[i] = (byte)UnityEngine.Random.Range(0, 256);
            }

            return res;
        }

        /**
         * 将字节数组转换成十六进制字符串
         * @param iDelta
         * @return
         */
        public static byte[] getHexBytes(string _hexStr)
        {
            if(null == _hexStr)
                return null;

            //创建结果数组
            byte[] res = new byte[_hexStr.Length / 2];

            int pos = 0;
            //逐个字符进行处理
            for(int i = 0; i < _hexStr.Length; i++)
            {
                char c = _hexStr[i];

                //查询c所在的索引
                byte prePos = (byte)g_sHexStrIdxString.IndexOf(c);

                //增加索引
                i++;

                c = _hexStr[i];

                //查询c所在的索引
                byte secPos = (byte)g_sHexStrIdxString.IndexOf(c);

                //计算具体数值
                res[pos] = (byte)((prePos << 4) | secPos);
                //下标加1
                pos++;
            }

            return res;
        }
        public static byte[] getHexBytes(char[] _hexStr)
        {
            if(null == _hexStr)
                return null;

            //创建结果数组
            byte[] res = new byte[_hexStr.Length / 2];

            int pos = 0;
            //逐个字符进行处理
            for(int i = 0; i < _hexStr.Length; i++)
            {
                char c = _hexStr[i];

                //查询c所在的索引
                byte prePos = (byte)g_sHexStrIdxString.IndexOf(c);

                //增加索引
                i++;

                c = _hexStr[i];

                //查询c所在的索引
                byte secPos = (byte)g_sHexStrIdxString.IndexOf(c);

                //计算具体数值
                res[pos] = (byte)((prePos << 4) | secPos);
                //下标加1
                pos++;
            }

            return res;
        }
        /// <summary>
        /// 从带入的起始位置偏移开始，对十六进制文本进行解码
        /// </summary>
        /// <param name="_hexStr"></param>
        /// <param name="_startIdx"></param>
        /// <returns></returns>
        public static byte[] getHexBytes(string _hexStr, int _startIdx)
        {
            if(null == _hexStr)
                return null;

            //创建结果数组
            byte[] res = new byte[(_hexStr.Length - _startIdx) / 2];

            int pos = 0;
            //逐个字符进行处理
            for(int i = _startIdx; i < _hexStr.Length; i++)
            {
                char c = _hexStr[i];

                //查询c所在的索引
                byte prePos = (byte)g_sHexStrIdxString.IndexOf(c);

                //增加索引
                i++;

                c = _hexStr[i];

                //查询c所在的索引
                byte secPos = (byte)g_sHexStrIdxString.IndexOf(c);

                //计算具体数值
                res[pos] = (byte)((prePos << 4) | secPos);
                //下标加1
                pos++;
            }

            return res;
        }

        /*********************
         * 获取指定长度的随机十六进制字符串
         * @company Isg @author alzq.zhf
         * 2014-4-15 下午2:59:21
         */
        public static string getRndDexStr(int _len)
        {
            StringBuilder builder = new StringBuilder(_len);

            for (int i = 0; i < _len; i++)
            {
                int rndNum = g_rRandom.Next(0, g_sHexStrIdxString.Length);
                //插入字符串结尾
                builder.Append(g_sHexStrIdxString[rndNum]);
            }

            return builder.ToString();
        }

        /**************
         * 获取枚举个数
         **/
        public static int getEnumCount(System.Type _enumType)
        {
            return System.Enum.GetValues(_enumType).Length;
        }

        /// <summary>
        /// 传入一个路径，为路径末尾补上斜杠符号
        /// </summary>
        public static string directoryInsure(string _path)
        {
            if(string.IsNullOrEmpty(_path))
            {
                Debug.LogError("[directoryInsure] 传入的path不是一个路径");
                return string.Empty;
            }

            _path = _path.Replace("\\", "/");

            // 判断末尾是不是斜杠结尾
            if(_path[_path.Length - 1] != '/')
            {
                _path += "/"; // 如果不是就加上斜杠
            }
            return _path;
        }
        public static string urlEndInsure(string _path)
        {
            if(string.IsNullOrEmpty(_path))
            {
                Debug.LogError("[directoryInsure] 传入的path不是一个路径");
                return string.Empty;
            }

            // 判断末尾是不是斜杠结尾
            if(_path[_path.Length - 1] != '/')
            {
                _path += "/"; // 如果不是就加上斜杠
            }
            return _path;
        }


        /// <summary>
        /// 生成对应文件的md5值
        /// </summary>
        public static string generateMD5(string _filePath)
        {
            FileInfo file = new FileInfo(_filePath);
            if (!file.Exists)
            {
                UnityEngine.Debug.LogError($"[generateMD5] 目标文件不存在：\"{_filePath}");
                return string.Empty;
            }

            return generateMD5(file);
        }
        public static string generateMD5(FileInfo _file)
        {
            if (_file == null || !_file.Exists)
            {
                UnityEngine.Debug.LogError($"[generateMD5] 目标文件不存在");
                return string.Empty;
            }

            string output = string.Empty;
            try
            {
                using (FileStream fileStream = _file.OpenRead())
                {
                    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                    byte[] md5Byte = md5.ComputeHash(fileStream);
                    output = BitConverter.ToString(md5Byte).Replace("-", "");
                }
            }
            catch (Exception _ex)
            {
                UnityEngine.Debug.LogError($"[generateMD5] 生成MD5发生错误：\"{_ex}");
            }

            return output;
        }

        /// <summary>
        /// 删除目录下的一级文件，不删除子目录及本目录
        /// </summary>
        /// <param name="srcPath"></param>
        public static void DelectDirectoryFiles(string _srcPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(_srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        continue;
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception )
            {
                throw;
            }
        }

        /// <summary>
        /// 打印方便阅读的String
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static string ToReadableString(Delegate _delegate)
        {
            if (_delegate != null)
            {
                //                    string className = _delegate.Method.ReflectedType != null ? _delegate.Method.ReflectedType.FullName : "null";
                string targetId;
                try
                {
                    targetId = _delegate.Target != null ? _delegate.Target.GetHashCode().ToString() : "null";
                }
                catch (Exception )
                {
                    targetId = "null";//ILRuntime的GetHashCode有时候会报空。。先catch起来吧
                }
                return $"【{_delegate.Target}】  ->  {_delegate.Method.Name}  ->  {targetId}";
            }
            return "null";
        }

        /// <summary>
        /// 判断一个路径是否保护扩展名
        /// </summary>
        public static bool hasExtension(string _path)
        {
            if (string.IsNullOrEmpty(_path))
                return false;
            
            // 检查最后一个部分中是否有 '.' 且不在结尾
            int dotIndex = _path.LastIndexOf('.');
        
            // '.' 存在并且后面有字符，表示有扩展名
            return dotIndex > 0 && dotIndex < _path.Length - 1;
        }
    }
}
