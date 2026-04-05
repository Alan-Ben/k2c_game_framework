package NPCommon.RefData;

import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;
import NPCommon.RefData.Ref.Matcher.NumberRange;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefReloader.RefDataReloader;
import NPCommon.Util.CommClass;
import NPCommon.Util.CommFile;
import NPCommon.Util.ListList;

import java.io.File;
import java.io.IOException;
import java.lang.reflect.*;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

public abstract class AbstractRefDataMgr
{
    /**
     * 是否开启了初始化，为了避免重复初始化而使用的变量
     */
    private boolean _m_bIsInited;
    private boolean _m_bInitRes;

    /**
     * 数据容器队列
     */
    private ArrayList<RefContainerBase<?>> _m_lContainerList;

    /**
     * 配表重加载管理器
     */
    private RefDataReloader _m_objRefDataReloader;

    protected AbstractRefDataMgr()
    {
        _m_bIsInited = false;
        _m_bInitRes = false;

        _m_lContainerList = new ArrayList<RefContainerBase<?>>();
        _m_objRefDataReloader = new RefDataReloader(this);
    }

    public ArrayList<RefContainerBase<?>> getAllContainerList()
    {
        return new ArrayList<>(_m_lContainerList);
    }

    public RefDataReloader getRefDataReloader()
    {
        return _m_objRefDataReloader;
    }

    /************
     * 加载数据的处理函数
     *
     * @param _refReloadThreadIdx 指定一个线程idx专门执行配表重载
     * @author alzq.z
     * @time 2019年4月3日 下午8:28:28
     */
    public synchronized boolean loadData(int _refReloadThreadIdx)
    {
        //这里不重复进行初始化
        if(_m_bIsInited)
            return _m_bInitRes;

        _m_bIsInited = true;

        ALServerLog.Info("=======开始加载配表");
        // 加载 准备工作
        try
        {
            // 初始化
            _init(_refReloadThreadIdx);
            // 获取路径
            String refdataPath = getRefPath();
            // 检测路径是否合法
            if (!_checkDirects(refdataPath))
            {
                _m_bInitRes = false;
                return false;
            }

            //将目录下文件全部改为小写文件名
            renameRefFilesToLowcase();
            // 加载 配表信息
            if (!load(RefBase.class, getLoadRefdataClassPath()))
            {
                _m_bInitRes = false;
                return false;// 默认加载
            }

            ALServerLog.Info("=======配表加载结束");
            _m_bInitRes = true;
            return true;
        } catch (Exception e)
        {
            CommLog.error("AbstractRefDataMgr load failed", e);

            _m_bInitRes = false;
            return false;
        }
    }

    /************
     * 重新加载数据的处理函数
     *
     * @author alzq.z
     * @time 2019年4月3日 下午8:28:28
     */
    public boolean reloadData(int _refReloadThreadIdx)
    {
        ALServerLog.Info("=======开始重新加载配表");
        // 加载 准备工作
        try
        {
            // 初始化
            _init(_refReloadThreadIdx);
            // 获取路径
            String refdataPath = getRefPath();
            // 检测路径是否合法
            if (!_checkDirects(refdataPath))
                return false;

            //将目录下文件全部改为小写文件名
            renameRefFilesToLowcase();
            // 加载 配表信息
            if (!reload(RefBase.class, getLoadRefdataClassPath()))
                return false;// 默认加载

            ALServerLog.Info("=======配表重新加载结束");
            return true;
        } catch (Exception e)
        {
            CommLog.error("AbstractRefDataMgr reload failed", e);
            return false;
        }
    }

    /**************
     * 根据类加载本类目录下的相关子类信息
     *
     * @author alzq.z
     * @time 2019年4月3日 下午8:37:38
     */
    public <T extends RefBase> boolean load(Class<T> _clazz, String _packagePath)
    {
        List<Class<?>> refdatas = CommClass.getAllClassByInterface(_clazz, _packagePath);

        //遍历所有子类进行处理
        for (Class<?> cs : refdatas)
        {
            RefContainerBase<? extends RefBase> loadedDataMgr = _loadSingleClassData(cs);

            //此处不报错，因为上面的loadSingleClassData函数已经进行了报错处理
            if (null == loadedDataMgr)
                continue;

            //加入队列
            _m_lContainerList.add(loadedDataMgr);
        }
        return true;
    }

