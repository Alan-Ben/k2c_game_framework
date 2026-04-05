using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

/// <summary>
/// 服务器信息
/// </summary>
public class NP_SYS_ServerItem : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 服务器大区
/// </summary>
private string areaTag;
/// <summary>
/// 服务器分组id
/// </summary>
private long groupId;
/// <summary>
/// 外部赋予的服务器id
/// </summary>
private int serverLogicId;
/// <summary>
/// 服务器typeId
/// </summary>
private int serverTypeId;
/// <summary>
/// 服务器名
/// </summary>
private string serverName;
/// <summary>
/// 在线状态 EServerOnlineState
/// </summary>
private int onlineStateTypeId;
/// <summary>
/// 对外显示状态 EServerShowState
/// </summary>
private int showStateTypeId;
/// <summary>
/// 是否新服
/// </summary>
private bool isNew;
/// <summary>
/// 开服时间 日期格式：yyyy-mm-dd 
/// </summary>
private string startDate;
/// <summary>
/// 预留
/// </summary>
private string ext;


public NP_SYS_ServerItem() {
	areaTag = "";
	groupId = (long)0;
	serverLogicId = 0;
	serverTypeId = 0;
	serverName = "";
	onlineStateTypeId = 0;
	showStateTypeId = 0;
	isNew = false;
	startDate = "";
	ext = "";
}

public NP_SYS_ServerItem(
	string _areaTag
	, long _groupId
	, int _serverLogicId
	, int _serverTypeId
	, string _serverName
	, int _onlineStateTypeId
	, int _showStateTypeId
	, bool _isNew
	, string _startDate
	, string _ext
) {	areaTag = _areaTag;
	groupId = _groupId;
	serverLogicId = _serverLogicId;
	serverTypeId = _serverTypeId;
	serverName = _serverName;
	onlineStateTypeId = _onlineStateTypeId;
	showStateTypeId = _showStateTypeId;
	isNew = _isNew;
	startDate = _startDate;
	ext = _ext;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 服务器大区
/// </summary>
public string getAreaTag() { return areaTag; }
/// <summary>
/// 服务器大区
/// </summary>
public void setAreaTag(string _areaTag) { areaTag = _areaTag; }
/// <summary>
/// 服务器分组id
/// </summary>
public long getGroupId() { return groupId; }
/// <summary>
/// 服务器分组id
/// </summary>
public void setGroupId(long _groupId) { groupId = _groupId; }
/// <summary>
/// 外部赋予的服务器id
/// </summary>
public int getServerLogicId() { return serverLogicId; }
/// <summary>
/// 外部赋予的服务器id
/// </summary>
public void setServerLogicId(int _serverLogicId) { serverLogicId = _serverLogicId; }
/// <summary>
/// 服务器typeId
/// </summary>
public int getServerTypeId() { return serverTypeId; }
/// <summary>
/// 服务器typeId
/// </summary>
public void setServerTypeId(int _serverTypeId) { serverTypeId = _serverTypeId; }
/// <summary>
/// 服务器名
/// </summary>
public string getServerName() { return serverName; }
/// <summary>
/// 服务器名
/// </summary>
public void setServerName(string _serverName) { serverName = _serverName; }
/// <summary>
/// 在线状态 EServerOnlineState
/// </summary>
public int getOnlineStateTypeId() { return onlineStateTypeId; }
/// <summary>
/// 在线状态 EServerOnlineState
/// </summary>
public void setOnlineStateTypeId(int _onlineStateTypeId) { onlineStateTypeId = _onlineStateTypeId; }
/// <summary>
/// 对外显示状态 EServerShowState
/// </summary>
public int getShowStateTypeId() { return showStateTypeId; }
/// <summary>
/// 对外显示状态 EServerShowState
/// </summary>
public void setShowStateTypeId(int _showStateTypeId) { showStateTypeId = _showStateTypeId; }
/// <summary>
/// 是否新服
/// </summary>
public bool getIsNew() { return isNew; }
/// <summary>
/// 是否新服
/// </summary>
public void setIsNew(bool _isNew) { isNew = _isNew; }
/// <summary>
/// 开服时间 日期格式：yyyy-mm-dd 
/// </summary>
public string getStartDate() { return startDate; }
/// <summary>
/// 开服时间 日期格式：yyyy-mm-dd 
/// </summary>
public void setStartDate(string _startDate) { startDate = _startDate; }
/// <summary>
/// 预留
/// </summary>
public string getExt() { return ext; }
/// <summary>
/// 预留
/// </summary>
public void setExt(string _ext) { ext = _ext; }


public int GetBufSize() {
	int _size = 25;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(areaTag);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(serverName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(startDate);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ext);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 27;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(areaTag);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(serverName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(startDate);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ext);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	areaTag = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serverLogicId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serverTypeId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serverName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	onlineStateTypeId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	showStateTypeId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isNew = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startDate = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	ext = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(areaTag);
	_buf.putLong(groupId);
	_buf.putInt(serverLogicId);
	_buf.putInt(serverTypeId);
	_buf.putString(serverName);
	_buf.putInt(onlineStateTypeId);
	_buf.putInt(showStateTypeId);
	_buf.put(isNew?(byte)1:(byte)0);
	_buf.putString(startDate);
	_buf.putString(ext);
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
	builder.Append("areaTag").Append(":").Append(areaTag.ToString()).Append(", ");
	builder.Append("groupId").Append(":").Append(groupId.ToString()).Append(", ");
	builder.Append("serverLogicId").Append(":").Append(serverLogicId.ToString()).Append(", ");
	builder.Append("serverTypeId").Append(":").Append(serverTypeId.ToString()).Append(", ");
	builder.Append("serverName").Append(":").Append(serverName.ToString()).Append(", ");
	builder.Append("onlineStateTypeId").Append(":").Append(onlineStateTypeId.ToString()).Append(", ");
	builder.Append("showStateTypeId").Append(":").Append(showStateTypeId.ToString()).Append(", ");
	builder.Append("isNew").Append(":").Append(isNew.ToString()).Append(", ");
	builder.Append("startDate").Append(":").Append(startDate.ToString()).Append(", ");
	builder.Append("ext").Append(":").Append(ext.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

