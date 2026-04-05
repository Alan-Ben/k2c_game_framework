import com.thoughtworks.qdox.JavaProjectBuilder;
import com.thoughtworks.qdox.model.JavaClass;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class Main
{
    public static void main(String[] args)
    {
        if (args.length != 1)
        {
            System.out.println("please input dir");
            return;
        }

        System.out.println("target dir: " + args[0]);

        List<String> allFiles = FileUtils.getAllFiles(args[0]);

        List<String> skillFileList = new ArrayList<>();
        //正则表达式
        String regex = "public\\s+void\\s+resetRef\\s*\\(.*?\\)\\s*\\{.*?\\}";
        for (String fileDir : allFiles)
        {
            //先获取指定源文件的JavaProjectBuilder
            JavaProjectBuilder javaProjectBuilder = MethodBuilder.buildJavaProjectBuilder(fileDir);
            //获取JavaClass
            JavaClass clazz = MethodBuilder.getJavaClass(javaProjectBuilder);
            //检查是否有resetRef方法, 如果不存在则跳过
            if (!MethodBuilder.checkHasResetRef(clazz))
            {
                skillFileList.add(fileDir);
                continue;
            }
            //替换的内容
            String method = MethodBuilder.buildMethod(clazz);
            //替换
            try
            {
                FileContentReplacer.replaceFileContent(fileDir, regex, method);
            } catch (IOException e)
            {
                e.printStackTrace();
            }
        }

        //输出成功处理的文件数量和跳过的文件数量，最后输出跳过的文件列表
        System.out.println("success count: " + (allFiles.size() - skillFileList.size()));
        System.out.println("skip count: " + skillFileList.size() + ", skip files: ");
        for (String file : skillFileList)
        {
            System.out.println(file);
        }
    }
}