    /**************
     * 根据类加载本类目录下的相关子类信息
     *
     * @author alzq.z
     * @time 2019年4月3日 下午8:37:38
     */
    public boolean load(List<Class<? extends RefBase>> refdatas)
    {
        //遍历所有子类进行处理
        for (Class<?> cs : refdatas)
        {
            RefContainerBase<? extends RefBase> loadedDataMgr = _loadSingleClassData(cs);

            //此处不报错，因为上面的loadSingleClassData函数已经进行了报错处理
            if (null == loadedDataMgr)
                continue;

            //加入队列
            _m_lContainerList.add(loadedDataMgr);
        }
        return true;
    }

    /*********************
     * 加载单个类的数据，并返回数据集合对象
     *
     * @author alzq.z
     * @time 2019年4月8日 下午10:59:55
     */
    protected <T extends RefBase> RefContainerBase<? extends RefBase> _loadSingleClassData(Class<?> _cs)
    {
        RefBase refdata = null;
        try
        {
            refdata = (RefBase) CommClass.forName(_cs.getName()).newInstance();
        } catch (Exception e)
        {
            CommLog.error("onAutoLoad class:{} occured Error:", _cs.getSimpleName(), e);
            return null;
        }
        if (null == refdata)
        {
            return null;
        }

        //使用默认类名作为文件名
        String name = _cs.getSimpleName();
        //获取注解，使用@RefTable注解，注解内容见RefTable类
        RefTable tableInfo = _cs.getAnnotation(RefTable.class);
        //获取表设置
        if (null == tableInfo)
        {
            //无法获取信息则返回
            ALServerLog.Fatal("[{" + _cs.getSimpleName() + "}]未配置数据结构RefTable信息 参考格式在class头部前添加： @RefTable(tableName = \"xxx\", isSingletonKey = false)");
            return null;
        }

        //判断是否屏蔽这个读取
        if (tableInfo.ignore())
        {
            return null;
        }

        //获取加载文件名称
        if (!tableInfo.tableName().isEmpty())
        {
            name = tableInfo.tableName();
        } else
        {
            //报错
            ALServerLog.Fatal("Class: " + name + " Didn't define RefTable For resource file path!");
            return null;
        }

        //拼凑最终加载路径
        String path = String.format("%s%c%s.txt", this.getRefPath(), File.separatorChar, name.toLowerCase());

        //创建数据集合对象
        RefContainerBase<? extends RefBase> dataMgr = refdata.getStaticContainer();
        dataMgr.setTableName(name);
        dataMgr.setRefClass(refdata.getClass());
        try
        {
            //加载数据
            _loadClassData(refdata.getClass(), tableInfo, path, dataMgr);
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }

        //根据是否允许为空进行判断
        if (!tableInfo.canbeEmpty() && dataMgr.isEmpty())
        {
            //警告
            ALServerLog.Fatal("Class: " + name + " Data is empty!");
            return null;
        }

        //对数据进行排序
        dataMgr.internalSortRefs();
        //调用加载函数
        dataMgr.onLoaded();

        return dataMgr;
    }

    /**************
     * 根据类重新加载本类目录下的相关子类信息
     *
     * @author alzq.z
     * @time 2019年4月3日 下午8:37:38
     */
    public <T extends RefBase> boolean reload(Class<T> _clazz, String _packagePath)
    {
        List<Class<?>> refdatas = CommClass.getAllClassByInterface(_clazz, _packagePath);

        //遍历所有子类进行处理
        for (Class<?> cs : refdatas)
        {
            RefContainerBase<? extends RefBase> loadedDataMgr = reloadSingleClassData(cs);

            //此处不报错，因为上面的loadSingleClassData函数已经进行了报错处理
            if (null == loadedDataMgr)
                continue;

            //先删除旧数据
            removeRefMgrByTableName(loadedDataMgr.getTableName());
            //加入队列
            _m_lContainerList.add(loadedDataMgr);
        }
        return true;
    }


