namespace Reversi_Playlogic
{
    using Play;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Playlogic : IPlay
    {
        private List<List<int>> _backupReversiBoard = new List<List<int>>();

        public string PlayerNickName => "밍";

        public int Player { get; set; }

        public void Move(out int row, out int col, List<List<int>> reversiBoard)
        {
            // 랜덤으로 칸을 선택한다.
            //Random rnd = new Random();
            //col = rnd.Next(0, reversiBoard.Count);
            //row = rnd.Next(0, reversiBoard[0].Count);

            var bestMove = this.GetBestMove(reversiBoard);
            row = bestMove.row;
            col = bestMove.col;
        }

        private void BackupReversiBoard(List<List<int>> reversiBoard)
        {
            _backupReversiBoard = new List<List<int>>();

            for (int x = 0; x < reversiBoard[0].Count; x++)
            {
                _backupReversiBoard.Add(new List<int>());
                for (int y = 0; y < reversiBoard.Count; y++)
                {
                    int tmp = reversiBoard[x][y];
                    _backupReversiBoard[x].Add(tmp);
                }
            }
        }


        private (int row, int col) GetBestMove(List<List<int>> reversiBoard)
        {
            this.BackupReversiBoard(reversiBoard);

            int scores = 0;
            Dictionary<Tuple<int, int>, int> bestXY = new Dictionary<Tuple<int, int>, int>();

            for (int x = 0; x < _backupReversiBoard[0].Count; x++)
            {
                for (int y = 0; y < _backupReversiBoard.Count; y++)
                {
                    List<bool> movedList = new List<bool>();
                    movedList.Add(this.FlipLeft(x - 1, y, Player, 1, _backupReversiBoard));
                    movedList.Add(this.FlipRight(x + 1, y, Player, 1, _backupReversiBoard));
                    movedList.Add(this.FlipUp(x, y - 1, Player, 1, _backupReversiBoard));
                    movedList.Add(this.FlipDown(x, y + 1, Player, 1, _backupReversiBoard));
                    movedList.Add(this.FlipLeftDown(x - 1, y + 1, Player, 1, _backupReversiBoard));
                    movedList.Add(this.FlipRightDown(x + 1, y + 1, Player, 1, _backupReversiBoard));
                    movedList.Add(this.FlipLeftUp(x - 1, y - 1, Player, 1, _backupReversiBoard));
                    movedList.Add(this.FlipRightUp(x + 1, y - 1, Player, 1, _backupReversiBoard));

                    if (movedList.Any(p => p))
                    {
                        // 이기고 있는 상황인지 체크 및 스코어 정보
                        var checkScores = this.CheckScores(_backupReversiBoard);
                        if (checkScores.isWin)
                        {
                            bestXY.Add(new Tuple<int, int>(x, y), checkScores.score);
                        }
                        else
                        {
                            // 이기고 있는 가상 케이스가 없는 경우 -1스코어 점수로 하고 돌을 놓을 수 있는 위치 아무곳에 돌을 놓는다.
                            bestXY.Add(new Tuple<int, int>(x, y), -1);
                        }

                        // 백업본 다시 원본으로 교체
                        // **다시 가상으로 칸 선택 후 스코어 체크 반복**
                        this.BackupReversiBoard(reversiBoard);
                    }
                }
            }

            int bestX = -1;
            int bestY = -1;
            // bestXY정보 정렬
            foreach (var xy in bestXY.OrderByDescending(p => p.Value).Select(p => p.Key))
            {
                if (this.ValidMove(xy.Item1, xy.Item2, reversiBoard))
                {
                    bestX = xy.Item1;
                    bestY = xy.Item2;
                    break;
                }
            }

            Console.WriteLine("bestRow : " + bestX.ToString());
            Console.WriteLine("bestCol : " + bestY.ToString());
            return (bestX, bestY);
        }

        private bool ValidMove(int row, int col, List<List<int>> reversiBoard)
        {
            int valid_turn = (Player == 1) ? 2 : 1;

            Console.WriteLine("row : " + row.ToString());
            Console.WriteLine("col : " + col.ToString());
            Console.WriteLine("reversiBoard.Count : " + reversiBoard.Count.ToString());
            Console.WriteLine("valid_turn : " + valid_turn.ToString());

            if (reversiBoard[col][row] != 0)
                return false;

            if (row + 1 < reversiBoard[0].Count && reversiBoard[col][row + 1] == valid_turn)
            {
                return true;
            }
            else if (row > 0 && reversiBoard[col][row - 1] == valid_turn)
            {
                return true;
            }
            else if (col > 0 && reversiBoard[col - 1][row] == valid_turn)
            {
                return true;
            }
            else if (col + 1 < reversiBoard.Count && reversiBoard[col + 1][row] == valid_turn)
            {
                return true;
            }
            else if (col > 0 && row > 0 && reversiBoard[col - 1][row - 1] == valid_turn)
            {
                return true;
            }
            else if (col > 0 && row + 1 < reversiBoard.Count && reversiBoard[col - 1][row + 1] == valid_turn)
            {
                return true;
            }
            else if (col + 1 < reversiBoard.Count && row + 1 < reversiBoard[0].Count && reversiBoard[col + 1][row + 1] == valid_turn)
            {
                return true;
            }
            else if (col + 1 < reversiBoard.Count && row > 0 && reversiBoard[col + 1][row - 1] == valid_turn)
            {
                return true;
            }

            return false;
        }

        private bool FlipRightUp(int row, int col, int state, int distance, List<List<int>> reversiBoard)
        {
            if (col < 0 || row >= reversiBoard.Count || reversiBoard[col][row] == 0)
                return false;

            if (reversiBoard[col][row] == state)
                return (distance > 1);

            if (this.FlipRightUp(row + 1, col - 1, state, distance + 1, reversiBoard))
            {
                reversiBoard[col][row] = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FlipRightDown(int row, int col, int state, int distance, List<List<int>> reversiBoard)
        {
            if (col >= reversiBoard.Count || row >= reversiBoard.Count || reversiBoard[col][row] == 0)
                return false;

            if (reversiBoard[col][row] == state)
                return (distance > 1);

            if (this.FlipRightDown(row + 1, col + 1, state, distance + 1, reversiBoard))
            {
                reversiBoard[col][row] = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FlipLeftUp(int row, int col, int state, int distance, List<List<int>> reversiBoard)
        {
            if (col < 0 || row < 0 || reversiBoard[col][row] == 0)
                return false;

            if (reversiBoard[col][row] == state)
                return (distance > 1);

            if (this.FlipLeftUp(row - 1, col - 1, state, distance + 1, reversiBoard))
            {
                reversiBoard[col][row] = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FlipLeftDown(int row, int col, int state, int distance, List<List<int>> reversiBoard)
        {
            if (col >= reversiBoard.Count || row < 0 || reversiBoard[col][row] == 0)
                return false;

            if (reversiBoard[col][row] == state)
                return (distance > 1);

            if (this.FlipLeftDown(row - 1, col + 1, state, distance + 1, reversiBoard))
            {
                reversiBoard[col][row] = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FlipDown(int row, int col, int state, int distance, List<List<int>> reversiBoard)
        {
            if (col >= reversiBoard.Count || reversiBoard[col][row] == 0)
                return false;

            if (reversiBoard[col][row] == state)
                return (distance > 1);

            if (this.FlipDown(row, col + 1, state, distance + 1, reversiBoard))
            {
                reversiBoard[col][row] = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FlipUp(int row, int col, int state, int distance, List<List<int>> reversiBoard)
        {
            if (col < 0 || reversiBoard[col][row] == 0)
                return false;

            if (reversiBoard[col][row] == state)
                return (distance > 1);

            if (this.FlipUp(row, col - 1, state, distance + 1, reversiBoard))
            {
                reversiBoard[col][row] = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FlipRight(int row, int col, int state, int distance, List<List<int>> reversiBoard)
        {
            if (row >= reversiBoard.Count || reversiBoard[col][row] == 0)
                return false;

            Console.WriteLine(col);
            Console.WriteLine(reversiBoard[col][row]);

            if (reversiBoard[col][row] == state)
                return (distance > 1);

            if (this.FlipRight(row + 1, col, state, distance + 1, reversiBoard))
            {
                reversiBoard[col][row] = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FlipLeft(int row, int col, int state, int distance, List<List<int>> reversiBoard)
        {
            if (row < 0 || reversiBoard[col][row] == 0)
                return false;

            if (reversiBoard[col][row] == state)
                return (distance > 1);

            if (this.FlipLeft(row - 1, col, state, distance + 1, reversiBoard))
            {
                reversiBoard[col][row] = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 내가 이기고 있는 상황인지 여부
        /// </summary>
        /// <param name="reversiBoard"></param>
        /// <returns></returns>
        private (bool isWin, int score) CheckScores(List<List<int>> reversiBoard)
        {
            int row_len = reversiBoard[0].Count;
            int col_len = reversiBoard.Count;
            int blackScore = 0;
            int whiteScore = 0;

            for (int x = 0; x < row_len; x++)
            {
                for (int y = 0; y < col_len; y++)
                {
                    if (reversiBoard[x][y] == 1)
                        blackScore += 1;
                    if (reversiBoard[x][y] == 2)
                        whiteScore += 1;
                }
            }

            if (Player == 1)
            {
                return ((blackScore > whiteScore), blackScore);
            }
            else
            {
                return ((whiteScore > blackScore), whiteScore);
            }
        }
    }
}
