using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p021_PlayerInfo
{

/// <summary>
/// 变更好友分组的名称
/// </summary>
public class GC2GS_021_039_ReqChgFriendGroupName : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 目标分组数据id
/// </summary>
private long groupDbId;
private string name;


public GC2GS_021_039_ReqChgFriendGroupName() {
	groupDbId = (long)0;
	name = "";
}

public GC2GS_021_039_ReqChgFriendGroupName(
	long _groupDbId
	, string _name
) {	groupDbId = _groupDbId;
	name = _name;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)39; }

/// <summary>
/// 目标分组数据id
/// </summary>
public long getGroupDbId() { return groupDbId; }
/// <summary>
/// 目标分组数据id
/// </summary>
public void setGroupDbId(long _groupDbId) { groupDbId = _groupDbId; }
public string getName() { return name; }
public void setName(string _name) { name = _name; }


public int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	groupDbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	name = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(groupDbId);
	_buf.putString(name);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)39);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)39);
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
	builder.Append("groupDbId").Append(":").Append(groupDbId.ToString()).Append(", ");
	builder.Append("name").Append(":").Append(name.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

