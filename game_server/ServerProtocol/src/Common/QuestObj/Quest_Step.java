package Common.QuestObj;

import java.nio.ByteBuffer;
/*********
 * 任务步骤数据
 **/
public class Quest_Step implements ALBasicProtocolPack._IALProtocolStructure {
private long questStep;
/** 任务目标数据列表 */
private java.util.ArrayList<Common.QuestObj.Quest_Target> targetList;
/** 任务超时时间标记，单位秒。0表示永久 */
private int expireTimeTagS;


public Quest_Step() {
	questStep = (long)0;
	targetList = new java.util.ArrayList<Common.QuestObj.Quest_Target>();
	expireTimeTagS = 0;
}

public Quest_Step(
	 long _questStep
	, java.util.ArrayList<Common.QuestObj.Quest_Target> _targetList
	, int _expireTimeTagS
) {	questStep = _questStep;
	targetList = _targetList;
	expireTimeTagS = _expireTimeTagS;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getQuestStep() { return questStep; }
public void setQuestStep(long _questStep) { questStep = _questStep; }
/** 任务目标数据列表 */
public java.util.ArrayList<Common.QuestObj.Quest_Target> getTargetList() { return targetList; }
/** 任务目标数据列表 */
public void addTargetList(Common.QuestObj.Quest_Target _targetList) { targetList.add(_targetList); }
/** 任务超时时间标记，单位秒。0表示永久 */
public int getExpireTimeTagS() { return expireTimeTagS; }
/** 任务超时时间标记，单位秒。0表示永久 */
public void setExpireTimeTagS(int _expireTimeTagS) { expireTimeTagS = _expireTimeTagS; }


public final int GetBufSize() {
	int _size = 12;
	_size += 2 + (targetList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (targetList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questStep = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _targetListCount = _buf.getShort();
	for(int _i = 0; _i < _targetListCount; _i++) { 
		Common.QuestObj.Quest_Target _targetList = new Common.QuestObj.Quest_Target();
		if(_buf.remaining() <= 0) return;
	int __targetListCustLen = _buf.getInt();
	int __targetListCurPos = _buf.position();
	_targetList.ReadUnzipBuf(_buf, __targetListCurPos + __targetListCustLen);
	_buf.position(__targetListCurPos + __targetListCustLen);

		targetList.add(_targetList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expireTimeTagS = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(questStep);
	_buf.putShort((short)targetList.size());
	for(int _i = 0; _i < targetList.size(); _i++) { 
		_buf.putInt(targetList.get(_i).GetBufSize());
	targetList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(expireTimeTagS);
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

