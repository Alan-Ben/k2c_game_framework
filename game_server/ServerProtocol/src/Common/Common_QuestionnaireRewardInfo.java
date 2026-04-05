package Common;

import java.nio.ByteBuffer;
public class Common_QuestionnaireRewardInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 问卷实例id */
private long questionnaireId;
/** 是否已领取 */
private boolean hasDraw;


public Common_QuestionnaireRewardInfo() {
	questionnaireId = (long)0;
	hasDraw = false;
}

public Common_QuestionnaireRewardInfo(
	 long _questionnaireId
	, boolean _hasDraw
) {	questionnaireId = _questionnaireId;
	hasDraw = _hasDraw;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 问卷实例id */
public long getQuestionnaireId() { return questionnaireId; }
/** 问卷实例id */
public void setQuestionnaireId(long _questionnaireId) { questionnaireId = _questionnaireId; }
/** 是否已领取 */
public boolean getHasDraw() { return hasDraw; }
/** 是否已领取 */
public void setHasDraw(boolean _hasDraw) { hasDraw = _hasDraw; }


public final int GetBufSize() {
	int _size = 9;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) questionnaireId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasDraw = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(questionnaireId);
	_buf.put(hasDraw?(byte)1:(byte)0);
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

