package GS2GC.p028_QuestOp;

import java.nio.ByteBuffer;
/*********
 * 已领取阶段奖励变更
 **/
public class GS2GC_028_061_OnPlayerDailyQuestActiveRewardChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 日常任务类型 */
private Common.QuestEnum.EDailyQuestType type;
/** 刷新序列号 */
private long refreshSerial;
/** 已领取活跃度奖励id */
private java.util.ArrayList<Long> hasTakenActiveRewardRefIdList;


public GS2GC_028_061_OnPlayerDailyQuestActiveRewardChg() {
	type = Common.QuestEnum.EDailyQuestType.values()[0];
	refreshSerial = (long)0;
	hasTakenActiveRewardRefIdList = new java.util.ArrayList<Long>();
}

public GS2GC_028_061_OnPlayerDailyQuestActiveRewardChg(
	 Common.QuestEnum.EDailyQuestType _type
	, long _refreshSerial
	, java.util.ArrayList<Long> _hasTakenActiveRewardRefIdList
) {	type = _type;
	refreshSerial = _refreshSerial;
	hasTakenActiveRewardRefIdList = _hasTakenActiveRewardRefIdList;
}

public final byte getMainOrder() { return (byte)28; }

public final byte getSubOrder() { return (byte)61; }

/** 日常任务类型 */
public Common.QuestEnum.EDailyQuestType getType() { return type; }
/** 日常任务类型 */
public void setType(Common.QuestEnum.EDailyQuestType _type) { type = _type; }
/** 刷新序列号 */
public long getRefreshSerial() { return refreshSerial; }
/** 刷新序列号 */
public void setRefreshSerial(long _refreshSerial) { refreshSerial = _refreshSerial; }
/** 已领取活跃度奖励id */
public java.util.ArrayList<Long> getHasTakenActiveRewardRefIdList() { return hasTakenActiveRewardRefIdList; }
/** 已领取活跃度奖励id */
public void addHasTakenActiveRewardRefIdList(long _hasTakenActiveRewardRefIdList) { hasTakenActiveRewardRefIdList.add(_hasTakenActiveRewardRefIdList); }


public final int GetBufSize() {
	int _size = 12;
	_size += 2 + (hasTakenActiveRewardRefIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (hasTakenActiveRewardRefIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = Common.QuestEnum.EDailyQuestType.EDailyQuestType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refreshSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hasTakenActiveRewardRefIdListCount = _buf.getShort();
	for(int _i = 0; _i < _hasTakenActiveRewardRefIdListCount; _i++) { 
		long _hasTakenActiveRewardRefIdList = (long)0;
		if(_buf.remaining() > 0) _hasTakenActiveRewardRefIdList = _buf.getLong();
		hasTakenActiveRewardRefIdList.add(_hasTakenActiveRewardRefIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type.ordinal());

	_buf.putLong(refreshSerial);
	_buf.putShort((short)hasTakenActiveRewardRefIdList.size());
	for(int _i = 0; _i < hasTakenActiveRewardRefIdList.size(); _i++) { 
		_buf.putLong(hasTakenActiveRewardRefIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)61);
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

