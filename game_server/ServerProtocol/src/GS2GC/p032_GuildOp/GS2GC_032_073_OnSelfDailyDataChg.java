package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_073_OnSelfDailyDataChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildObj.Guild_MemberDailyData data;


public GS2GC_032_073_OnSelfDailyDataChg() {
	data = new Common.GuildObj.Guild_MemberDailyData();
}

public GS2GC_032_073_OnSelfDailyDataChg(
	 Common.GuildObj.Guild_MemberDailyData _data
) {	data = _data;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)73; }

public Common.GuildObj.Guild_MemberDailyData getData() { return data; }
public void setData(Common.GuildObj.Guild_MemberDailyData _data) { data = _data; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + data.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + data.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _dataCustLen = _buf.getInt();
	int _dataCurPos = _buf.position();
	data.ReadUnzipBuf(_buf, _dataCurPos + _dataCustLen);
	_buf.position(_dataCurPos + _dataCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(data.GetBufSize());
	data.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)73);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)73);
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

