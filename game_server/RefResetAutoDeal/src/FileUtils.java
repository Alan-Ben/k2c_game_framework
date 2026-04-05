import java.io.File;
import java.util.ArrayList;
import java.util.List;

/**
 * 文件工具类
 */
public class FileUtils {
    public static List<String> getAllFiles(String directoryPath) {
        List<String> fileList = new ArrayList<>();
        File directory = new File(directoryPath);

        // 检查目录是否存在
        if (!directory.exists() || !directory.isDirectory()) {
            throw new IllegalArgumentException("目录不存在：" + directoryPath);
        }

        // 递归获取目录下所有文件
        getAllFilesRecursive(directory, fileList);

        return fileList;
    }

    private static void getAllFilesRecursive(File directory, List<String> fileList) {
        File[] files = directory.listFiles();

        if (files != null) {
            for (File file : files) {
                if (file.isFile()) {
                    fileList.add(file.getAbsolutePath());
                } else if (file.isDirectory()) {
                    getAllFilesRecursive(file, fileList);
                }
            }
        }
    }
}
