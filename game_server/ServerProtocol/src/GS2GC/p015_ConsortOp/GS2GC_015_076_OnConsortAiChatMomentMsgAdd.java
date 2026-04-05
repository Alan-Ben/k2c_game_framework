package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人朋友圈AI对话新增消息
 **/
public class GS2GC_015_076_OnConsortAiChatMomentMsgAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 实例id */
private long instanceId;
/** 消息内容 */
private String msg;
/** 错误码 */
private int errCode;
/** 家人ID */
private long consortId;


public GS2GC_015_076_OnConsortAiChatMomentMsgAdd() {
	instanceId = (long)0;
	msg = "";
	errCode = 0;
	consortId = (long)0;
}

public GS2GC_015_076_OnConsortAiChatMomentMsgAdd(
	 long _instanceId
	, String _msg
	, int _errCode
	, long _consortId
) {	instanceId = _instanceId;
	msg = _msg;
	errCode = _errCode;
	consortId = _consortId;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)76; }

/** 实例id */
public long getInstanceId() { return instanceId; }
/** 实例id */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 消息内容 */
public String getMsg() { return msg; }
/** 消息内容 */
public void setMsg(String _msg) { msg = _msg; }
/** 错误码 */
public int getErrCode() { return errCode; }
/** 错误码 */
public void setErrCode(int _errCode) { errCode = _errCode; }
/** 家人ID */
public long getConsortId() { return consortId; }
/** 家人ID */
public void setConsortId(long _consortId) { consortId = _consortId; }


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
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) msg = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, msg);
	_buf.putInt(errCode);
	_buf.putLong(consortId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)76);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)76);
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

