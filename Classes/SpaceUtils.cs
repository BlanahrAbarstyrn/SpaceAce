using Godot;

namespace SpaceAce.Classes;
public static class SpaceUtils
{
    public static void ActivateArea2D(Area2D area, bool active)
    {
        area.SetDeferred(Area2D.PropertyName.Monitoring, active);
        area.SetDeferred(Area2D.PropertyName.Monitorable, active);
    }

    public static void SetAndStartTimer(Timer timer, float target, float variance)
    {
        timer.Start(target + GD.RandRange(-variance, variance));
    }
}