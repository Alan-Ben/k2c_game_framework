package GC2GS.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人-领取对话奖励
 **/
public class GC2GS_015_021_ReqConsortDrawDialogueReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 家人ID */
private long consortId;
/** 对话ID */
private long dialogueId;


public GC2GS_015_021_ReqConsortDrawDialogueReward() {
	consortId = (long)0;
	dialogueId = (long)0;
}

public GC2GS_015_021_ReqConsortDrawDialogueReward(
	 long _consortId
	, long _dialogueId
) {	consortId = _consortId;
	dialogueId = _dialogueId;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)21; }

/** 家人ID */
public long getConsortId() { return consortId; }
/** 家人ID */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 对话ID */
public long getDialogueId() { return dialogueId; }
/** 对话ID */
public void setDialogueId(long _dialogueId) { dialogueId = _dialogueId; }


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
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dialogueId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putLong(dialogueId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)21);
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

