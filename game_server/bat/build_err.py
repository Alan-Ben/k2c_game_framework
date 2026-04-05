# coding=utf-8
import os
from mako.template import Template

allLine = []


class SharpConf:
    def __init__(self, sharpType: str, path: str, name_space: str, class_name: str):
        self.sharpType = sharpType
        self.path = path
        self.name_space = name_space
        self.class_name = class_name


# C#程序集配置，目前只有主工程和热更工程
csharp_confs = {
    "main": SharpConf("main", "../ClientProtocol/ResultRegisters", "Common",
                      "MainRegister"),
    "hotfix": SharpConf("hotfix", "../ClientProtocol/ResultRegisters", "Common",
                        "HotFixRegister")
}
# C#注册主类数据
AllSharpMainRegisters = {
    "main": [],
    "hotfix": []
}


class ErrFile:
    def __init__(self, _className: str, _comment: str, _lines):
        self.class_name = _className
        self.lines = _lines
        self.comment = _comment


class Line:
    def __init__(self, _code: int, _name: str, _msg: str):
        self.code = _code
        self.name = _name
        self.msg = _msg


def genErrCode(cSharpType, javaPath, javaPackage, className, lines, comment):
    err_file = ErrFile(className, comment, lines)  # 错误文件数据

    # 生成java错误码文件
    java_fullPath = os.path.join(javaPath, javaPackage.replace('.', '/'))
    if not os.path.exists(java_fullPath):
        os.makedirs(java_fullPath, True)  # 循环创建目录

    java_full_path_name = os.path.join(java_fullPath, className + ".java")
    java_handler_file = open(java_full_path_name, "wt", encoding="utf-8", newline='\n')
    java_template = Template(filename='MakoTemplate/err.mako')

    java_code = java_template.render(err_file=err_file, java_package=javaPackage)
    java_handler_file.write(java_code)
    java_handler_file.close()

    # 生成C#错误码文件
    sharpConf = csharp_confs[cSharpType]
    if not os.path.exists(sharpConf.path):
        os.makedirs(sharpConf.path, True)  # 循环创建目录
    sharp_full_path_name = os.path.join(sharpConf.path, className + ".cs")  # 得到路径文件名
    sharp_handler_file = open(sharp_full_path_name, "wt", encoding="utf-8", newline='\n')  # 打开文件
    sharp_template = Template(filename='MakoTemplate/err_sharp.mako')  # 生成模板
    sharp_code = sharp_template.render(err_file=err_file, name_space=sharpConf.name_space)  # 渲染模板得到代码
    sharp_handler_file.write(sharp_code)  # 写入文件
    sharp_handler_file.close()  # 关闭文件

    AllSharpMainRegisters[cSharpType].append(className)

    print("generated java:" + java_full_path_name)
    print("generated sharp:" + sharp_full_path_name)


def tryGenErrorCode(fileName, fileFullPathName):
    name = fileName.split(".")[0]
    words = name.split("_")
    comment = ""  # 类注释
    if len(words) < 2:
        print("skip file :" + fileName)
        return
    if not words[0].isdigit():
        print("skip file " + fileName)
        return
    mainId = int(words[0])
    className = words[1].strip()
    if len(words) > 2:
        comment = words[2]

    fileHandler = open(fileFullPathName, "r", encoding='utf-8')
    lines = []  # 本文件的所有行
    javaPath = ""
    javaPackage = ""
    cSharpType = "main"

    idSet = set()
    for line in fileHandler.readlines():
        lineTxt = line.strip()
        if len(lineTxt) == 0:
            continue
        if lineTxt.startswith("java_path"):
            javaPath = lineTxt.split("=")[1].strip()
            continue
        if lineTxt.startswith("java_package"):
            javaPackage = lineTxt.split("=")[1].strip()
            continue
        if lineTxt.startswith("csharp_type"):
            cSharpType = lineTxt.split("=")[1].strip()
            continue
        words = lineTxt.split("\t")
        if len(words) < 3:
            print("skip line:" + lineTxt)
        if words[0].isdigit():
            subId = int(words[0])
            if subId in idSet:
                print(f"!!!warning:duplicated err sub id:{subId} in: {lineTxt}")
                continue
            oneLine = Line(mainId * 10000 + subId, words[1].strip(), words[2].strip())
            lines.append(oneLine)
            allLine.append(oneLine)
            idSet.add(subId)

    if len(javaPath) == 0:
        print("not found java_path,skip file:" + fileName)
        return

    if len(javaPackage) == 0:
        print("not found javaPackage,skip file:" + fileName)
        return

    genErrCode(cSharpType, javaPath, javaPackage, className, lines, comment)


def genSharpRegister(conf: SharpConf, class_name_list: list):
    if not os.path.exists(conf.path):
        os.makedirs(conf.path, True)  # 循环创建目录
    sharp_full_path_name = os.path.join(conf.path, conf.class_name + ".cs")  # 得到路径文件名
    sharp_handler_file = open(sharp_full_path_name, "wt", encoding="utf-8", newline='\n')  # 打开文件
    sharp_template = Template(filename='MakoTemplate/err_sharp_register.mako')  # 生成模板
    sharp_code = sharp_template.render(conf=conf, class_name_list=class_name_list)  # 渲染模板得到代码
    sharp_handler_file.write(sharp_code)  # 写入文件
    sharp_handler_file.close()  # 关闭文件

    print("generated sharp register:" + sharp_full_path_name)


def walkFile(rootDir):
    for root, dirs, files in os.walk(rootDir):
        for f in files:
            if f.endswith(".txt"):
                fullPath = os.path.join(root, f)
                tryGenErrorCode(f, fullPath)

    # 生成CSharp注册器主类
    for sharpType in AllSharpMainRegisters:
        conf = csharp_confs[sharpType]
        classNameList = AllSharpMainRegisters[sharpType]
        if len(classNameList) == 0:
            continue
        genSharpRegister(conf, classNameList)


if __name__ == '__main__':
    walkFile("./err")
    file = open("./err/all_err.txt", "w", encoding="utf-8")

    txtLines = [f'{x.code}\t{x.name}\t{x.msg}' for x in allLine]
    allErrTxt = '\n'.join(txtLines)
    file.write("错误码\t名称\t说明\n")
    file.write(allErrTxt)
    file.close()
