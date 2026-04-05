using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p021_PlayerInfo
{

/// <summary>
/// 房间皮肤删除推送
/// </summary>
public class GS2GC_021_094_OnRoomSkinDel : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 房间皮肤ID
/// </summary>
private long roomSkinId;


public GS2GC_021_094_OnRoomSkinDel() {
	roomSkinId = (long)0;
}

public GS2GC_021_094_OnRoomSkinDel(
	long _roomSkinId
) {	roomSkinId = _roomSkinId;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)94; }

/// <summary>
/// 房间皮肤ID
/// </summary>
public long getRoomSkinId() { return roomSkinId; }
/// <summary>
/// 房间皮肤ID
/// </summary>
public void setRoomSkinId(long _roomSkinId) { roomSkinId = _roomSkinId; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	roomSkinId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(roomSkinId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)94);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)94);
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
	builder.Append("roomSkinId").Append(":").Append(roomSkinId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

