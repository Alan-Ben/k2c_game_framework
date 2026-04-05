package NPCommon.Util;

import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;

import java.io.*;
import java.nio.CharBuffer;
import java.nio.channels.FileChannel;
import java.nio.charset.Charset;
import java.nio.charset.StandardCharsets;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;

public class CommFile
{

    public static void close(Closeable... closeables)
    {
        for (Closeable c : closeables)
        {
            try
            {
                if (c != null)
                {
                    c.close();
                }
            } catch (IOException e)
            {
                CommLog.error("", e);
            }
        }
    }

    /**
     * 读文件
     * @param path
     * @return
     * @throws IOException
     */
    public static String bufferedReader(String path) throws IOException
    {
        File file = new File(path);
        if (!file.exists() || file.isDirectory())
        {
            throw new FileNotFoundException("file not exist:" + path);
        }

        BufferedReader br = new BufferedReader(new FileReader(file));
        StringBuilder sb = new StringBuilder();

        String line = null;
        while ((line = br.readLine()) != null)
        {
            sb.append(line).append("\n");
        }
        br.close();
        return sb.toString();
    }

    /**
     * 读文件, 解决编码问题
     * @param path
     * @param code
     * @return
     * @throws IOException
     */
    @SuppressWarnings("resource")
    public static String BufferedReaderEncode(String path, String code) throws IOException
    {
        File file = new File(path);
        if (!file.exists() || file.isDirectory())
        {
            throw new FileNotFoundException();
        }

        FileInputStream fr = new FileInputStream(path);
        BufferedReader br = new BufferedReader(new InputStreamReader(fr, code));

        StringBuilder sb = new StringBuilder();
        String temp = br.readLine();

        while (null != temp)
        {
            sb.append(temp).append(" ");
            temp = br.readLine();
        }
        return sb.toString();
    }

    /**
     * 将文件内容解析为二维表
     * @param path
     * @param fieldLine   字段所在行(首行从1算起)
     * @param contentLine 正文起始行
     * @return
     */
    public static ArrayList<Map<String, String>> GetTable(String path, int fieldLine, int contentLine)
    {
        ArrayList<Map<String, String>> table = new ArrayList<>();

        File file = new File(path);
        if (!file.exists() || file.isDirectory() || fieldLine >= contentLine || fieldLine <= 0 || contentLine <= 0)
        {
            ALServerLog.Error("file not exist:" + path);
            return table;
        }

        Charset cs = Charset.forName("utf-8");
        BufferedReader br = null;
        try
        {
            FileInputStream fr = new FileInputStream(file);
            br = new BufferedReader(new InputStreamReader(fr, cs));
        } catch (FileNotFoundException e)
        {
            CommLog.error("", e);
        }
        if (null == br)
        {
            ALServerLog.Error("Refdata readfailed:" + path);
            return table;
        }

        String temp = null;

        String[] fields = null;
        int curLine = 0;
        do
        {
            try
            {
                temp = br.readLine();
                if (null == temp)
                {
                    break;
                }

                temp = temp.trim();
                curLine += 1;

                if (temp.equals(""))
                {
                    continue;
                }

                if (fieldLine == curLine)
                {
                    // 解析表头
                    String[] values = CommonFunc.charSplit(temp, '\t');
                    if (0 >= values.length)
                    {
                        CommFile.close(br);
                        ALServerLog.Error("ParseTable fieldCnt <= 0, path:" + path);
                        return table;
                    }
                    // 过滤空格
                    fields = new String[values.length];
                    for (int index = 0; index < values.length; index++)
                    {
                        fields[index] = values[index].trim().toLowerCase();
                    }
                } else if (curLine >= contentLine)
                {
                    if (null == fields)
                    {
                        CommFile.close(br);
                        ALServerLog.Error("ParseTable fieldCnt null(fieldLine: {" + fieldLine + "}), path:" + path);
                        return table;
                    }

                    // 解析内容
                    String[] values = CommonFunc.charSplit(temp, '\t');
                    if (fields.length < values.length)
                    {
                        ALServerLog.Error("解析表失败 [{" + file.getName() + "}]头部字段数({" + fields.length + "})<值字段数({" + values.length + "}) 无法解析, content:{" + temp + "}, path:{}");
                        continue;
                    }

                    // 单行字典
                    Map<String, String> lineValue = new ConcurrentHashMap<>();
                    for (int index = 0; index < fields.length; index++)
                    {
                        if (values.length > index)
                        {
                            lineValue.put(fields[index], values[index].trim());
                        } else
                        {
                            lineValue.put(fields[index], "");
                        }
                    }

                    // 放入总表
                    table.add(lineValue);
                }
            } catch (IOException e)
            {
                CommLog.error("", e);
            }
        } while (null != temp);

        CommFile.close(br);

        return table;
    }

