using Godot;
using System;

public partial class basicenemy : CharacterBody3D
{
    public bool ActiveAI;
    public float DeathAnimationTimer;
    public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    public CharacterBody3D playerNode;
    public Sprite3D sprite;

    public CpuParticles3D bloodNode;
    public CpuParticles3D boomNode;
    public bool CanJump;
    public int hitpoints;
    public Random rng;
    public void EmitBlood() {
        if (ActiveAI == true)
        {
            bloodNode.Amount = 30;
            bloodNode.Emitting = true;
        }
    }
    public override void _Ready()
    {
        playerNode = GetNode<CharacterBody3D>("/root/Node3D/player");
        rng = new Random();
        hitpoints = 10;
        ActiveAI = true;
        DeathAnimationTimer = 0;
        bloodNode = GetNode<CpuParticles3D>("blood");
         boomNode = GetNode<CpuParticles3D>("boom");
        boomNode.Visible = false;
        boomNode.Emitting = false;
        sprite = GetNode<Sprite3D>("sprite");
    }

	public bool IsEnemy() {
        return true;
    }

	public void TakeDamage(int i) {
        hitpoints -= i;
    }
    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
        float dt = (float)delta;
        if (ActiveAI == true)
        {
            Vector3 targetVelocity = (
                (playerNode.Position - Position).Normalized()
            );
            if (Position.DistanceTo(playerNode.Position) < 30)
            {
                velocity += new Vector3(targetVelocity.X, 0, targetVelocity.Z).Normalized() * 100 * dt;
            }
            velocity.X = Math.Clamp(velocity.X, -10, 10);
            velocity.Z = Math.Clamp(velocity.Z, -10, 10);

            // horrible, horrbile code but i dont care cause this is a placeholder enemy
            if (Math.Abs(velocity.X) + Math.Abs(velocity.Z) < 4)
            {
                if (CanJump == true)
                {
                    velocity.Y = 25;
                    CanJump = false;
                }
            }

            // if (rng.Next(1,60*4) == 4) {
            //     velocity.X *= 5;
            //     velocity.Z *= 5;
            // }

            if (IsOnFloor())
            {
                CanJump = true;
            }
            if (!IsOnFloor())
            {
                if (CanJump == true)
                {
                    velocity.Y = 35;
                    CanJump = false;
                }
                else { velocity.Y -= 100 * (float)delta; }
            }

            Velocity = velocity;
            }
            MoveAndSlide();

            if (hitpoints < 1)
            {
            ActiveAI = false;
            
            // this.QueueFree();
            }

            if (ActiveAI == false) {
            // Visible = false;
            Velocity = Vector3.Zero + new Vector3(rng.Next(-5,6),0,rng.Next(-5,6));
            DeathAnimationTimer += dt;
            sprite.Visible = false;
            
            if (DeathAnimationTimer < 0.1f)
            {
                boomNode.Visible = true;
                boomNode.Emitting = true;
                bloodNode.Amount = 40;
                bloodNode.Emitting = true;
            }
            if (DeathAnimationTimer > 1f) {
                this.QueueFree();
            }
        }
        
    }
}
