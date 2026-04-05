# coding=utf-8
import os
import sys
import turtle

if sys.version > '3':
    from importlib import reload
    
event_tempalte = open("./template_event.java", "r", encoding="utf-8").read()

# 重新生成目标文件；event.txt，用于数值查询
if (os.path.exists("../ServerProtocol/ProtocolScripts/Enum/event.txt")):
    os.remove("../ServerProtocol/ProtocolScripts/Enum/event.txt")
# 重新写入文件
eventFp = open("../ServerProtocol/ProtocolScripts/Enum/event.txt", "w", encoding="utf-8")

allIdMap = {}


def genEventCode(pathDir, java_package, event_name, desc):
    if not os.path.exists(pathDir):
        os.makedirs(pathDir)  # 循环创建目录
    eventClassPathName = pathDir + "/Event_" + event_name + ".java"  # 处理类完整路径名
    handlerFile = open(eventClassPathName, "wt", encoding="utf-8")

    eventId = desc.get("id")
    if eventId in allIdMap.keys():
        turtle.write("重复的事件 id:%d\n" % eventId, font=("黑体", 20, 'bold'))
        turtle.done()
        return

    txt = event_tempalte.replace("<%PackageName%>", java_package)
    txt = txt.replace("<%EventName%>", event_name)
    txt = txt.replace("<%Comment%>", desc.get("comment", ""))
    txt = txt.replace("<%id%>", str(desc.get("id")))

    field_list = desc.get("field")
    param_list = ["long _" + x[0] for x in field_list]
    paramNameList = ["\"%s\"" % x[0] for x in field_list]

    # 参数名称
    txt = txt.replace("<%ParamNameList%>", ','.join(paramNameList))

    # 参数列表
    strParamList = ""
    if len(param_list) > 0:
        strParamList = "," + ','.join(param_list)
    txt = txt.replace("<%paramList%>", strParamList)
    # 构造函数
    initStr = ''.join(["\t\tset_%s(_%s);\n" % (x[0], x[0]) for x in field_list])
    txt = txt.replace("<%InitParams%>", initStr)

    # 数值需要的文件
    eventStr = "[%s]:%s =%d" % (event_name, desc.get("comment", ""), desc.get("id"))

    # getter and setter
    strGetterSetter = ""
    for index in range(len(field_list)):
        field = field_list[index]
        fieldName = field[0]
        fieldComment = field[1]
        strGetterSetter += "\t//%s\n\tpublic void set_%s(long _%s){ setParamValue(%d, _%s); }\n" % (
            fieldComment, fieldName, fieldName, index, fieldName)
        strGetterSetter += "\tpublic long get_%s(){ return getParamValue(%d); }\n" % (fieldName, index)
        # 数值文件内容
        eventStr = eventStr + "\n" + fieldName + ":" + fieldComment
    txt = txt.replace("<%GetterSetter%>", strGetterSetter)

    handlerFile.write(txt)
    print("已生成文件：" + eventClassPathName)

    # 数值文件
    eventStr = eventStr + "\n\n"
    eventFp.write(eventStr)

    allIdMap[eventId] = eventId


def delDir(dir):
    if not os.path.exists(dir):
        return False
    if os.path.isfile(dir):
        os.remove(dir)
        return
    for i in os.listdir(dir):
        t = os.path.join(dir, i)
        if os.path.isdir(t):
            delDir(t) # 重新调用次方法
        else:
            os.unlink(t)
            print("已删除文件：" + t)
    os.removedirs(dir) # 递归删除目录下面的空文件夹


def tryGenEventCode(fileName):
    name = fileName.split(".")[0]
    tbl = __import__(name)
    tbl = reload(tbl)
    pathDir = tbl.project_root_path + "/" + tbl.java_package.replace('.', "/")
    delDir(pathDir) # 删除该目录下所有文件
    print("===")
    print("===")
    print("===")
    for key in tbl.events.keys():
        desc = tbl.events.get(key)
        genEventCode(pathDir, tbl.java_package, key, desc)


def walkFile(dir):
    sys.path.append(dir)  # 加入系统路径
    for root, dirs, files in os.walk(dir):
        for f in files:
            if f.endswith(".py"):
                tryGenEventCode(f)
        for subDir in dirs:
            walkFile(subDir)


if __name__ == '__main__':
    walkFile("./events")

eventFp.close()
