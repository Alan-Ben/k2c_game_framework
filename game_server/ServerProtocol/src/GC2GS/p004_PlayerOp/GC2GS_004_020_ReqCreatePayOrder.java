package GC2GS.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 请求创建支付订单
 **/
public class GC2GS_004_020_ReqCreatePayOrder implements ALBasicProtocolPack._IALProtocolStructure {
/** 礼包id */
private long giftPackId;
/** 额外数据 */
private byte[] extraData;


public GC2GS_004_020_ReqCreatePayOrder() {
	giftPackId = (long)0;
	extraData = null;
}

public GC2GS_004_020_ReqCreatePayOrder(
	 long _giftPackId
	, byte[] _extraData
) {	giftPackId = _giftPackId;
	extraData = _extraData;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)20; }

/** 礼包id */
public long getGiftPackId() { return giftPackId; }
/** 礼包id */
public void setGiftPackId(long _giftPackId) { giftPackId = _giftPackId; }
/** 额外数据 */
public byte[] getExtraData() { return extraData; }
public java.nio.ByteBuffer get_buffer_ExtraData() { if(null == extraData)return null; else return ByteBuffer.wrap(extraData); }

/** 额外数据 */
public void setExtraData(byte[] _extraData) { extraData = _extraData; }
public void setExtraData(java.nio.ByteBuffer _extraData) 
{
	if(null == _extraData){return;}
	int _oldPos = _extraData.position();
	int _bufLength = _extraData.remaining();
	extraData = new byte[_bufLength];
	_extraData.get(extraData);
	_extraData.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 8;
	_size += 4 + (extraData == null ? 0 : extraData.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + (extraData == null ? 0 : extraData.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) giftPackId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _extraDataCount = _buf.getInt();
	if(0 < _extraDataCount){
		extraData = new byte[_extraDataCount];
		_buf.get(extraData);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(giftPackId);
	_buf.putInt((extraData == null ? 0 : extraData.length));
	if(null != extraData){_buf.put(extraData);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)20);
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

