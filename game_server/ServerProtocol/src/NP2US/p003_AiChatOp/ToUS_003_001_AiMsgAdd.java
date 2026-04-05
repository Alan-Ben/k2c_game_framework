package NP2US.p003_AiChatOp;

import java.nio.ByteBuffer;
/*********
 * Ai消息新增
 **/
public class ToUS_003_001_AiMsgAdd implements ALBasicProtocolPack._IALProtocolStructure {
private long usRequestId;
private int errCode;
private String response;


public ToUS_003_001_AiMsgAdd() {
	usRequestId = (long)0;
	errCode = 0;
	response = "";
}

public ToUS_003_001_AiMsgAdd(
	 long _usRequestId
	, int _errCode
	, String _response
) {	usRequestId = _usRequestId;
	errCode = _errCode;
	response = _response;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)1; }

public long getUsRequestId() { return usRequestId; }
public void setUsRequestId(long _usRequestId) { usRequestId = _usRequestId; }
public int getErrCode() { return errCode; }
public void setErrCode(int _errCode) { errCode = _errCode; }
public String getResponse() { return response; }
public void setResponse(String _response) { response = _response; }


public final int GetBufSize() {
	int _size = 12;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(response);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(response);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usRequestId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) response = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(usRequestId);
	_buf.putInt(errCode);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, response);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)1);
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