    /**
     * 将文件内容解析为行表
     * @param path
     * @param fieldLine   字段所在行(首行从1算起)
     * @param contentLine 正文起始行
     * @return
     */
    public static List<Map<String, String>> GetRowSet(String path, int fieldLine, int contentLine)
    {
        List<Map<String, String>> lines = new ArrayList<>();
        Map<String, Map<String, String>> table = new ConcurrentHashMap<>();

        File file = new File(path);
        if (!file.exists() || file.isDirectory() || fieldLine >= contentLine || fieldLine <= 0 || contentLine <= 0)
        {
            return lines;
        }

        Charset cs = Charset.forName("utf-8");
        BufferedReader br = null;
        try
        {
            FileInputStream fr = new FileInputStream(file);
            br = new BufferedReader(new InputStreamReader(fr, cs));
        } catch (FileNotFoundException e)
        {
            CommLog.error("", e);
        }
        if (null == br)
        {
            return lines;
        }

        String temp = null;

        String[] fields = null;
        int curLine = 0;
        do
        {
            try
            {
                temp = br.readLine();
                if (null == temp)
                {
                    break;
                }

                temp = temp.trim();
                curLine += 1;

                if (temp.equals(""))
                {
                    continue;
                }

                if (fieldLine == curLine)
                {
                    // 解析表头
                    String[] values = CommonFunc.charSplit(temp, '\t');
                    if (0 >= values.length)
                    {
                        CommFile.close(br);
                        ALServerLog.Error("ParseTable fieldCnt <= 0, path:" + path);
                        return lines;
                    }
                    // 过滤空格
                    fields = new String[values.length];
                    for (int index = 0; index < values.length; index++)
                    {
                        fields[index] = values[index].trim();
                    }
                } else if (curLine >= contentLine)
                {
                    if (null == fields)
                    {
                        CommFile.close(br);
                        ALServerLog.Error("ParseTable fieldCnt null(fieldLine: " + fieldLine + "), path:" + path);
                        return lines;
                    }

                    // 解析内容
                    String[] values = CommonFunc.charSplit(temp, '\t');
                    if (fields.length < values.length)
                    {
                        ALServerLog.Error("解析表失败 代码字段({" + fields.length + "}) < 配表字段({" + values.length + "}), content:{" + temp + "}, path:{" + path + "}");
                        continue;
                    }

                    // 单行字典
                    Map<String, String> lineValue = new ConcurrentHashMap<>();
                    for (int index = 0; index < fields.length; index++)
                    {
                        if (values.length > index)
                        {
                            lineValue.put(fields[index], values[index].trim());
                        } else
                        {
                            lineValue.put(fields[index], "");
                        }
                    }

                    // 放入总表
                    String key = values[0];
                    if (table.containsKey(key))
                    {
                        ALServerLog.Error("键值重复: {" + key + "}, at line:{" + curLine + "}, path:{" + path + "}");
                    }
                    table.put(key, lineValue);
                    lines.add(lineValue);
                }
            } catch (IOException e)
            {
                CommLog.error("", e);
            }
        } while (null != temp);

        CommFile.close(br);

        return lines;
    }

    /**
     * 递归获取指定后缀的文件
     * @param dirPath
     * @param fileList
     * @param postfix
     */
    public static void getFileListByPath(String dirPath, List<File> fileList, String postfix)
    {
        File dir = new File(dirPath);
        File[] files = dir.listFiles();

        if (files == null || files.length == 0)
        {
            return;
        }

        List<File> subDirFileList = new ArrayList<>();
        List<File> curDirFileList = new ArrayList<>();

        for (File file : files)
        {
            if (file.isDirectory())
            {
                getFileListByPath(file.getAbsolutePath(), subDirFileList, postfix);
            } else
            {
                if (!postfix.isEmpty() && !file.getName().toLowerCase().endsWith("." + postfix.trim()))
                {
                    // curDirFileList.add(file);
                    continue;
                } else
                {
                    curDirFileList.add(file);
                }
            }
        }

        CommonFunc.appendList(fileList, subDirFileList);
        CommonFunc.appendList(fileList, curDirFileList);
    }

