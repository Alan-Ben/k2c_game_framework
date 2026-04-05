package GC2GS.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
/*********
 * 公会副本-一键领取所有奖励
 **/
public class GC2GS_037_008_ReqGainAllDungeonReward implements ALBasicProtocolPack._IALProtocolStructure {


public GC2GS_037_008_ReqGainAllDungeonReward() {
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)8; }



public final int GetBufSize() {
	int _size = 0;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
}

public final void PutUnzipBuf(ByteBuffer _buf) {
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)8);
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

