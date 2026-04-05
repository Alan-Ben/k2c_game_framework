package Common.QuestObj;

import java.nio.ByteBuffer;
/*********
 * 任务数据
 **/
public class Quest_info implements ALBasicProtocolPack._IALProtocolStructure {
/** 任务配置ID */
private long questId;
/** 任务状态 */
private Common.QuestEnum.EQuestStatus questStatus;
/** 任务步骤数据 */
private Common.QuestObj.Quest_Step questStep;


public Quest_info() {
	questId = (long)0;
	questStatus = Common.QuestEnum.EQuestStatus.values()[0];
	questStep = new Common.QuestObj.Quest_Step();
}

public Quest_info(
	 long _questId
	, Common.QuestEnum.EQuestStatus _questStatus
	, Common.QuestObj.Quest_Step _questStep
) {	questId = _questId;
	questStatus = _questStatus;
	questStep = _questStep;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 任务配置ID */
public long getQuestId() { return questId; }
/** 任务配置ID */
public void setQuestId(long _questId) { questId = _questId; }
/** 任务状态 */
public Common.QuestEnum.EQuestStatus getQuestStatus() { return questStatus; }
/** 任务状态 */
public void setQuestStatus(Common.QuestEnum.EQuestStatus _questStatus) { questStatus = _questStatus; }
/** 任务步骤数据 */
public Common.QuestObj.Quest_Step getQuestStep() { return questStep; }
/** 任务步骤数据 */
public void setQuestStep(Common.QuestObj.Quest_Step _questStep) { questStep = _questStep; }


public final int GetBufSize() {
	int _size = 12;
	_size += 4 + questStep.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + questStep.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questStatus = Common.QuestEnum.EQuestStatus.EQuestStatus_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _questStepCustLen = _buf.getInt();
	int _questStepCurPos = _buf.position();
	questStep.ReadUnzipBuf(_buf, _questStepCurPos + _questStepCustLen);
	_buf.position(_questStepCurPos + _questStepCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(questId);
	_buf.putInt(questStatus.ordinal());

	_buf.putInt(questStep.GetBufSize());
	questStep.PutUnzipBuf(_buf);
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

