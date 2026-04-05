using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p023_ArenaOp
{

/// <summary>
/// 反击选择出战大臣
/// </summary>
public class GC2GS_023_006_ReqFightBackSelectHero : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 反击id
/// </summary>
private long dbId;
/// <summary>
/// 道具id
/// </summary>
private long itemId;
private long heroId;
private long buffId;


public GC2GS_023_006_ReqFightBackSelectHero() {
	dbId = (long)0;
	itemId = (long)0;
	heroId = (long)0;
	buffId = (long)0;
}

public GC2GS_023_006_ReqFightBackSelectHero(
	long _dbId
	, long _itemId
	, long _heroId
	, long _buffId
) {	dbId = _dbId;
	itemId = _itemId;
	heroId = _heroId;
	buffId = _buffId;
}

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)6; }

/// <summary>
/// 反击id
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 反击id
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }
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
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buffId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putLong(itemId);
	_buf.putLong(heroId);
	_buf.putLong(buffId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)6);
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
	builder.Append("dbId").Append(":").Append(dbId.ToString()).Append(", ");
	builder.Append("itemId").Append(":").Append(itemId.ToString()).Append(", ");
	builder.Append("heroId").Append(":").Append(heroId.ToString()).Append(", ");
	builder.Append("buffId").Append(":").Append(buffId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

