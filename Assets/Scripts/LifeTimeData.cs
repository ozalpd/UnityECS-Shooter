using Unity.Entities;

[GenerateAuthoringComponent]
public struct LifeTimeData : IComponentData
{
    public float timeLeft;
    public bool timeIsUp { get { return timeLeft <= 0f; } }
}
