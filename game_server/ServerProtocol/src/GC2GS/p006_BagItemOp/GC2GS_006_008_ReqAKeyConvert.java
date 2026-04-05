package GC2GS.p006_BagItemOp;

import java.nio.ByteBuffer;
/*********
 * 一键合成转换
 **/
public class GC2GS_006_008_ReqAKeyConvert implements ALBasicProtocolPack._IALProtocolStructure {
/** 道具合成列表 */
private java.util.ArrayList<NPCommon.NPCommon_SingleItemConvert> itemConvertList;


public GC2GS_006_008_ReqAKeyConvert() {
	itemConvertList = new java.util.ArrayList<NPCommon.NPCommon_SingleItemConvert>();
}

public GC2GS_006_008_ReqAKeyConvert(
	 java.util.ArrayList<NPCommon.NPCommon_SingleItemConvert> _itemConvertList
) {	itemConvertList = _itemConvertList;
}

public final byte getMainOrder() { return (byte)6; }

public final byte getSubOrder() { return (byte)8; }

/** 道具合成列表 */
public java.util.ArrayList<NPCommon.NPCommon_SingleItemConvert> getItemConvertList() { return itemConvertList; }
/** 道具合成列表 */
public void addItemConvertList(NPCommon.NPCommon_SingleItemConvert _itemConvertList) { itemConvertList.add(_itemConvertList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < itemConvertList.size(); _i++) {
	_size += 4 + itemConvertList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < itemConvertList.size(); _i++) {
	_size += 4 + itemConvertList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _itemConvertListCount = _buf.getShort();
	for(int _i = 0; _i < _itemConvertListCount; _i++) { 
		NPCommon.NPCommon_SingleItemConvert _itemConvertList = new NPCommon.NPCommon_SingleItemConvert();
		if(_buf.remaining() <= 0) return;
	int __itemConvertListCustLen = _buf.getInt();
	int __itemConvertListCurPos = _buf.position();
	_itemConvertList.ReadUnzipBuf(_buf, __itemConvertListCurPos + __itemConvertListCustLen);
	_buf.position(__itemConvertListCurPos + __itemConvertListCustLen);

		itemConvertList.add(_itemConvertList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)itemConvertList.size());
	for(int _i = 0; _i < itemConvertList.size(); _i++) { 
		_buf.putInt(itemConvertList.get(_i).GetBufSize());
	itemConvertList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)6);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
	_recBuf.put((byte)8);
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

