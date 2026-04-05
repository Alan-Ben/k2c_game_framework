package ALLRPC.US.Guild;

import java.nio.ByteBuffer;
public class GuildMemberInitState_Return implements ALBasicProtocolPack._IALProtocolStructure {
private String guildName;
private String guildSimpleName;
private int guildLvl;
private java.util.ArrayList<Integer> buildingAddPerArr;


public GuildMemberInitState_Return() {
	guildName = "";
	guildSimpleName = "";
	guildLvl = 0;
	buildingAddPerArr = new java.util.ArrayList<Integer>();
}

public GuildMemberInitState_Return(
	 String _guildName
	, String _guildSimpleName
	, int _guildLvl
	, java.util.ArrayList<Integer> _buildingAddPerArr
) {	guildName = _guildName;
	guildSimpleName = _guildSimpleName;
	guildLvl = _guildLvl;
	buildingAddPerArr = _buildingAddPerArr;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public String getGuildName() { return guildName; }
public void setGuildName(String _guildName) { guildName = _guildName; }
public String getGuildSimpleName() { return guildSimpleName; }
public void setGuildSimpleName(String _guildSimpleName) { guildSimpleName = _guildSimpleName; }
public int getGuildLvl() { return guildLvl; }
public void setGuildLvl(int _guildLvl) { guildLvl = _guildLvl; }
public java.util.ArrayList<Integer> getBuildingAddPerArr() { return buildingAddPerArr; }
public void addBuildingAddPerArr(int _buildingAddPerArr) { buildingAddPerArr.add(_buildingAddPerArr); }


public final int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildSimpleName);
	_size += 2 + (buildingAddPerArr.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildSimpleName);
	_size += 2 + (buildingAddPerArr.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildSimpleName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildLvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _buildingAddPerArrCount = _buf.getShort();
	for(int _i = 0; _i < _buildingAddPerArrCount; _i++) { 
		int _buildingAddPerArr = 0;
		if(_buf.remaining() > 0) _buildingAddPerArr = _buf.getInt();
		buildingAddPerArr.add(_buildingAddPerArr);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, guildName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, guildSimpleName);
	_buf.putInt(guildLvl);
	_buf.putShort((short)buildingAddPerArr.size());
	for(int _i = 0; _i < buildingAddPerArr.size(); _i++) { 
		_buf.putInt(buildingAddPerArr.get(_i));
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