    /*********************
     * 重新加载单个类的数据并返回新的数据集对象
     *
     * @author alzq.z
     * @time 2019年4月8日 下午10:59:55
     */
    @SuppressWarnings("unchecked")
    public <T extends RefBase> RefContainerBase<? extends RefBase> reloadSingleClassData(Class<?> _cs)
    {
        RefBase refdata = null;
        try
        {
            refdata = (RefBase) CommClass.forName(_cs.getName()).newInstance();
        } catch (Exception e)
        {
            CommLog.error("onAutoLoad class:{} occured Error:", _cs.getSimpleName(), e);
            return null;
        }
        if (null == refdata)
        {
            return null;
        }

        //使用默认类名作为文件名
        String name = _cs.getSimpleName();
        //获取注解，使用@RefTable注解，注解内容见RefTable类
        RefTable tableInfo = _cs.getAnnotation(RefTable.class);
        //获取表设置
        if (null == tableInfo)
        {
            //无法获取信息则返回
            ALServerLog.Fatal("[{" + _cs.getSimpleName() + "}]未配置数据结构RefTable信息 参考格式在class头部前添加： @RefTable(tableName = \"xxx\", isSingletonKey = false)");
            return null;
        }

        //判断是否屏蔽这个读取
        if (tableInfo.ignore())
        {
            return null;
        }

        //获取加载文件名称
        if (!tableInfo.tableName().isEmpty())
        {
            name = tableInfo.tableName();
        } else
        {
            //报错
            ALServerLog.Fatal("Class: " + name + " Didn't define RefTable For resource file path!");
            return null;
        }

        //拼凑最终加载路径
        String path = String.format("%s%c%s.txt", this.getRefPath(), File.separatorChar, name.toLowerCase());

        //创建数据集合对象
        RefContainerBase<? extends RefBase> dataMgr;
        try
        {
            //使用默认的类数据，创建一个新实例。并在实例中进行数据的读取处理
            dataMgr = (RefContainerBase<? extends RefBase>) refdata.getStaticContainer().getClass().newInstance();
        } catch (Exception _ex)
        {
            //报错
            ALServerLog.Fatal("Class: " + name + " new Instance for Data Mgr Error! 检查是否构造函数存在默认无参数构造函数");
            ALServerLog.Fatal(_ex.getMessage());
            return null;
        }

        dataMgr.setTableName(name);
        dataMgr.setRefClass(refdata.getClass());
        try
        {
            //加载数据
            _loadClassData(refdata.getClass(), tableInfo, path, dataMgr);
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }

        //根据是否允许为空进行判断
        if (!tableInfo.canbeEmpty() && dataMgr.isEmpty())
        {
            //警告
            ALServerLog.Fatal("Class: " + name + " Data is empty!");
            return null;
        }

        //对数据进行排序
        dataMgr.internalSortRefs();
        //调用加载函数
        dataMgr.onLoaded();

        //读取完成直接设置对应类的数据
        refdata.setStaticContainer(dataMgr);

        return dataMgr;
    }

    /***********
     * 检查数据加载路径是否合法
     *
     * @author alzq.z
     * @time 2019年4月3日 下午8:30:20
     */
    protected boolean _checkDirects(String _refdataPath)
    {
        File path = new File(_refdataPath);
        if (!path.exists() || !path.isDirectory())
        {
            ALServerLog.Fatal("Refdata Path Error!");
            return false;
        }
        return true;
    }

