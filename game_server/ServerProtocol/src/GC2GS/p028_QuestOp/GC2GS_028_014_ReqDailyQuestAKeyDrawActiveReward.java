package GC2GS.p028_QuestOp;

import java.nio.ByteBuffer;
/*********
 * 日常任务一键领取活跃奖励
 **/
public class GC2GS_028_014_ReqDailyQuestAKeyDrawActiveReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 任务刷新序列号 */
private long refreshSerial;
/** 日常任务类型 */
private Common.QuestEnum.EDailyQuestType type;


public GC2GS_028_014_ReqDailyQuestAKeyDrawActiveReward() {
	refreshSerial = (long)0;
	type = Common.QuestEnum.EDailyQuestType.values()[0];
}

public GC2GS_028_014_ReqDailyQuestAKeyDrawActiveReward(
	 long _refreshSerial
	, Common.QuestEnum.EDailyQuestType _type
) {	refreshSerial = _refreshSerial;
	type = _type;
}

public final byte getMainOrder() { return (byte)28; }

public final byte getSubOrder() { return (byte)14; }

/** 任务刷新序列号 */
public long getRefreshSerial() { return refreshSerial; }
/** 任务刷新序列号 */
public void setRefreshSerial(long _refreshSerial) { refreshSerial = _refreshSerial; }
/** 日常任务类型 */
public Common.QuestEnum.EDailyQuestType getType() { return type; }
/** 日常任务类型 */
public void setType(Common.QuestEnum.EDailyQuestType _type) { type = _type; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refreshSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = Common.QuestEnum.EDailyQuestType.EDailyQuestType_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(refreshSerial);
	_buf.putInt(type.ordinal());

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
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

