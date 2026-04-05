package NPCommon;

import java.nio.ByteBuffer;
/*********
 * 单道具合成
 **/
public class NPCommon_SingleItemConvert implements ALBasicProtocolPack._IALProtocolStructure {
/** 目标道具 */
private NPCommon.NPCommon_ItemInfo targetItem;
/** 原道具列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> originItemList;


public NPCommon_SingleItemConvert() {
	targetItem = new NPCommon.NPCommon_ItemInfo();
	originItemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
}

public NPCommon_SingleItemConvert(
	 NPCommon.NPCommon_ItemInfo _targetItem
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _originItemList
) {	targetItem = _targetItem;
	originItemList = _originItemList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 目标道具 */
public NPCommon.NPCommon_ItemInfo getTargetItem() { return targetItem; }
/** 目标道具 */
public void setTargetItem(NPCommon.NPCommon_ItemInfo _targetItem) { targetItem = _targetItem; }
/** 原道具列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getOriginItemList() { return originItemList; }
/** 原道具列表 */
public void addOriginItemList(NPCommon.NPCommon_ItemInfo _originItemList) { originItemList.add(_originItemList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + targetItem.GetBufSize();
	_size += 2;
	for(int _i = 0; _i < originItemList.size(); _i++) {
	_size += 4 + originItemList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + targetItem.GetBufSize();
	_size += 2;
	for(int _i = 0; _i < originItemList.size(); _i++) {
	_size += 4 + originItemList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _targetItemCustLen = _buf.getInt();
	int _targetItemCurPos = _buf.position();
	targetItem.ReadUnzipBuf(_buf, _targetItemCurPos + _targetItemCustLen);
	_buf.position(_targetItemCurPos + _targetItemCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _originItemListCount = _buf.getShort();
	for(int _i = 0; _i < _originItemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _originItemList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __originItemListCustLen = _buf.getInt();
	int __originItemListCurPos = _buf.position();
	_originItemList.ReadUnzipBuf(_buf, __originItemListCurPos + __originItemListCustLen);
	_buf.position(__originItemListCurPos + __originItemListCustLen);

		originItemList.add(_originItemList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(targetItem.GetBufSize());
	targetItem.PutUnzipBuf(_buf);
	_buf.putShort((short)originItemList.size());
	for(int _i = 0; _i < originItemList.size(); _i++) { 
		_buf.putInt(originItemList.get(_i).GetBufSize());
	originItemList.get(_i).PutUnzipBuf(_buf);
	}
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

