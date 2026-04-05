package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人对话选择变更推送
 **/
public class GS2GC_015_072_OnConsortChatDialogueOptionChg implements ALBasicProtocolPack._IALProtocolStructure {
private long consortId;
/** 对话ID */
private long dialogueId;
/** 对话选项信息 */
private Common.ConsortObj.Consort_DialogueOption optionInfo;


public GS2GC_015_072_OnConsortChatDialogueOptionChg() {
	consortId = (long)0;
	dialogueId = (long)0;
	optionInfo = new Common.ConsortObj.Consort_DialogueOption();
}

public GS2GC_015_072_OnConsortChatDialogueOptionChg(
	 long _consortId
	, long _dialogueId
	, Common.ConsortObj.Consort_DialogueOption _optionInfo
) {	consortId = _consortId;
	dialogueId = _dialogueId;
	optionInfo = _optionInfo;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)72; }

public long getConsortId() { return consortId; }
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 对话ID */
public long getDialogueId() { return dialogueId; }
/** 对话ID */
public void setDialogueId(long _dialogueId) { dialogueId = _dialogueId; }
/** 对话选项信息 */
public Common.ConsortObj.Consort_DialogueOption getOptionInfo() { return optionInfo; }
/** 对话选项信息 */
public void setOptionInfo(Common.ConsortObj.Consort_DialogueOption _optionInfo) { optionInfo = _optionInfo; }


public final int GetBufSize() {
	int _size = 36;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dialogueId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _optionInfoCustLen = _buf.getInt();
	int _optionInfoCurPos = _buf.position();
	optionInfo.ReadUnzipBuf(_buf, _optionInfoCurPos + _optionInfoCustLen);
	_buf.position(_optionInfoCurPos + _optionInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putLong(dialogueId);
	_buf.putInt(optionInfo.GetBufSize());
	optionInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)72);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)72);
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

