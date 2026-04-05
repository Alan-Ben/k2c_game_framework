package NPCommon.Util;

import java.io.File;
import java.io.IOException;
import java.util.Collections;
import java.util.List;

/*****
 * Shell脚本，可以查看运行时服务器目录和文件
 */
public class CommShell
{
    public static CommShell getInstance()
    {
        return _instance;
    }

    private static CommShell _instance = new CommShell();

    private File _m_curDir;//当前目录
    private String _rootPath = "";//程序根目录

    private CommShell()
    {
        _m_curDir = new File(".");
        _rootPath = getCurPath();
    }

    private File getCurDir() //返回当前目录
    {
        return _m_curDir;
    }

    private String getRootPath()
    {
        return _rootPath;
    }//返回根目录

    private String getSlash()
    {
        return "\n" + getCurPath() + ">";
    }

    public String getCurPath()//返回当前路径
    {
        try
        {
            return getCurDir().getCanonicalPath();
        } catch (IOException e)
        {
            return getCurDir().getAbsolutePath();
        }
    }

    public String ll()
    {
        File[] files = getCurDir().listFiles();
        StringBuilder sb = new StringBuilder();
        if (files != null)
        {
            for (File file : files)
            {
                sb.append(getFixLenString(file.getName(), 50));
                if (file.isDirectory())
                    sb.append("<DIR>");
                else
                    sb.append(formatNumber(file.length()));
                sb.append('\n');
            }
        }
        sb.append(getSlash());
        return sb.toString();
    }

    private String getFixLenString(String _str, int _fixLen)
    {
        String ret = _str;
        int needNum = _fixLen - _str.length();
        if (needNum > 0)
        {
            ret = ret + String.join("", Collections.nCopies(needNum, " "));
        }
        return ret;
    }

    public String cd(String _subDir)
    {
        File dir = new File(getCurPath() + "/" + _subDir);
        if (!dir.isDirectory())
        {
            return _subDir + " is not dir";
        }
        _m_curDir = dir;
        return getSlash();

    }

    public String back()
    {
        File dir = new File(getRootPath());
        _m_curDir = dir;
        return getSlash();

    }

    public String tail(String _subFileName, int _num, int _pageIndex)
    {
        String path = getCurPath() + "/" + _subFileName;
        File file = new File(path);
        if (!file.exists())
            return _subFileName + " do not exists";
        if (!file.isFile())
            return _subFileName + " is not a file!";
        if (!file.canRead())
            return _subFileName + " can not read";
        List<String> strList = CommFile.readReverse(path, _num, _pageIndex);
        return StringFunc.list2String(strList, '\n');
    }

    public String more(String _subFileName, int _pageSize, int _pageIndex)
    {
        String path = getCurPath() + "/" + _subFileName;
        File file = new File(path);
        if (!file.exists())
            return _subFileName + " do not exists";
        if (!file.isFile())
            return _subFileName + " is not a file!";
        if (!file.canRead())
            return _subFileName + " can not read";
        List<String> strList = CommFile.readPage(path, _pageSize, _pageIndex);
        return StringFunc.list2String(strList, '\n');

    }

    private static String formatNumber(long length)
    {
        if (length <= 0)
            return length + "B";
        if (length > 1024 * 1024)
            return (length * 1000 / 1024 / 1024 / 1000D) + "MB";
        if (length > 1024)
            return (length * 1000 / 1024 / 1000D) + "KB";
        return length + "B";
    }
}
