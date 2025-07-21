using DefaultEcs.System;

public class SystemsManager
{
    public Dictionary<ISystem<float>, int> Systems;
    public List<ISystem<float>> SortedSystems { get; private set; }
    public int NextSortValue = 0;

    public SystemsManager()
    {
        Systems = new Dictionary<ISystem<float>, int>();
        SortedSystems = new List<ISystem<float>>();
        NextSortValue = 0;
    }


    public void Add(ISystem<float> system, int? sortValue = null)
    {
        var assignedSortValue = sortValue ?? NextSortValue;

        if (NextSortValue < assignedSortValue)
        {
            NextSortValue = assignedSortValue;
        }

        Systems.Add(system, assignedSortValue);
        NextSortValue++;

        SortSystems();
    }

    public void Remove(ISystem<float> system)
    {
        SortedSystems.Remove(system);
        Systems.Remove(system);

        NextSortValue = MaxSortValue() + 1;
    }

    public void Update(float state)
    {
        foreach (var system in SortedSystems)
            system.Update(state);
    }

    private void SortSystems()
    {
        SortedSystems.Clear();

        foreach (var system in Systems)
            SortedSystems.Add(system.Key);

        SortedSystems.Sort((a, b) => Systems[a].CompareTo(Systems[b]));
    }

    private int MaxSortValue()
        => Systems.Max(system => system.Value);
}
