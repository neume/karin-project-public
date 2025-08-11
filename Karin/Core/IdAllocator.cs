namespace Karin.Core;

public class IdAllocator
{
    private int _nextId = 0;

    public int Allocate()
    {
        return System.Threading.Interlocked.Increment(ref _nextId);
    }
}
