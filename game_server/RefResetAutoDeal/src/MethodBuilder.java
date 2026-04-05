import com.thoughtworks.qdox.JavaProjectBuilder;
import com.thoughtworks.qdox.model.JavaAnnotation;
import com.thoughtworks.qdox.model.JavaClass;
import com.thoughtworks.qdox.model.JavaMethod;

import java.io.File;
import java.nio.charset.StandardCharsets;
import java.util.List;
import java.util.stream.Collectors;

/**
 * 方法构建器
 * 需要实现buildMethod方法：
 * 使用qDox解析java源文件，生成resetRef方法
 * 传入参数：源文件路径、
 * 返回：生成的方法字符串
 */
public class MethodBuilder
{
    /**
     * 生成指定java文件的JavaProjectBuilder
     */
    public static JavaProjectBuilder buildJavaProjectBuilder(String filePath)
    {
        JavaProjectBuilder builder = new JavaProjectBuilder();
        builder.setEncoding(StandardCharsets.UTF_8.toString());
        builder.addSourceTree(new File(filePath));
        return builder;
    }

    /**
     * 使用获取到的JavaProjectBuilder获取JavaClass
     */
    public static JavaClass getJavaClass(JavaProjectBuilder _builder)
    {
        return _builder.getClasses().iterator().next();
    }

    /**
     * 检查JavaClass的methods中是否有resetRef方法
     */
    public static boolean checkHasResetRef(JavaClass _clazz)
    {
        List<JavaMethod> methods = _clazz.getMethods();
        //检查methods中是否有resetRef方法
        for (JavaMethod method : methods)
        {
            if (method.getName().equals("resetRef"))
            {
                return true;
            }
        }
        return false;
    }

    /**
     * 生成resetRef方法
     * @param _clazz JavaClass
     * @return 生成的方法字符串
     */
    public static String buildMethod(JavaClass _clazz)
    {
        List<String> fieldNames = _clazz.getFields().stream()
                .filter(f -> !f.isStatic()).filter(f ->
                {
                    for (JavaAnnotation annotation : f.getAnnotations())
                    {
                        if (annotation.getType().getValue().equals("RefField"))
                        {
                            Object isIgnore = annotation.getNamedParameter("isIgnore");
                            if (isIgnore != null)
                            {
                                return !Boolean.parseBoolean(isIgnore.toString());
                            }
                        }
                    }
                    return true;
                })
                .map(f -> f.getName())
                .collect(Collectors.toList());

        String code = fieldNames.stream().map(f -> f + " = newRef." + f).collect(Collectors.joining(";\n        "));
        String clazzName = _clazz.getName();
        return "public void resetRef(RefBase _newRef)\n" +
                "    {\n" +
                "        " + clazzName + " newRef = (" + clazzName + ") _newRef;\n        " + code + ";\n" +
                "    }";
    }
}
