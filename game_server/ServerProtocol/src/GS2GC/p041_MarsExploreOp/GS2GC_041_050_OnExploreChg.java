package GS2GC.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 探索数据变化
 **/
public class GS2GC_041_050_OnExploreChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 探索数据 */
private Common.MarsObj.Mars_Explore explore;


public GS2GC_041_050_OnExploreChg() {
	explore = new Common.MarsObj.Mars_Explore();
}

public GS2GC_041_050_OnExploreChg(
	 Common.MarsObj.Mars_Explore _explore
) {	explore = _explore;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)50; }

/** 探索数据 */
public Common.MarsObj.Mars_Explore getExplore() { return explore; }
/** 探索数据 */
public void setExplore(Common.MarsObj.Mars_Explore _explore) { explore = _explore; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _exploreCustLen = _buf.getInt();
	int _exploreCurPos = _buf.position();
	explore.ReadUnzipBuf(_buf, _exploreCurPos + _exploreCustLen);
	_buf.position(_exploreCurPos + _exploreCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(explore.GetBufSize());
	explore.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)50);
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

