package ALLRPC.CrossTeamServer.Team;

import java.nio.ByteBuffer;
public class CTSGetPlayerTeamBase_Return implements ALBasicProtocolPack._IALProtocolStructure {
private Common.CrossTeamObj.CrossTeam_BaseInfo teamBase;


public CTSGetPlayerTeamBase_Return() {
	teamBase = new Common.CrossTeamObj.CrossTeam_BaseInfo();
}

public CTSGetPlayerTeamBase_Return(
	 Common.CrossTeamObj.CrossTeam_BaseInfo _teamBase
) {	teamBase = _teamBase;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public Common.CrossTeamObj.CrossTeam_BaseInfo getTeamBase() { return teamBase; }
public void setTeamBase(Common.CrossTeamObj.CrossTeam_BaseInfo _teamBase) { teamBase = _teamBase; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + teamBase.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + teamBase.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _teamBaseCustLen = _buf.getInt();
	int _teamBaseCurPos = _buf.position();
	teamBase.ReadUnzipBuf(_buf, _teamBaseCurPos + _teamBaseCustLen);
	_buf.position(_teamBaseCurPos + _teamBaseCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(teamBase.GetBufSize());
	teamBase.PutUnzipBuf(_buf);
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

