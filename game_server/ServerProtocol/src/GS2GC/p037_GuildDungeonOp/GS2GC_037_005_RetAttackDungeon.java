package GS2GC.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
public class GS2GC_037_005_RetAttackDungeon implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否击杀怪物 */
private boolean isKilled;
/** 获得物品列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> gainItemList;


public GS2GC_037_005_RetAttackDungeon() {
	isKilled = false;
	gainItemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
}

public GS2GC_037_005_RetAttackDungeon(
	 boolean _isKilled
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _gainItemList
) {	isKilled = _isKilled;
	gainItemList = _gainItemList;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)5; }

/** 是否击杀怪物 */
public boolean getIsKilled() { return isKilled; }
/** 是否击杀怪物 */
public void setIsKilled(boolean _isKilled) { isKilled = _isKilled; }
/** 获得物品列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getGainItemList() { return gainItemList; }
/** 获得物品列表 */
public void addGainItemList(NPCommon.NPCommon_ItemInfo _gainItemList) { gainItemList.add(_gainItemList); }


public final int GetBufSize() {
	int _size = 1;
	_size += 2;
	for(int _i = 0; _i < gainItemList.size(); _i++) {
	_size += 4 + gainItemList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;
	_size += 2;
	for(int _i = 0; _i < gainItemList.size(); _i++) {
	_size += 4 + gainItemList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isKilled = (_buf.get() != 0);
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
	_buf.put(isKilled?(byte)1:(byte)0);
	_buf.putShort((short)gainItemList.size());
	for(int _i = 0; _i < gainItemList.size(); _i++) { 
		_buf.putInt(gainItemList.get(_i).GetBufSize());
	gainItemList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)5);
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

