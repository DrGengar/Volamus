using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    class EndOfMatch
    {
        public void EndOfMatchWinner(Player winner)
        {
            //Flügel gehen nach oben
            winner.PositionLeftWing = new Vector3(winner.Position.X - 2, winner.Position.Y, winner.Position.Z - 1);
            winner.PositionRightWing = new Vector3(winner.Position.X + 2, winner.Position.Y, winner.Position.Z - 1);
            winner.HitAngleHigh = 20;

            //hüpft
            if (winner.Position.Z < 7 && !winner.IsFalling)
            {
                winner.PositionZ += winner.JumpVelocity;
            }
            else
            {
                if (winner.Position.Z > 0)
                {
                    winner.PositionZ -= winner.JumpVelocity;
                    winner.IsFalling = true;
                }
                else
                {
                    winner.IsFalling = false;
                }
            }
        }


        public void EndOfMatchLoser(Player Loser)
        {
            Loser.PositionLeftWing = new Vector3(Loser.Position.X + 5, Loser.Position.Y, Loser.Position.Z + 2);
            Loser.PositionRightWing = new Vector3(Loser.Position.X - 5, Loser.Position.Y, Loser.Position.Z + 2);
            Loser.HitAngleHigh = -40;
        }
    }
}
