# GitHub Copilot Instructions

This is a Java-based distributed game server system with extensive code generation and microservice architecture.

## Architecture Overview

This is a **microservice game server architecture** with the following core servers:
- **GatewayServer**: Client connection gateway and load balancing
- **UserServer**: Core game logic and player data management
- **CommonServer**: Shared services and inter-server communication
- **BusServer**: Inter-server message bus
- **LoginServer**: Authentication and account management
- **CrossGameServer/CrossRankServer**: Cross-server functionality

Each server follows the pattern: `[ServerName]/src/NP[ServerName]/` with separate configuration in `conf/` and `customConf/` (environment overrides).

## Critical Development Workflows

### Build System
```bash
# Full build sequence (ALWAYS follow this order)
cd bat && python build_enum.py && python build_err.py && python build_rpc.py
cd ../DBTool && python genAll.py  
cd ../build && build2.bat
```

**Build order matters**: Enums → Error codes → Protocols → Database → Ant build. The Ant build (`build2.xml`) creates JARs in `/Lib/` directory following dependency order: ServerProtocol → Common → GameRes → GameLogicCommon → individual servers.

### Code Generation (Core to this project)
- **Protocols**: `.alpro` files in `ServerProtocol/ProtocolScripts/ALLRPC/` → `python build_rpc.py` → Java classes
- **Error codes**: `bat/err/[MainCode]_[ClassName]_[Desc].txt` → `python build_err.py` → cross-language error enums
- **Database**: Python table definitions in `DBTool/source_db/` → `python genAll.py` → BO/BM classes
- **Post-build**: `ServerProtocol/ProtocolScripts/post_exec.bat` runs full generation pipeline

## Project-Specific Patterns

### Message Processing (UserServer Core Pattern)
**5-layer architecture** for player message handling:

1. **Auto-registration**: `NPUserMsgBasicDispatcher.autoRegistHandler()` scans for `NPUserMsgDealer<T>` subclasses
2. **Message items**: `NPUSUserRequestMsgItem` (with client sequence) vs `NPUSUserNormalMsgItem` (push)
3. **Dealers**: Named `MsgDealer_[Direction]_[MainProtocol]_[SubProtocol]_[Feature]` in `NPUserMsgDispather/p[XXX]_[Feature]/`
4. **Components**: Business logic in `[Feature]Component extends _ANPUserComponent`
5. **Writers**: Manual response builders `US2GCWriter_[Protocol]_[Feature]` in `Write/` directory

### Protocol Convention
- **Network format**: `[Direction]_[MainProtocol]_[SubProtocol]_[Feature]` (e.g., `GC2GS_036_001_ReqTreasureHuntOreCapture`)
- **Wire protocol**: `[MainProtocol(1byte)] + [SubProtocol(1byte)] + [Data(variable)]`
- **Package organization**: `p[XXX]_[FeatureName]Op` directories group related protocols

### Component System
- **Lifecycle**: Constructor auto-registers → `_init()` async → `getDependCompList()` → `onInited()` callback
- **Threading**: Player-level locks via `getUserData().lockUser()` for data consistency
- **Persistence**: BM (Business Manager) pattern with `[Feature]BO` data objects and `[Feature]BM.inst` managers

### Configuration System
- **Dual-layer**: Base config in `conf/[ServerName]Conf.properties`, overrides in `customConf/`
- **Pattern**: Singleton `[ServerName]Conf.getInstance().init()` loads both layers
- **Hot-reload**: Many configs support runtime updates

## Integration Points

### Database Integration
- **BM Pattern**: `[Feature]BM.inst.insertOrUpdate[Feature]([Feature]BO, context)`
- **Routing**: `EDBTag` enum routes to different databases
- **Transactions**: Context-based transaction management through BM layer
- **Backup**: Automatic `.backup` files with timestamps

### Cross-Server Communication
- **BusServer**: Central message bus for inter-server communication
- **Protocol sharing**: Same `.alpro` definitions generate client and server-side code
- **Service discovery**: Configuration-based server addressing

### Hotfix System
- **ActivitiesV01**: Dedicated hotfix module for game content updates
- **Deployment**: `cp_hotifx.bat` copies hotfix classes to deployment locations
- **Integration**: Hotfix classes can override base functionality

## Key Files & Directories

- `bat/build_*.py`: Code generation scripts (run these before Ant build)
- `build/build2.xml`: Ant build configuration with dependency order
- `ServerProtocol/ProtocolScripts/ALLRPC/`: Protocol definitions (`.alpro` files)
- `[Server]/src/NP[Server]/`: Main server implementation
- `UserServer/src/NPUSServer/NPUserMsgDispather/`: Message handling logic
- `Common/src/`: Shared protocol and utility classes
- `DBTool/source_db/`: Database table definitions

## Anti-Patterns to Avoid

- **Never** edit generated files (they're overwritten on next build)
- **Never** skip the enum→error→protocol→database→build sequence
- **Never** modify files in `Common/src/` manually (they're generated from protocols)
- **Don't** create message dealers without corresponding Writers in `Write/` directory
- **Don't** forget component dependency declarations in `getDependCompList()`

## Quick Reference

**Add new player feature**: Define `.alpro` → Run `build_rpc.py` → Create `MsgDealer_*` → Create `Writer_*` → Implement `*Component` → Build
**Add database table**: Create Python definition in `DBTool/source_db/[Server]/` → Run `genAll.py` → Use generated `*BO/*BM` classes
**Debug protocol issues**: Check `Common/src/` for generated classes, verify `.alpro` syntax, ensure `build_rpc.py` ran successfully
