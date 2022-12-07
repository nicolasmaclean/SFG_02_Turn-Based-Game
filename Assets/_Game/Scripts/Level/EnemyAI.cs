// AI idea from https://www.reddit.com/r/gamedev/comments/99cmnh/comment/e4mzgmv/?utm_source=share&utm_medium=web2x&context=3

using System.Collections.Generic;
using Game.Play.Units;
using Game.Play.Skills;
using Gummi;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace Game.Level
{
    public static class EnemyAI
    {
        public static TurnData PlanTurn(Board board, Pawn pawn)
        {
            // attempt a kill
            // do the most damage
            // move towards player
            HashSet<Vector2Int> inRangeOfSelf = board.GetTilesInRange(pawn.Position, pawn.Movement);

            // get all tiles this unit AND a player unit could move to
            HashSet<Vector2Int> intersection = new HashSet<Vector2Int>();
            foreach (Pawn playerPawn in board.Pawns(Team.Player))
            {
                HashSet<Vector2Int> inRangeOfPlayer = board.GetTilesInRange(playerPawn.Position, playerPawn.Movement+1);
                inRangeOfPlayer.IntersectWith(inRangeOfSelf);
                
                intersection.UnionWith(inRangeOfPlayer);
            }

            // default possible moves to all
            if (intersection.Count == 0)
            {
                intersection = inRangeOfSelf;
            }
            
            Effective effective = Effective.None;
            TurnData turn = new TurnData
            {
                Move = pawn.Position,
                Skill = null,
            };
            
            // try all of the skills
            foreach (var skill in pawn.GetSkills(SkillTag.Attack))
            {
                // try all the moves
                foreach (Vector2Int position in intersection)
                {
                    Vector2Int target = skill.Evaluate(board, pawn, position, out Effective eff);
                    
                    // ignore, this move is not as effective
                    if (!eff.Compare(effective)) continue;

                    effective = eff;
                    turn.Skill = skill;
                    turn.Move = position;
                    turn.Target = target - position;
                }
            }

            // don't use skill if it doesn't help
            if (effective == Effective.None)
            {
                turn.Skill = null;
                
                // make a random move
                turn.Move = inRangeOfSelf.PickRandom();
            }

            Debug.Log($"Planned Turn: { effective }\n" +
                             $"{ pawn.Position } -> { turn.Move }\n" +
                             $"use { turn.Skill } on { turn.Target }"
            );
            return turn;
        }
    }

    public struct TurnData
    {
        public Vector2Int Move;
        public SkillSO Skill;
        public Vector2Int Target;
    }
}