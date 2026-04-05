package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_043_RetGraveCelebrate implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private long buffId;
/** 领取的物品列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> gainItemList;


public GS2GC_004_043_RetGraveCelebrate() {
	cid = (long)0;
	buffId = (long)0;
	gainItemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
}

public GS2GC_004_043_RetGraveCelebrate(
	 long _cid
	, long _buffId
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _gainItemList
) {	cid = _cid;
	buffId = _buffId;
	gainItemList = _gainItemList;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)43; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getBuffId() { return buffId; }
public void setBuffId(long _buffId) { buffId = _buffId; }
/** 领取的物品列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getGainItemList() { return gainItemList; }
/** 领取的物品列表 */
public void addGainItemList(NPCommon.NPCommon_ItemInfo _gainItemList) { gainItemList.add(_gainItemList); }


public final int GetBufSize() {
	int _size = 16;
	_size += 2;
	for(int _i = 0; _i < gainItemList.size(); _i++) {
	_size += 4 + gainItemList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2;
	for(int _i = 0; _i < gainItemList.size(); _i++) {
	_size += 4 + gainItemList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buffId = _buf.getLong();
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
	_buf.putLong(cid);
	_buf.putLong(buffId);
	_buf.putShort((short)gainItemList.size());
	for(int _i = 0; _i < gainItemList.size(); _i++) { 
		_buf.putInt(gainItemList.get(_i).GetBufSize());
	gainItemList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)43);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)43);
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

