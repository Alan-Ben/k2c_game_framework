package WCGCS2US_RB.p003_CommOp;

import java.nio.ByteBuffer;
/*********
 * 请求指定宝箱数据
 **/
public class NP2US_RB_003_008_RetGetBoxInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 宝箱配置ID */
private long boxId;
/** 宝箱状态 */
private NPEnum.ENPBoxChatStatus boxStatus;
/** 已经领取的玩家CID列表 */
private java.util.ArrayList<Long> gainedCidList;


public NP2US_RB_003_008_RetGetBoxInfo() {
	boxId = (long)0;
	boxStatus = NPEnum.ENPBoxChatStatus.values()[0];
	gainedCidList = new java.util.ArrayList<Long>();
}

public NP2US_RB_003_008_RetGetBoxInfo(
	 long _boxId
	, NPEnum.ENPBoxChatStatus _boxStatus
	, java.util.ArrayList<Long> _gainedCidList
) {	boxId = _boxId;
	boxStatus = _boxStatus;
	gainedCidList = _gainedCidList;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)8; }

/** 宝箱配置ID */
public long getBoxId() { return boxId; }
/** 宝箱配置ID */
public void setBoxId(long _boxId) { boxId = _boxId; }
/** 宝箱状态 */
public NPEnum.ENPBoxChatStatus getBoxStatus() { return boxStatus; }
/** 宝箱状态 */
public void setBoxStatus(NPEnum.ENPBoxChatStatus _boxStatus) { boxStatus = _boxStatus; }
/** 已经领取的玩家CID列表 */
public java.util.ArrayList<Long> getGainedCidList() { return gainedCidList; }
/** 已经领取的玩家CID列表 */
public void addGainedCidList(long _gainedCidList) { gainedCidList.add(_gainedCidList); }


public final int GetBufSize() {
	int _size = 12;
	_size += 2 + (gainedCidList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (gainedCidList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) boxId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) boxStatus = NPEnum.ENPBoxChatStatus.ENPBoxChatStatus_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _gainedCidListCount = _buf.getShort();
	for(int _i = 0; _i < _gainedCidListCount; _i++) { 
		long _gainedCidList = (long)0;
		if(_buf.remaining() > 0) _gainedCidList = _buf.getLong();
		gainedCidList.add(_gainedCidList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(boxId);
	_buf.putInt(boxStatus.ordinal());

	_buf.putShort((short)gainedCidList.size());
	for(int _i = 0; _i < gainedCidList.size(); _i++) { 
		_buf.putLong(gainedCidList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
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

