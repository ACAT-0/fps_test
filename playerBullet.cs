using Godot;
using System;

public partial class playerBullet : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

    Area3D hurtbox;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	public override void _Ready() {
        hurtbox = GetNode<Area3D>("hurtbox");
    }
	public override void _PhysicsProcess(double delta)
	{
		if (GetSlideCollisionCount() > 0) {
            this.QueueFree();
        }

		foreach (var body in hurtbox.GetOverlappingBodies()) {
            if (body.HasMethod("IsEnemy"))
            {
                body.Call("TakeDamage", 1);
                body.Call("EmitBlood");
                this.QueueFree();
            }
		} 
		MoveAndSlide();
	}
}
