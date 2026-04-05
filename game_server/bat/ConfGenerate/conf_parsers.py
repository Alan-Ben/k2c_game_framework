import json
import os
import argparse

def generate_properties_files(json_file_path, output_dir):
    # 确保输出目录存在
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)
    
    # 读取JSON文件
    with open(json_file_path, 'r', encoding='utf-8') as f:
        config = json.load(f)
    
    # 遍历每个配置块
    for section_name, section_data in config.items():
        # 处理raw类型的配置文件
        if "raw" in section_data:
            output_path = os.path.join(output_dir, section_name)
            with open(output_path, 'w', encoding='utf-8') as f:
                json.dump(section_data["raw"], f, indent=4, ensure_ascii=False)
            continue

        if not section_name.endswith('.properties'):
            continue
            
        # 创建输出文件
        output_path = os.path.join(output_dir, section_name)
        with open(output_path, 'w', encoding='utf-8') as f:
            comments = section_data.get('_comments', {})
            other_props = {k: v for k, v in section_data.items() if k != '_comments'}
            
            # 记录当前处理的配置项序号
            current_index = 1
            
            # 写入属性和对应的注释
            for key, value in other_props.items():
                # 查找对应序号的注释
                if str(current_index) in comments:
                    f.write(f'#{comments[str(current_index)]}\n')
                
                # 写入配置项
                if isinstance(value, bool):
                    value = str(value).lower()
                f.write(f'{key} = {value}\n\n')
                
                # 更新序号
                current_index += 1

if __name__ == '__main__':
    # 创建命令行参数解析器
    parser = argparse.ArgumentParser(description='配置文件生成工具')
    parser.add_argument('--template-dir', type=str, default='conf_template',
                      help='模板目录名称 (默认: conf_template)')
    parser.add_argument('--output-dir', type=str, default='conf_output',
                      help='输出目录名称 (默认: conf_output)')
    parser.add_argument('--conf-dir', type=str, default='conf',
                      help='配置文件子目录名称 (默认: conf)')
    args = parser.parse_args()

    # 获取脚本所在目录
    script_dir = os.path.dirname(os.path.abspath(__file__))
    
    # 构建模板目录路径和输出目录路径
    template_dir = os.path.join(script_dir, args.template_dir)
    output_base_dir = os.path.join(script_dir, args.output_dir)
    
    # 创建输出目录
    if not os.path.exists(output_base_dir):
        os.makedirs(output_base_dir)
    
    if os.path.exists(template_dir):
        # 获取conf_template目录下的所有json文件
        json_files = [f for f in os.listdir(template_dir) if f.endswith('.json')]
        
        if json_files:
            for json_file in json_files:
                # 获取服务器名称（不包含.json后缀）
                server_name = os.path.splitext(json_file)[0]
                json_path = os.path.join(template_dir, json_file)
                
                # 在每个服务器目录下创建conf目录
                server_dir = os.path.join(output_base_dir, server_name)
                output_dir = os.path.join(server_dir, args.conf_dir)
                
                print(f"处理配置文件: {json_path}")
                try:
                    generate_properties_files(json_path, output_dir)
                    print(f"成功生成配置文件到: {output_dir}")
                except Exception as e:
                    print(f"处理 {json_path} 时出错: {str(e)}")
        else:
            print(f"在 conf_template 目录中未找到 JSON 文件")
    else:
        print(f"未找到 conf_template 目录在: {script_dir}")
            
    print("处理完成!")