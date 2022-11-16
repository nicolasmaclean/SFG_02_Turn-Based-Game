using System.Collections.Generic;
using Game.Level;
using Game.Play.Units;
using Gummi;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace Game.Play.Skills
{
    [CreateAssetMenu(menuName = "Skills/Punch")]
    public class Punch : SkillSO
    {
        [SerializeField]
        int _damage = 1;
        
        public override void Activate(Pawn user, Board board, Vector2Int target)
        {
            base.Activate(user, board, target);
            board.Hit(target, user.Position, _damage);
        }

        public override Vector2Int Evaluate(Board board, Pawn user, Vector2Int pos, out Effective effectiveness)
        {
            // prioritize kills
            List<Vector2Int> pawns = board.GetAdjacentPawns(pos, Team.Player);
            if (pawns.Count > 0)
            {
                foreach (Vector2Int target in pawns)
                {
                    int targetHealth = board.Spaces[target.x, target.y].Pawn.Health;
                    if (targetHealth <= _damage)
                    {
                        effectiveness = Effective.Kill;
                        return target;
                    }
                }
            }
            
            List<Vector2Int> buildings = board.GetAdjacentBuildings(pos);
            bool targetPawn = Random.Range(0, 2) == 1;

            // attack pawn
            if (pawns.Count > 0 && (targetPawn || buildings.Count == 0))
            {
                effectiveness = Effective.DamageUnit;
                return pawns.PickRandom();
            }
            
            // attack building
            if (buildings.Count > 0)
            {
                effectiveness = Effective.DamageBuilding;
                return buildings.PickRandom();
            }
            
            // no-op
            effectiveness = Effective.None;
            return Vector2Int.zero;
        }
    }
}