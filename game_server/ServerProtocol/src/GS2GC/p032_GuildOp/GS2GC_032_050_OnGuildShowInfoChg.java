package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_050_OnGuildShowInfoChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildObj.Guild_ShowInfo showInfo;


public GS2GC_032_050_OnGuildShowInfoChg() {
	showInfo = new Common.GuildObj.Guild_ShowInfo();
}

public GS2GC_032_050_OnGuildShowInfoChg(
	 Common.GuildObj.Guild_ShowInfo _showInfo
) {	showInfo = _showInfo;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)50; }

public Common.GuildObj.Guild_ShowInfo getShowInfo() { return showInfo; }
public void setShowInfo(Common.GuildObj.Guild_ShowInfo _showInfo) { showInfo = _showInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + showInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + showInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _showInfoCustLen = _buf.getInt();
	int _showInfoCurPos = _buf.position();
	showInfo.ReadUnzipBuf(_buf, _showInfoCurPos + _showInfoCustLen);
	_buf.position(_showInfoCurPos + _showInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(showInfo.GetBufSize());
	showInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
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

