using Godot;
using System;
using fps_test;
public partial class playerBody : CharacterBody3D
{
    public PlayerWeapon weapon;
    public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
    public PackedScene bullet;
    public PackedScene benjamin;

    public AudioStreamPlayer gunSound;


    public PlayerWeapon[] WeaponTypes;
    public float SpeedBoost;
    public Node3D MainNode;
    public Camera3D Camera;

    public float camOffset;
    public Sprite2D GunOverlay;

    public float FireDelay;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	public override void _Ready() {
        SpeedBoost = 0;
        WeaponTypes = new PlayerWeapon[] {
            new PlayerWeapon(3,0.5f, "pistol"),
            new PlayerWeapon(1,0.1f, "machinegun"),
        };
        weapon = WeaponTypes[0];
        Camera = GetNode<Camera3D>("Camera3D");
        GunOverlay = GetNode<Sprite2D>("Camera3D/overlay/hand");
        bullet = ResourceLoader.Load<PackedScene>("res://playerBullet.tscn");
        benjamin = ResourceLoader.Load<PackedScene>("res://basicTestEnemy.tscn");
        gunSound = GetNode<AudioStreamPlayer>("GUNSOUND");
        MainNode = GetParent<Node3D>();
        FireDelay = 0;
    }

	public static double ConvertDegreesToRadians (double degrees)
{
    double radians = (Math.PI / 180) * degrees;
    return (radians);
}
	public override void _PhysicsProcess(double delta)
	{
        
        float dt = (float)delta;
        FireDelay += dt;
        SpeedBoost -= 1 * dt;
		if (SpeedBoost < 0)
            SpeedBoost = 0;

        Vector3 _dir = new Vector3();
        Vector3 velCopy = Velocity;
        Transform3D camXform = Camera.GlobalTransform;

        if (IsOnFloor())
        {
            velCopy.X *= 0.8f;
            velCopy.Z *= 0.8f;
        }
        else {
            velCopy.X *= 0.82f;
            velCopy.Z *= 0.82f;
        }

        Vector2 inputMovementVector = new Vector2();

        if (Input.IsActionPressed("movement_forward"))
            inputMovementVector.Y += 1;
        if (Input.IsActionPressed("movement_backward"))
            inputMovementVector.Y -= 1;
        if (Input.IsActionPressed("movement_left"))
            inputMovementVector.X -= 1;
        if (Input.IsActionPressed("movement_right"))
            inputMovementVector.X += 1;

        inputMovementVector = inputMovementVector.Normalized() * 3.5f;

        // Basis vectors are already normalized.
        _dir += -camXform.Basis.Z * inputMovementVector.Y;
        _dir += camXform.Basis.X * inputMovementVector.X;
        _dir.Y = 0;

        if (Position.Y < -200) {
            Position = Vector3.Zero;
        }

		if (!IsOnFloor()) {
            _dir.Y -= 50 * dt;
        }
		if (Input.IsActionJustPressed("jump") & (IsOnFloor())) {
            _dir.Y = 15;
            // SpeedBoost += 1;
            // _dir.X *= 10.5f + SpeedBoost;
			// _dir.Z *= 10.5f + SpeedBoost;
        }

        if (IsOnWallOnly() & (Math.Abs(_dir.Z) + Math.Abs(_dir.X)) > 4) {
            _dir.X /= 0.85f;
            _dir.Z /= 0.85f;
            _dir.Y = 3.5f;
            velCopy.Y = 0;
        }

		if (Input.IsActionJustPressed("dash")) {
            _dir.X /= 0.85f;
            _dir.Z /= 0.85f;
            _dir.X *= 30.5f;
			_dir.Z *= 30.5f;
            velCopy.Y = 0;
            _dir.Y = 0;
        }


        // FOR TESTING PURPOSES
        if (Input.IsActionJustPressed("ui_right")) {
            CharacterBody3D newBenjamin = benjamin.Instantiate<CharacterBody3D>();
            newBenjamin.Position = Position;
            MainNode.AddChild(newBenjamin);
        }


        // this code sucks so much ass
        // please god find out a better way to do this
        if (Input.IsActionJustPressed("weapon-1")) {
            weapon = WeaponTypes[0];
        }
        if (Input.IsActionJustPressed("weapon-2")) {
            weapon = WeaponTypes[1];
        }
        if (Input.IsActionJustPressed("weapon-3")) {
            weapon = WeaponTypes[2];
        }

        if (Input.IsMouseButtonPressed(MouseButton.Left) & FireDelay > weapon.MaxFireDelay) {
            if (weapon.Name == "pistol" | weapon.Name == "machinegun")
            {
                // gunSound.PitchScale = new Random().NextSingle() + 1f;
                // gunSound.Play();
                FireDelay = 0;
                CharacterBody3D newBullet = bullet.Instantiate<CharacterBody3D>();
                newBullet.Velocity = -camXform.Basis.Z * 205;
                newBullet.Call("SetDamage", weapon.Damage);
                newBullet.Position = Position + (newBullet.Velocity * dt) + new Vector3(1f, 0.5f, 0) + Velocity * dt;
                MainNode.AddChild(newBullet);
            }
        }

        if (Input.IsActionPressed("slide") & IsOnFloor()) {
            _dir.X /= 0.55f;
            _dir.Z /= 0.55f;
            camOffset = -1f;
        }
        else {
            camOffset = 0;
        }

        Velocity = velCopy + _dir;
        Camera.Position = new Vector3(0.1f,0.9f + (float)Math.Sin((Math.Abs(Position.X) + Math.Abs(Position.Z)) / 2) / 5 + camOffset,0.1f);
        // GunOverlay.Position = new Vector2(240, 155 + (float)Math.Sin((Math.Abs(Position.X) + Math.Abs(Position.Z))));
        GD.Print(Velocity.ToString());
        MoveAndSlide();
    }
}
