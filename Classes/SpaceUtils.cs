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
    
    public static bool IsJoyVibrating(int device)
    {
        GodotObject inputObj = Engine.GetSingleton("Input");
        // Get the remaining duration of the vibration
        float duration = (float)inputObj.Call("get_joy_vibration_duration", device);
        // If duration is greater than 0, it is currently vibrating
        return  duration > 0;
    }
}