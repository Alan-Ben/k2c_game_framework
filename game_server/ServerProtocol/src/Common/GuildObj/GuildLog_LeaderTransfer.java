package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 盟主转让
 **/
public class GuildLog_LeaderTransfer implements ALBasicProtocolPack._IALProtocolStructure {
private String oriLeaderName;
private String newLeaderName;


public GuildLog_LeaderTransfer() {
	oriLeaderName = "";
	newLeaderName = "";
}

public GuildLog_LeaderTransfer(
	 String _oriLeaderName
	, String _newLeaderName
) {	oriLeaderName = _oriLeaderName;
	newLeaderName = _newLeaderName;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public String getOriLeaderName() { return oriLeaderName; }
public void setOriLeaderName(String _oriLeaderName) { oriLeaderName = _oriLeaderName; }
public String getNewLeaderName() { return newLeaderName; }
public void setNewLeaderName(String _newLeaderName) { newLeaderName = _newLeaderName; }


public final int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(oriLeaderName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(newLeaderName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(oriLeaderName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(newLeaderName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) oriLeaderName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) newLeaderName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, oriLeaderName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, newLeaderName);
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

