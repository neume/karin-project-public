namespace Karin.Defs;

public class EntityDefManager
{
    private readonly Dictionary<string, IEntityDef> _defs = new();

    public IEntityDef? GetDef(string name)
    {
        return _defs.ContainsKey(name) ? _defs[name] : null;
    }

    public void AddDef(string name, IEntityDef def)
    {
        _defs.Add(name, def);
    }

    public void AddDef<T>() where T : IEntityDef
    {
        var loadMethod = typeof(T).GetMethod("Load");
        if (loadMethod == null)
            throw new Exception($"No Load method found for {typeof(T).Name}");

        var def = loadMethod.Invoke(null, null) as IEntityDef;

        if (def == null)
            throw new Exception($"Load method returned null for {typeof(T).Name}");

        AddDef(def.DefName, def);
    }

    public void RemoveDef(string name)
    {
        _defs.Remove(name);
    }
}
