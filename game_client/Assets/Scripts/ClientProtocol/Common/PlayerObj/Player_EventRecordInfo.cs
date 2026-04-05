using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.PlayerObj
{

/// <summary>
/// 玩家事件记录信息
/// </summary>
public class Player_EventRecordInfo : ALBasicProtocolPack._IALProtocolStructure {
private Common.PlayerEnum.EPlayerEventRecordType type;
private long subId;
private long count;


public Player_EventRecordInfo() {
	type = 0;
	subId = (long)0;
	count = (long)0;
}

public Player_EventRecordInfo(
	Common.PlayerEnum.EPlayerEventRecordType _type
	, long _subId
	, long _count
) {	type = _type;
	subId = _subId;
	count = _count;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public Common.PlayerEnum.EPlayerEventRecordType getType() { return type; }
public void setType(Common.PlayerEnum.EPlayerEventRecordType _type) { type = _type; }
public long getSubId() { return subId; }
public void setSubId(long _subId) { subId = _subId; }
public long getCount() { return count; }
public void setCount(long _count) { count = _count; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = (Common.PlayerEnum.EPlayerEventRecordType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	subId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)type);

	_buf.putLong(subId);
	_buf.putLong(count);
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
	builder.Append("type").Append(":").Append(type.ToString()).Append(", ");
	builder.Append("subId").Append(":").Append(subId.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

