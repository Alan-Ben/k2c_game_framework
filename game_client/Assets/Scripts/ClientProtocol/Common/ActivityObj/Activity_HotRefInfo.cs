using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ActivityObj
{

/// <summary>
/// 活动热更配置信息
/// </summary>
public class Activity_HotRefInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例ID
/// </summary>
private long activityInstanceId;
/// <summary>
/// 活动ID
/// </summary>
private long activityId;
/// <summary>
/// 热更文件名
/// </summary>
private string fileName;
/// <summary>
/// 热更文件MD5
/// </summary>
private string fileMd5;
/// <summary>
/// 热更文件目录
/// </summary>
private string fileDir;


public Activity_HotRefInfo() {
	activityInstanceId = (long)0;
	activityId = (long)0;
	fileName = "";
	fileMd5 = "";
	fileDir = "";
}

public Activity_HotRefInfo(
	long _activityInstanceId
	, long _activityId
	, string _fileName
	, string _fileMd5
	, string _fileDir
) {	activityInstanceId = _activityInstanceId;
	activityId = _activityId;
	fileName = _fileName;
	fileMd5 = _fileMd5;
	fileDir = _fileDir;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 活动实例ID
/// </summary>
public long getActivityInstanceId() { return activityInstanceId; }
/// <summary>
/// 活动实例ID
/// </summary>
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/// <summary>
/// 活动ID
/// </summary>
public long getActivityId() { return activityId; }
/// <summary>
/// 活动ID
/// </summary>
public void setActivityId(long _activityId) { activityId = _activityId; }
/// <summary>
/// 热更文件名
/// </summary>
public string getFileName() { return fileName; }
/// <summary>
/// 热更文件名
/// </summary>
public void setFileName(string _fileName) { fileName = _fileName; }
/// <summary>
/// 热更文件MD5
/// </summary>
public string getFileMd5() { return fileMd5; }
/// <summary>
/// 热更文件MD5
/// </summary>
public void setFileMd5(string _fileMd5) { fileMd5 = _fileMd5; }
/// <summary>
/// 热更文件目录
/// </summary>
public string getFileDir() { return fileDir; }
/// <summary>
/// 热更文件目录
/// </summary>
public void setFileDir(string _fileDir) { fileDir = _fileDir; }


public int GetBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(fileName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(fileMd5);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(fileDir);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(fileName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(fileMd5);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(fileDir);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activityId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	fileName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	fileMd5 = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	fileDir = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putLong(activityId);
	_buf.putString(fileName);
	_buf.putString(fileMd5);
	_buf.putString(fileDir);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("activityInstanceId").Append(":").Append(activityInstanceId.ToString()).Append(", ");
	builder.Append("activityId").Append(":").Append(activityId.ToString()).Append(", ");
	builder.Append("fileName").Append(":").Append(fileName.ToString()).Append(", ");
	builder.Append("fileMd5").Append(":").Append(fileMd5.ToString()).Append(", ");
	builder.Append("fileDir").Append(":").Append(fileDir.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

