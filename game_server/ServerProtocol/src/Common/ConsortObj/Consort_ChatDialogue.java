package Common.ConsortObj;

import java.nio.ByteBuffer;
/*********
 * 家人聊天对话数据
 **/
public class Consort_ChatDialogue implements ALBasicProtocolPack._IALProtocolStructure {
/** 对话id */
private long dialogueId;
/** 触发时间戳 */
private long triggerTimeMs;
/** 选项列表 */
private java.util.ArrayList<Common.ConsortObj.Consort_DialogueOption> optionList;
/** 是否领奖 */
private boolean hadDraw;
/** 领奖时间ms */
private long drawRewardTimeMs;


public Consort_ChatDialogue() {
	dialogueId = (long)0;
	triggerTimeMs = (long)0;
	optionList = new java.util.ArrayList<Common.ConsortObj.Consort_DialogueOption>();
	hadDraw = false;
	drawRewardTimeMs = (long)0;
}

public Consort_ChatDialogue(
	 long _dialogueId
	, long _triggerTimeMs
	, java.util.ArrayList<Common.ConsortObj.Consort_DialogueOption> _optionList
	, boolean _hadDraw
	, long _drawRewardTimeMs
) {	dialogueId = _dialogueId;
	triggerTimeMs = _triggerTimeMs;
	optionList = _optionList;
	hadDraw = _hadDraw;
	drawRewardTimeMs = _drawRewardTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 对话id */
public long getDialogueId() { return dialogueId; }
/** 对话id */
public void setDialogueId(long _dialogueId) { dialogueId = _dialogueId; }
/** 触发时间戳 */
public long getTriggerTimeMs() { return triggerTimeMs; }
/** 触发时间戳 */
public void setTriggerTimeMs(long _triggerTimeMs) { triggerTimeMs = _triggerTimeMs; }
/** 选项列表 */
public java.util.ArrayList<Common.ConsortObj.Consort_DialogueOption> getOptionList() { return optionList; }
/** 选项列表 */
public void addOptionList(Common.ConsortObj.Consort_DialogueOption _optionList) { optionList.add(_optionList); }
/** 是否领奖 */
public boolean getHadDraw() { return hadDraw; }
/** 是否领奖 */
public void setHadDraw(boolean _hadDraw) { hadDraw = _hadDraw; }
/** 领奖时间ms */
public long getDrawRewardTimeMs() { return drawRewardTimeMs; }
/** 领奖时间ms */
public void setDrawRewardTimeMs(long _drawRewardTimeMs) { drawRewardTimeMs = _drawRewardTimeMs; }


public final int GetBufSize() {
	int _size = 25;
	_size += 2 + (optionList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 27;
	_size += 2 + (optionList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dialogueId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) triggerTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _optionListCount = _buf.getShort();
	for(int _i = 0; _i < _optionListCount; _i++) { 
		Common.ConsortObj.Consort_DialogueOption _optionList = new Common.ConsortObj.Consort_DialogueOption();
		if(_buf.remaining() <= 0) return;
	int __optionListCustLen = _buf.getInt();
	int __optionListCurPos = _buf.position();
	_optionList.ReadUnzipBuf(_buf, __optionListCurPos + __optionListCustLen);
	_buf.position(__optionListCurPos + __optionListCustLen);

		optionList.add(_optionList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDraw = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) drawRewardTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dialogueId);
	_buf.putLong(triggerTimeMs);
	_buf.putShort((short)optionList.size());
	for(int _i = 0; _i < optionList.size(); _i++) { 
		_buf.putInt(optionList.get(_i).GetBufSize());
	optionList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.put(hadDraw?(byte)1:(byte)0);
	_buf.putLong(drawRewardTimeMs);
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

