package GS2GC.p040_MarsPeopleOp;

import java.nio.ByteBuffer;
/*********
 * 求助变更
 **/
public class GS2GC_040_055_OnHelpChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.MarsObj.Mars_Help help;


public GS2GC_040_055_OnHelpChg() {
	help = new Common.MarsObj.Mars_Help();
}

public GS2GC_040_055_OnHelpChg(
	 Common.MarsObj.Mars_Help _help
) {	help = _help;
}

public final byte getMainOrder() { return (byte)40; }

public final byte getSubOrder() { return (byte)55; }

public Common.MarsObj.Mars_Help getHelp() { return help; }
public void setHelp(Common.MarsObj.Mars_Help _help) { help = _help; }


public final int GetBufSize() {
	int _size = 33;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 35;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _helpCustLen = _buf.getInt();
	int _helpCurPos = _buf.position();
	help.ReadUnzipBuf(_buf, _helpCurPos + _helpCustLen);
	_buf.position(_helpCurPos + _helpCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(help.GetBufSize());
	help.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)40);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)40);
	_recBuf.put((byte)55);
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

