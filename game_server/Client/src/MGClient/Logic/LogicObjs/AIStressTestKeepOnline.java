package MGClient.Logic.LogicObjs;

import MGClient.Logic.ClinetStateBase;
import MGClient.Logic.LogicBase;
import MGClient.Logic.LogicObjs.ModelProcessors.MP_PlayerLevelup;
import MGClient.Logic.LogicObjs.ModelProcessors.ModelProcesorBase;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate.HandlerNone;
import NPCommon.Util.Delegate._IHandlerHolder;

import java.util.ArrayList;
import java.util.List;


public class AIStressTestKeepOnline extends LogicBase implements _IHandlerHolder
{

    private List<ModelProcesorBase> _m_alModelProcessor = new ArrayList<>();

    public AIStressTestKeepOnline()
    {
        _m_alModelProcessor.add(new MP_PlayerLevelup());

    }


    private class ConsiderState extends ClinetStateBase //思考决定做什么
    {
        private int _m_waitTime = 3;

        public ConsiderState(LogicBase _logic)
        {
            super(_logic);
        }

        @Override
        public void processState()
        {
            if (!getOwner().getIsOnline())
            {
                return;
            }
            if (getTickCount() == _m_waitTime)
            {
                if (_m_alModelProcessor.isEmpty())
                {
                    getLogic().getOwner().logout();
                } else
                {
                    ModelProcesorBase processor = _m_alModelProcessor.remove(0);
                    changeState(new DealingState(processor, getLogic()));
                }
            }
        }
    }

    private class DealingState extends ClinetStateBase //模块处理
    {
        ModelProcesorBase _m_processor;

        public DealingState(ModelProcesorBase _processor, LogicBase _logic)
        {
            super(_logic);
            _m_processor = _processor;
            CommLog.info("start dealing:{}", _m_processor.getClass().getSimpleName());
        }

        @Override
        public void processState()
        {
            try
            {
                _m_processor.process(getLogic().getOwner(), getTickCount());
            } catch (Exception e)
            {
                CommLog.error("processor:{} process error", _m_processor.getClass().getName(), e);
                changeState(new ConsiderState(getLogic()));
                return;
            }

            if (_m_processor.isDone())
            {
                changeState(new ConsiderState(getLogic()));
            }
        }

        @Override
        public void onEnterState()
        {
            super.onEnterState();
            try
            {
                _m_processor.onStart(getLogic().getOwner());
            } catch (Exception e)
            {
                CommLog.error("processer:{} start error", _m_processor.getClass().getName(), e);
            }

        }
    }

    @Override
    protected void _initLogic()
    {
        LogicBase self = this;
        getOwner().OnLoginGame.addHandler(this, new HandlerNone()
        {
            @Override
            public void handle()
            {
                changeState(new ConsiderState(self));
            }
        });


    }

    @Override
    protected void _clear()
    {
        getOwner().OnLoginGame.clear(this);
    }

}
