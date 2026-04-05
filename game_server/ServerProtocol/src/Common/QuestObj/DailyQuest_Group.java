package Common.QuestObj;

import java.nio.ByteBuffer;
/*********
 * 日常任务刷新组
 **/
public class DailyQuest_Group implements ALBasicProtocolPack._IALProtocolStructure {
/** 日常任务类型 */
private Common.QuestEnum.EDailyQuestType type;
/** 刷新序列号 */
private long refreshSerial;
/** 下一次刷新时间 */
private long nextFreshTimeMs;
/** 任务目标信息 */
private java.util.ArrayList<Common.QuestObj.DailyQuest_Info> questInfoList;
/** 随机任务目标信息 */
private java.util.ArrayList<Common.QuestObj.DailyQuest_Info> randomQuestInfoList;
/** 已领取活跃度奖励id */
private java.util.ArrayList<Long> hasTakenActiveRewardRefIdList;


public DailyQuest_Group() {
	type = Common.QuestEnum.EDailyQuestType.values()[0];
	refreshSerial = (long)0;
	nextFreshTimeMs = (long)0;
	questInfoList = new java.util.ArrayList<Common.QuestObj.DailyQuest_Info>();
	randomQuestInfoList = new java.util.ArrayList<Common.QuestObj.DailyQuest_Info>();
	hasTakenActiveRewardRefIdList = new java.util.ArrayList<Long>();
}

public DailyQuest_Group(
	 Common.QuestEnum.EDailyQuestType _type
	, long _refreshSerial
	, long _nextFreshTimeMs
	, java.util.ArrayList<Common.QuestObj.DailyQuest_Info> _questInfoList
	, java.util.ArrayList<Common.QuestObj.DailyQuest_Info> _randomQuestInfoList
	, java.util.ArrayList<Long> _hasTakenActiveRewardRefIdList
) {	type = _type;
	refreshSerial = _refreshSerial;
	nextFreshTimeMs = _nextFreshTimeMs;
	questInfoList = _questInfoList;
	randomQuestInfoList = _randomQuestInfoList;
	hasTakenActiveRewardRefIdList = _hasTakenActiveRewardRefIdList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 日常任务类型 */
public Common.QuestEnum.EDailyQuestType getType() { return type; }
/** 日常任务类型 */
public void setType(Common.QuestEnum.EDailyQuestType _type) { type = _type; }
/** 刷新序列号 */
public long getRefreshSerial() { return refreshSerial; }
/** 刷新序列号 */
public void setRefreshSerial(long _refreshSerial) { refreshSerial = _refreshSerial; }
/** 下一次刷新时间 */
public long getNextFreshTimeMs() { return nextFreshTimeMs; }
/** 下一次刷新时间 */
public void setNextFreshTimeMs(long _nextFreshTimeMs) { nextFreshTimeMs = _nextFreshTimeMs; }
/** 任务目标信息 */
public java.util.ArrayList<Common.QuestObj.DailyQuest_Info> getQuestInfoList() { return questInfoList; }
/** 任务目标信息 */
public void addQuestInfoList(Common.QuestObj.DailyQuest_Info _questInfoList) { questInfoList.add(_questInfoList); }
/** 随机任务目标信息 */
public java.util.ArrayList<Common.QuestObj.DailyQuest_Info> getRandomQuestInfoList() { return randomQuestInfoList; }
/** 随机任务目标信息 */
public void addRandomQuestInfoList(Common.QuestObj.DailyQuest_Info _randomQuestInfoList) { randomQuestInfoList.add(_randomQuestInfoList); }
/** 已领取活跃度奖励id */
public java.util.ArrayList<Long> getHasTakenActiveRewardRefIdList() { return hasTakenActiveRewardRefIdList; }
/** 已领取活跃度奖励id */
public void addHasTakenActiveRewardRefIdList(long _hasTakenActiveRewardRefIdList) { hasTakenActiveRewardRefIdList.add(_hasTakenActiveRewardRefIdList); }


public final int GetBufSize() {
	int _size = 20;
	_size += 2 + (questInfoList.size() * 21);
	_size += 2 + (randomQuestInfoList.size() * 21);
	_size += 2 + (hasTakenActiveRewardRefIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;
	_size += 2 + (questInfoList.size() * 21);
	_size += 2 + (randomQuestInfoList.size() * 21);
	_size += 2 + (hasTakenActiveRewardRefIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = Common.QuestEnum.EDailyQuestType.EDailyQuestType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refreshSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) nextFreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _questInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _questInfoListCount; _i++) { 
		Common.QuestObj.DailyQuest_Info _questInfoList = new Common.QuestObj.DailyQuest_Info();
		if(_buf.remaining() <= 0) return;
	int __questInfoListCustLen = _buf.getInt();
	int __questInfoListCurPos = _buf.position();
	_questInfoList.ReadUnzipBuf(_buf, __questInfoListCurPos + __questInfoListCustLen);
	_buf.position(__questInfoListCurPos + __questInfoListCustLen);

		questInfoList.add(_questInfoList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _randomQuestInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _randomQuestInfoListCount; _i++) { 
		Common.QuestObj.DailyQuest_Info _randomQuestInfoList = new Common.QuestObj.DailyQuest_Info();
		if(_buf.remaining() <= 0) return;
	int __randomQuestInfoListCustLen = _buf.getInt();
	int __randomQuestInfoListCurPos = _buf.position();
	_randomQuestInfoList.ReadUnzipBuf(_buf, __randomQuestInfoListCurPos + __randomQuestInfoListCustLen);
	_buf.position(__randomQuestInfoListCurPos + __randomQuestInfoListCustLen);

		randomQuestInfoList.add(_randomQuestInfoList);
	}
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
	_buf.putLong(nextFreshTimeMs);
	_buf.putShort((short)questInfoList.size());
	for(int _i = 0; _i < questInfoList.size(); _i++) { 
		_buf.putInt(questInfoList.get(_i).GetBufSize());
	questInfoList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)randomQuestInfoList.size());
	for(int _i = 0; _i < randomQuestInfoList.size(); _i++) { 
		_buf.putInt(randomQuestInfoList.get(_i).GetBufSize());
	randomQuestInfoList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)hasTakenActiveRewardRefIdList.size());
	for(int _i = 0; _i < hasTakenActiveRewardRefIdList.size(); _i++) { 
		_buf.putLong(hasTakenActiveRewardRefIdList.get(_i));
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

