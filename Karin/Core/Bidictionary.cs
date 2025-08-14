namespace Karin.Core;

public class Bidictionary<TKey1, TValue>
    where TKey1 : notnull
    where TValue : notnull
{
    private readonly Dictionary<TKey1, TValue> _forward;
    private readonly Dictionary<TValue, TKey1> _reverse;

    public Bidictionary()
    {
        _forward = new Dictionary<TKey1, TValue>();
        _reverse = new Dictionary<TValue, TKey1>();
    }

    public int Count => _forward.Count;

    public void Set(TKey1 key, TValue value)
    {
        if (_forward.TryGetValue(key, out var oldValue))
        {
            if (!EqualityComparer<TValue>.Default.Equals(oldValue, value))
            {
                _reverse.Remove(oldValue);
                _forward[key] = value;
                _reverse[value] = key;
            }

            return;
        }

        if (_reverse.TryGetValue(value, out var oldKey))
        {
            _forward.Remove(oldKey);
        }

        _forward[key] = value;
        _reverse[value] = key;
    }

    public bool RemoveByKey(TKey1 key)
    {
        if (!_forward.TryGetValue(key, out var value))
            return false;

        _forward.Remove(key);
        _reverse.Remove(value);

        return true;
    }

    public bool RemoveByValue(TValue value)
    {
        if (!_reverse.TryGetValue(value, out var key))
            return false;

        _reverse.Remove(value);
        _forward.Remove(key);
        return true;
    }

    public bool TryGetValue(TKey1 key, out TValue? value)
        => _forward.TryGetValue(key, out value);

    public bool TryGetKey(TValue value, out TKey1? key)
        => _reverse.TryGetValue(value, out key);

    public bool ContainsKey(TKey1 key)
        => _forward.ContainsKey(key);

    public bool ContainsValue(TValue value)
        => _reverse.ContainsKey(value);

    public bool Remove(TKey1 key)
        => _forward.Remove(key);
    
    public bool Remove(TValue value)
        => _reverse.Remove(value);

}
