package ALLRPC.CrossTeamServer.Team;

import java.nio.ByteBuffer;
public class CTSGetUsAllTeam_Return implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.ServerObj.ServerObj_ActivityTeamGroupList> groupTeamList;


public CTSGetUsAllTeam_Return() {
	groupTeamList = new java.util.ArrayList<Common.ServerObj.ServerObj_ActivityTeamGroupList>();
}

public CTSGetUsAllTeam_Return(
	 java.util.ArrayList<Common.ServerObj.ServerObj_ActivityTeamGroupList> _groupTeamList
) {	groupTeamList = _groupTeamList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public java.util.ArrayList<Common.ServerObj.ServerObj_ActivityTeamGroupList> getGroupTeamList() { return groupTeamList; }
public void addGroupTeamList(Common.ServerObj.ServerObj_ActivityTeamGroupList _groupTeamList) { groupTeamList.add(_groupTeamList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < groupTeamList.size(); _i++) {
	_size += 4 + groupTeamList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < groupTeamList.size(); _i++) {
	_size += 4 + groupTeamList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _groupTeamListCount = _buf.getShort();
	for(int _i = 0; _i < _groupTeamListCount; _i++) { 
		Common.ServerObj.ServerObj_ActivityTeamGroupList _groupTeamList = new Common.ServerObj.ServerObj_ActivityTeamGroupList();
		if(_buf.remaining() <= 0) return;
	int __groupTeamListCustLen = _buf.getInt();
	int __groupTeamListCurPos = _buf.position();
	_groupTeamList.ReadUnzipBuf(_buf, __groupTeamListCurPos + __groupTeamListCustLen);
	_buf.position(__groupTeamListCurPos + __groupTeamListCustLen);

		groupTeamList.add(_groupTeamList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)groupTeamList.size());
	for(int _i = 0; _i < groupTeamList.size(); _i++) { 
		_buf.putInt(groupTeamList.get(_i).GetBufSize());
	groupTeamList.get(_i).PutUnzipBuf(_buf);
	}
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

