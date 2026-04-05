package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人对话信息新增
 **/
public class GS2GC_015_075_OnConsortChatInfoAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 对话信息 */
private Common.ConsortObj.Consort_ChatInfo chatInfo;


public GS2GC_015_075_OnConsortChatInfoAdd() {
	chatInfo = new Common.ConsortObj.Consort_ChatInfo();
}

public GS2GC_015_075_OnConsortChatInfoAdd(
	 Common.ConsortObj.Consort_ChatInfo _chatInfo
) {	chatInfo = _chatInfo;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)75; }

/** 对话信息 */
public Common.ConsortObj.Consort_ChatInfo getChatInfo() { return chatInfo; }
/** 对话信息 */
public void setChatInfo(Common.ConsortObj.Consort_ChatInfo _chatInfo) { chatInfo = _chatInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + chatInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + chatInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _chatInfoCustLen = _buf.getInt();
	int _chatInfoCurPos = _buf.position();
	chatInfo.ReadUnzipBuf(_buf, _chatInfoCurPos + _chatInfoCustLen);
	_buf.position(_chatInfoCurPos + _chatInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(chatInfo.GetBufSize());
	chatInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)75);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)75);
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

