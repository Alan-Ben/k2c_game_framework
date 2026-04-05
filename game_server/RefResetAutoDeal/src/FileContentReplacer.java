import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class FileContentReplacer {
    public static void replaceFileContent(String filePath, String regex, String replacement) throws IOException {
        Path path = Paths.get(filePath);

        // 检查文件是否存在
        if (!Files.exists(path) || !Files.isRegularFile(path)) {
            throw new IllegalArgumentException("文件不存在：" + filePath);
        }

        // 读取文件内容
        String content = new String(Files.readAllBytes(path), StandardCharsets.UTF_8);

        Pattern pattern = Pattern.compile(regex, Pattern.DOTALL); // 使用 DOTALL 标志
        Matcher matcher = pattern.matcher(content);

        String replacedCode = matcher.replaceAll(replacement);

        // 写入新内容
        Files.write(path, replacedCode.getBytes(StandardCharsets.UTF_8));
    }
}
