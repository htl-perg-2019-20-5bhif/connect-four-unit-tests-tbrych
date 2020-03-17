using ConnectFour.Logic;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ConnectFour.API.Controllers
{

    [ApiController]
    [Route("connectfour")]
    public class ConnectFourController : ControllerBase
    {
        private Game game;

        public ConnectFourController(Game game)
        {
            this.game = game;
        }

        [HttpGet]
        [Route("board")]
        public ActionResult<int[][]> GetBoard()
        {
            var cBoard = game.board.GetBoard();
            var arBoard = new int[6][];

            for (byte row = 0; row < 6; row++)
            {
                arBoard[row] = new int[7];
            }

            for (byte row = 0; row < 6; row++)
            {
                for (byte column = 0; column < 7; column++)
                {
                    arBoard[6 - 1 - row][column] = (int)cBoard[column, row];
                }
            }
            return Ok(arBoard);
        }

        [HttpPost]
        [Route("set/{column}")]
        public IActionResult SetStone(byte column)
        {
            try
            {
                return Ok(game.board.SetStone(column));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("restart")]
        public ActionResult<int[][]> RestartGame()
        {
            game.board = new GameBoard();
            return GetBoard();
        }
    }
}
