namespace Play
{
    using System;
    using System.Collections.Generic;

    public interface IPlay
    {
        /// <summary>
        /// 플레이어 닉네임
        /// </summary>
        string PlayerNickName { get; }

        /// <summary>
        /// 플레이어 흑돌 or 백돌 여부 <para/>
        /// 1 = 흑돌 / 2 = 백돌 입니다.
        /// </summary>
        int Player { get; set; }

        /// <summary>
        /// 지정한 좌표에 돌을 놓고, 상대편 돌을 뒤집습니다.<para/>
        /// 뒤집을 수 있는 돌들은 게임에서 자동으로 뒤집힙니다.<para/>
        /// 돌이 놓을 수 있는 칸을 못 찾는 경우 또는 한턴 쉬는 경우 row = -1, col = -1로 대입하는 경우 바로 다음 플레이어에게 돌아갑니다.
        /// </summary>
        /// <param name="row">X좌표 값</param>
        /// <param name="col">Y좌표 값</param>
        /// <param name="reversiBoard">게임 보드 결과<para/>2차원 배열 형태이며,<para/>0 = 빈칸, 1 = 흑돌, 2 = 백돌</param>
        void Move(out int row, out int col, List<List<int>> reversiBoard);
    }
}
