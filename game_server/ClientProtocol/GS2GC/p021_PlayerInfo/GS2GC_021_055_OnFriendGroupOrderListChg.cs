using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p021_PlayerInfo
{

/// <summary>
/// 好友分组顺序变更
/// </summary>
public class GS2GC_021_055_OnFriendGroupOrderListChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 好友分组顺序
/// </summary>
private List<long> friendGroupOrderList;


public GS2GC_021_055_OnFriendGroupOrderListChg() {
	friendGroupOrderList = new List<long>();
}

public GS2GC_021_055_OnFriendGroupOrderListChg(
	List<long> _friendGroupOrderList
) {	friendGroupOrderList = _friendGroupOrderList;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)55; }

/// <summary>
/// 好友分组顺序
/// </summary>
public List<long> getFriendGroupOrderList() { return friendGroupOrderList; }
/// <summary>
/// 好友分组顺序
/// </summary>
public void addFriendGroupOrderList(long _friendGroupOrderList) { friendGroupOrderList.Add(_friendGroupOrderList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (friendGroupOrderList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (friendGroupOrderList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _friendGroupOrderListCount = _buf.getShort();
	for(int _i = 0; _i < _friendGroupOrderListCount; _i++) { 
		long _friendGroupOrderList = (long)0;
		_friendGroupOrderList = _buf.getLong();
		friendGroupOrderList.Add(_friendGroupOrderList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)friendGroupOrderList.Count);
	for(int _i = 0; _i < friendGroupOrderList.Count; _i++) { 
		_buf.putLong(friendGroupOrderList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)55);
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
	builder.Append("friendGroupOrderList").Append(":").Append(friendGroupOrderList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

