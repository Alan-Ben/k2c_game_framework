package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_007_RetBagItemList implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<NPCommon.NPCommon_BagItemInfo> bagItemList;


public GS2GC_002_007_RetBagItemList() {
	bagItemList = new java.util.ArrayList<NPCommon.NPCommon_BagItemInfo>();
}

public GS2GC_002_007_RetBagItemList(
	 java.util.ArrayList<NPCommon.NPCommon_BagItemInfo> _bagItemList
) {	bagItemList = _bagItemList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)7; }

public java.util.ArrayList<NPCommon.NPCommon_BagItemInfo> getBagItemList() { return bagItemList; }
public void addBagItemList(NPCommon.NPCommon_BagItemInfo _bagItemList) { bagItemList.add(_bagItemList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (bagItemList.size() * 32);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (bagItemList.size() * 32);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _bagItemListCount = _buf.getShort();
	for(int _i = 0; _i < _bagItemListCount; _i++) { 
		NPCommon.NPCommon_BagItemInfo _bagItemList = new NPCommon.NPCommon_BagItemInfo();
		if(_buf.remaining() <= 0) return;
	int __bagItemListCustLen = _buf.getInt();
	int __bagItemListCurPos = _buf.position();
	_bagItemList.ReadUnzipBuf(_buf, __bagItemListCurPos + __bagItemListCustLen);
	_buf.position(__bagItemListCurPos + __bagItemListCustLen);

		bagItemList.add(_bagItemList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)bagItemList.size());
	for(int _i = 0; _i < bagItemList.size(); _i++) { 
		_buf.putInt(bagItemList.get(_i).GetBufSize());
	bagItemList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)7);
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

