package ALLRPC.Common;

import java.nio.ByteBuffer;
public class AllExecGmCommand_Req implements ALBasicProtocolPack._IALProtocolStructure {
private String command;
private java.util.ArrayList<String> exCommands;


public AllExecGmCommand_Req() {
	command = "";
	exCommands = new java.util.ArrayList<String>();
}

public AllExecGmCommand_Req(
	 String _command
	, java.util.ArrayList<String> _exCommands
) {	command = _command;
	exCommands = _exCommands;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public String getCommand() { return command; }
public void setCommand(String _command) { command = _command; }
public java.util.ArrayList<String> getExCommands() { return exCommands; }
public void addExCommands(String _exCommands) { exCommands.add(_exCommands); }


public final int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(command);
	_size += 2;
	for(int _i = 0; _i < exCommands.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(exCommands.get(_i));
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(command);
	_size += 2;
	for(int _i = 0; _i < exCommands.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(exCommands.get(_i));
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) command = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _exCommandsCount = _buf.getShort();
	for(int _i = 0; _i < _exCommandsCount; _i++) { 
		String _exCommands = "";
		if(_buf.remaining() > 0) _exCommands = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
		exCommands.add(_exCommands);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, command);
	_buf.putShort((short)exCommands.size());
	for(int _i = 0; _i < exCommands.size(); _i++) { 
		ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, exCommands.get(_i));
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

