package GS2GC.p009_MailOp;

import java.nio.ByteBuffer;
public class GS2GC_009_006_RetAKeyTakeAll implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> gainItemList;
/** 未读的必读邮件id列表 */
private java.util.ArrayList<Long> mustReadMailList;


public GS2GC_009_006_RetAKeyTakeAll() {
	gainItemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	mustReadMailList = new java.util.ArrayList<Long>();
}

public GS2GC_009_006_RetAKeyTakeAll(
	 java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _gainItemList
	, java.util.ArrayList<Long> _mustReadMailList
) {	gainItemList = _gainItemList;
	mustReadMailList = _mustReadMailList;
}

public final byte getMainOrder() { return (byte)9; }

public final byte getSubOrder() { return (byte)6; }

public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getGainItemList() { return gainItemList; }
public void addGainItemList(NPCommon.NPCommon_ItemInfo _gainItemList) { gainItemList.add(_gainItemList); }
/** 未读的必读邮件id列表 */
public java.util.ArrayList<Long> getMustReadMailList() { return mustReadMailList; }
/** 未读的必读邮件id列表 */
public void addMustReadMailList(long _mustReadMailList) { mustReadMailList.add(_mustReadMailList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < gainItemList.size(); _i++) {
	_size += 4 + gainItemList.get(_i).GetBufSize();
	}

	_size += 2 + (mustReadMailList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < gainItemList.size(); _i++) {
	_size += 4 + gainItemList.get(_i).GetBufSize();
	}

	_size += 2 + (mustReadMailList.size() * 8);

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
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _mustReadMailListCount = _buf.getShort();
	for(int _i = 0; _i < _mustReadMailListCount; _i++) { 
		long _mustReadMailList = (long)0;
		if(_buf.remaining() > 0) _mustReadMailList = _buf.getLong();
		mustReadMailList.add(_mustReadMailList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)gainItemList.size());
	for(int _i = 0; _i < gainItemList.size(); _i++) { 
		_buf.putInt(gainItemList.get(_i).GetBufSize());
	gainItemList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)mustReadMailList.size());
	for(int _i = 0; _i < mustReadMailList.size(); _i++) { 
		_buf.putLong(mustReadMailList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)6);
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

