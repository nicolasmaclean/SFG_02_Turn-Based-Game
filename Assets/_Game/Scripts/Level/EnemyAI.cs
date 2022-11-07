using System.Collections.Generic;
using System.Linq;
using Game.Play.Units;
using Game.Play.Skills;
using Gummi;
using Gummi.Utility;
using UnityEngine;

namespace Game.Level
{
    public static class EnemyAI
    {
        public static TurnData PlanTurn(Board board, Pawn pawn)
        {
            HashSet<Vector2Int> inRangeOfSelf = board.GetTilesInRange(pawn.Position, pawn.Movement);

            // get all tiles this unit AND a player unit could move to
            HashSet<Vector2Int> intersection = new HashSet<Vector2Int>();
            foreach (Pawn playerPawn in board.Pawns(Team.Player))
            {
                HashSet<Vector2Int> inRangeOfPlayer = board.GetTilesInRange(playerPawn.Position, playerPawn.Movement+1);
                inRangeOfPlayer.IntersectWith(inRangeOfSelf);
                
                intersection.UnionWith(inRangeOfPlayer);
            }
            
            // player units are too far
            // time to wreck shit up
            if (intersection.Count == 0)
            {
                // get buildings that we can attack
                HashSet<Vector2Int> nextToBuildings = board.GetTilesInRange(pawn.Position, pawn.Movement);
                nextToBuildings.RemoveWhere(position => !board.HasAdjacentBuilding(position));
                
                // there are no buildings :((
                Vector2Int randomMove;
                if (nextToBuildings.Count == 0)
                {
                    randomMove = inRangeOfSelf.PickRandom();
                    return new TurnData
                    {
                        OldPosition = pawn.Position,
                        NewPosition = randomMove,
                    };
                }
                
                // attack!
                randomMove = nextToBuildings.PickRandom();
                return new TurnData
                {
                    OldPosition = pawn.Position,
                    NewPosition = randomMove,
                    Action = pawn.GetSkill(SkillTag.Attack),
                    Target = board.GetAdjacentBuilding(randomMove).Value,
                };
            }
            
            // TODO: attack player??
            
            return new TurnData();
        }
    }

    public struct TurnData
    {
        public Vector2Int OldPosition;
        public Vector2Int NewPosition;
        public SkillSO Action;
        public Vector2Int Target;
    }
}