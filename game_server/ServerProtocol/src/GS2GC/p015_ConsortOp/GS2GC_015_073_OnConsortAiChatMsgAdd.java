package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人AI对话新增消息
 **/
public class GS2GC_015_073_OnConsortAiChatMsgAdd implements ALBasicProtocolPack._IALProtocolStructure {
private long consortId;
/** 消息内容 */
private String msg;
/** 错误码 */
private int errCode;
/** 客户端数据ID */
private long clientDataId;


public GS2GC_015_073_OnConsortAiChatMsgAdd() {
	consortId = (long)0;
	msg = "";
	errCode = 0;
	clientDataId = (long)0;
}

public GS2GC_015_073_OnConsortAiChatMsgAdd(
	 long _consortId
	, String _msg
	, int _errCode
	, long _clientDataId
) {	consortId = _consortId;
	msg = _msg;
	errCode = _errCode;
	clientDataId = _clientDataId;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)73; }

public long getConsortId() { return consortId; }
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 消息内容 */
public String getMsg() { return msg; }
/** 消息内容 */
public void setMsg(String _msg) { msg = _msg; }
/** 错误码 */
public int getErrCode() { return errCode; }
/** 错误码 */
public void setErrCode(int _errCode) { errCode = _errCode; }
/** 客户端数据ID */
public long getClientDataId() { return clientDataId; }
/** 客户端数据ID */
public void setClientDataId(long _clientDataId) { clientDataId = _clientDataId; }


public final int GetBufSize() {
	int _size = 20;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(msg);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(msg);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) msg = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientDataId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, msg);
	_buf.putInt(errCode);
	_buf.putLong(clientDataId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)73);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)73);
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

