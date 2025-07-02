namespace QuokkaServiceRegistry.Models;

public class ServiceSubscription
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SeatsUsed { get; set; }
    public int SeatsAvailable { get; set; }
    public double CostPerSeatUsd { get; set; }
    public TimeSpan TermDuration { get; set; }
}