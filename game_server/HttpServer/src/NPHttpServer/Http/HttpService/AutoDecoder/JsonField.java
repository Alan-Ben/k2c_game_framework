package NPHttpServer.Http.HttpService.AutoDecoder;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/**
 * @description: JSON字段映射注解
 * 用于标记Java字段与JSON字段的映射关系
 * <p>
 * 使用示例:
 * @JsonField("serverName") private String serverName;
 * @JsonField(value = "userId", required = true)
 * private long userId;
 * @JsonField(value = "count", defaultValue = "0")
 * private int count;
 * @author: claude
 * @date: 2025-08-06
 */
@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.FIELD)
public @interface JsonField
{

    /**
     * JSON字段名，如果为空则使用Java字段名
     * @return JSON字段名
     */
    String value() default "";

    /**
     * 是否为必填字段
     * @return true表示必填，false表示可选
     */
    boolean required() default false;

    /**
     * 默认值（字符串格式）
     * 仅在字段缺失或为null时使用
     * @return 默认值字符串
     */
    String defaultValue() default "";

    /**
     * 是否忽略此字段
     * @return true表示忽略，false表示处理
     */
    boolean ignore() default false;

    /**
     * 字段描述，用于调试和文档
     * @return 字段描述
     */
    String description() default "";
}