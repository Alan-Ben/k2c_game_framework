using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p004_PlayerOp
{

/// <summary>
/// 标记推送礼包为已读
/// </summary>
public class GC2GS_004_048_ReqMarkPushGiftAsRead : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 礼包组id
/// </summary>
private long groupId;
/// <summary>
/// 推送礼包id
/// </summary>
private long pushGiftId;


public GC2GS_004_048_ReqMarkPushGiftAsRead() {
	groupId = (long)0;
	pushGiftId = (long)0;
}

public GC2GS_004_048_ReqMarkPushGiftAsRead(
	long _groupId
	, long _pushGiftId
) {	groupId = _groupId;
	pushGiftId = _pushGiftId;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)48; }

/// <summary>
/// 礼包组id
/// </summary>
public long getGroupId() { return groupId; }
/// <summary>
/// 礼包组id
/// </summary>
public void setGroupId(long _groupId) { groupId = _groupId; }
/// <summary>
/// 推送礼包id
/// </summary>
public long getPushGiftId() { return pushGiftId; }
/// <summary>
/// 推送礼包id
/// </summary>
public void setPushGiftId(long _pushGiftId) { pushGiftId = _pushGiftId; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	pushGiftId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(groupId);
	_buf.putLong(pushGiftId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)48);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)48);
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
	builder.Append("groupId").Append(":").Append(groupId.ToString()).Append(", ");
	builder.Append("pushGiftId").Append(":").Append(pushGiftId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

