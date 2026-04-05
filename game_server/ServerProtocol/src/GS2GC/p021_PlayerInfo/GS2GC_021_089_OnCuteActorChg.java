package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * Q版形象变更推送
 **/
public class GS2GC_021_089_OnCuteActorChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.NpPlayerInfoObj.PlayerInfo_CuteActor info;


public GS2GC_021_089_OnCuteActorChg() {
	info = new Common.NpPlayerInfoObj.PlayerInfo_CuteActor();
}

public GS2GC_021_089_OnCuteActorChg(
	 Common.NpPlayerInfoObj.PlayerInfo_CuteActor _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)89; }

public Common.NpPlayerInfoObj.PlayerInfo_CuteActor getInfo() { return info; }
public void setInfo(Common.NpPlayerInfoObj.PlayerInfo_CuteActor _info) { info = _info; }


public final int GetBufSize() {
	int _size = 17;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.position();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.position(_infoCurPos + _infoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)89);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)89);
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

