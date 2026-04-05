package Common.BagItemUseObj;

import java.nio.ByteBuffer;
/*********
 * 背包使用道具-妃子展示信息
 **/
public class BagItemUse_ConsortShowInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 妃子id */
private long consortId;
private Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType type;
/** 单次数值 */
private long count;


public BagItemUse_ConsortShowInfo() {
	consortId = (long)0;
	type = Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType.values()[0];
	count = (long)0;
}

public BagItemUse_ConsortShowInfo(
	 long _consortId
	, Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType _type
	, long _count
) {	consortId = _consortId;
	type = _type;
	count = _count;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 妃子id */
public long getConsortId() { return consortId; }
/** 妃子id */
public void setConsortId(long _consortId) { consortId = _consortId; }
public Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType getType() { return type; }
public void setType(Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType _type) { type = _type; }
/** 单次数值 */
public long getCount() { return count; }
/** 单次数值 */
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
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType.EBagItemUse_ConsortDrawShowType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
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

