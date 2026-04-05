package GS2GC.p012_ActivityTeamOp;

import java.nio.ByteBuffer;
public class GS2GC_012_011_RetActivityTeamApplyList implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.CrossTeamObj.CrossTeam_ApplyInfo> applyList;


public GS2GC_012_011_RetActivityTeamApplyList() {
	applyList = new java.util.ArrayList<Common.CrossTeamObj.CrossTeam_ApplyInfo>();
}

public GS2GC_012_011_RetActivityTeamApplyList(
	 java.util.ArrayList<Common.CrossTeamObj.CrossTeam_ApplyInfo> _applyList
) {	applyList = _applyList;
}

public final byte getMainOrder() { return (byte)12; }

public final byte getSubOrder() { return (byte)11; }

public java.util.ArrayList<Common.CrossTeamObj.CrossTeam_ApplyInfo> getApplyList() { return applyList; }
public void addApplyList(Common.CrossTeamObj.CrossTeam_ApplyInfo _applyList) { applyList.add(_applyList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (applyList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (applyList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _applyListCount = _buf.getShort();
	for(int _i = 0; _i < _applyListCount; _i++) { 
		Common.CrossTeamObj.CrossTeam_ApplyInfo _applyList = new Common.CrossTeamObj.CrossTeam_ApplyInfo();
		if(_buf.remaining() <= 0) return;
	int __applyListCustLen = _buf.getInt();
	int __applyListCurPos = _buf.position();
	_applyList.ReadUnzipBuf(_buf, __applyListCurPos + __applyListCustLen);
	_buf.position(__applyListCurPos + __applyListCustLen);

		applyList.add(_applyList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)applyList.size());
	for(int _i = 0; _i < applyList.size(); _i++) { 
		_buf.putInt(applyList.get(_i).GetBufSize());
	applyList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)12);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)12);
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

