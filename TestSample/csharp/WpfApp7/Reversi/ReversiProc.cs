namespace Reversi
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class ReversiProc
    {
        const int ROWS = 8;
        const int COLS = 8;
        private List<List<ReversiCellModel>> _boardArray;
        private List<List<int>> _boardArrayByResponse;

        public ReversiProc()
        {
            var idx = 0;

            _boardArray = new List<List<ReversiCellModel>>();
            for (int x = 0; x < ROWS; x++)
            {
                _boardArray.Add(new List<ReversiCellModel>());

                for (int y = 0; y < COLS; y++)
                {
                    _boardArray[x].Add(new ReversiCellModel());
                    _boardArray[x][y].Idx = idx;
                    idx++;
                }
            }
            
            // 초기 세팅
            _boardArray[3][3].CellState = 1;
            _boardArray[4][3].CellState = 2;
            _boardArray[3][4].CellState = 2;
            _boardArray[4][4].CellState = 1;


            // 플레이어에게 제공 용도 2차원 배열 board정보
            _boardArrayByResponse = new List<List<int>>();
            for (int x = 0; x < ROWS; x++)
            {
                _boardArrayByResponse.Add(new List<int>());

                for (int y = 0; y < COLS; y++)
                {
                    _boardArrayByResponse[x].Add(0);
                }
            }

            _boardArrayByResponse[3][3] = 1;
            _boardArrayByResponse[4][3] = 2;
            _boardArrayByResponse[3][4] = 2;
            _boardArrayByResponse[4][4] = 1;
        }

        public List<List<ReversiCellModel>> BoardArray
        {
            get { return _boardArray; }
        }

        public List<List<int>> BoardArrayByResponse
        {
            get { return _boardArrayByResponse; }
        }

        public int BlackScore
        {
            private set;
            get;
        }

        public int WhiteScore
        {
            private set;
            get;
        }

        /// <summary>
        /// 돌 뒤집기 시도
        /// </summary>
        /// <param name="state"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>null반환시 게임 오버</returns>
        public bool? MakeMove(int state, int row, int col)
        {
            try
            {
                if (_boardArray[col][row].CellState != 0)
                    return false;

                List<bool> movedList = new List<bool>();
                movedList.Add(this.FlipLeft(row - 1, col, state, 1));
                movedList.Add(this.FlipRight(row + 1, col, state, 1));
                movedList.Add(this.FlipUp(row, col - 1, state, 1));
                movedList.Add(this.FlipDown(row, col + 1, state, 1));
                movedList.Add(this.FlipLeftDown(row - 1, col + 1, state, 1));
                movedList.Add(this.FlipRightDown(row + 1, col + 1, state, 1));
                movedList.Add(this.FlipLeftUp(row - 1, col - 1, state, 1));
                movedList.Add(this.FlipRightUp(row + 1, col - 1, state, 1));

                if (movedList.Any(p => p))
                {
                    _boardArray[col][row].CellState = state;

                    // 플레이어 제공자 board업데이트
                    for (int x = 0; x < _boardArray[0].Count; x++)
                    {
                        for (int y = 0; y < _boardArray.Count; y++)
                        {
                            _boardArrayByResponse[x][y] = _boardArray[x][y].CellState;
                        }
                    }

                    if (this.CalcScores())
                    {
                        return null;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool FlipRightUp(int row, int col, int state, int distance) {
            if (col < 0 || row >= _boardArray.Count || _boardArray[col][row].CellState == 0)
                return false;

            if (_boardArray[col][row].CellState == state)
                return (distance > 1);

            if (this.FlipRightUp(row + 1, col - 1, state, distance + 1)) {
                _boardArray[col][row].CellState = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FlipRightDown(int row, int col, int state, int distance)
        {
            if (col >= _boardArray.Count || row >= _boardArray.Count || _boardArray[col][row].CellState == 0)
                return false;

            if (_boardArray[col][row].CellState == state)
                return (distance > 1);

            if (this.FlipRightDown(row + 1, col + 1, state, distance + 1))
            {
                _boardArray[col][row].CellState = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FlipLeftUp(int row, int col, int state, int distance)
        {
            if (col < 0 || row < 0 || _boardArray[col][row].CellState == 0)
                return false;

            if (_boardArray[col][row].CellState == state)
                return (distance > 1);

            if (this.FlipLeftUp(row - 1, col - 1, state, distance + 1))
            {
                _boardArray[col][row].CellState = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FlipLeftDown(int row, int col, int state, int distance)
        {
            if (col >= _boardArray.Count || row < 0 || _boardArray[col][row].CellState == 0)
                return false;

            if (_boardArray[col][row].CellState == state)
                return (distance > 1);

            if (this.FlipLeftDown(row - 1, col + 1, state, distance + 1))
            {
                _boardArray[col][row].CellState = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FlipDown(int row, int col, int state, int distance)
        {
            if (col >= _boardArray.Count || _boardArray[col][row].CellState == 0)
                return false;

            if (_boardArray[col][row].CellState == state)
                return (distance > 1);

            if (this.FlipDown(row, col + 1, state, distance + 1))
            {
                _boardArray[col][row].CellState = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FlipUp(int row, int col, int state, int distance)
        {
            if (col < 0 || _boardArray[col][row].CellState == 0)
                return false;

            if (_boardArray[col][row].CellState == state)
                return (distance > 1);

            if (this.FlipUp(row, col - 1, state, distance + 1))
            {
                _boardArray[col][row].CellState = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FlipRight(int row, int col, int state, int distance)
        {
            if (row >= _boardArray.Count || _boardArray[col][row].CellState == 0)
                return false;

            if (_boardArray[col][row].CellState == state)
                return (distance > 1);

            if (this.FlipRight(row + 1, col, state, distance + 1))
            {
                _boardArray[col][row].CellState = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FlipLeft(int row, int col, int state, int distance)
        {
            if (row < 0 || _boardArray[col][row].CellState == 0)
                return false;

            if (_boardArray[col][row].CellState == state)
                return (distance > 1);

            if (this.FlipLeft(row - 1, col, state, distance + 1))
            {
                _boardArray[col][row].CellState = state;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 점수 계산
        /// </summary>
        /// <returns>true = 게임오버 / false = 게임 진행중</returns>
        public bool CalcScores()
        {
            int row_len = _boardArray[0].Count;
            int col_len = _boardArray.Count;
            int blackScore = 0;
            int whiteScore = 0;

            bool isGameOver = false;

            for (int x = 0; x < row_len; x++)
            {
                for(int y = 0; y < col_len; y++)
                {
                    if (_boardArray[x][y].CellState == 1)
                        blackScore += 1;
                    if (_boardArray[x][y].CellState == 2)
                        whiteScore += 1;
                }
            }

            BlackScore = blackScore;
            WhiteScore = whiteScore;

            if ((blackScore + whiteScore) >= (row_len * col_len))
            {
                isGameOver = true;
            }
            else if(this.LegalMove(1) == false && this.LegalMove(2) == false)
            {
                isGameOver = true;
            }
            return isGameOver;
        }

        private bool LegalMove(int state)
        {
            for (int x = 0; x < _boardArray[0].Count; x++)
            {
                for (int y = 0; y < _boardArray.Count; y++)
                {
                    if (this.ValidMove(x, y, state))
                        return true;
                }
            }

            return false;
        }

        private bool ValidMove(int row, int col, int valid_state)
        {
            if (_boardArray[col][row].CellState != 0)
                return false;

            if (row + 1 < _boardArray[0].Count && _boardArray[col][row + 1].CellState == valid_state)
            {
                return true;
            }
            else if (row > 0 && _boardArray[col][row - 1].CellState == valid_state)
            {
                return true;
            }
            else if (col > 0 && _boardArray[col - 1][row].CellState == valid_state)
            {
                return true;
            }
            else if (col + 1 < _boardArray.Count && _boardArray[col + 1][row].CellState == valid_state)
            {
                return true;
            }
            else if (col > 0 && row > 0 && _boardArray[col - 1][row - 1].CellState == valid_state)
            {
                return true;
            }
            else if (col > 0 && row + 1 < _boardArray.Count && _boardArray[col - 1][row + 1].CellState == valid_state)
            {
                return true;
            }
            else if (col + 1 < _boardArray.Count && row + 1 < _boardArray[0].Count && _boardArray[col + 1][row + 1].CellState == valid_state)
            {
                return true;
            }
            else if (col + 1 < _boardArray.Count && row > 0 && _boardArray[col + 1][row - 1].CellState == valid_state)
            {
                return true;
            }

            return false;
        }
    }
}
