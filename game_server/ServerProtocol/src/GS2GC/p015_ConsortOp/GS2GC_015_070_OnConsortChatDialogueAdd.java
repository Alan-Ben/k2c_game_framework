package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 新增家人对话推送
 **/
public class GS2GC_015_070_OnConsortChatDialogueAdd implements ALBasicProtocolPack._IALProtocolStructure {
private long consortId;
/** 对话ID */
private long dialogueId;
/** 触发时间戳 */
private long triggerTimeMs;


public GS2GC_015_070_OnConsortChatDialogueAdd() {
	consortId = (long)0;
	dialogueId = (long)0;
	triggerTimeMs = (long)0;
}

public GS2GC_015_070_OnConsortChatDialogueAdd(
	 long _consortId
	, long _dialogueId
	, long _triggerTimeMs
) {	consortId = _consortId;
	dialogueId = _dialogueId;
	triggerTimeMs = _triggerTimeMs;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)70; }

public long getConsortId() { return consortId; }
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 对话ID */
public long getDialogueId() { return dialogueId; }
/** 对话ID */
public void setDialogueId(long _dialogueId) { dialogueId = _dialogueId; }
/** 触发时间戳 */
public long getTriggerTimeMs() { return triggerTimeMs; }
/** 触发时间戳 */
public void setTriggerTimeMs(long _triggerTimeMs) { triggerTimeMs = _triggerTimeMs; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dialogueId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) triggerTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putLong(dialogueId);
	_buf.putLong(triggerTimeMs);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)70);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)70);
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

