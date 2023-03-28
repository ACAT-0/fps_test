using Godot;
using System;

namespace fps_test {
    public class PlayerWeapon {

        public int Damage;

        public string Name;
        public float MaxFireDelay;
        public PlayerWeapon(int Setdamage, float maxfiredelay, string name) {
            Damage = Setdamage;
            MaxFireDelay = maxfiredelay;
            Name = name;
        }
    }
}