    /*************
     * 将所有文件命名为小写文件名，这样在读取的时候可以不用对文件名大小进行判断和处理
     *
     * @author alzq.z
     * @time 2019年4月3日 下午8:35:04
     */
    public boolean renameRefFilesToLowcase()
    {
        File path = new File(this.getRefPath());
        File files[] = path.listFiles();
        for (File refFile : files)
        {
            if (!refFile.isFile())
            {
                // 不是文件，跳过
                continue;
            }
            if (!refFile.getName().toLowerCase().endsWith(".txt"))
            {
                // 不是配置文件，跳过
                continue;
            }
            File lowcaseFile = new File(refFile.getParentFile(), refFile.getName().toLowerCase());
            if (lowcaseFile.exists())
            {
                // 已经是小写文件 || windows底下不区分则跳过
                continue;
            }
            refFile.renameTo(lowcaseFile);
        }
        return true;
    }

    /*********************
     * 根据类配置相关信息加载类对应的数据
     *
     * @author alzq.z
     * @time 2019年4月4日 下午8:37:21
     */
    @SuppressWarnings("unchecked")
    protected static <T extends RefBase> void _loadClassData(Class<? extends RefBase> _clazz, RefTable _refTableInfo, String _filePath, RefContainerBase<T> _dataMgr) throws Exception
    {
        //获取表设置
        if (null == _refTableInfo)
        {
            //无法获取信息则返回
            ALServerLog.Fatal("[{" + _clazz.getSimpleName() + "}]未配置数据结构RefTable信息 参考格式在class头部前添加： @RefTable(tableName = \"xxx\", isSingletonKey = false)");
            return;
        }

        //获取原始数据集合
        ArrayList<Map<String, String>> tableLines = CommFile.GetTable(_filePath, 1, 2);

        //每个数据进行处理
        for (Map<String, String> lineValue : tableLines)
        {
            T ref = null;

            //创建实例
            try
            {
                ref = (T) _clazz.newInstance();
            } catch (Exception e)
            {
                CommLog.error("创建实例失败", e);
                //System.exit(1);
                throw new Exception("创建实例失败 - " + e.getMessage());
            }

            //加载数据
            try
            {
                loadLineValue(ref, lineValue);
            } catch (Exception e)
            {
                CommLog.error("[{" + _clazz.getName() + "}]解析配置时发生错误:", e);
            }

            //判断是否单键
            if (_refTableInfo.isSingletonKey())
            {
                //进行单键判断
                if (null != _dataMgr.get(ref.Id()))
                {
                    ALServerLog.Fatal("[{" + _clazz.getName() + "}]重复键值: " + ref.Id());
                    continue;
                }
            }

            //加入数据
            _dataMgr.initPut(ref);
        }
    }

    /**
     * 获取同一路径下所有子类或接口实现类
     * @param intf
     * @return
     * @throws IOException
     * @throws ClassNotFoundException
     */
    public static List<Class<?>> getAllAssignedClass(Class<?> cls) throws IOException, ClassNotFoundException
    {
        List<Class<?>> classes = new ArrayList<Class<?>>();
        for (Class<?> c : getClasses(cls))
        {
            if (cls.isAssignableFrom(c) && !cls.equals(c))
            {
                classes.add(c);
            }
        }
        return classes;
    }

    /**
     * 取得当前类路径下的所有类
     * @param cls
     * @return
     * @throws IOException
     * @throws ClassNotFoundException
     */
    public static List<Class<?>> getClasses(Class<?> cls) throws IOException, ClassNotFoundException
    {
        String pk = cls.getPackage().getName();
        String path = pk.replace('.', '/');
        ClassLoader classloader = Thread.currentThread().getContextClassLoader();
        URL url = classloader.getResource(path);
        return getClasses(new File(url.getFile()), pk);
    }

    /**
     * 迭代查找类
     * @param dir
     * @param pk
     * @return
     * @throws ClassNotFoundException
     */
    private static List<Class<?>> getClasses(File dir, String pk) throws ClassNotFoundException
    {
        List<Class<?>> classes = new ArrayList<Class<?>>();
        if (!dir.exists())
        {
            return classes;
        }
        for (File f : dir.listFiles())
        {
            if (f.isDirectory())
            {
                classes.addAll(getClasses(f, pk + "." + f.getName()));
            }
            String name = f.getName();
            if (name.endsWith(".class"))
            {
                classes.add(Class.forName(pk + "." + name.substring(0, name.length() - 6)));
            }
        }
        return classes;
    }

