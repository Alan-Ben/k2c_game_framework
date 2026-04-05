package GC2GS.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人-选择对话选项
 **/
public class GC2GS_015_020_ReqConsortChooseDialogueOption implements ALBasicProtocolPack._IALProtocolStructure {
/** 家人ID */
private long consortId;
/** 对话ID */
private long dialogueId;
/** 句子ID */
private long sentenceId;
/** 选项ID */
private long optionId;


public GC2GS_015_020_ReqConsortChooseDialogueOption() {
	consortId = (long)0;
	dialogueId = (long)0;
	sentenceId = (long)0;
	optionId = (long)0;
}

public GC2GS_015_020_ReqConsortChooseDialogueOption(
	 long _consortId
	, long _dialogueId
	, long _sentenceId
	, long _optionId
) {	consortId = _consortId;
	dialogueId = _dialogueId;
	sentenceId = _sentenceId;
	optionId = _optionId;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)20; }

/** 家人ID */
public long getConsortId() { return consortId; }
/** 家人ID */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 对话ID */
public long getDialogueId() { return dialogueId; }
/** 对话ID */
public void setDialogueId(long _dialogueId) { dialogueId = _dialogueId; }
/** 句子ID */
public long getSentenceId() { return sentenceId; }
/** 句子ID */
public void setSentenceId(long _sentenceId) { sentenceId = _sentenceId; }
/** 选项ID */
public long getOptionId() { return optionId; }
/** 选项ID */
public void setOptionId(long _optionId) { optionId = _optionId; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dialogueId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sentenceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) optionId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putLong(dialogueId);
	_buf.putLong(sentenceId);
	_buf.putLong(optionId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)20);
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

