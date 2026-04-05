package WCGCS2US.p002_MatchOp;

import java.nio.ByteBuffer;
public class WCGCS2US_002_001_OnPlayerMatchSuccess implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private Common.Common_Lineup lineup;


public WCGCS2US_002_001_OnPlayerMatchSuccess() {
	uid = (long)0;
	lineup = new Common.Common_Lineup();
}

public WCGCS2US_002_001_OnPlayerMatchSuccess(
	 long _uid
	, Common.Common_Lineup _lineup
) {	uid = _uid;
	lineup = _lineup;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)1; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public Common.Common_Lineup getLineup() { return lineup; }
public void setLineup(Common.Common_Lineup _lineup) { lineup = _lineup; }


public final int GetBufSize() {
	int _size = 8;
	_size += 4 + lineup.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + lineup.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _lineupCustLen = _buf.getInt();
	int _lineupCurPos = _buf.position();
	lineup.ReadUnzipBuf(_buf, _lineupCurPos + _lineupCustLen);
	_buf.position(_lineupCurPos + _lineupCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	_buf.putInt(lineup.GetBufSize());
	lineup.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
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

