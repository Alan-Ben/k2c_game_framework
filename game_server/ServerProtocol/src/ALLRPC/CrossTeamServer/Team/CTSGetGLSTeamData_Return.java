package ALLRPC.CrossTeamServer.Team;

import java.nio.ByteBuffer;
public class CTSGetGLSTeamData_Return implements ALBasicProtocolPack._IALProtocolStructure {
/** GLS GroupMgr所需的队伍数据 */
private Common.ServerObj.ServerObj_FirstTeam_Team teamData;


public CTSGetGLSTeamData_Return() {
	teamData = new Common.ServerObj.ServerObj_FirstTeam_Team();
}

public CTSGetGLSTeamData_Return(
	 Common.ServerObj.ServerObj_FirstTeam_Team _teamData
) {	teamData = _teamData;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** GLS GroupMgr所需的队伍数据 */
public Common.ServerObj.ServerObj_FirstTeam_Team getTeamData() { return teamData; }
/** GLS GroupMgr所需的队伍数据 */
public void setTeamData(Common.ServerObj.ServerObj_FirstTeam_Team _teamData) { teamData = _teamData; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + teamData.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + teamData.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _teamDataCustLen = _buf.getInt();
	int _teamDataCurPos = _buf.position();
	teamData.ReadUnzipBuf(_buf, _teamDataCurPos + _teamDataCustLen);
	_buf.position(_teamDataCurPos + _teamDataCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(teamData.GetBufSize());
	teamData.PutUnzipBuf(_buf);
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

