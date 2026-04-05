using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p006_BagItemOp
{

/// <summary>
/// 指定大臣使用道具
/// </summary>
public class GC2GS_006_009_ReqBagUseItemForSelectHero : ALBasicProtocolPack._IALProtocolStructure {
private long heroId;
private long itemId;
private int count;


public GC2GS_006_009_ReqBagUseItemForSelectHero() {
	heroId = (long)0;
	itemId = (long)0;
	count = 0;
}

public GC2GS_006_009_ReqBagUseItemForSelectHero(
	long _heroId
	, long _itemId
	, int _count
) {	heroId = _heroId;
	itemId = _itemId;
	count = _count;
}

public byte getMainOrder() { return (byte)6; }

public byte getSubOrder() { return (byte)9; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
public long getItemId() { return itemId; }
public void setItemId(long _itemId) { itemId = _itemId; }
public int getCount() { return count; }
public void setCount(int _count) { count = _count; }


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
	heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(heroId);
	_buf.putLong(itemId);
	_buf.putInt(count);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)6);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
	_recBuf.put((byte)9);
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
	builder.Append("heroId").Append(":").Append(heroId.ToString()).Append(", ");
	builder.Append("itemId").Append(":").Append(itemId.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

