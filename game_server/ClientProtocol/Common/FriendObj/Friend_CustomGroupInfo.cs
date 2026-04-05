using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.FriendObj
{

/// <summary>
/// 自定义好友分组数据
/// </summary>
public class Friend_CustomGroupInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 数据库id
/// </summary>
private long dbId;
/// <summary>
/// 分组名
/// </summary>
private string name;
/// <summary>
/// 好友列表
/// </summary>
private List<long> cidList;


public Friend_CustomGroupInfo() {
	dbId = (long)0;
	name = "";
	cidList = new List<long>();
}

public Friend_CustomGroupInfo(
	long _dbId
	, string _name
	, List<long> _cidList
) {	dbId = _dbId;
	name = _name;
	cidList = _cidList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 数据库id
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 数据库id
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }
/// <summary>
/// 分组名
/// </summary>
public string getName() { return name; }
/// <summary>
/// 分组名
/// </summary>
public void setName(string _name) { name = _name; }
/// <summary>
/// 好友列表
/// </summary>
public List<long> getCidList() { return cidList; }
/// <summary>
/// 好友列表
/// </summary>
public void addCidList(long _cidList) { cidList.Add(_cidList); }


public int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += 2 + (cidList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += 2 + (cidList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	name = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _cidListCount = _buf.getShort();
	for(int _i = 0; _i < _cidListCount; _i++) { 
		long _cidList = (long)0;
		_cidList = _buf.getLong();
		cidList.Add(_cidList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putString(name);
	_buf.putShort((short)cidList.Count);
	for(int _i = 0; _i < cidList.Count; _i++) { 
		_buf.putLong(cidList[_i]);
	}
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
	builder.Append("dbId").Append(":").Append(dbId.ToString()).Append(", ");
	builder.Append("name").Append(":").Append(name.ToString()).Append(", ");
	builder.Append("cidList").Append(":").Append(cidList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

