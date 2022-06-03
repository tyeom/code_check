using Play;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hareversi
{
    public class Ha : IPlay
    {
        public string PlayerNickName => "Ha";

        public int Player { get; set; }

        // 변 방향 : 0, 2, 4, 6
        // 모서리 방향 : 1, 3, 5, 7
        int[] dx = { 0, 1, 1, 1, 0, -1, -1, -1 };
        int[] dy = { -1, -1, 0, 1, 1, 1, 0, -1 };
        // score -1 일 때 턴을 넘긴 횟수
        int PassCount = 0;

        public void Move(out int row, out int col, List<List<int>> reversiBoard)
        {
            var max = -1;
            ColRow cr = new ColRow(-1, -1);

            var length = reversiBoard.Count;

            // 특수 조건 체크
            if (Condition != null)
            {
                var item = Condition(reversiBoard);
                if (item.Col != -1)
                {
                    row = item.Row;
                    col = item.Col;
                    Condition = null;

                    return;
                }

                Condition = null;
            }

            // 둘 수 있는 곳을 읽어들인다.
            foreach (var item in ReadNextMove(reversiBoard, Player))
            {
                var score = 0;
                var copyBoard = ChangeBoard(item.Row, item.Col, reversiBoard, Player);
                if (!copyBoard.Any(i => i.Any(j => j == 0)))
                {
                    cr = item;
                    break;
                }

                // 0. 무조건 두는 경우
                bool CheckPerfect(int xs, int xe, int ys, int ye)
                {
                    if (xs == xe && ys == ye) return false; // 모서리에 자체에 대해선 체크할 필요 없음
                    int check = Player;
                    for (int i = xs; i <= xe; i++)
                    {
                        for (int j = ys; j <= ye; j++)
                        {
                            check &= copyBoard[i][j];
                        }
                    }
                    return check == Player;
                }

                if (CheckPerfect(0, item.Col, 0, item.Row) ||
                    CheckPerfect(0, item.Col, item.Row, length - 1) ||
                    CheckPerfect(item.Col, length - 1, 0, item.Row) ||
                    CheckPerfect(item.Col, length - 1, item.Row, length - 1))
                {
                    score += 100;
                }

                // 1. 모서리에 둘 수 있을 경우
                if (IsVertex(length, item))
                {
                    //  a. 모서리를 기준으로 각 변에 (모서리 - (...중간에 빈칸 하나...)내돌 - 반대편 모서리는 빈칸) 이 될 경우 두지 말것
                    score += 10;
                    var colV = item.Col;
                    var rowV = item.Row;
                    if (copyBoard[colV ^ (length - 1)][rowV] == 0 && copyBoard[colV ^ (length - 2)][rowV] == Player)
                    {
                        int count = 0;
                        int temp = 0;
                        for (int i = (colV ^ 1); i < (colV ^ (length - 2)); i++)
                        {
                            if (copyBoard[i][rowV] == 0)
                            {
                                temp = i;
                                count++;
                            }
                        }
                        if (count == 1)
                        {
                            score -= 11;
                        }
                    }
                    else if (copyBoard[colV][rowV ^ (length - 1)] == 0 && copyBoard[colV][rowV ^ (length - 2)] == Player)
                    {
                        int count = 0;
                        int temp = 0;
                        for (int i = (rowV ^ 1); i < (rowV ^ (length - 2)); i++)
                        {
                            if (copyBoard[colV][i] == 0)
                            {
                                temp = i;
                                count++;
                            }
                        }
                        if (count == 1)
                        {
                            score -= 11;
                        }
                    }
                }

                // 2. 사점에 두게 되는 경우
                //  a. 내돌이 이미 모서리를 차지한 경우 + 10
                //  b. 상대가 모서리알고리즘이 있다고 가정하고, 모서리를 먹을 때 1-a 형태가 되게끔 되는 경우 사점에 두기 (이 경우, 다음 빈칸에 내가 두어야함)
                if (IsDeadPoint(length, item))
                {
                    score -= 1;

                    var bitLen = length;
                    var bitCnt = 0;
                    while ((bitLen /= 2) != 1) bitCnt++;

                    int colV = (item.Col >> bitCnt) * (length - 1);
                    int rowV = (item.Row >> bitCnt) * (length - 1);
                    if (copyBoard[colV][rowV] == Player)
                    {
                        score += 10;
                    }
                    else if (copyBoard[colV][rowV] == 0)
                    {
                        if (copyBoard[colV ^ (length - 1)][rowV] == 0 && copyBoard[colV ^ (length - 2)][rowV] == Player)
                        {
                            int count = 0;
                            int temp = 0;
                            for (int i = (colV ^ 1); i < (colV ^ (length - 2)); i++)
                            {
                                if (copyBoard[i][rowV] == 0)
                                {
                                    temp = i;
                                    count++;
                                }
                            }
                            if (count == 1)
                            {
                                var copyBoard2 = ChangeBoard(colV, rowV, copyBoard, Player ^ 3);
                                if (ReadNextMove(copyBoard2, Player).Any(n => n.Col == temp && n.Row == rowV))
                                {
                                    score += 9;
                                    // Next Step 저장
                                    Condition = (board) =>
                                    {
                                        if (board[colV][rowV] == (Player ^ 3))
                                        {
                                            return new ColRow(temp, rowV);
                                        }
                                        else
                                        {
                                            return new ColRow(-1, -1);
                                        }
                                    };
                                }
                            }
                        }
                        else if (copyBoard[colV][rowV ^ (length - 1)] == 0 && copyBoard[colV][rowV ^ (length - 2)] == Player)
                        {
                            int count = 0;
                            int temp = 0;
                            for (int i = (rowV ^ 1); i < (rowV ^ (length - 2)); i++)
                            {
                                if (copyBoard[colV][i] == 0)
                                {
                                    temp = i;
                                    count++;
                                }
                            }
                            if (count == 1)
                            {
                                var copyBoard2 = ChangeBoard(colV, rowV, copyBoard, Player ^ 3);
                                if (ReadNextMove(copyBoard2, Player).Any(n => n.Col == colV && n.Row == temp))
                                {
                                    score += 9;
                                    // Next Step 저장
                                    Condition = (board) =>
                                    {
                                        if (board[colV][rowV] == (Player ^ 3))
                                        {
                                            return new ColRow(colV, temp);
                                        }
                                        else
                                        {
                                            return new ColRow(-1, -1);
                                        }
                                    };
                                }
                            }
                        }
                    }
                }

                // 두는게 불리할 경우 -1, -1 (Pass Count 센다.)
                if (score > max || PassCount >= 10)
                {
                    max = score;
                    cr = item;
                    PassCount = 0;
                    if (score == 110) break;
                }
            }

            col = cr.Col;
            row = cr.Row;

            if (col == -1)
            {
                PassCount++;
            }
            else
            {
                PassCount = 0;
            }
        }

        /// <summary>
        /// 모서리 직전 포인트 체크
        /// </summary>
        /// <param name="length"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsDeadPoint(int length, ColRow item)
        {
            return item.Col == 0 && item.Row == 1 ||
                                item.Col == 0 && item.Row == (length - 2) ||
                                item.Col == 1 && item.Row == 0 ||
                                item.Col == 1 && item.Row == 1 ||
                                item.Col == 1 && item.Row == (length - 2) ||
                                item.Col == 1 && item.Row == (length - 1) ||
                                item.Col == 1 && item.Row == 0 ||
                                item.Col == (length - 2) && item.Row == 0 ||
                                item.Col == 0 && item.Row == 1 ||
                                item.Col == 1 && item.Row == 1 ||
                                item.Col == (length - 2) && item.Row == 1 ||
                                item.Col == (length - 1) && item.Row == 1;
        }

        /// <summary>
        /// 모서리 체크
        /// </summary>
        /// <param name="length"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsVertex(int length, ColRow item)
        {
            return item.Col == 0 && item.Row == 0 ||
                                item.Col == 0 && item.Row == length - 1 ||
                                item.Col == length - 1 && item.Row == 0 ||
                                item.Col == length - 1 && item.Row == length - 1;
        }

        /// <summary>
        ///  특수 조건 저장
        /// </summary>
        Func<List<List<int>>, ColRow> Condition;

        /// <summary>
        /// 임시적으로 다음 수를 두어본다.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="reversiBoard"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private List<List<int>> ChangeBoard(int row, int col, List<List<int>> reversiBoard, int player)
        {
            List<List<int>> answer = new List<List<int>>();
            foreach (var item in reversiBoard)
            {
                var list = new List<int>();
                foreach (var item2 in item)
                {
                    list.Add(item2);
                }
                answer.Add(list);
            }

            int nx = col;
            int ny = row;

            answer[nx][ny] = player;

            for (int i = 0; i < dx.Length; i++)
            {
                nx += dx[i];
                ny += dy[i];
                while (nx >= 0 && ny >= 0 && nx < reversiBoard.Count && ny < reversiBoard.Count)
                {
                    if (answer[nx][ny] == (player ^ 3))
                    {
                        answer[nx][ny] = player;
                    }
                    else
                    {
                        break;
                    }
                    nx += dx[i];
                    ny += dy[i];
                }
            }

            return answer;
        }

        /// <summary>
        /// 둘 수 있는 칸을 읽어들인다.
        /// </summary>
        /// <param name="reversiBoard"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private IEnumerable<ColRow> ReadNextMove(List<List<int>> reversiBoard, int player)
        {
            for (int x = 0; x < reversiBoard.Count; x++)
            {
                for (int y = 0; y < reversiBoard[x].Count; y++)
                {
                    if (reversiBoard[x][y] == 0)
                    {
                        for (int i = 0; i < dx.Length; i++)
                        {
                            int nx = x + dx[i];
                            int ny = y + dy[i];
                            if (nx >= 0 && ny >= 0 && nx < reversiBoard.Count && ny < reversiBoard[x].Count)
                            {
                                if (reversiBoard[nx][ny] == (player ^ 3))
                                {
                                    nx += dx[i];
                                    ny += dy[i];
                                    while (nx >= 0 && ny >= 0 && nx < reversiBoard.Count && ny < reversiBoard[x].Count)
                                    {
                                        if (reversiBoard[nx][ny] == 0) break;

                                        if (reversiBoard[nx][ny] == player)
                                        {
                                            yield return new ColRow(x, y);
                                        }

                                        nx += dx[i];
                                        ny += dy[i];
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // 좌표용 구조체
        struct ColRow
        {
            public ColRow(int Col, int Row)
            {
                this.Col = Col;
                this.Row = Row;
            }
            public int Row;
            public int Col;
        }
    }
}
