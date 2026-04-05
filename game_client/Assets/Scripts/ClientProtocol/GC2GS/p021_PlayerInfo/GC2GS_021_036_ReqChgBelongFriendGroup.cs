using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p021_PlayerInfo
{

/// <summary>
/// 变更归属的好友分组
/// </summary>
public class GC2GS_021_036_ReqChgBelongFriendGroup : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 好友列表
/// </summary>
private List<long> cidList;
/// <summary>
/// 目标分组数据id
/// </summary>
private long groupDbId;


public GC2GS_021_036_ReqChgBelongFriendGroup() {
	cidList = new List<long>();
	groupDbId = (long)0;
}

public GC2GS_021_036_ReqChgBelongFriendGroup(
	List<long> _cidList
	, long _groupDbId
) {	cidList = _cidList;
	groupDbId = _groupDbId;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)36; }

/// <summary>
/// 好友列表
/// </summary>
public List<long> getCidList() { return cidList; }
/// <summary>
/// 好友列表
/// </summary>
public void addCidList(long _cidList) { cidList.Add(_cidList); }
/// <summary>
/// 目标分组数据id
/// </summary>
public long getGroupDbId() { return groupDbId; }
/// <summary>
/// 目标分组数据id
/// </summary>
public void setGroupDbId(long _groupDbId) { groupDbId = _groupDbId; }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (cidList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (cidList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _cidListCount = _buf.getShort();
	for(int _i = 0; _i < _cidListCount; _i++) { 
		long _cidList = (long)0;
		_cidList = _buf.getLong();
		cidList.Add(_cidList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	groupDbId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)cidList.Count);
	for(int _i = 0; _i < cidList.Count; _i++) { 
		_buf.putLong(cidList[_i]);
	}
	_buf.putLong(groupDbId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)36);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)36);
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
	builder.Append("cidList").Append(":").Append(cidList.ToString()).Append(", ");
	builder.Append("groupDbId").Append(":").Append(groupDbId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

