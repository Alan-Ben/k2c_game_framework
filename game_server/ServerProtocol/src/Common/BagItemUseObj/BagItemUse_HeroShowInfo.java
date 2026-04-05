package Common.BagItemUseObj;

import java.nio.ByteBuffer;
/*********
 * 背包使用道具-大臣展示信息
 **/
public class BagItemUse_HeroShowInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣id */
private long heroId;
private Common.BagItemUseEnum.EBagItemUse_HeroDrawShowType type;
private long count;


public BagItemUse_HeroShowInfo() {
	heroId = (long)0;
	type = Common.BagItemUseEnum.EBagItemUse_HeroDrawShowType.values()[0];
	count = (long)0;
}

public BagItemUse_HeroShowInfo(
	 long _heroId
	, Common.BagItemUseEnum.EBagItemUse_HeroDrawShowType _type
	, long _count
) {	heroId = _heroId;
	type = _type;
	count = _count;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 大臣id */
public long getHeroId() { return heroId; }
/** 大臣id */
public void setHeroId(long _heroId) { heroId = _heroId; }
public Common.BagItemUseEnum.EBagItemUse_HeroDrawShowType getType() { return type; }
public void setType(Common.BagItemUseEnum.EBagItemUse_HeroDrawShowType _type) { type = _type; }
public long getCount() { return count; }
public void setCount(long _count) { count = _count; }


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
	if(_buf.remaining() > 0) type = Common.BagItemUseEnum.EBagItemUse_HeroDrawShowType.EBagItemUse_HeroDrawShowType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putInt(type.ordinal());

	_buf.putLong(count);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

