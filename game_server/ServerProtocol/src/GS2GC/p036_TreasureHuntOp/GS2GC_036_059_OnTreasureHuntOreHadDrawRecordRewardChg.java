package GS2GC.p036_TreasureHuntOp;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-已领取矿石记录档位变更
 **/
public class GS2GC_036_059_OnTreasureHuntOreHadDrawRecordRewardChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 矿石ID */
private long oreId;
/** 已领取奖励档位列表 */
private java.util.ArrayList<Integer> hadDrawRecordRewardList;


public GS2GC_036_059_OnTreasureHuntOreHadDrawRecordRewardChg() {
	oreId = (long)0;
	hadDrawRecordRewardList = new java.util.ArrayList<Integer>();
}

public GS2GC_036_059_OnTreasureHuntOreHadDrawRecordRewardChg(
	 long _oreId
	, java.util.ArrayList<Integer> _hadDrawRecordRewardList
) {	oreId = _oreId;
	hadDrawRecordRewardList = _hadDrawRecordRewardList;
}

public final byte getMainOrder() { return (byte)36; }

public final byte getSubOrder() { return (byte)59; }

/** 矿石ID */
public long getOreId() { return oreId; }
/** 矿石ID */
public void setOreId(long _oreId) { oreId = _oreId; }
/** 已领取奖励档位列表 */
public java.util.ArrayList<Integer> getHadDrawRecordRewardList() { return hadDrawRecordRewardList; }
/** 已领取奖励档位列表 */
public void addHadDrawRecordRewardList(int _hadDrawRecordRewardList) { hadDrawRecordRewardList.add(_hadDrawRecordRewardList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (hadDrawRecordRewardList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (hadDrawRecordRewardList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) oreId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadDrawRecordRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawRecordRewardListCount; _i++) { 
		int _hadDrawRecordRewardList = 0;
		if(_buf.remaining() > 0) _hadDrawRecordRewardList = _buf.getInt();
		hadDrawRecordRewardList.add(_hadDrawRecordRewardList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(oreId);
	_buf.putShort((short)hadDrawRecordRewardList.size());
	for(int _i = 0; _i < hadDrawRecordRewardList.size(); _i++) { 
		_buf.putInt(hadDrawRecordRewardList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)59);
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

