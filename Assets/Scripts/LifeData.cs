using Unity.Entities;

[GenerateAuthoringComponent]
public struct LifeData : IComponentData
{
    public bool isAlive;
}
