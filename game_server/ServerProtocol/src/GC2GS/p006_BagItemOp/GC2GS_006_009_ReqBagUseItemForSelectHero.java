package GC2GS.p006_BagItemOp;

import java.nio.ByteBuffer;
/*********
 * 指定大臣使用道具
 **/
public class GC2GS_006_009_ReqBagUseItemForSelectHero implements ALBasicProtocolPack._IALProtocolStructure {
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

public final byte getMainOrder() { return (byte)6; }

public final byte getSubOrder() { return (byte)9; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
public long getItemId() { return itemId; }
public void setItemId(long _itemId) { itemId = _itemId; }
public int getCount() { return count; }
public void setCount(int _count) { count = _count; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putLong(itemId);
	_buf.putInt(count);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)6);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
	_recBuf.put((byte)9);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

