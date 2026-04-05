package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_065_OnQuestionnaireRewardAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 问卷实例id */
private long questionnaireId;


public GS2GC_007_065_OnQuestionnaireRewardAdd() {
	questionnaireId = (long)0;
}

public GS2GC_007_065_OnQuestionnaireRewardAdd(
	 long _questionnaireId
) {	questionnaireId = _questionnaireId;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)65; }

/** 问卷实例id */
public long getQuestionnaireId() { return questionnaireId; }
/** 问卷实例id */
public void setQuestionnaireId(long _questionnaireId) { questionnaireId = _questionnaireId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questionnaireId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(questionnaireId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)65);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)65);
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

