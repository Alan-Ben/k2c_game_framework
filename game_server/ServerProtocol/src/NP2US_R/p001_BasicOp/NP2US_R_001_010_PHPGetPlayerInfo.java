package NP2US_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2US_R_001_010_PHPGetPlayerInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private String name;


public NP2US_R_001_010_PHPGetPlayerInfo() {
	cid = (long)0;
	name = "";
}

public NP2US_R_001_010_PHPGetPlayerInfo(
	 long _cid
	, String _name
) {	cid = _cid;
	name = _name;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)10; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public String getName() { return name; }
public void setName(String _name) { name = _name; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)10);
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

