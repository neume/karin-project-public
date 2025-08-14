namespace Karin.Core;

public class Bidictionary<TKey1, TValue>
{
    private readonly Dictionary<TKey1, TValue> _map1;
    private readonly Dictionary<TValue, TKey1> _map2;

    public Bidictionary()
    {
        _map1 = new Dictionary<TKey1, TValue>();
        _map2 = new Dictionary<TValue, TKey1>();
    }

    public void Add(TKey1 key, TValue value)
    {
        _map1[key] = value;
        _map2[value] = key;
    }

    public bool TryGetValue(TKey1 key, out TValue value)
        => _map1.TryGetValue(key, out value);

    public bool TryGetKey(TValue value, out TKey1 key)
        => _map2.TryGetValue(value, out key);

    public bool ContainsKey(TKey1 key)
        => _map1.ContainsKey(key);

    public bool ContainsValue(TValue value)
        => _map2.ContainsKey(value);

    public bool Remove(TKey1 key)
        => _map1.Remove(key);
    
    public bool Remove(TValue value)
        => _map2.Remove(value);

    public int Count => _map1.Count;
}
