using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 加入限制信息
/// </summary>
public class Guild_JoinLimitInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 限制类型
/// </summary>
private Common.GuildEnum.EGuildJoinLimitType type;
/// <summary>
/// 数值
/// </summary>
private long value;


public Guild_JoinLimitInfo() {
	type = 0;
	value = (long)0;
}

public Guild_JoinLimitInfo(
	Common.GuildEnum.EGuildJoinLimitType _type
	, long _value
) {	type = _type;
	value = _value;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 限制类型
/// </summary>
public Common.GuildEnum.EGuildJoinLimitType getType() { return type; }
/// <summary>
/// 限制类型
/// </summary>
public void setType(Common.GuildEnum.EGuildJoinLimitType _type) { type = _type; }
/// <summary>
/// 数值
/// </summary>
public long getValue() { return value; }
/// <summary>
/// 数值
/// </summary>
public void setValue(long _value) { value = _value; }


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
	type = (Common.GuildEnum.EGuildJoinLimitType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	value = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)type);

	_buf.putLong(value);
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
	builder.Append("value").Append(":").Append(value.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