    /**
     * 把字典里的key、value 对应设置到obj身上
     * @param <M>
     * @param <T>
     * @param obj
     * @param lineValue
     * @throws IllegalAccessException
     * @throws IllegalArgumentException
     * @throws InstantiationException
     */
    @SuppressWarnings({"unchecked", "rawtypes"})
    public static <T extends RefBase> void loadLineValue(T obj, Map<String, String> lineValue) throws Exception
    {
        String parsingTable = obj.getClass().getSimpleName();

        //Field[] fields = obj.getClass().getDeclaredFields();
        /***
         * getFields()：获得某个类的所有的公共（public）的字段，包括父类中的字段。 
         * getDeclaredFields()：获得某个类的所有声明的字段，即包括public、private和proteced，但是不包括父类的申明字段。
         */
        Field[] fields = obj.getClass().getFields();
        for (Field field : fields)
        {
            int modifiers = field.getModifiers();
            if (Modifier.isStatic(modifiers) || Modifier.isFinal(modifiers))
            {
                // SKIP static field
                continue;
            }
            RefField annotation = field.getAnnotation(RefField.class);
            if (annotation != null && annotation.isIgnore())
            {
                continue;
            }
            String varName = field.getName().toLowerCase();
            String varValue = lineValue.get(varName);
            if (null == varValue)
            {
                // CommLog.info("配置表:{} 缺少 {} 字段的定义",parsingTable,field.getName());
                //此时不返回，因为后续还会对队列做初始化处理
            }
            Class<?> typeClass = field.getType();
            Object fieldValue = null;

            // --------------------------------------------------
            // List<M>
            if (List.class.isAssignableFrom(typeClass))
            {
                List<Object> listValue = null;
                if (typeClass.isAssignableFrom(List.class))
                {// 声明为list接口
                    listValue = new ArrayList<>();
                } else
                {
                    listValue = (List<Object>) typeClass.newInstance();
                }

                if (null != varValue && !varValue.trim().equals(""))
                {
                    Type gt = field.getGenericType(); // 得到泛型类型
                    ParameterizedType pt = (ParameterizedType) gt;
                    Class<?> clazz = (Class<?>) pt.getActualTypeArguments()[0];
                    String[] valueList = varValue.split(annotation != null ? annotation.arrayToken() : ";");
                    int length = valueList.length;
                    for (int index = 0; index < length; index++)
                    {
                        Object arrayObj = parseCreateObj(clazz, valueList[index], parsingTable, varName);
                        if (null != arrayObj)
                        {
                            listValue.add(arrayObj);
                        }
                    }
                    fieldValue = listValue;
                } else
                {//没有配置

                    if (field.get(obj) == null)//没有默认值
                    {
                        fieldValue = listValue;
                    }
                }

            } // M[] 不支持int等简单类型
            else if (typeClass.isArray())
            {
                Class<Object[]> typeListClass = (Class<Object[]>) field.getType();

                // M的类型
                Class<Object> subTypeClass = (Class<Object>) typeClass.getComponentType();

                Object[] listValue = (Object[]) Array.newInstance(subTypeClass, 0);
                if (null != varValue && !varValue.equals(""))
                {
                    String[] valueList = varValue.split(annotation != null ? annotation.arrayToken() : ";");
                    int length = valueList.length;

                    listValue = (Object[]) Array.newInstance(subTypeClass, length);

                    for (int index = 0; index < length; index++)
                    {
                        listValue[index] = parseCreateObj(typeListClass.getComponentType(), valueList[index],
                                parsingTable, varName);
                    }
                }

                fieldValue = listValue;
            } else if (ListList.class.isAssignableFrom(typeClass))
            {
                ListList listValue = new ListList<>();
                if (varValue != null && !varValue.isEmpty())
                {
                    Type gt = field.getGenericType(); // 得到泛型类型
                    ParameterizedType pt = (ParameterizedType) gt;
                    Class<?> clazz = (Class<?>) pt.getActualTypeArguments()[0];
                    listValue.fromString(varValue, clazz);
                }

                fieldValue = listValue;
            } else if (null != varValue)
            {
                fieldValue = parseCreateObj(typeClass, varValue, parsingTable, varName);
            } else
            {
                fieldValue = null;
            }

            // --------------------------------------------------
            // 填充
            if (null != fieldValue)
            {
                field.set(obj, fieldValue);
            }
        }
    }