    /**
     * 递归获取指定后缀的文件
     * @param _dirPath
     * @param _fileList 输出参数
     * @param _postfix 文件后缀名
     * @param _bRecursion 是否递归查找
     */
    public static void getFileListByPath(String _dirPath, List<File> _fileList, String _postfix, boolean _bRecursion)
    {
        File dir = new File(_dirPath);
        File[] files = dir.listFiles();

        if (files == null || files.length == 0)
        {
            return;
        }

        List<File> subDirFileList = new ArrayList<>();
        List<File> curDirFileList = new ArrayList<>();

        for (File file : files)
        {
            if (file.isDirectory() && _bRecursion)
            {
                getFileListByPath(file.getAbsolutePath(), subDirFileList, _postfix, true);
            }
            else
            {
                if (!_postfix.isEmpty() && !file.getName().toLowerCase().endsWith("." + _postfix.trim()))
                {
                    // curDirFileList.add(file);
                    continue;
                }
                else
                {
                    curDirFileList.add(file);
                }
            }
        }

        CommonFunc.appendList(_fileList, subDirFileList);
        CommonFunc.appendList(_fileList, curDirFileList);
    }

    public static void getLinesFromFile(List<String> methodList, String strFilePath)
    {
        File file = null;
        BufferedReader br = null;
        String strLine = null;
        try
        {
            file = new File(strFilePath);// 文件路径
            br = new BufferedReader(new FileReader(file));
            while ((strLine = br.readLine()) != null)
            {
                if (!strLine.trim().isEmpty())
                {
                    methodList.add(strLine.trim());
                }
            }
        } catch (FileNotFoundException e)
        {
            CommLog.error("", e);
        } catch (IOException e)
        {
            CommLog.error("", e);
        }
    }

    /**
     * get the byte data of the file
     * @param filePath
     * @return char array of the file content
     */
    public static CharBuffer getFileData(String filePath)
    {
        CharBuffer data = null;

        File file = new File(filePath);
        if (!file.exists())
            return null;
        FileInputStream fin = null;
        UnicodeReader isReader = null;
        BufferedReader bufReader = null;
        try
        {
            fin = new FileInputStream(file);
            isReader = new UnicodeReader(fin, "utf-8");
            bufReader = new BufferedReader(isReader);

            data = CharBuffer.allocate((int) file.length());
            int count = bufReader.read(data);
            data.limit(count);
        } catch (Exception e)
        {
            return null;
        } finally
        {
            if (fin != null)
            {
                try
                {
                    fin.close();
                } catch (IOException e)
                {
                }
            }
            if (isReader != null)
            {
                try
                {
                    isReader.close();
                } catch (IOException e)
                {
                }
            }
            if (bufReader != null)
            {
                try
                {
                    bufReader.close();
                } catch (IOException e)
                {
                }
            }
        }

        return data;
    }

    public static String getTextFromFile(String strFilePath)
    {
        CharBuffer buf = getFileData(strFilePath);
        if (buf != null)
        {
            String res = new String(buf.array());
            return res.trim();
        }
        return null;
    }

    public static Set<String> getLineSetsFromFile(String strFilePath)
    {
        Set<String> rowsets = new HashSet<>();
        List<String> lines = getLinesFromFile(strFilePath);
        for (String line : lines)
        {
            rowsets.add(line);
        }

        return rowsets;
    }

    public static List<String> getLinesFromFile(String strFilePath)
    {
        File file = null;
        BufferedReader br = null;
        String strLine = null;
        List<String> rowsets = new LinkedList<>();
        try
        {
            file = new File(strFilePath);// 文件路径
            br = new BufferedReader(new FileReader(file));
            while ((strLine = br.readLine()) != null)
            {
                if (!strLine.trim().isEmpty())
                {
                    rowsets.add(strLine.trim());
                }
            }
        } catch (FileNotFoundException e)
        {
            CommLog.error("", e);
        } catch (IOException e)
        {
            CommLog.error("", e);
        } finally
        {
            if (br != null)
            {
                try
                {
                    br.close();

                } catch (Exception e)
                {
                    CommLog.error("getLinesFromFile:", e);
                }
            }
        }
        return rowsets;
    }

