using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p024_DungeonOp
{

/// <summary>
/// 午间副本借用大臣攻击
/// </summary>
public class GC2GS_024_002_ReqMiddayDungeonBorrowAttack : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家ID
/// </summary>
private long cid;
/// <summary>
/// 大臣ID
/// </summary>
private long heroId;


public GC2GS_024_002_ReqMiddayDungeonBorrowAttack() {
	cid = (long)0;
	heroId = (long)0;
}

public GC2GS_024_002_ReqMiddayDungeonBorrowAttack(
	long _cid
	, long _heroId
) {	cid = _cid;
	heroId = _heroId;
}

public byte getMainOrder() { return (byte)24; }

public byte getSubOrder() { return (byte)2; }

/// <summary>
/// 玩家ID
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 玩家ID
/// </summary>
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 大臣ID
/// </summary>
public long getHeroId() { return heroId; }
/// <summary>
/// 大臣ID
/// </summary>
public void setHeroId(long _heroId) { heroId = _heroId; }


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
	heroId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cid);
	_buf.putLong(heroId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)2);
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
	builder.Append("heroId").Append(":").Append(heroId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