    @SuppressWarnings("unchecked")
    public static Object parseCreateObjAndList(Field field, String value, String tbName, String fieldName)
    {
        Class<?> typeClass = field.getType();
        if (List.class.isAssignableFrom(typeClass))
        {

            List<Object> listValue = null;
            if (typeClass.isAssignableFrom(List.class))
            {// 声明为list接口
                listValue = new ArrayList<>();
            } else
            {
                try
                {
                    listValue = (List<Object>) typeClass.newInstance();
                } catch (Exception e)
                {
                    CommLog.error("can not instance class:{}", typeClass.getSimpleName(), e);
                    return null;
                }

            }

            if (null != value && !value.trim().equals(""))
            {
                Type gt = field.getGenericType(); // 得到泛型类型
                ParameterizedType pt = (ParameterizedType) gt;
                Class<?> clazz = (Class<?>) pt.getActualTypeArguments()[0];
                String[] valueList = value.split(";");
                int length = valueList.length;
                for (int index = 0; index < length; index++)
                {
                    Object arrayObj = parseCreateObj(clazz, valueList[index], tbName, fieldName);
                    if (null != arrayObj)
                    {
                        listValue.add(arrayObj);
                    }
                }
            }
            return listValue;

        } else if (ListList.class.isAssignableFrom(typeClass))
        {
            ListList<Object> listValue = new ListList<>();
            if (value != null && !value.isEmpty())
            {
                Type gt = field.getGenericType(); // 得到泛型类型
                ParameterizedType pt = (ParameterizedType) gt;
                Class<?> clazz = (Class<?>) pt.getActualTypeArguments()[0];
                listValue.fromString(value, clazz);
            }

            return listValue;
        }
        // M[] 不支持int等简单类型
        else if (typeClass.isArray())
        {
            Class<Object[]> typeListClass = (Class<Object[]>) field.getType();

            // M的类型
            Class<Object> subTypeClass = (Class<Object>) typeClass.getComponentType();

            Object[] listValue = (Object[]) Array.newInstance(subTypeClass, 0);
            if (null != value && !value.equals(""))
            {
                String[] valueList = value.split(";");
                int length = valueList.length;

                listValue = (Object[]) Array.newInstance(subTypeClass, length);

                for (int index = 0; index < length; index++)
                {
                    listValue[index] = parseCreateObj(typeListClass.getComponentType(), valueList[index], tbName, fieldName);
                }
            }

            return listValue;
        } // 非重复类型
        else if (null != value)
        {
            return parseCreateObj(typeClass, value, tbName, fieldName);
        } else
        {
            return null;
        }
    }

