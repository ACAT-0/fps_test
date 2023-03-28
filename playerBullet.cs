using Godot;
using System;

public partial class playerBullet : CharacterBody3D
{
	public const float Speed = 5.0f;
    public string Type;
    public int damage;
    public const float JumpVelocity = 4.5f;
    public double time;
    Area3D hurtbox;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public void SetDamage(int dmg) {
        damage = dmg;
    }	
    public void SetType(string type) {
        Type = type;
    }	
    public override void _Ready() {
        hurtbox = GetNode<Area3D>("hurtbox");
        time = 0;
    }
	public override void _PhysicsProcess(double delta)
	{
        time += delta;
        if (time > 5) {
            QueueFree();
        }
        if (GetSlideCollisionCount() > 0) {
            this.QueueFree();
        }

        if (Type == "shell") {
            Vector3 velCopy = Velocity;
            velCopy *= 0.99f;
            Velocity = velCopy;
        }

		foreach (var body in hurtbox.GetOverlappingBodies()) {
            if (body.HasMethod("IsEnemy"))
            {
                body.Call("TakeDamage", damage);
                body.Call("EmitBlood");
                if (Type != "shell") {
                    this.QueueFree();
                }
            
            }
		} 
		MoveAndSlide();
	}
}
