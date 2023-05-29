﻿namespace Movement.Components
{
    public interface IFighterReceiver : IRecevier
    {
        public void Attack1ServerRpc();
        public void Attack2ServerRpc();
        public void TakeHitServerRpc(int dmg);
        public void DieServerRpc();
        public float GetLife();
    }
}