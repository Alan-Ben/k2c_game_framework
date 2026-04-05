using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p023_ArenaOp
{

/// <summary>
/// 指定攻击选择出战大臣
/// </summary>
public class GC2GS_023_003_ReqSelectAttackSelectHero : ALBasicProtocolPack._IALProtocolStructure {
private long opponentCid;
/// <summary>
/// 道具id
/// </summary>
private long itemId;
private long heroId;
private long buffId;


public GC2GS_023_003_ReqSelectAttackSelectHero() {
	opponentCid = (long)0;
	itemId = (long)0;
	heroId = (long)0;
	buffId = (long)0;
}

public GC2GS_023_003_ReqSelectAttackSelectHero(
	long _opponentCid
	, long _itemId
	, long _heroId
	, long _buffId
) {	opponentCid = _opponentCid;
	itemId = _itemId;
	heroId = _heroId;
	buffId = _buffId;
}

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)3; }

public long getOpponentCid() { return opponentCid; }
public void setOpponentCid(long _opponentCid) { opponentCid = _opponentCid; }
/// <summary>
/// 道具id
/// </summary>
public long getItemId() { return itemId; }
/// <summary>
/// 道具id
/// </summary>
public void setItemId(long _itemId) { itemId = _itemId; }
public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
public long getBuffId() { return buffId; }
public void setBuffId(long _buffId) { buffId = _buffId; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	opponentCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buffId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(opponentCid);
	_buf.putLong(itemId);
	_buf.putLong(heroId);
	_buf.putLong(buffId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)3);
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
	builder.Append("opponentCid").Append(":").Append(opponentCid.ToString()).Append(", ");
	builder.Append("itemId").Append(":").Append(itemId.ToString()).Append(", ");
	builder.Append("heroId").Append(":").Append(heroId.ToString()).Append(", ");
	builder.Append("buffId").Append(":").Append(buffId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

