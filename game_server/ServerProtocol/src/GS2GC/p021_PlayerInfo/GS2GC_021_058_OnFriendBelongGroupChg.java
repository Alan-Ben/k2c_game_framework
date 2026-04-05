package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 好友归属分组变更
 **/
public class GS2GC_021_058_OnFriendBelongGroupChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 目标分组数据id */
private long targetGroupDbId;
/** 变动玩家列表 */
private java.util.ArrayList<Long> cidList;


public GS2GC_021_058_OnFriendBelongGroupChg() {
	targetGroupDbId = (long)0;
	cidList = new java.util.ArrayList<Long>();
}

public GS2GC_021_058_OnFriendBelongGroupChg(
	 long _targetGroupDbId
	, java.util.ArrayList<Long> _cidList
) {	targetGroupDbId = _targetGroupDbId;
	cidList = _cidList;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)58; }

/** 目标分组数据id */
public long getTargetGroupDbId() { return targetGroupDbId; }
/** 目标分组数据id */
public void setTargetGroupDbId(long _targetGroupDbId) { targetGroupDbId = _targetGroupDbId; }
/** 变动玩家列表 */
public java.util.ArrayList<Long> getCidList() { return cidList; }
/** 变动玩家列表 */
public void addCidList(long _cidList) { cidList.add(_cidList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (cidList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (cidList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) targetGroupDbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _cidListCount = _buf.getShort();
	for(int _i = 0; _i < _cidListCount; _i++) { 
		long _cidList = (long)0;
		if(_buf.remaining() > 0) _cidList = _buf.getLong();
		cidList.add(_cidList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(targetGroupDbId);
	_buf.putShort((short)cidList.size());
	for(int _i = 0; _i < cidList.size(); _i++) { 
		_buf.putLong(cidList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)58);
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

