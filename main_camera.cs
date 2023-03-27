using Godot;
using System;

public partial class main_camera : Camera3D
{
    // Called when the node enters the scene tree for the first time.
    float mouseSensitivity = 0.6f;
    public override void _Ready()
	{
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.

public static double ConvertDegreesToRadians (double degrees)
{
    double radians = (Math.PI / 180) * degrees;
    return (radians);
}
	public override void _Process(double delta)
	{
       if (Input.IsActionJustPressed("quit")) {
            if (Input.MouseMode == Input.MouseModeEnum.Captured) {
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
            else {
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
            

        }
		if (Input.IsActionPressed("ui_up")) {
            RotateZ(0.05f);
        }
		if (Input.IsActionPressed("ui_down")) {
            RotateZ(-0.05f);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion & Input.MouseMode == Input.MouseModeEnum.Captured) {
			InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
            RotateY((float)ConvertDegreesToRadians(-mouseEvent.Relative.X * mouseSensitivity));
        }
        }

    }