    /**
     * 拷贝文件
     * @param from
     * @param to
     * @throws IOException
     */
    public static void Copy(String from, String to) throws IOException
    {
        RandomAccessFile fromFile = null;
        RandomAccessFile toFile = null;
        try
        {

            String currentDir = System.getProperty("user.dir");
            ALServerLog.Info("Current dir using System:" + currentDir);

            fromFile = new RandomAccessFile(from, "r");
            FileChannel fromChannel = fromFile.getChannel();

            toFile = new RandomAccessFile(to, "rw");
            FileChannel toChannel = toFile.getChannel();

            long size = fromChannel.size();

            fromChannel.transferTo(0, size, toChannel);

        } finally
        {
            if (fromFile != null)
            {
                fromFile.close();
            }
            if (toFile != null)
            {
                toFile.close();
            }
        }
    }

    /**
     * 保存内容到路径
     * @param path
     * @param content
     * @throws IOException
     */
    public static void Write(String path, byte[] content) throws IOException
    {
        Write(path, new String(content));
    }

    public static void Write(String path, String content) throws IOException
    {
        File f = new File(path);

        FileOutputStream fos = new FileOutputStream(f);
        OutputStreamWriter osw = new OutputStreamWriter(fos, "UTF-8");
        osw.write(content);
        osw.flush();
        osw.close();
    }

    /**
     * 保存内容到路径，如果目录不存在则自动创建
     *
     * @param path 文件路径
     * @param content 文件内容
     * @throws IOException 当无法创建目录或写入文件时抛出
     */
    public static void WriteWithMkdirs(String path, String content) throws IOException
    {
        File f = new File(path);

        // 确保父目录存在
        File parentDir = f.getParentFile();
        if (parentDir != null && !parentDir.exists())
        {
            if (!parentDir.mkdirs())
            {
                throw new IOException("无法创建目录: " + parentDir.getAbsolutePath());
            }
        }

        FileOutputStream fos = new FileOutputStream(f);
        OutputStreamWriter osw = new OutputStreamWriter(fos, "UTF-8");
        osw.write(content);
        osw.flush();
        osw.close();
    }

    /**
     * 保存字节数组到路径，如果目录不存在则自动创建
     *
     * @param path 文件路径
     * @param content 文件内容（字节数组）
     * @throws IOException 当无法创建目录或写入文件时抛出
     */
    public static void WriteWithMkdirs(String path, byte[] content) throws IOException
    {
        WriteWithMkdirs(path, new String(content));
    }

    /**
     * 从文件末尾开始分页读取
     * @param filename file path
     */
    public static LinkedList<String> readReverse(String filename, int _pageSize, int _pageIndex)
    {
        LinkedList<String> lines = new LinkedList<>();
        File file = new File(filename);// 文件路径
        if (!file.exists())
            return lines;
        if (file.isDirectory())
            return lines;
        if (!file.isFile())
            return lines;
        if (!file.canRead())
            return lines;

        RandomAccessFile rf = null;
        int rowCount = 0;
        try
        {
            rf = new RandomAccessFile(filename, "r");
            long fileLength = rf.length();
            long start = rf.getFilePointer();// 返回此文件中的当前偏移量
            long readIndex = start + fileLength - 1;
            String line;
            rf.seek(readIndex);// 设置偏移量为文件末尾
            int c = -1;
            while (readIndex > start)
            {
                c = rf.read();
                if (c == '\n')
                {
                    rowCount++;
                    if (rowCount > _pageSize * _pageIndex)
                    {
                        line = rf.readLine();
                        if (line != null)
                        {
                            String newLine;
                            newLine = new String(line.getBytes(StandardCharsets.ISO_8859_1), StandardCharsets.UTF_8);
                            lines.addFirst(newLine);
                        }
                    }
                    readIndex--;
                }
                readIndex--;
                rf.seek(readIndex);
                if (readIndex == 0)
                {// 当文件指针退至文件开始处，输出第一行
                    lines.addFirst(rf.readLine());
                }
                if (lines.size() >= _pageSize)
                    break;
            }
        } catch (FileNotFoundException e)
        {
            e.printStackTrace();
        } catch (IOException e)
        {
            e.printStackTrace();
        } finally
        {
            try
            {
                if (rf != null)
                    rf.close();
            } catch (IOException e)
            {
                e.printStackTrace();
            }
        }
        return lines;
    }

