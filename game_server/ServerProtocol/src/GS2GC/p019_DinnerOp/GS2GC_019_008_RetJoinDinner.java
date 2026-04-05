package GS2GC.p019_DinnerOp;

import java.nio.ByteBuffer;
/*********
 * 加入宴会
 **/
public class GS2GC_019_008_RetJoinDinner implements ALBasicProtocolPack._IALProtocolStructure {
/** 领取的物品列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> gainItemList;


public GS2GC_019_008_RetJoinDinner() {
	gainItemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
}

public GS2GC_019_008_RetJoinDinner(
	 java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _gainItemList
) {	gainItemList = _gainItemList;
}

public final byte getMainOrder() { return (byte)19; }

public final byte getSubOrder() { return (byte)8; }

/** 领取的物品列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getGainItemList() { return gainItemList; }
/** 领取的物品列表 */
public void addGainItemList(NPCommon.NPCommon_ItemInfo _gainItemList) { gainItemList.add(_gainItemList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < gainItemList.size(); _i++) {
	_size += 4 + gainItemList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < gainItemList.size(); _i++) {
	_size += 4 + gainItemList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _gainItemListCount = _buf.getShort();
	for(int _i = 0; _i < _gainItemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _gainItemList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __gainItemListCustLen = _buf.getInt();
	int __gainItemListCurPos = _buf.position();
	_gainItemList.ReadUnzipBuf(_buf, __gainItemListCurPos + __gainItemListCustLen);
	_buf.position(__gainItemListCurPos + __gainItemListCustLen);

		gainItemList.add(_gainItemList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)gainItemList.size());
	for(int _i = 0; _i < gainItemList.size(); _i++) { 
		_buf.putInt(gainItemList.get(_i).GetBufSize());
	gainItemList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
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

