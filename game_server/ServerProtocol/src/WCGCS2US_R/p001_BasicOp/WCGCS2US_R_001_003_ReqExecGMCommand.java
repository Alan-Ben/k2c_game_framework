package WCGCS2US_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class WCGCS2US_R_001_003_ReqExecGMCommand implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private String gmCommand;


public WCGCS2US_R_001_003_ReqExecGMCommand() {
	uid = (long)0;
	gmCommand = "";
}

public WCGCS2US_R_001_003_ReqExecGMCommand(
	 long _uid
	, String _gmCommand
) {	uid = _uid;
	gmCommand = _gmCommand;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)3; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public String getGmCommand() { return gmCommand; }
public void setGmCommand(String _gmCommand) { gmCommand = _gmCommand; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(gmCommand);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(gmCommand);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gmCommand = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, gmCommand);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

