package NPCommon.Util;

import NPCommon.Log.CommLog;

import java.io.File;
import java.io.FileFilter;
import java.io.IOException;
import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.net.JarURLConnection;
import java.net.URL;
import java.net.URLDecoder;
import java.util.*;
import java.util.jar.JarEntry;
import java.util.jar.JarFile;

public class CommClass
{

    private static ClassLoader classLoader = Thread.currentThread().getContextClassLoader();

    public static void setClassLoader(ClassLoader classLoader)
    {
        CommClass.classLoader = classLoader;
    }

    /**
     * 从包package中获取所有的Class
     * @param _pack
     * @return
     */
    public static Set<Class<?>> getClasses(String _pack)
    {
        // 第一个class类的集合
        Set<Class<?>> classes = new LinkedHashSet<>();
        // 是否循环迭代
        boolean recursive = true;
        // 获取包的名字 并进行替换
        String packageName = _pack;
        String packageDirName = packageName.replace('.', '/');
        // 定义一个枚举的集合 并进行循环来处理这个目录下的things
        Enumeration<URL> dirs;
        try
        {
            dirs = Thread.currentThread().getContextClassLoader().getResources(packageDirName);
            // 循环迭代下去
            while (dirs.hasMoreElements())
            {
                // 获取下一个元素
                URL url = dirs.nextElement();
                // 得到协议的名称
                String protocol = url.getProtocol();
                if (null != protocol) // 如果是以文件的形式保存在服务器上
                // 如果是以文件的形式保存在服务器上
                {
                    switch (protocol)
                    {
                        case "file":
                            // System.err.println("file类型的扫描");
                            // 获取包的物理路径
                            String filePath = URLDecoder.decode(url.getFile(), "UTF-8");
                            // 以文件的方式扫描整个包下的文件 并添加到集合中
                            findAndAddClassesInPackageByFile(packageName, filePath, recursive, classes);
                            break;
                        case "jar":
                            // 如果是jar包文件
                            // 定义一个JarFile
                            // System.err.println("jar类型的扫描");
                            JarFile jar;
                            try
                            {
                                // 获取jar
                                jar = ((JarURLConnection) url.openConnection()).getJarFile();
                                // 从此jar包 得到一个枚举类
                                Enumeration<JarEntry> entries = jar.entries();
                                // 同样的进行循环迭代
                                while (entries.hasMoreElements())
                                {
                                    // 获取jar里的一个实体 可以是目录 和一些jar包里的其他文件 如META-INF等文件
                                    JarEntry entry = entries.nextElement();
                                    String name = entry.getName();
                                    // 如果是以/开头的
                                    if (name.charAt(0) == '/')
                                    {
                                        // 获取后面的字符串
                                        name = name.substring(1);
                                    }
                                    // 如果前半部分和定义的包名相同
                                    if (name.startsWith(packageDirName))
                                    {
                                        int idx = name.lastIndexOf('/');
                                        // 如果以"/"结尾 是一个包
                                        if (idx != -1)
                                        {
                                            // 获取包名 把"/"替换成"."
                                            packageName = name.substring(0, idx).replace('/', '.');
                                        }
                                        // 如果可以迭代下去 并且是一个包
                                        if ((idx != -1) || recursive)
                                        {
                                            // 如果是一个.class文件 而且不是目录
                                            if (name.endsWith(".class") && !entry.isDirectory())
                                            {
                                                // 去掉后面的".class" 获取真正的类名
                                                String className = name.substring(packageName.length() + 1, name.length() - 6);
                                                try
                                                {
                                                    // 添加到classes
                                                    classes.add(classLoader.loadClass(packageName + '.' + className));
                                                } catch (ClassNotFoundException e)
                                                {
                                                    // log
                                                    // .error("添加用户自定义视图类错误 找不到此类的.class文件");
                                                    CommLog.error(CommClass.class.getName(), e);
                                                }
                                            }
                                        }
                                    }
                                }
                            } catch (IOException e)
                            {
                                // log.error("在扫描用户定义视图时从jar包获取文件出错");
                                CommLog.error(CommClass.class.getName(), e);
                            }
                            break;
                    }
                }
            }
        } catch (IOException e)
        {
            CommLog.error(CommClass.class.getName(), e);
        }

        return classes;
    }

