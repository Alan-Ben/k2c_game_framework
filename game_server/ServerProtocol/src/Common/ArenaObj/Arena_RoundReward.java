package Common.ArenaObj;

import java.nio.ByteBuffer;
/*********
 * 竞技场回合奖励
 **/
public class Arena_RoundReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 获得道具列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> gainItem;


public Arena_RoundReward() {
	gainItem = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
}

public Arena_RoundReward(
	 java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _gainItem
) {	gainItem = _gainItem;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 获得道具列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getGainItem() { return gainItem; }
/** 获得道具列表 */
public void addGainItem(NPCommon.NPCommon_ItemInfo _gainItem) { gainItem.add(_gainItem); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < gainItem.size(); _i++) {
	_size += 4 + gainItem.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < gainItem.size(); _i++) {
	_size += 4 + gainItem.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _gainItemCount = _buf.getShort();
	for(int _i = 0; _i < _gainItemCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _gainItem = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __gainItemCustLen = _buf.getInt();
	int __gainItemCurPos = _buf.position();
	_gainItem.ReadUnzipBuf(_buf, __gainItemCurPos + __gainItemCustLen);
	_buf.position(__gainItemCurPos + __gainItemCustLen);

		gainItem.add(_gainItem);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)gainItem.size());
	for(int _i = 0; _i < gainItem.size(); _i++) { 
		_buf.putInt(gainItem.get(_i).GetBufSize());
	gainItem.get(_i).PutUnzipBuf(_buf);
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

