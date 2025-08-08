using ECS = DefaultEcs;

namespace Karin.Defs;

public interface IEntityDef
{
    public string DefName { get; }
    public ECS.Entity Create(ECS.World world);
}
