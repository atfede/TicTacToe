using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicTacToe.Models
{
    public class Game
    {
        public enum Piece { Empty = 0, X = 1, O = 2 };

        static int[,] winConditions = new int[8, 3]
        {
        { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 },
        { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 },
        { 0, 4, 8 }, { 2, 4, 6 }
        };

        public Piece[] Grid = new Piece[9];

        public Piece CurrentTurn = Piece.X;

        int Choice = 0;

        public Piece Computer;
        public Piece Player;

        public Game()
        {
            CurrentTurn = Piece.X;
            Player = Piece.X;
        }

        public void Reset()
        {
            CurrentTurn = Piece.X;
            SetPlayer(Piece.X);
            Grid = new Piece[9];
        }

        public void SetPlayer(Piece Player)
        {
            this.Player = Player;
            this.Computer = SwitchPiece(Player);
        }

        public void MakeMove(int Move)
        {
            if (CurrentTurn == Player)
            {
                Grid = MakeGridMove(Grid, CurrentTurn, Move);
                CurrentTurn = SwitchPiece(CurrentTurn);
            }
            else if (CurrentTurn == Computer)
            {
                Minimax(CloneGrid(Grid), CurrentTurn);
                Grid = MakeGridMove(Grid, CurrentTurn, Choice);
                CurrentTurn = SwitchPiece(CurrentTurn);
                Console.WriteLine(Choice.ToString());
            }
        }

        int Minimax(Piece[] InputGrid, Piece Player)
        {
            Piece[] Grid = CloneGrid(InputGrid);

            if (CheckScore(Grid, Player) != 0)
                return CheckScore(Grid, Player);
            else if (CheckGameEnd(Grid)) return 0;

            List<int> scores = new List<int>();
            List<int> moves = new List<int>();

            for (int i = 0; i < 9; i++)
            {
                if (Grid[i] == Piece.Empty)
                {
                    scores.Add(Minimax(MakeGridMove(Grid, Player, i), SwitchPiece(Player)));
                    moves.Add(i);
                }
            }

            if (Player == Computer)
            {
                int MaxScoreIndex = scores.IndexOf(scores.Max());
                Choice = moves[MaxScoreIndex];
                return scores.Max();
            }
            else
            {
                int MinScoreIndex = scores.IndexOf(scores.Min());
                Choice = moves[MinScoreIndex];
                return scores.Min();
            }
        }

        static int CheckScore(Piece[] Grid, Piece Player)
        {
            if (CheckGameWin(Grid, Player)) return 10;

            else if (CheckGameWin(Grid, SwitchPiece(Player))) return -10;

            else return 0;
        }

        static bool CheckGameWin(Piece[] Grid, Piece Player)
        {
            for (int i = 0; i < 8; i++)
            {
                if
                (
                    Grid[winConditions[i, 0]] == Player &&
                    Grid[winConditions[i, 1]] == Player &&
                    Grid[winConditions[i, 2]] == Player
                )
                {
                    return true;
                }
            }
            return false;
        }

        static bool CheckGameEnd(Piece[] Grid)
        {
            foreach (Piece p in Grid) if (p == Piece.Empty) return false;
            return true;
        }

        static Piece SwitchPiece(Piece Piece)
        {
            if (Piece == Piece.X) return Piece.O;
            else return Piece.X;
        }

        static Piece[] CloneGrid(Piece[] Grid)
        {
            Piece[] Clone = new Piece[9];
            for (int i = 0; i < 9; i++) Clone[i] = Grid[i];

            return Clone;
        }

        static Piece[] MakeGridMove(Piece[] Grid, Piece Move, int Position)
        {
            Piece[] newGrid = CloneGrid(Grid);
            newGrid[Position] = Move;
            return newGrid;
        }
    }
}