    /**
     * 以文件的形式来获取包下的所有Class
     * @param packageName
     * @param packagePath
     * @param recursive
     * @param classes
     */
    public static void findAndAddClassesInPackageByFile(String packageName, String packagePath, final boolean recursive, Set<Class<?>> classes)
    {
        // 获取此包的目录 建立一个File
        File dir = new File(packagePath);
        // 如果不存在或者 也不是目录就直接返回
        if (!dir.exists() || !dir.isDirectory())
        {
            // log.warn("用户定义包名 " + packageName + " 下没有任何文件");
            return;
        }
        // 如果存在 就获取包下的所有文件 包括目录
        File[] dirfiles = dir.listFiles(new FileFilter()
        {
            // 自定义过滤规则 如果可以循环(包含子目录) 或则是以.class结尾的文件(编译好的java类文件)
            @Override
            public boolean accept(File file)
            {
                return (recursive && file.isDirectory()) || (file.getName().endsWith(".class"));
            }
        });
        // 循环所有文件
        for (File file : dirfiles)
        {
            // 如果是目录 则继续扫描
            if (file.isDirectory())
            {
                findAndAddClassesInPackageByFile(packageName + "." + file.getName(), file.getAbsolutePath(), recursive, classes);
            } else
            {
                // 如果是java类文件 去掉后面的.class 只留下类名
                String className = file.getName().substring(0, file.getName().length() - 6);
                try
                {
                    // 添加到集合中去
                    // 经过回复同学的提醒，这里用forName有一些不好，会触发static方法，没有使用classLoader的load干净
                    classes.add(classLoader.loadClass(packageName + '.' + className));
                } catch (ClassNotFoundException e)
                {
                    // log.error("添加用户自定义视图类错误 找不到此类的.class文件");
                    CommLog.error(CommClass.class.getName(), e);
                }
            }
        }
    }

    /**
     * 获取同一路径下所有子类或接口实现类
     * @param cls
     * @return
     * @throws IOException
     * @throws ClassNotFoundException
     */
    public static List<Class<?>> getAllAssignedClass(Class<?> cls) throws IOException, ClassNotFoundException
    {
        List<Class<?>> classes = new ArrayList<>();
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
        List<Class<?>> classes = new ArrayList<>();
        if (!dir.exists())
        {
            return classes;
        }
        for (File f : dir.listFiles())
        {
            if (f.isDirectory())
            {
                CommonFunc.appendList(classes, getClasses(f, pk + "." + f.getName()));
            }
            String name = f.getName();
            if (name.endsWith(".class"))
            {
                classes.add(CommClass.forName(pk + "." + name.substring(0, name.length() - 6)));
            }
        }
        return classes;
    }

    /**
     * 给一个接口（或者父类），返回这个接口（或者父类）的所有实现类（或者子类）
     * （这些实现类（或者子类）的的包的路径必须要是接口（或者父类）的同一层或者下层）
     * @param c
     * @return
     */
    @SuppressWarnings({"unchecked", "rawtypes"})
    public static List<Class> getAllClassByInterface(Class c)
    {
        List<Class> returnClassList = new ArrayList<>(); // 返回结果

        // 如果不是一个接口，则不做处理
        if (c.isInterface() || Modifier.isAbstract(c.getModifiers()))
        {
            String packageName = c.getPackage().getName(); // 获得当前的包名
            Set<Class<?>> allClass = getClasses(packageName); // 获得当前包下以及子包下的所有类

            for (Class<?> cs : allClass)
            {
                // 判断是不是相同接口
                if (!c.isAssignableFrom(cs))
                {
                    continue;
                }

                // 本身不加进去
                if (c.equals(cs))
                {
                    continue;
                }

                returnClassList.add(cs);
            }
        }
        return returnClassList;
    }

    /**
     * 给一个接口（或者父类），返回这个包（含子包）的接口（或者父类）的所有实现类
     * @param _c
     * @param _packageName 包名
     * @return
     */
    @SuppressWarnings("unchecked")
    public static <T> List<Class<? extends T>> getAllClassByInterface(Class<? extends T> _c, String _packageName)
    {
        List<Class<? extends T>> returnClassList = new ArrayList<Class<? extends T>>(); // 返回结果

        //判断是否interface或虚类，不是任意一个则不处理
        if (!_c.isInterface() && !Modifier.isAbstract(_c.getModifiers()))
            return returnClassList;

        Set<Class<?>> allClass = getClasses(_packageName); // 获得当前包下以及子包下的所有类
        for (Class<?> cs : allClass)
        {
            // 判断是不是相同接口
            if (!_c.isAssignableFrom(cs))
                continue;

            // 不要抽象类
            if (Modifier.isAbstract(cs.getModifiers()))
                continue;

            // 本身不加进去
            if (_c.equals(cs))
                continue;

            returnClassList.add((Class<? extends T>) cs);
        }

        return returnClassList;
    }

