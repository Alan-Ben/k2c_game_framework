package Common.ConsortObj;

import java.nio.ByteBuffer;
/*********
 * 家人聊天句子选择结果
 **/
public class Consort_DialogueOption implements ALBasicProtocolPack._IALProtocolStructure {
/** 句子id */
private long sentenceId;
/** 选项id */
private long optionId;


public Consort_DialogueOption() {
	sentenceId = (long)0;
	optionId = (long)0;
}

public Consort_DialogueOption(
	 long _sentenceId
	, long _optionId
) {	sentenceId = _sentenceId;
	optionId = _optionId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 句子id */
public long getSentenceId() { return sentenceId; }
/** 句子id */
public void setSentenceId(long _sentenceId) { sentenceId = _sentenceId; }
/** 选项id */
public long getOptionId() { return optionId; }
/** 选项id */
public void setOptionId(long _optionId) { optionId = _optionId; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sentenceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) optionId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(sentenceId);
	_buf.putLong(optionId);
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