    /********
     * 分页读取指定行
     * @param pathName
     * @param _pageSize
     * @param _pageIndex
     * @return
     */
    public static List<String> readPage(String pathName, int _pageSize, int _pageIndex)
    {
        List<String> ret = new ArrayList<>();
        File file = new File(pathName);// 文件路径
        if (!file.exists())
            return ret;
        if (file.isDirectory())
            return ret;
        if (!file.isFile())
            return ret;
        if (!file.canRead())
            return ret;

        BufferedReader br = null;
        FileReader fr = null;
        try
        {

            String strLine = null;
            fr = new FileReader(file);
            br = new BufferedReader(fr);
            int count = 0;
            int readedLineCount = 0;
            while ((strLine = br.readLine()) != null)
            {
                readedLineCount++;
                if (readedLineCount > _pageSize * _pageIndex)
                {
                    ret.add(strLine);
                    count++;
                    if (count >= _pageSize)
                        break;
                }
            }
        } catch (Exception e)
        {
            CommLog.error("read file：{} err", pathName, e);
        } finally
        {

            if (null != br)
            {
                try
                {
                    br.close();
                } catch (Exception e2)
                {
                    CommLog.error("close BufferedReader:{} err", pathName, e2);
                }
            }
            if (null != fr)
            {
                try
                {
                    fr.close();
                } catch (Exception e2)
                {
                    CommLog.error("close FileReader:{} err", pathName, e2);
                }
            }
        }
        return ret;

    }

    /*******
     * 把文本解析成二维表
     * @param _tableName
     * @param _text
     * @param fieldLine
     * @param contentLine
     * @return
     */
    public static ArrayList<Map<String, String>> GetTableFromTxt(String _tableName, String _text, int fieldLine, int contentLine)
    {

        ArrayList<Map<String, String>> table = new ArrayList<>();
        String[] Lines = CommonFunc.charSplit(_text, new char[]{'\n'}, Integer.MAX_VALUE, false);
        String temp = null;

        String[] fields = null;

        for (int curLine = 0; curLine < Lines.length; curLine++)
        {

            try
            {
                temp = Lines[curLine];
                if (null == temp)
                {
                    break;
                }

                temp = temp.trim();
                if (temp.equals(""))
                {
                    continue;
                }
                if (fieldLine == curLine)
                {
                    // 解析表头
                    String[] values = CommonFunc.charSplit(temp, '\t');
                    if (0 >= values.length)
                    {
                        CommLog.error("ParseTable {} fieldCnt <= 0, fieldLine:{}", _tableName, fieldLine, new Exception());
                        return table;
                    }
                    // 过滤空格
                    fields = new String[values.length];
                    for (int index = 0; index < values.length; index++)
                    {
                        fields[index] = values[index].trim().toLowerCase();
                    }
                } else if (curLine >= contentLine)
                {
                    if (null == fields)
                    {
                        CommLog.error("ParseTable {} fieldCnt null, fieldLine:{}", _tableName, fieldLine, new Exception());
                        return table;
                    }

                    // 解析内容
                    String[] values = CommonFunc.charSplit(temp, '\t');
                    if (fields.length < values.length)
                    {
                        CommLog.error("ParseTable {} line:{} filed lenth:{} < content length:{} ", _tableName, curLine, fields.length, values.length, new Exception());
                        continue;
                    }

                    // 单行字典
                    Map<String, String> lineValue = new ConcurrentHashMap<>();
                    for (int index = 0; index < fields.length; index++)
                    {
                        if (values.length > index)
                        {
                            lineValue.put(fields[index], values[index].trim());
                        } else
                        {
                            lineValue.put(fields[index], "");
                        }
                    }

                    // 放入总表
                    table.add(lineValue);
                }
            } catch (Exception e)
            {
                CommLog.error("", e);
            }

        }
        return table;
    }
}
