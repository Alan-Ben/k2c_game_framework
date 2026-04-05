using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p037_GuildDungeonOp
{

/// <summary>
/// 副本怪物数据变化
/// </summary>
public class GS2GC_037_052_OnDungeonMonsterChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 公会副本实例ID
/// </summary>
private long id;
/// <summary>
/// 怪物ID
/// </summary>
private long monsterId;
/// <summary>
/// 当前血量
/// </summary>
private long hp;


public GS2GC_037_052_OnDungeonMonsterChg() {
	id = (long)0;
	monsterId = (long)0;
	hp = (long)0;
}

public GS2GC_037_052_OnDungeonMonsterChg(
	long _id
	, long _monsterId
	, long _hp
) {	id = _id;
	monsterId = _monsterId;
	hp = _hp;
}

public byte getMainOrder() { return (byte)37; }

public byte getSubOrder() { return (byte)52; }

/// <summary>
/// 公会副本实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 公会副本实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 怪物ID
/// </summary>
public long getMonsterId() { return monsterId; }
/// <summary>
/// 怪物ID
/// </summary>
public void setMonsterId(long _monsterId) { monsterId = _monsterId; }
/// <summary>
/// 当前血量
/// </summary>
public long getHp() { return hp; }
/// <summary>
/// 当前血量
/// </summary>
public void setHp(long _hp) { hp = _hp; }


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
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	monsterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hp = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(monsterId);
	_buf.putLong(hp);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)52);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("monsterId").Append(":").Append(monsterId.ToString()).Append(", ");
	builder.Append("hp").Append(":").Append(hp.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