    @SuppressWarnings({"unchecked", "rawtypes"})
    public static Object parseCreateObj(Class<?> field, String value, String tbName, String fieldName)
    {
        try
        {
            if (field == int.class || field == Integer.class)
            {
                // 小数取整
                int index = value.indexOf(".");
                if (-1 != index)
                {
                    value = value.substring(0, index);
                }
                // 空取0
                if (value.equals(""))
                {
                    return 0;
                }
                int ret = Integer.valueOf(value);
                return ret;
            } else if (field == long.class || field == Long.class)
            {
                // 空取0
                if (value.equals(""))
                {
                    return 0L;
                }
                return Long.valueOf(value);
            } else if (field == short.class || field == Short.class)
            {
                // 空取0
                if (value.equals(""))
                {
                    return 0L;
                }
                return Short.valueOf(value);
            } else if (field == float.class || field == Float.class)
            {
                // 空取0
                if (value.equals(""))
                {
                    return 0F;
                }
                return Float.valueOf(value);
            } else if (field == double.class || field == Double.class)
            {
                // 空取0
                if (value.equals(""))
                {
                    return 0F;
                }
                return Double.valueOf(value);
            } else if (field == byte.class || field == Byte.class)
            {
                // 空取0
                if (value.equals(""))
                {
                    return 0F;
                }
                return Byte.valueOf(value);
            } else if (field == boolean.class || field == Boolean.class)
            {
                return Boolean.valueOf(value);
            } else if (field == NumberRange.class)
            {
                return NumberRange.parse(value);
            } else if (field == String.class)
            {
                return value;
            } else if (field.isEnum())
            {
                if (value.isEmpty())
                {
                    return ((Class) field).getEnumConstants()[0];
                }

                try
                {
                    return Enum.valueOf((Class) field, value.toUpperCase().trim());
                } catch (Exception e)
                {
                    CommLog.error("BaseRefDataMgr 解析值失败 [{}] field:[{}] type:[{}] value:[{}] ", tbName, fieldName, field.getSimpleName(), value, e);
                    return ((Class) field).getEnumConstants()[0];
                }
            } else if (_IParseFromStringable.class.isAssignableFrom(field))
            {
                _IParseFromStringable obj = (_IParseFromStringable) field.newInstance();
                if (!obj.parseFromString(value))
                {
                    CommLog.error("BaseRefDataMgr 解析值失败 [{}] field:[{}] type:[{}] value:[{}] ", tbName, fieldName, field.getSimpleName(), value);
                    return null;
                } else
                {
                    return obj;
                }

            }
            // 自定义类 暂不支持，容易导致初始化时查找对象死循环
        } catch (Exception e)
        {
            CommLog.error("BaseRefDataMgr 解析值失败 [{}] field:[{}] type:[{}] value:[{}] ", tbName, fieldName, field.getSimpleName(), value, e);

        }
        return null;
    }

    //根据数据表名检索数据集合
    public RefContainerBase<?> lookupRefMgrByTableName(String _tableName)
    {
        RefContainerBase<?> tmp = null;
        for (int i = 0; i < _m_lContainerList.size(); i++)
        {
            tmp = _m_lContainerList.get(i);
            if (null == tmp)
                continue;

            if (tmp.getTableName().compareToIgnoreCase(_tableName) == 0)
            {
                return tmp;
            }
        }
        return null;
    }

    /*********
     * 根据数据表名删除数据集合
     *
     * @author alzq.z
     * @time 2019年4月8日 下午11:38:01
     */
    public void removeRefMgrByTableName(String _tableName)
    {
        RefContainerBase<?> tmp = null;
        for (int i = 0; i < _m_lContainerList.size(); i++)
        {
            tmp = _m_lContainerList.get(i);
            if (null == tmp)
                continue;

            if (tmp.getTableName().compareToIgnoreCase(_tableName) == 0)
            {
                _m_lContainerList.remove(i);
                return;
            }
        }
    }

    /*************
     * 处理初始化配置路径相关处理
     *
     * @author alzq.z
     * @time 2019年4月3日 下午8:28:45
     * @param _refReloadThreadIdx 指定一个线程idx，用来专门执行配表重载操作
     */
    protected abstract void _init(int _refReloadThreadIdx);

    /********
     * 获取加载数据的路径
     *
     * @author alzq.z
     * @time 2019年4月3日 下午8:29:20
     */
    public abstract String getRefPath();

    /***********
     * 获取加载的数据类存放包路径
     *
     * @author alzq.z
     * @time 2019年4月3日 下午10:06:35
     */
    public abstract String getLoadRefdataClassPath();

    /****
     * 重新加载
     */
    public abstract void reload();
}