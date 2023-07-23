using System.Threading.Tasks.Dataflow;

namespace DaemonTechChallenge.ETL;

public class Pipeline
{
    private readonly DataflowLinkOptions _options;
    private IDataflowBlock? _initialStage;
    private IDataflowBlock? _lastStage;

    public Pipeline(IDataflowBlock? block, DataflowLinkOptions? options)
    {
        _initialStage = block;
        _options = options ?? new DataflowLinkOptions();
        _lastStage = _initialStage;
    }

    public void AddStageToPipeline<T>(ITargetBlock<T> block)
    {
        if (_lastStage != null)
        {
            var targetBlock = _lastStage as ISourceBlock<T>;
            targetBlock?.LinkTo(block, _options);
        }
        else
        {
            _initialStage = block;
        }

        _lastStage = block;
    }

    public async Task Startup<T>(T data) {
        var firstStage = _initialStage as ITargetBlock<T>;

        if (firstStage != null && _lastStage != null)
        {
            firstStage.Post(data);

            firstStage.Complete();

            await _lastStage.Completion;
        }
    }
}
