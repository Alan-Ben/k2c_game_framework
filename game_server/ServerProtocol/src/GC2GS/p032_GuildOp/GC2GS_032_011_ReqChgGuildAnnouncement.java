package GC2GS.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 请求变更联盟公告
 **/
public class GC2GS_032_011_ReqChgGuildAnnouncement implements ALBasicProtocolPack._IALProtocolStructure {
private String announcement;


public GC2GS_032_011_ReqChgGuildAnnouncement() {
	announcement = "";
}

public GC2GS_032_011_ReqChgGuildAnnouncement(
	 String _announcement
) {	announcement = _announcement;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)11; }

public String getAnnouncement() { return announcement; }
public void setAnnouncement(String _announcement) { announcement = _announcement; }


public final int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(announcement);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(announcement);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) announcement = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, announcement);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)11);
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

