package WCGCS2US_R.p003_MailOp;

import java.nio.ByteBuffer;
public class WCGCS2US_R_003_003_ReqSendPersonalMailByMID implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private long mailId;
private String params;


public WCGCS2US_R_003_003_ReqSendPersonalMailByMID() {
	uid = (long)0;
	mailId = (long)0;
	params = "";
}

public WCGCS2US_R_003_003_ReqSendPersonalMailByMID(
	 long _uid
	, long _mailId
	, String _params
) {	uid = _uid;
	mailId = _mailId;
	params = _params;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)3; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public long getMailId() { return mailId; }
public void setMailId(long _mailId) { mailId = _mailId; }
public String getParams() { return params; }
public void setParams(String _params) { params = _params; }


public final int GetBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(params);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(params);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mailId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) params = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	_buf.putLong(mailId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, params);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)3);
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

