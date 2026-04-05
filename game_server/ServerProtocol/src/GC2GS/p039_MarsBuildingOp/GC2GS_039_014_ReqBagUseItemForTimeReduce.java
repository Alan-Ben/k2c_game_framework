package GC2GS.p039_MarsBuildingOp;

import java.nio.ByteBuffer;
/*********
 * 使用时间减少道具
 **/
public class GC2GS_039_014_ReqBagUseItemForTimeReduce implements ALBasicProtocolPack._IALProtocolStructure {
/** 使用道具列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> useItemList;
private Common.MarsEnum.EMarsBagItemUseTimeType objType;
/** 对象ID */
private long objId;


public GC2GS_039_014_ReqBagUseItemForTimeReduce() {
	useItemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	objType = Common.MarsEnum.EMarsBagItemUseTimeType.values()[0];
	objId = (long)0;
}

public GC2GS_039_014_ReqBagUseItemForTimeReduce(
	 java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _useItemList
	, Common.MarsEnum.EMarsBagItemUseTimeType _objType
	, long _objId
) {	useItemList = _useItemList;
	objType = _objType;
	objId = _objId;
}

public final byte getMainOrder() { return (byte)39; }

public final byte getSubOrder() { return (byte)14; }

/** 使用道具列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getUseItemList() { return useItemList; }
/** 使用道具列表 */
public void addUseItemList(NPCommon.NPCommon_ItemInfo _useItemList) { useItemList.add(_useItemList); }
public Common.MarsEnum.EMarsBagItemUseTimeType getObjType() { return objType; }
public void setObjType(Common.MarsEnum.EMarsBagItemUseTimeType _objType) { objType = _objType; }
/** 对象ID */
public long getObjId() { return objId; }
/** 对象ID */
public void setObjId(long _objId) { objId = _objId; }


public final int GetBufSize() {
	int _size = 12;
	_size += 2;
	for(int _i = 0; _i < useItemList.size(); _i++) {
	_size += 4 + useItemList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 2;
	for(int _i = 0; _i < useItemList.size(); _i++) {
	_size += 4 + useItemList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _useItemListCount = _buf.getShort();
	for(int _i = 0; _i < _useItemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _useItemList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __useItemListCustLen = _buf.getInt();
	int __useItemListCurPos = _buf.position();
	_useItemList.ReadUnzipBuf(_buf, __useItemListCurPos + __useItemListCustLen);
	_buf.position(__useItemListCurPos + __useItemListCustLen);

		useItemList.add(_useItemList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) objType = Common.MarsEnum.EMarsBagItemUseTimeType.EMarsBagItemUseTimeType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) objId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)useItemList.size());
	for(int _i = 0; _i < useItemList.size(); _i++) { 
		_buf.putInt(useItemList.get(_i).GetBufSize());
	useItemList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(objType.ordinal());

	_buf.putLong(objId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)39);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
	_recBuf.put((byte)14);
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

