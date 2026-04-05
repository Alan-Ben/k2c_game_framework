package NPCommon;

import java.nio.ByteBuffer;
public class NPCommon_ItemInfo implements ALBasicProtocolPack._IALProtocolStructure {
private int itemType;
private long subId;
private long count;
private byte[] extData;


public NPCommon_ItemInfo() {
	itemType = 0;
	subId = (long)0;
	count = (long)0;
	extData = null;
}

public NPCommon_ItemInfo(
	 int _itemType
	, long _subId
	, long _count
	, byte[] _extData
) {	itemType = _itemType;
	subId = _subId;
	count = _count;
	extData = _extData;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getItemType() { return itemType; }
public void setItemType(int _itemType) { itemType = _itemType; }
public long getSubId() { return subId; }
public void setSubId(long _subId) { subId = _subId; }
public long getCount() { return count; }
public void setCount(long _count) { count = _count; }
public byte[] getExtData() { return extData; }
public java.nio.ByteBuffer get_buffer_ExtData() { if(null == extData)return null; else return ByteBuffer.wrap(extData); }

public void setExtData(byte[] _extData) { extData = _extData; }
public void setExtData(java.nio.ByteBuffer _extData) 
{
	if(null == _extData){return;}
	int _oldPos = _extData.position();
	int _bufLength = _extData.remaining();
	extData = new byte[_bufLength];
	_extData.get(extData);
	_extData.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 20;
	_size += 4 + (extData == null ? 0 : extData.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;
	_size += 4 + (extData == null ? 0 : extData.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) itemType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) subId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _extDataCount = _buf.getInt();
	if(0 < _extDataCount){
		extData = new byte[_extDataCount];
		_buf.get(extData);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(itemType);
	_buf.putLong(subId);
	_buf.putLong(count);
	_buf.putInt((extData == null ? 0 : extData.length));
	if(null != extData){_buf.put(extData);}

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

