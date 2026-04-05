using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟今日建造信息
/// </summary>
public class Guild_ConstructInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 建造类型
/// </summary>
private Common.GuildEnum.EGuildConstructType type;
/// <summary>
/// 已建造次数
/// </summary>
private int num;
/// <summary>
/// 获得联盟经验
/// </summary>
private long gainGuildExpCount;
/// <summary>
/// 获得联盟财富
/// </summary>
private long gainGuildWealthCount;


public Guild_ConstructInfo() {
	type = 0;
	num = 0;
	gainGuildExpCount = (long)0;
	gainGuildWealthCount = (long)0;
}

public Guild_ConstructInfo(
	Common.GuildEnum.EGuildConstructType _type
	, int _num
	, long _gainGuildExpCount
	, long _gainGuildWealthCount
) {	type = _type;
	num = _num;
	gainGuildExpCount = _gainGuildExpCount;
	gainGuildWealthCount = _gainGuildWealthCount;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 建造类型
/// </summary>
public Common.GuildEnum.EGuildConstructType getType() { return type; }
/// <summary>
/// 建造类型
/// </summary>
public void setType(Common.GuildEnum.EGuildConstructType _type) { type = _type; }
/// <summary>
/// 已建造次数
/// </summary>
public int getNum() { return num; }
/// <summary>
/// 已建造次数
/// </summary>
public void setNum(int _num) { num = _num; }
/// <summary>
/// 获得联盟经验
/// </summary>
public long getGainGuildExpCount() { return gainGuildExpCount; }
/// <summary>
/// 获得联盟经验
/// </summary>
public void setGainGuildExpCount(long _gainGuildExpCount) { gainGuildExpCount = _gainGuildExpCount; }
/// <summary>
/// 获得联盟财富
/// </summary>
public long getGainGuildWealthCount() { return gainGuildWealthCount; }
/// <summary>
/// 获得联盟财富
/// </summary>
public void setGainGuildWealthCount(long _gainGuildWealthCount) { gainGuildWealthCount = _gainGuildWealthCount; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = (Common.GuildEnum.EGuildConstructType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	num = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainGuildExpCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainGuildWealthCount = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)type);

	_buf.putInt(num);
	_buf.putLong(gainGuildExpCount);
	_buf.putLong(gainGuildWealthCount);
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
	builder.Append("num").Append(":").Append(num.ToString()).Append(", ");
	builder.Append("gainGuildExpCount").Append(":").Append(gainGuildExpCount.ToString()).Append(", ");
	builder.Append("gainGuildWealthCount").Append(":").Append(gainGuildWealthCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

