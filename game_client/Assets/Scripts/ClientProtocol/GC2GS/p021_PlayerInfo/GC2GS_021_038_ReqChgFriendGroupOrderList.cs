using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p021_PlayerInfo
{

/// <summary>
/// 变更好友分组的顺序
/// </summary>
public class GC2GS_021_038_ReqChgFriendGroupOrderList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 分组id的顺序列表
/// </summary>
private List<long> groupIdList;


public GC2GS_021_038_ReqChgFriendGroupOrderList() {
	groupIdList = new List<long>();
}

public GC2GS_021_038_ReqChgFriendGroupOrderList(
	List<long> _groupIdList
) {	groupIdList = _groupIdList;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)38; }

/// <summary>
/// 分组id的顺序列表
/// </summary>
public List<long> getGroupIdList() { return groupIdList; }
/// <summary>
/// 分组id的顺序列表
/// </summary>
public void addGroupIdList(long _groupIdList) { groupIdList.Add(_groupIdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (groupIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (groupIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _groupIdListCount = _buf.getShort();
	for(int _i = 0; _i < _groupIdListCount; _i++) { 
		long _groupIdList = (long)0;
		_groupIdList = _buf.getLong();
		groupIdList.Add(_groupIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)groupIdList.Count);
	for(int _i = 0; _i < groupIdList.Count; _i++) { 
		_buf.putLong(groupIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)38);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)38);
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
	builder.Append("groupIdList").Append(":").Append(groupIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

