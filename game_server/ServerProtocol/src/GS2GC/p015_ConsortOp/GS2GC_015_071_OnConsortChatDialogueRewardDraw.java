package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人对话奖励领取推送
 **/
public class GS2GC_015_071_OnConsortChatDialogueRewardDraw implements ALBasicProtocolPack._IALProtocolStructure {
private long consortId;
/** 对话ID */
private long dialogueId;


public GS2GC_015_071_OnConsortChatDialogueRewardDraw() {
	consortId = (long)0;
	dialogueId = (long)0;
}

public GS2GC_015_071_OnConsortChatDialogueRewardDraw(
	 long _consortId
	, long _dialogueId
) {	consortId = _consortId;
	dialogueId = _dialogueId;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)71; }

public long getConsortId() { return consortId; }
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
	_buf.put((byte)71);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)71);
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

