using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.OfflineRewardObj
{

/// <summary>
/// 玩家禁言
/// </summary>
public class Offline_ForbidChat : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 截至时间戳（毫秒）
/// </summary>
private long endMs;
/// <summary>
/// 房间类型
/// </summary>
private int roomType;


public Offline_ForbidChat() {
	endMs = (long)0;
	roomType = 0;
}

public Offline_ForbidChat(
	long _endMs
	, int _roomType
) {	endMs = _endMs;
	roomType = _roomType;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 截至时间戳（毫秒）
/// </summary>
public long getEndMs() { return endMs; }
/// <summary>
/// 截至时间戳（毫秒）
/// </summary>
public void setEndMs(long _endMs) { endMs = _endMs; }
/// <summary>
/// 房间类型
/// </summary>
public int getRoomType() { return roomType; }
/// <summary>
/// 房间类型
/// </summary>
public void setRoomType(int _roomType) { roomType = _roomType; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	endMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	roomType = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(endMs);
	_buf.putInt(roomType);
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
	builder.Append("endMs").Append(":").Append(endMs.ToString()).Append(", ");
	builder.Append("roomType").Append(":").Append(roomType.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

