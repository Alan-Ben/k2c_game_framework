package NPUSServer.HotActivity.Annotation;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/*******
 * 热更活动的注解，带此注解的类被标识为入口类
 */
@Target(ElementType.TYPE)
@Retention(RetentionPolicy.RUNTIME)
public @interface HotActivityJar
{
    /**
     * @return jar包说明
     */
    String comment();
}
