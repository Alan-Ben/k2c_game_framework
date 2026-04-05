using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildDungeonObj
{

/// <summary>
/// 公会副本伤害排行
/// </summary>
public class GuildDungeon_DamageRankItem : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家CID
/// </summary>
private long cid;
/// <summary>
/// 分数
/// </summary>
private long value;


public GuildDungeon_DamageRankItem() {
	cid = (long)0;
	value = (long)0;
}

public GuildDungeon_DamageRankItem(
	long _cid
	, long _value
) {	cid = _cid;
	value = _value;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 玩家CID
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 玩家CID
/// </summary>
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 分数
/// </summary>
public long getValue() { return value; }
/// <summary>
/// 分数
/// </summary>
public void setValue(long _value) { value = _value; }


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
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	value = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cid);
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
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("value").Append(":").Append(value.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

