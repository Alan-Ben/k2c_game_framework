package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 禁言数据变更推送
 **/
public class GS2GC_004_068_OnForbidChatChg implements ALBasicProtocolPack._IALProtocolStructure {
private NPCommon.NPCommon_ForbidChatInfo forbidChat;


public GS2GC_004_068_OnForbidChatChg() {
	forbidChat = new NPCommon.NPCommon_ForbidChatInfo();
}

public GS2GC_004_068_OnForbidChatChg(
	 NPCommon.NPCommon_ForbidChatInfo _forbidChat
) {	forbidChat = _forbidChat;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)68; }

public NPCommon.NPCommon_ForbidChatInfo getForbidChat() { return forbidChat; }
public void setForbidChat(NPCommon.NPCommon_ForbidChatInfo _forbidChat) { forbidChat = _forbidChat; }


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
	if(_buf.remaining() <= 0) return;
	int _forbidChatCustLen = _buf.getInt();
	int _forbidChatCurPos = _buf.position();
	forbidChat.ReadUnzipBuf(_buf, _forbidChatCurPos + _forbidChatCustLen);
	_buf.position(_forbidChatCurPos + _forbidChatCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(forbidChat.GetBufSize());
	forbidChat.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)68);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)68);
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

