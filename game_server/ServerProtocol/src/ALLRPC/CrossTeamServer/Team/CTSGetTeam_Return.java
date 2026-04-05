package ALLRPC.CrossTeamServer.Team;

import java.nio.ByteBuffer;
public class CTSGetTeam_Return implements ALBasicProtocolPack._IALProtocolStructure {
private Common.CrossTeamObj.CrossTeam_Info team;


public CTSGetTeam_Return() {
	team = new Common.CrossTeamObj.CrossTeam_Info();
}

public CTSGetTeam_Return(
	 Common.CrossTeamObj.CrossTeam_Info _team
) {	team = _team;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public Common.CrossTeamObj.CrossTeam_Info getTeam() { return team; }
public void setTeam(Common.CrossTeamObj.CrossTeam_Info _team) { team = _team; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + team.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + team.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _teamCustLen = _buf.getInt();
	int _teamCurPos = _buf.position();
	team.ReadUnzipBuf(_buf, _teamCurPos + _teamCustLen);
	_buf.position(_teamCurPos + _teamCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(team.GetBufSize());
	team.PutUnzipBuf(_buf);
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

