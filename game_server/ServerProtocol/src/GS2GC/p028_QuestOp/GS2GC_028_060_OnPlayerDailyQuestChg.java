package GS2GC.p028_QuestOp;

import java.nio.ByteBuffer;
/*********
 * 日常任务信息变更
 **/
public class GS2GC_028_060_OnPlayerDailyQuestChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 日常任务类型 */
private Common.QuestEnum.EDailyQuestType type;
/** 任务数据更新 */
private Common.QuestObj.DailyQuest_Info quest;


public GS2GC_028_060_OnPlayerDailyQuestChg() {
	type = Common.QuestEnum.EDailyQuestType.values()[0];
	quest = new Common.QuestObj.DailyQuest_Info();
}

public GS2GC_028_060_OnPlayerDailyQuestChg(
	 Common.QuestEnum.EDailyQuestType _type
	, Common.QuestObj.DailyQuest_Info _quest
) {	type = _type;
	quest = _quest;
}

public final byte getMainOrder() { return (byte)28; }

public final byte getSubOrder() { return (byte)60; }

/** 日常任务类型 */
public Common.QuestEnum.EDailyQuestType getType() { return type; }
/** 日常任务类型 */
public void setType(Common.QuestEnum.EDailyQuestType _type) { type = _type; }
/** 任务数据更新 */
public Common.QuestObj.DailyQuest_Info getQuest() { return quest; }
/** 任务数据更新 */
public void setQuest(Common.QuestObj.DailyQuest_Info _quest) { quest = _quest; }


public final int GetBufSize() {
	int _size = 25;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 27;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = Common.QuestEnum.EDailyQuestType.EDailyQuestType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _questCustLen = _buf.getInt();
	int _questCurPos = _buf.position();
	quest.ReadUnzipBuf(_buf, _questCurPos + _questCustLen);
	_buf.position(_questCurPos + _questCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type.ordinal());

	_buf.putInt(quest.GetBufSize());
	quest.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)60);
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