    /**
     * 获取键值对打印到控制台
     * @param object
     * @return
     */
    public static String printClassInfo(Object object)
    {
        StringBuilder sBuilder = new StringBuilder();
        String ent = System.lineSeparator();
        sBuilder.append("output:").append(object.getClass().getSimpleName()).append(ent);

        Field[] fields = object.getClass().getDeclaredFields();
        for (Field field : fields)
        {
            try
            {
                boolean accessFlag = field.isAccessible();
                field.setAccessible(true);
                String varName = field.getName();
                Object varValue = field.get(object);
                sBuilder.append(String.format("(%s)%s = %s", field.getType().getSimpleName(), varName, varValue)).append(ent);
                field.setAccessible(accessFlag);
            } catch (SecurityException | IllegalArgumentException | IllegalAccessException e)
            {
                CommLog.error(CommClass.class.getName(), e);
            }
        }

        return sBuilder.toString();
    }

    public static String getClassPropertyInfos(Object object)
    {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append("[");

        Field[] fields = object.getClass().getDeclaredFields();
        for (Field field : fields)
        {
            try
            {
                boolean accessFlag = field.isAccessible();
                field.setAccessible(true);
                String varName = field.getName();
                Object varValue = field.get(object);
                Class<?> type = field.getType();
                if (type.isAssignableFrom(Collection.class.getClass()))
                {
                    Collection<?> lst = (Collection<?>) varValue;
                    for (Object objInlist : lst)
                    {
                        sBuilder.append(getClassPropertyInfos(objInlist));
                    }
                } else
                {
                    sBuilder.append(String.format("%s:%s,", varName, varValue));
                }

                field.setAccessible(accessFlag);
            } catch (SecurityException | IllegalArgumentException | IllegalAccessException e)
            {
                CommLog.error(CommClass.class.getName(), e);
            }
        }
        sBuilder.append("],");

        return sBuilder.toString();
    }

    public static Class<?> forName(String name) throws ClassNotFoundException
    {
        return classLoader.loadClass(name);
    }

    /*********
     * 返回指定Jar包下面所有类.
     * @param pack
     * @return
     */
    public static Set<Class<?>> getClassByPackFromJar(URL _jarUrl, String pack)
    {
        // 第一个class类的集合
        Set<Class<?>> classes = new LinkedHashSet<>();
        // 是否循环迭代
        boolean recursive = true;

        String packageName = pack == null ? "" : pack;
        String packageDirName = packageName.replace('.', '/');
        JarFile jar;
        try
        {
            jar = ((JarURLConnection) _jarUrl.openConnection()).getJarFile();
        } catch (Exception e)
        {
            CommLog.error("open jar url failed:{}", _jarUrl, e);
            return classes;
        }
        Enumeration<JarEntry> entries = jar.entries();
        // 同样的进行循环迭代
        while (entries.hasMoreElements())
        {
            // 获取jar里的一个实体 可以是目录 和一些jar包里的其他文件 如META-INF等文件
            JarEntry entry = entries.nextElement();
            String name = entry.getName();

            if (name.startsWith("temp"))
            {
                continue;
            }
            // 如果是以/开头的
            if (name.charAt(0) == '/')
            {
                // 获取后面的字符串
                name = name.substring(1);
            }
            // 如果前半部分和定义的包名相同
            if (packageDirName.isEmpty() || name.startsWith(packageDirName))
            {
                int idx = name.lastIndexOf('/');
                // 如果以"/"结尾 是一个包
                if (idx != -1)
                {
                    // 获取包名 把"/"替换成"."
                    packageName = name.substring(0, idx).replace('/', '.');
                }
                // 如果可以迭代下去 并且是一个包
                if ((idx != -1) || recursive)
                {
                    // 如果是一个.class文件 而且不是目录
                    if (name.endsWith(".class") && !entry.isDirectory())
                    {
                        // 去掉后面的".class" 获取真正的类名
                        String className = name.substring(packageName.length() + 1, name.length() - 6);
                        try
                        {
                            // 添加到classes
                            String fullClassName = packageName.isEmpty() ? className : packageName + '.' + className;
                            classes.add(ClassLoader.getSystemClassLoader().loadClass(fullClassName));
                        } catch (ClassNotFoundException e)
                        {
                            // log
                            // .error("添加用户自定义视图类错误 找不到此类的.class文件");
                            CommLog.error("Class {} NotFoundException:", className, e);
                        }
                    }
                }
            }
        }
        return classes;
    }
}
