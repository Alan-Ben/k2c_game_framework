package Common.OfflineRewardObj;

import java.nio.ByteBuffer;
/*********
 * 玩家GM命令
 **/
public class Offline_PlayerGmCommand implements ALBasicProtocolPack._IALProtocolStructure {
private String command;


public Offline_PlayerGmCommand() {
	command = "";
}

public Offline_PlayerGmCommand(
	 String _command
) {	command = _command;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public String getCommand() { return command; }
public void setCommand(String _command) { command = _command; }


public final int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(command);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(command);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) command = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, command);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

